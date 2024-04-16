#if VISUAL_SCRIPTING_ENABLED

using Unity.VisualScripting;

namespace UnityEngine.AdaptivePerformance.VisualScripting
{
    [UnitShortTitle("Cluster Info")]
    [UnitSubtitle("CPU Core Cluster")]
    [UnitCategory("AdaptivePerformance/Performance")]
    public class ClusterInfoUnit : Unit
    {
        [DoNotSerialize]
        public ValueOutput bigCore;
        [DoNotSerialize]
        public ValueOutput mediumCore;
        [DoNotSerialize]
        public ValueOutput littleCore;

        int BigCore = -1;
        int MediumCore = -1;
        int LittleCore = -1;

        protected override void Definition()
        {
            bigCore = ValueOutput<int>("Big Core", (flow) => { UpdateStats(); return BigCore; });
            mediumCore = ValueOutput<int>("Medium Core", (flow) => { UpdateStats(); return MediumCore; });
            littleCore = ValueOutput<int>("Little Core", (flow) => { UpdateStats(); return LittleCore; });
        }

        void UpdateStats()
        {
            if (Application.isPlaying && Holder.Instance != null)
            {
                var ci = Holder.Instance.PerformanceStatus.PerformanceMetrics.ClusterInfo;
                BigCore = ci.BigCore;
                MediumCore = ci.MediumCore;
                LittleCore = ci.LittleCore;
            }
        }
    }
}
#endif
