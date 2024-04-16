#if VISUAL_SCRIPTING_ENABLED

using System;
using Unity.VisualScripting;

namespace UnityEngine.AdaptivePerformance.VisualScripting
{
    [UnitShortTitle("On Performance Level")]
    [UnitSubtitle("CPU and GPU Levels")]
    [UnitCategory("AdaptivePerformance/Performance")]
    public class OnPerformanceLevelsUnit : EventUnit<PerformanceLevelChangeEventArgs>
    {
        [DoNotSerialize]
        public ValueOutput CpuLevel;

        [DoNotSerialize]
        public ValueOutput GpuLevel;

        int cpuLevel = -1;
        int gpuLevel = -1;

        protected override bool register => true;

        public override EventHook GetHook(GraphReference reference)
        {
            return new EventHook(AdaptivePerformanceEventHooks.OnPerformanceLevelEvent);
        }

        protected override void AssignArguments(Flow flow, PerformanceLevelChangeEventArgs data)
        {
            flow.SetValue(CpuLevel, data.CpuLevel);
            flow.SetValue(GpuLevel, data.GpuLevel);
        }

        protected override void Definition()
        {
            base.Definition();
            CpuLevel = ValueOutput<int>("CPU Level", (flow) => { UpdateStats(); return cpuLevel; });
            GpuLevel = ValueOutput<int>("GPU Level", (flow) => { UpdateStats(); return gpuLevel; });
        }

        void UpdateStats()
        {
            if (Application.isPlaying && Holder.Instance != null)
            {
                var pm = Holder.Instance.PerformanceStatus.PerformanceMetrics;
                cpuLevel = pm.CurrentCpuLevel;
                gpuLevel = pm.CurrentGpuLevel;
            }
        }
    }
}
#endif
