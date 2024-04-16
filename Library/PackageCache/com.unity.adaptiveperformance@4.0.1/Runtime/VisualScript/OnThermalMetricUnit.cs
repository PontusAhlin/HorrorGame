#if VISUAL_SCRIPTING_ENABLED

using System;
using Unity.VisualScripting;

namespace UnityEngine.AdaptivePerformance.VisualScripting
{
    [UnitShortTitle("On Thermal Metric")]
    [UnitSubtitle("Throttling and Thermal Info")]
    [UnitCategory("AdaptivePerformance/Thermal")]
    public class OnThermalMetricUnit : EventUnit<WarningLevel>
    {
        [DoNotSerialize]
        public ValueOutput warningLevel { get; private set; }

        [DoNotSerialize]
        public ValueOutput throttlingImminent;

        [DoNotSerialize]
        public ValueOutput throttling;

        string WarningLevel = "unknown";
        bool ThrottlingImminent = false;
        bool Throttling = false;

        protected override bool register => true;

        public override EventHook GetHook(GraphReference reference)
        {
            return new EventHook(AdaptivePerformanceEventHooks.OnThermalEvent);
        }

        protected override void AssignArguments(Flow flow, WarningLevel data)
        {
            flow.SetValue(warningLevel, data.ToString());
            flow.SetValue(throttlingImminent, data == AdaptivePerformance.WarningLevel.ThrottlingImminent);
            flow.SetValue(throttling, data == AdaptivePerformance.WarningLevel.Throttling);
        }

        protected override void Definition()
        {
            base.Definition();
            warningLevel = ValueOutput<String>(nameof(warningLevel), (flow) => { UpdateStats(); return WarningLevel; });
            throttlingImminent = ValueOutput<bool>("throttlingImminent", (flow) => { UpdateStats(); return ThrottlingImminent; });
            throttling = ValueOutput<bool>("throttling", (flow) => { UpdateStats(); return Throttling; });
        }

        void UpdateStats()
        {
            if (Application.isPlaying && Holder.Instance != null)
            {
                var tm = Holder.Instance.ThermalStatus.ThermalMetrics;
                ThrottlingImminent = tm.WarningLevel == AdaptivePerformance.WarningLevel.ThrottlingImminent;
                Throttling = tm.WarningLevel == AdaptivePerformance.WarningLevel.Throttling;
                WarningLevel = tm.WarningLevel.ToString();
            }
        }
    }
}
#endif
