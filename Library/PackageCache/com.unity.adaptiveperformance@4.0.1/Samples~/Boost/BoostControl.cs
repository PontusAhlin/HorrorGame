using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AdaptivePerformance;
using UnityEngine.UI;

public class BoostControl : MonoBehaviour
{
    IAdaptivePerformance ap;
    SampleFactory factory;

    public GameObject cpuLoader;
    public GameObject gpuLoader;

    public float timeOut = 3000;
    string state = "";
    public Text bottleneckStatus;
    bool testRunning = false;

    PerformanceBottleneck targetBottleneck = PerformanceBottleneck.CPU;
    BottleneckUI bottleneckUI;

    System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();

    void Start()
    {
        factory = FindObjectOfType<SampleFactory>();
        bottleneckUI = FindObjectOfType<BottleneckUI>();

        factory.RunTest = false;
        ap = Holder.Instance;

        if (ap == null || !ap.Active)
        {
            state = "Adaptive Performance not active";
            Debug.Log("[AP Boost] Adaptive Performance not active");
            return;
        }
        else
        {
            watch.Start();
            state = "Waiting on Load";
            targetBottleneck = PerformanceBottleneck.TargetFrameRate;
            Debug.LogFormat("[AP Boost] Starting Test Timestamp : {0} s , Label : {1} , Objects : {2} \n", watch.ElapsedMilliseconds / 1000f, state, factory.internalObjs);
        }
        bottleneckStatus.text = state;
        ap.PerformanceStatus.PerformanceBoostChangeEvent += OnBoostModeEvent;
    }

    private void OnDestroy()
    {
        if (ap == null || !ap.Active)
            return;

        ap.PerformanceStatus.PerformanceBoostChangeEvent -= OnBoostModeEvent;
    }

    void OnBoostModeEvent(PerformanceBoostChangeEventArgs ev)
    {
        if (!ev.CpuBoost || !ev.GpuBoost)
            LogResult();
    }

    public void BoostCPU()
    {
        if (ap == null || !ap.Active)
            return;
        ap.DevicePerformanceControl.CpuPerformanceBoost = true;
    }

    public void BoostGPU()
    {
        if (ap == null || !ap.Active)
            return;
        ap.DevicePerformanceControl.GpuPerformanceBoost = true;
    }

    public void CPULoad()
    {
        StartTest(PerformanceBottleneck.CPU);
    }

    public void GPULoad()
    {
        StartTest(PerformanceBottleneck.GPU);
    }

    void StartTest(PerformanceBottleneck target)
    {
        LogResult();
        factory.FlushObjects();

        switch (target)
        {
            case PerformanceBottleneck.GPU:
                factory.RunTest = true;
                factory.prefab = gpuLoader;
                factory.spawnAmount = 50f;
                factory.LimitCount = 50;
                targetBottleneck = PerformanceBottleneck.GPU;
                testRunning = true;
                break;
            case PerformanceBottleneck.TargetFrameRate:
                factory.RunTest = false;
                state = "";
                targetBottleneck = PerformanceBottleneck.TargetFrameRate;
                testRunning = false;
                break;
            case PerformanceBottleneck.CPU:
                factory.RunTest = true;
                factory.prefab = cpuLoader;
                factory.spawnAmount = 1;
                factory.LimitCount = 2000;
                targetBottleneck = PerformanceBottleneck.CPU;
                testRunning = true;
                break;
        }
        state = $"Changed to {target} load";
        watch.Reset();
        watch.Start();
        LogResult();
    }

    void LogResult()
    {
        Debug.LogFormat("[AP Boost] Timestamp : {0} s , Label : {1} , Objects : {2} \n", watch.ElapsedMilliseconds / 1000f, state, factory.internalObjs);
    }

    void Update()
    {
        bottleneckStatus.text = $"{state}" + (testRunning ? $"{Environment.NewLine} Timeout in {timeOut-watch.ElapsedMilliseconds / 1000f:F2}s" : "");
        if (bottleneckUI != null)
            bottleneckUI.targetBottleneck = targetBottleneck;

        if (ap == null || !ap.Active)
            return;

        if (!testRunning)
            return;

        if (watch.ElapsedMilliseconds / 1000f > timeOut)
            StartTest(PerformanceBottleneck.TargetFrameRate);
    }
}
