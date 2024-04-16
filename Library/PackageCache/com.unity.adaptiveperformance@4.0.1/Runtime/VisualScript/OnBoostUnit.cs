#if VISUAL_SCRIPTING_ENABLED

using Unity.VisualScripting;

namespace UnityEngine.AdaptivePerformance.VisualScripting
{
    [UnitShortTitle("On CPU/GPU Boosted")]
    [UnitSubtitle("CPU and GPU Boost Event")]
    [UnitCategory("AdaptivePerformance/Performance")]
    public class OnBoostUnit : EventUnit<PerformanceBoostChangeEventArgs>
    {
        [DoNotSerialize]
        public ValueOutput cpuBoost;

        [DoNotSerialize]
        public ValueOutput gpuBoost;

        bool CpuBoost = false;
        bool GpuBoost = false;

        protected override bool register => true;

        public override EventHook GetHook(GraphReference reference)
        {
            return new EventHook(AdaptivePerformanceEventHooks.OnBoostEvent);
        }

        protected override void AssignArguments(Flow flow, PerformanceBoostChangeEventArgs data)
        {
            flow.SetValue(cpuBoost, data.CpuBoost);
            flow.SetValue(gpuBoost, data.GpuBoost);
        }

        protected override void Definition()
        {
            base.Definition();
            cpuBoost = ValueOutput<bool>("CPU Boost", (flow) => { UpdateStats(); return CpuBoost; });
            gpuBoost = ValueOutput<bool>("GPU Boost", (flow) => { UpdateStats(); return GpuBoost; });
        }

        void UpdateStats()
        {
            if (Application.isPlaying && Holder.Instance != null)
            {
                var pm = Holder.Instance.PerformanceStatus.PerformanceMetrics;
                CpuBoost = pm.CpuPerformanceBoost;
                GpuBoost = pm.GpuPerformanceBoost;
            }
        }
    }
}
#endif
