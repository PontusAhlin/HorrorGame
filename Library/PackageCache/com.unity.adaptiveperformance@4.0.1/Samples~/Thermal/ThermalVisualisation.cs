using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AdaptivePerformance;

public class ThermalVisualisation : MonoBehaviour
{
    public Slider temperatureTrendSlider;
    public Slider temperatureLevelSlider;
    public Text thermalWarning;
    public Image thermalPanel;

    private IAdaptivePerformance ap;

    void Update()
    {
        if (ap == null)
            return;

        temperatureTrendSlider.value = ap.ThermalStatus.ThermalMetrics.TemperatureTrend;
        temperatureLevelSlider.value = ap.ThermalStatus.ThermalMetrics.TemperatureLevel;
    }

    void OnThermalEvent(ThermalMetrics ev)
    {
        thermalWarning.text = ev.WarningLevel.ToString();
        switch (ev.WarningLevel)
        {
            case WarningLevel.NoWarning:
                thermalPanel.color = Color.green;
                break;
            case WarningLevel.ThrottlingImminent:
                thermalPanel.color = Color.magenta;
                break;
            case WarningLevel.Throttling:
                thermalPanel.color = Color.red;
                break;
        }
    }

    void Start()
    {
        ap = Holder.Instance;
        if (ap == null)
        {
            Debug.Log("[Thermal Visualization] Warning Adaptive Performance Manager was not found and does not report");
            return;
        }
        ap.ThermalStatus.ThermalEvent += OnThermalEvent;
    }
}
