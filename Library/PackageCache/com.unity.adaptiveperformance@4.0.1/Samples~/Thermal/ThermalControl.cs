using System.Collections;
using UnityEngine;
using UnityEngine.AdaptivePerformance;

public class ThermalControl : MonoBehaviour
{
    public SampleFactory objectFactory;

    private IAdaptivePerformance ap;
    private int originalLimitCount;

    void OnThermalEvent(ThermalMetrics ev)
    {
        switch (ev.WarningLevel)
        {
            case WarningLevel.NoWarning:
                objectFactory.LimitCount = originalLimitCount;
                break;
            case WarningLevel.ThrottlingImminent:
                objectFactory.LimitCount = originalLimitCount / 4;
                break;
            case WarningLevel.Throttling:
                objectFactory.LimitCount = originalLimitCount / 100;
                break;
        }
    }

    void Start()
    {
        ap = Holder.Instance;
        if (ap == null)
            return;

        originalLimitCount = objectFactory.LimitCount;
        ap.ThermalStatus.ThermalEvent += OnThermalEvent;

        StartCoroutine(TestTimeout());
    }

    IEnumerator TestTimeout()
    {
        while (true)
        {
            yield return new WaitForSeconds(300);
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
