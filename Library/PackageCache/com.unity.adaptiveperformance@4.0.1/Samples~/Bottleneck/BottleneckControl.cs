using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AdaptivePerformance;
using UnityEngine.UI;

public class BottleneckControl : MonoBehaviour
{
    class Marker
    {
        public float time;
        public string label;
        public int objectCount;
    }

    IAdaptivePerformance ap;
    SampleFactory factory;

    public GameObject cpuLoader;
    public GameObject gpuLoader;

    float timer = 0;
    bool switching = false;
    public float timeOut = 40;
    float timeOuttimer = 0;
    public float waitTimeBeforeSwitch = 5;
    public float waitTimeAfterTargetReached = 3;
    string state = "Waiting for Target FPS";
    public Text bottleneckStatus;

    PerformanceBottleneck targetBottleneck = PerformanceBottleneck.CPU;
    BottleneckUI bottleneckUI;

    System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
    List<Marker> markers = new List<Marker>();

    public int stateChangeIterations = 3;

    void Start()
    {
        StartCoroutine(Begin());
    }

    IEnumerator Begin()
    {
        yield return new WaitForSeconds(1);
        factory = FindObjectOfType<SampleFactory>();
        bottleneckUI = FindObjectOfType<BottleneckUI>();

        factory.RunTest = false;
        timer = waitTimeBeforeSwitch;
        ap = Holder.Instance;

        if (ap == null || !ap.Active)
        {
            state = "Adaptive Performance not active";
            Debug.Log("[AP Bottleneck] Adaptive Performance not active");
            FinishTest();
        }
        else
        {
            timeOuttimer = timeOut;
            watch.Start();
            factory.prefab = cpuLoader;
            factory.RunTest = true;
            state = "Ramping up CPU load";
            StartCoroutine(ObserveBottleneck());
            markers.Add(new Marker { label = state, time = watch.ElapsedMilliseconds, objectCount = factory.internalObjs });
            Debug.Log("[AP Bottleneck] Starting Test");
        }
        bottleneckStatus.text = state;
    }

    IEnumerator ObserveBottleneck()
    {
        while (true)
        {
            if (timer < 0)
            {
                timer = waitTimeBeforeSwitch;
                StartCoroutine(NextBottleneck());
            }
            yield return new WaitForEndOfFrame();
        }
    }

    void FinishTest()
    {
        markers.Add(new Marker { time = watch.ElapsedMilliseconds, label = "Test Finish", objectCount = factory.internalObjs });
        foreach (var t in markers)
        {
            Debug.LogFormat("Timestamp : {0} s , Label : {1} , Objects : {2} \n", t.time / 1000f, t.label, t.objectCount);
        }

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    IEnumerator NextBottleneck(bool timeout = false)
    {
        switching = true;
        stateChangeIterations--;
        timeOuttimer = timeOut;
        Debug.Log($"State change = {stateChangeIterations}");

        switch (targetBottleneck)
        {
            case PerformanceBottleneck.CPU:
                yield return StartCoroutine(LogResult("GPU", timeout));
                factory.FlushObjects();
                factory.RunTest = true;
                state = "Ramping up GPU Load";
                factory.prefab = gpuLoader;
                factory.spawnAmount = 0.1f;
                targetBottleneck = PerformanceBottleneck.GPU;
                break;
            case PerformanceBottleneck.GPU:
                yield return StartCoroutine(LogResult("TargetFrameRate", timeout));
                factory.FlushObjects();
                factory.RunTest = false;
                state = "Waiting for TargetFrameRate";
                targetBottleneck = PerformanceBottleneck.TargetFrameRate;
                break;
            case PerformanceBottleneck.TargetFrameRate:
                yield return StartCoroutine(LogResult("CPU", timeout));
                factory.FlushObjects();
                factory.RunTest = true;
                state = "Ramping up CPU Load";
                factory.prefab = cpuLoader;
                factory.spawnAmount = 1;
                targetBottleneck = PerformanceBottleneck.CPU;
                break;
        }
        switching = false;
    }

    IEnumerator LogResult(string nextBottleneck, bool timeout = false)
    {
        if (timeout)
        {
            markers.Add(new Marker { time = watch.ElapsedMilliseconds, label = $"Timed out before reaching {targetBottleneck} Bottleneck, switching to {nextBottleneck} in 3 seconds", objectCount = factory.internalObjs});
        }
        else
        {
            markers.Add(new Marker { time = watch.ElapsedMilliseconds, label = $"{targetBottleneck} Bottleneck reached, switching to {nextBottleneck} in 3 seconds", objectCount = factory.internalObjs });
        }
        state = $"Waiting {waitTimeAfterTargetReached}s before changing to {nextBottleneck}";
        yield return new WaitForSeconds(waitTimeAfterTargetReached);

        if (stateChangeIterations <= 0)
        {
            FinishTest();
        }
        state = $"Changed to {nextBottleneck} target bottleneck";
        markers.Add(new Marker { time = watch.ElapsedMilliseconds, label = state, objectCount = factory.internalObjs });
    }

    void Update()
    {
        bottleneckStatus.text = $"{state} {Environment.NewLine} Timeout in {timeOuttimer:F2}s";
        if (bottleneckUI != null)
            bottleneckUI.targetBottleneck = targetBottleneck;

        if (ap == null || !ap.Active)
            return;

        if (switching)
            return;

        if (ap.PerformanceStatus.PerformanceMetrics.PerformanceBottleneck == targetBottleneck)
        {
            state = $"Changing in {timer:F2}s...";
            timer -= Time.deltaTime;
        }
        else
        {
            timer = waitTimeBeforeSwitch;
        }

        timeOuttimer -= Time.deltaTime;
        if (timeOuttimer < 0)
        {
            StartCoroutine(NextBottleneck(true));
            timeOuttimer = timeOut;
        }
    }
}
