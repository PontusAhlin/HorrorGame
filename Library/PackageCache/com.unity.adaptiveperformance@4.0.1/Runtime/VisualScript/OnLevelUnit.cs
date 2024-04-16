#if VISUAL_SCRIPTING_ENABLED

using Unity.VisualScripting;

namespace UnityEngine.AdaptivePerformance.VisualScripting
{
    [UnitShortTitle("On Level Scaler")]
    [UnitSubtitle("Scaler level")]
    [UnitCategory("AdaptivePerformance/Scaler")]
    public class OnLevelUnit : EventUnit<AdaptivePerformanceScalerEvent>
    {
        [DoNotSerialize]
        public ValueOutput onScalerLevel { get; private set; }

        [DoNotSerialize]
        public ValueOutput scalerLevel { get; private set; }

        protected override bool register => true;

        public override EventHook GetHook(GraphReference reference)
        {
            return new EventHook(AdaptivePerformanceEventHooks.OnScalerEvent);
        }

        protected override void AssignArguments(Flow flow, AdaptivePerformanceScalerEvent scalerEvent)
        {
            flow.SetValue(onScalerLevel, scalerEvent.Name);
            flow.SetValue(scalerLevel, scalerEvent.Level);
        }

        protected override void Definition()
        {
            base.Definition();
            onScalerLevel = ValueOutput<string>("Scaler Name", (flow) => "");
            scalerLevel = ValueOutput<int>("Scaler Level", (flow) => - 1);
        }
    }
}
#endif
