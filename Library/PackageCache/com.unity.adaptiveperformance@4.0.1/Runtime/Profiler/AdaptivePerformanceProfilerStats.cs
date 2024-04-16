using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Profiling;
using UnityEngine.Profiling;
using UnityEngine;

/// <summary>
/// Profiler Stats reporting helper class. Stores all adaptive performance counters and helper functions.
/// </summary>
public static class AdaptivePerformanceProfilerStats
{
    /// <summary>
    /// Profiler Category is set to scripts for Adaptive Performance.
    /// </summary>
    public static readonly ProfilerCategory AdaptivePerformanceProfilerCategory = ProfilerCategory.Scripts;

    /// <summary>
    /// Profiler counter to report cpu frametime.
    /// </summary>
    public static ProfilerCounter<float> CurrentCPUCounter = new ProfilerCounter<float>(AdaptivePerformanceProfilerCategory, "CPU frametime", ProfilerMarkerDataUnit.TimeNanoseconds);
    /// <summary>
    /// Profiler counter to report cpu average frametime.
    /// </summary>
    public static ProfilerCounter<float> AvgCPUCounter = new ProfilerCounter<float>(AdaptivePerformanceProfilerCategory, "CPU avg frametime", ProfilerMarkerDataUnit.TimeNanoseconds);
    /// <summary>
    /// Profiler counter to report gpu frametime.
    /// </summary>
    public static ProfilerCounter<float> CurrentGPUCounter = new ProfilerCounter<float>(AdaptivePerformanceProfilerCategory, "GPU frametime", ProfilerMarkerDataUnit.TimeNanoseconds);
    /// <summary>
    /// Profiler counter to report gpu average frametime.
    /// </summary>
    public static ProfilerCounter<float> AvgGPUCounter = new ProfilerCounter<float>(AdaptivePerformanceProfilerCategory, "GPU avg frametime", ProfilerMarkerDataUnit.TimeNanoseconds);
    /// <summary>
    /// Profiler counter to report cpu performance level.
    /// </summary>
    public static ProfilerCounter<int> CurrentCPULevelCounter = new ProfilerCounter<int>(AdaptivePerformanceProfilerCategory, "CPU performance level", ProfilerMarkerDataUnit.Count);
    /// <summary>
    /// Profiler counter to report gpu performance level.
    /// </summary>
    public static ProfilerCounter<int> CurrentGPULevelCounter = new ProfilerCounter<int>(AdaptivePerformanceProfilerCategory, "GPU performance level", ProfilerMarkerDataUnit.Count);
    /// <summary>
    /// Profiler counter to report frametime.
    /// </summary>
    public static ProfilerCounter<float> CurrentFrametimeCounter = new ProfilerCounter<float>(AdaptivePerformanceProfilerCategory, "Frametime", ProfilerMarkerDataUnit.TimeNanoseconds);
    /// <summary>
    /// Profiler counter to report average frametime.
    /// </summary>
    public static ProfilerCounter<float> AvgFrametimeCounter = new ProfilerCounter<float>(AdaptivePerformanceProfilerCategory, "Avg frametime", ProfilerMarkerDataUnit.TimeNanoseconds);
    /// <summary>
    /// Profiler counter to report the thermal warning level.
    /// </summary>
    public static ProfilerCounter<int> WarningLevelCounter = new ProfilerCounter<int>(AdaptivePerformanceProfilerCategory, "Thermal Warning Level", ProfilerMarkerDataUnit.Count);
    /// <summary>
    /// Profiler counter to report the temperature level.
    /// </summary>
    public static ProfilerCounter<float> TemperatureLevelCounter = new ProfilerCounter<float>(AdaptivePerformanceProfilerCategory, "Temperature Level", ProfilerMarkerDataUnit.Count);
    /// <summary>
    /// Profiler counter to report the temperature trend.
    /// </summary>
    public static ProfilerCounter<float> TemperatureTrendCounter = new ProfilerCounter<float>(AdaptivePerformanceProfilerCategory, "Temperature Trend", ProfilerMarkerDataUnit.Count);
    /// <summary>
    /// Profiler counter to report the bottleneck.
    /// </summary>
    public static ProfilerCounter<int> BottleneckCounter = new ProfilerCounter<int>(AdaptivePerformanceProfilerCategory, "Bottleneck", ProfilerMarkerDataUnit.Count);

