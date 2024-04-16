#if VISUAL_SCRIPTING_ENABLED

using Unity.VisualScripting;

namespace UnityEngine.AdaptivePerformance.VisualScripting
{
    [UnitShortTitle("FPS")]
    [UnitSubtitle("Frames Per Seconnd")]
    [UnitCategory("AdaptivePerformance/Performance")]
    public class FPSUnit : Unit
    {
        [DoNotSerialize]
        public ValueOutput fps;

        private int FPS;

        protected override void Definition()
        {
            fps = ValueOutput<float>("FPS", (flow) => { UpdateStats();  return FPS; });
        }

        void UpdateStats()
        {
            if (Application.isPlaying && Holder.Instance != null)
            {
                var ft = Holder.Instance.PerformanceStatus.FrameTiming;
                FPS = (int)(1.0f / ft.CurrentFrameTime);
            }
        }
    }
}
#endif
