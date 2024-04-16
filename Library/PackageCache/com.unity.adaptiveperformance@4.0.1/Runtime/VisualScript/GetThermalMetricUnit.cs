#if VISUAL_SCRIPTING_ENABLED

using System;
using Unity.VisualScripting;

namespace UnityEngine.AdaptivePerformance.VisualScripting
{
    [UnitShortTitle("Thermal Metric")]
    [UnitSubtitle("Throttling and Thermal Info")]
    [UnitCategory("AdaptivePerformance/Thermal")]
    public class GetThermalMetricUnit : Unit
    {
        [DoNotSerialize]
        public ValueOutput warningLevel { get; private set; }

        [DoNotSerialize]
        public ValueOutput throttlingImminent;

        [DoNotSerialize]
        public ValueOutput throttling;

        [DoNotSerialize]
        public ValueOutput temperatureLevel;

        [DoNotSerialize]
        public ValueOutput temperatureTrend;

        float TemperatureLevel = -1.0f;
        float TemperatureTrend = -1.0f;
        string WarningLevel = "unknown";
        bool ThrottlingImminent = false;
        bool Throttling = false;

        protected override void Definition()
        {
            warningLevel = ValueOutput<String>(nameof(warningLevel), (flow) => { UpdateStats(); return WarningLevel; });
            throttlingImminent = ValueOutput<bool>("throttlingImminent", (flow) => { UpdateStats(); return ThrottlingImminent; });
            throttling = ValueOutput<bool>("throttling", (flow) => { UpdateStats(); return Throttling; });
            temperatureLevel = ValueOutput<float>("Temperature Level", (flow) => { UpdateStats();  return TemperatureLevel; });
            temperatureTrend = ValueOutput<float>("Temperature Trend", (flow) => { UpdateStats();  return TemperatureTrend; });
        }

        void UpdateStats()
        {
            if (Application.isPlaying && Holder.Instance != null)
            {
                var tm = Holder.Instance.ThermalStatus.ThermalMetrics;
                TemperatureLevel = tm.TemperatureLevel;
                TemperatureTrend = tm.TemperatureTrend;
                ThrottlingImminent = tm.WarningLevel == AdaptivePerformance.WarningLevel.ThrottlingImminent;
                Throttling = tm.WarningLevel == AdaptivePerformance.WarningLevel.Throttling;
                WarningLevel = tm.WarningLevel.ToString();
            }
        }
    }
}
#endif
