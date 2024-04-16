#if VISUAL_SCRIPTING_ENABLED

using System;
using Unity.VisualScripting;

namespace UnityEngine.AdaptivePerformance.VisualScripting
{
    [UnitShortTitle("Set Boost")]
    [UnitSubtitle("Set CPU and GPU Boost")]
    [UnitCategory("AdaptivePerformance/Performance")]
    public class SetBoostUnit : Unit
    {
        [DoNotSerialize]
        public ControlInput inputTrigger;

        [DoNotSerialize]
        public ControlOutput outputTrigger;

        [DoNotSerialize]
        public ValueInput cpuBoostInput;

        [DoNotSerialize]
        public ValueInput gpuBoostInput;

        protected override void Definition()
        {
            inputTrigger = ControlInput("inputTrigger", (flow) => {
                if (Application.isPlaying && Holder.Instance != null)
                {
                    var pc = Holder.Instance.DevicePerformanceControl;
                    var cpuInput = flow.GetValue<bool>(cpuBoostInput);
                    var gpuInput = flow.GetValue<bool>(gpuBoostInput);
                    if (cpuInput)
                        pc.CpuPerformanceBoost = cpuInput;
                    if (gpuInput)
                        pc.GpuPerformanceBoost = gpuInput;
                }

                return outputTrigger;
            });
            outputTrigger = ControlOutput("outputTrigger");

            cpuBoostInput = ValueInput<bool>("CPU Boost", false);
            gpuBoostInput = ValueInput<bool>("GPU Boost", false);
        }
    }
}
#endif
