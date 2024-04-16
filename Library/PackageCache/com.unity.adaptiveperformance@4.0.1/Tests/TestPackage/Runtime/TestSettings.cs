using UnityEngine;
using UnityEngine.AdaptivePerformance;

namespace UnityEngine.AdaptivePerformance.TestPackage
{
    [AdaptivePerformanceConfigurationData("Test Settings", Constants.k_SettingsKey)]
    public class TestSettings : IAdaptivePerformanceSettings
    {
#if !UNITY_EDITOR
        internal static TestSettings s_Settings;

        public void Awake()
        {
            s_Settings = this;
        }

#endif
    }
}
