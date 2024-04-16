using UnityEngine;
using UnityEngine.AdaptivePerformance;
using UnityEngine.UI;

public class AdaptiveFrameRateSettings : MonoBehaviour
{
    public Slider VRRSettingsMin;
    public Text MinText;
    public Slider VRRSettingsMax;
    public Text MaxText;
    public Toggle VRREnabled;
#if UNITY_ADAPTIVE_PERFORMANCE_SAMSUNG_ANDROID
    AdaptiveVariableRefreshRate AdaptiveVRRO;
#endif

    void Awake()
    {
#if UNITY_ADAPTIVE_PERFORMANCE_SAMSUNG_ANDROID
        AdaptiveVRRO = GameObject.FindObjectOfType<AdaptiveVariableRefreshRate>();
        if (!AdaptiveVRRO)
            return;

        VRREnabled.isOn = AdaptiveVRRO.Enabled;

        VRRSettingsMin.value = AdaptiveVRRO.MinBound;
        VRRSettingsMax.value = AdaptiveVRRO.MaxBound;
#else
        Debug.Log("Adaptive VRR not supported, please install a provider with VRR support.");
#endif
    }

    public void ToggleVRR()
    {
#if UNITY_ADAPTIVE_PERFORMANCE_SAMSUNG_ANDROID
        if (AdaptiveVRRO)
            AdaptiveVRRO.Enabled = VRREnabled.isOn;
#endif
    }

    void Update()
    {
#if UNITY_ADAPTIVE_PERFORMANCE_SAMSUNG_ANDROID
        if (!AdaptiveVRRO)
            return;

        AdaptiveVRRO.MinBound = VRRSettingsMin.value;
        AdaptiveVRRO.MaxBound = VRRSettingsMax.value;
#endif
        MinText.text = $"Min FPS - {VRRSettingsMin.value}";
        MaxText.text = $"Max FPS - {VRRSettingsMax.value}";
    }
}
