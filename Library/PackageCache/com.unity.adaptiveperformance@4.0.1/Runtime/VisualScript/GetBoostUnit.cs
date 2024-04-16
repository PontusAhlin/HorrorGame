#if VISUAL_SCRIPTING_ENABLED

using Unity.VisualScripting;

namespace UnityEngine.AdaptivePerformance.VisualScripting
{
    [UnitShortTitle("Get Boost")]
    [UnitSubtitle("Get CPU and GPU Boost")]
    [UnitCategory("AdaptivePerformance/Performance")]
    public class GetBoostUnit : Unit
    {
        [DoNotSerialize]
        public ValueOutput cpuBoost;

        [DoNotSerialize]
        public ValueOutput gpuBoost;

        bool CpuBoost;
        bool GpuBoost;

        protected override void Definition()
        {
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
