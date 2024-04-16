#if VISUAL_SCRIPTING_ENABLED

using Unity.VisualScripting;

namespace UnityEngine.AdaptivePerformance.VisualScripting
{
    [UnitShortTitle("Frame Timing")]
    [UnitSubtitle("Frame Time Metric")]
    [UnitCategory("AdaptivePerformance/Performance")]
    public class FrameTimingUnit : Unit
    {
        [DoNotSerialize]
        public ValueOutput currentFrameTime;
        [DoNotSerialize]
        public ValueOutput averageFrameTime;
        [DoNotSerialize]
        public ValueOutput currentGpuFrameTime;
        [DoNotSerialize]
        public ValueOutput averageGpuFrameTime;
        [DoNotSerialize]
        public ValueOutput currentCpuFrameTime;
        [DoNotSerialize]
        public ValueOutput averageCpuFrameTime;

        float CurrentFrameTime = -1.0f;
        float AverageFrameTime = -1.0f;
        float CurrentGpuFrameTime = -1.0f;
        float AverageGpuFrameTime = -1.0f;
        float CurrentCpuFrameTime = -1.0f;
        float AverageCpuFrameTime = -1.0f;

        protected override void Definition()
        {
            currentFrameTime = ValueOutput<float>("Current Frametime", (flow) => { UpdateStats();  return CurrentFrameTime; });
            averageFrameTime = ValueOutput<float>("Average Frametime", (flow) => { UpdateStats(); return AverageFrameTime; });
            currentGpuFrameTime = ValueOutput<float>("Current Gpu Frametime", (flow) => { UpdateStats(); return CurrentGpuFrameTime; });
            averageGpuFrameTime = ValueOutput<float>("Average Gpu Frametime", (flow) => { UpdateStats(); return AverageGpuFrameTime; });
            currentCpuFrameTime = ValueOutput<float>("Current Cpu Frametime", (flow) => { UpdateStats(); return CurrentCpuFrameTime; });
            averageCpuFrameTime = ValueOutput<float>("Average Cpu Frametime", (flow) => { UpdateStats(); return AverageCpuFrameTime; });
        }

        void UpdateStats()
        {
            if (Application.isPlaying && Holder.Instance != null)
            {
                var ft = Holder.Instance.PerformanceStatus.FrameTiming;
                CurrentFrameTime = ft.CurrentFrameTime;
                AverageFrameTime = ft.AverageFrameTime;
                CurrentGpuFrameTime = ft.CurrentGpuFrameTime;
                AverageGpuFrameTime = ft.AverageGpuFrameTime;
                CurrentCpuFrameTime = ft.CurrentCpuFrameTime;
                AverageCpuFrameTime = ft.AverageCpuFrameTime;
            }
        }
    }
}
#endif
