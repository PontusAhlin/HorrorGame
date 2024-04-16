#if VISUAL_SCRIPTING_ENABLED

using Unity.VisualScripting;

namespace UnityEngine.AdaptivePerformance.VisualScripting
{
    [UnitShortTitle("Get Performance Levels")]
    [UnitSubtitle("CPU and GPU Levels")]
    [UnitCategory("AdaptivePerformance/Performance")]
    public class GetPerformanceLevelsUnit : Unit
    {
        [DoNotSerialize]
        public ValueOutput cpuLevel;

        [DoNotSerialize]
        public ValueOutput gpuLevel;

        int CpuLevel = -1;
        int GpuLevel = -1;

        protected override void Definition()
        {
            cpuLevel = ValueOutput<int>("CPU Level", (flow) => { UpdateStats(); return CpuLevel; });
            cpuLevel = ValueOutput<int>("GPU Level", (flow) => { UpdateStats(); return GpuLevel; });
        }

        void UpdateStats()
        {
            if (Application.isPlaying && Holder.Instance != null)
            {
                var pm = Holder.Instance.PerformanceStatus.PerformanceMetrics;
                CpuLevel = pm.CurrentCpuLevel;
                GpuLevel = pm.CurrentGpuLevel;
            }
        }
    }
}
#endif
