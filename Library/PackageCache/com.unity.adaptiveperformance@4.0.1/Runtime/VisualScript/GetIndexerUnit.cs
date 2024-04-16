#if VISUAL_SCRIPTING_ENABLED

using Unity.VisualScripting;

namespace UnityEngine.AdaptivePerformance.VisualScripting
{
    [UnitShortTitle("Get Indexer Data")]
    [UnitSubtitle("Performance and Thermal Actions")]
    [UnitCategory("AdaptivePerformance/Scaler")]
    public class GetIndexerUnit : Unit
    {
        [DoNotSerialize]
        public ValueOutput performanceAction;

        [DoNotSerialize]
        public ValueOutput thermalAction;

        [DoNotSerialize]
        public ValueOutput timeUntilNextAction;

        StateAction PerformanceAction;
        StateAction ThermalAction;
        float TimeUntilNextAction;

        protected override void Definition()
        {
            performanceAction = ValueOutput<StateAction>("Performance Action", (flow) => { UpdateStats(); return PerformanceAction; });
            thermalAction = ValueOutput<StateAction>("Thermal Action", (flow) => { UpdateStats(); return ThermalAction; });
            timeUntilNextAction = ValueOutput<float>("Time until next Action", (flow) => { UpdateStats(); return TimeUntilNextAction; });
        }

        void UpdateStats()
        {
            if (Application.isPlaying && Holder.Instance != null)
            {
                var indexer = Holder.Instance.Indexer;
                PerformanceAction = indexer.PerformanceAction;
                ThermalAction = indexer.ThermalAction;
                TimeUntilNextAction = indexer.TimeUntilNextAction;
            }
        }
    }
}
#endif
