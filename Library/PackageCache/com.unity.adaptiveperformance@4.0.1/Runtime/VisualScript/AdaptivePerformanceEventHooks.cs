#if VISUAL_SCRIPTING_ENABLED

namespace UnityEngine.AdaptivePerformance.VisualScripting
{
    /// <summary>
    /// Adaptive Performance Scaler Event
    /// </summary>
    public struct AdaptivePerformanceScalerEvent
    {
        /// <summary>
        /// Scaler triggered
        /// </summary>
        public string Name;
        /// <summary>
        /// Scaler level
        /// </summary>
        public int Level;
    }
    /// <summary>
    /// Adaptive Performance Event hooks for Visual Scripting
    /// </summary>
    public static class AdaptivePerformanceEventHooks
    {
        /// <summary>
        /// Thermal event triggered
        /// </summary>
        public const string OnThermalEvent = "OnThermalEvent";
        /// <summary>
        /// Bottleneck event triggered
        /// </summary>
        public const string OnBottleneckEvent = "OnBottleneckEvent";
        /// <summary>
        /// Bottleneck event triggered
        /// </summary>
        public const string OnBoostEvent = "OnBoostEvent";
        /// <summary>
        /// Performance Level event triggered
        /// </summary>
        public const string OnPerformanceLevelEvent = "OnPerformanceLevelEvent";
        /// <summary>
        /// Scaler event triggered
        /// </summary>
        public const string OnScalerEvent = "OnScalerEvent";
    }
}
#endif
