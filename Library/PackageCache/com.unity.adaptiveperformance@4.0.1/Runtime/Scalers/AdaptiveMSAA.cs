namespace UnityEngine.AdaptivePerformance
{
    /// <summary>
    /// A scaler used by <see cref="AdaptivePerformanceIndexer"/> to adjust the level of antialiasing.
    /// </summary>
    public class AdaptiveMSAA : AdaptivePerformanceScaler
    {
        int m_DefaultAA;

        /// <summary>
        /// Ensures settings are applied during startup.
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            if (m_Settings == null)
                return;
            ApplyDefaultSetting(m_Settings.scalerSettings.AdaptiveMSAA);
        }

        /// <summary>
        /// Callback when scaler gets disabled and removed from indexer
        /// </summary>
        protected override void OnDisabled()
        {
            AdaptivePerformanceRenderSettings.AntiAliasingQualityBias = m_DefaultAA;
        }

        /// <summary>
        /// Callback when scaler gets enabled and added to the indexer
        /// </summary>
        protected override void OnEnabled()
        {
            m_DefaultAA = AdaptivePerformanceRenderSettings.AntiAliasingQualityBias;
        }

        /// <summary>
        /// Callback for any level change.
        /// </summary>
        protected override void OnLevel()
        {
            if (ScaleChanged())
                AdaptivePerformanceRenderSettings.AntiAliasingQualityBias = (int)(2 * Scale);
        }
    }
}