    /// <summary>
    /// GUID for the Adaptive Performance Profile Module definition.
    /// </summary>
    public static readonly Guid kAdaptivePerformanceProfilerModuleGuid = new Guid("42c5aeb7-fb77-4172-a384-34063f1bd332");

    /// <summary>
    /// The Scaler data tag defines a tag for the scalers to send them via the emit frame data function.
    /// </summary>
    public static readonly int kScalerDataTag = 0;

    static Dictionary<string, ScalerInfo> scalerInfos = new Dictionary<string, ScalerInfo>();

    /// <summary>
    /// ScalerInfo is a struct used to collect and send scaler info to the profile collectively.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct ScalerInfo
    {
        /// <summary>
        /// The name of the scaler. 320 characters max.
        /// </summary>
        public fixed byte scalerName[320];
        /// <summary>
        /// If the scaler is currently enabled.
        /// </summary>
        public uint enabled;
        /// <summary>
        /// The override state of the scaler.
        /// </summary>
        public int overrideLevel;
        /// <summary>
        /// The current level of the scaler.
        /// </summary>
        public int currentLevel;
        /// <summary>
        /// The maximum level of the scaler.
        /// </summary>
        public int maxLevel;
        /// <summary>
        /// The actual scale of the scaler.
        /// </summary>
        public float scale;
        /// <summary>
        /// State if the scaler is currently applied.
        /// </summary>
        public uint applied;
    }

    /// <summary>
    /// Adaptive Performance sends scaler data to the profiler each frame. It is collected from multiple places with this method and flushed once with <see cref="FlushScalerDataToProfilerStream"/>.
    /// </summary>
    /// <param name="scalerName"> The name of the scaler. 320 characters max. </param>
    /// <param name="enabled"> If the scaler is currently enabled.</param>
    /// <param name="overrideLevel">The override state of the scaler.</param>
    /// <param name="currentLevel">The current level of the scaler.</param>
    /// <param name="scale">The actual scale of the scaler.</param>
    /// <param name="applied">If the scaler is currently applied.</param>
    /// <param name="maxLevel">The maximum level of the scaler.</param>
    [Conditional("ENABLE_PROFILER")]
    public static void EmitScalerDataToProfilerStream(string scalerName, bool enabled, int overrideLevel, int currentLevel, float scale, bool applied, int maxLevel)
    {
        if (!Profiler.enabled || scalerName.Length == 0)
            return;
        ScalerInfo scalerInfo;
        bool existingInfo = scalerInfos.TryGetValue(scalerName, out scalerInfo);

        if (!existingInfo)
            scalerInfo = new ScalerInfo();

        byte[] scalerNameBytes = Encoding.ASCII.GetBytes(scalerName);
        scalerInfo.enabled = (uint)(enabled ? 1 : 0);
        scalerInfo.overrideLevel = overrideLevel;
        scalerInfo.currentLevel = currentLevel;
        scalerInfo.scale = scale;
        scalerInfo.maxLevel = maxLevel;
        scalerInfo.applied = (uint)(applied ? 1 : 0);

        unsafe
        {
            fixed(byte* pSource = scalerNameBytes)
            {
                UnsafeUtility.MemCpy(scalerInfo.scalerName, pSource, scalerNameBytes.Length);
            }
        }

        if (!existingInfo)
            scalerInfos.Add(scalerName, scalerInfo);
        else
            scalerInfos[scalerName] = scalerInfo;
    }

    /// <summary>
    /// Flushes the Adaptive Performance scaler data for this frame. Used in conjunction with <see cref="EmitScalerDataToProfilerStream"/>.
    /// </summary>
    public static void FlushScalerDataToProfilerStream()
    {
        ScalerInfo[] arr = new ScalerInfo[scalerInfos.Count];
        scalerInfos.Values.CopyTo(arr, 0);
        Profiler.EmitFrameMetaData(kAdaptivePerformanceProfilerModuleGuid, kScalerDataTag, arr);
    }
}
