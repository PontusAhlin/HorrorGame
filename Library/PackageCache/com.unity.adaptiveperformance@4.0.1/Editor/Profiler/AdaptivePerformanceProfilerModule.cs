using System;
using Unity.Profiling;
using Unity.Profiling.Editor;

namespace UnityEditor.AdaptivePerformance.Editor
{
    [Serializable]
    [ProfilerModuleMetadata("Adaptive Performance")]
    internal class AdaptivePerformanceProfilerModule : ProfilerModule
    {
        static readonly ProfilerCounterDescriptor[] k_ChartCounters = new ProfilerCounterDescriptor[]
        {
            new ProfilerCounterDescriptor("CPU frametime", ProfilerCategory.Scripts),
            new ProfilerCounterDescriptor("GPU frametime", ProfilerCategory.Scripts),
            new ProfilerCounterDescriptor("CPU performance level", ProfilerCategory.Scripts),
            new ProfilerCounterDescriptor("GPU performance level", ProfilerCategory.Scripts),
            new ProfilerCounterDescriptor("Frametime", ProfilerCategory.Scripts),
            new ProfilerCounterDescriptor("Temperature Level", ProfilerCategory.Scripts),
            new ProfilerCounterDescriptor("Temperature Trend", ProfilerCategory.Scripts),
        };

        // Specify a list of Profiler category names, which should be auto-enabled when the module is active.
        static readonly string[] k_AutoEnabledCategoryNames = new string[]
        {
            AdaptivePerformanceProfilerStats.AdaptivePerformanceProfilerCategory.Name,
        };

        public override ProfilerModuleViewController CreateDetailsViewController()
        {
            return new AdaptivePerformanceDetailsViewController(ProfilerWindow);
        }

        public AdaptivePerformanceProfilerModule() : base(k_ChartCounters, ProfilerModuleChartType.Line, k_AutoEnabledCategoryNames) {}
    }
}
