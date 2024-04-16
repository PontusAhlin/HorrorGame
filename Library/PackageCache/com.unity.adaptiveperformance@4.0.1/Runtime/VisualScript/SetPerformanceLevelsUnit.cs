#if VISUAL_SCRIPTING_ENABLED

using System;
using Unity.VisualScripting;

namespace UnityEngine.AdaptivePerformance.VisualScripting
{
    [UnitShortTitle("Set Performance Level")]
    [UnitSubtitle("CPU and GPU Levels")]
    [UnitCategory("AdaptivePerformance/Performance")]
    public class SetPerformanceLevelsUnit : Unit
    {
        [DoNotSerialize]
        public ControlInput inputTrigger;

        [DoNotSerialize]
        public ControlOutput outputTrigger;

        [DoNotSerialize]
        public ValueInput cpuLevelInput;

        [DoNotSerialize]
        public ValueInput gpuLevelInput;

        protected override void Definition()
        {
            inputTrigger = ControlInput("inputTrigger", (flow) =>
            {
                if (Application.isPlaying && Holder.Instance != null)
                {
                    var pc = Holder.Instance.DevicePerformanceControl;
                    var cpuInput = flow.GetValue<int>(cpuLevelInput);
                    var gpuInput = flow.GetValue<int>(gpuLevelInput);
                    if (cpuInput >= 0)
                    {
                        pc.CpuLevel = cpuInput;
                    }
                    if (gpuInput >= 0)
                    {
                        pc.GpuLevel = gpuInput;
                    }
                }
                return outputTrigger;
            });
            outputTrigger = ControlOutput("outputTrigger");
            cpuLevelInput = ValueInput<int>("CPU Level", -1);
            gpuLevelInput = ValueInput<int>("GPU Level", -1);
        }
    }
}
#endif
