#if VISUAL_SCRIPTING_ENABLED

using System;
using Unity.VisualScripting;

namespace UnityEngine.AdaptivePerformance.VisualScripting
{
    [UnitShortTitle("On Bottleneck")]
    [UnitSubtitle("CPU, GPU, TargetFrameRate bottleneck")]
    [UnitCategory("AdaptivePerformance/Performance")]
    public class OnBottleneckUnit : EventUnit<PerformanceBottleneck>
    {
        [DoNotSerialize]
        public ValueOutput bottleneck { get; private set; }

        [DoNotSerialize]
        public ValueOutput cpu;

        [DoNotSerialize]
        public ValueOutput gpu;

        [DoNotSerialize]
        public ValueOutput targetFrameRate;

        protected override bool register => true;

        public override EventHook GetHook(GraphReference reference)
        {
            return new EventHook(AdaptivePerformanceEventHooks.OnBottleneckEvent);
        }

        protected override void AssignArguments(Flow flow, PerformanceBottleneck data)
        {
            flow.SetValue(bottleneck, data.ToString());
            flow.SetValue(cpu, data == PerformanceBottleneck.CPU);
            flow.SetValue(gpu, data == PerformanceBottleneck.GPU);
            flow.SetValue(targetFrameRate, data == PerformanceBottleneck.TargetFrameRate);
        }

        protected override void Definition()
        {
            base.Definition();
            bottleneck = ValueOutput<String>(nameof(bottleneck), (flow) => "Unknown");
            cpu = ValueOutput<bool>("Cpu", (flow) => false);
            gpu = ValueOutput<bool>("Gpu", (flow) => false);
            targetFrameRate = ValueOutput<bool>("TargetFrameRate", (flow) => false);
        }
    }
}
#endif
