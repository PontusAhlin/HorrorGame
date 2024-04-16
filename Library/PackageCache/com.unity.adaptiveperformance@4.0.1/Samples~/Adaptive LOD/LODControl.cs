using UnityEngine;
using UnityEngine.AdaptivePerformance;
using UnityEngine.UI;

public class LODControl : MonoBehaviour
{
    public Toggle LODToggle;
    AdaptivePerformanceScalerSettingsBase LODSettings;

    void Start()
    {
        var ap = Holder.Instance;
        if (ap == null || !ap.Active)
        {
            Debug.Log("[AP Indexer Visualisation] Adaptive Performance not active");
            enabled = false;
            return;
        }

        IAdaptivePerformanceSettings settings = AdaptivePerformanceGeneralSettings.Instance.Manager.activeLoader.GetSettings();
        if (settings == null)
            return;

        LODSettings = settings.scalerSettings.AdaptiveLOD;
        LODToggle.SetIsOnWithoutNotify(LODSettings.enabled);
    }

    public void ToggleAdaptiveLOD()
    {
        if (LODSettings == null)
            return;

        LODSettings.enabled = !LODSettings.enabled;
    }
}
