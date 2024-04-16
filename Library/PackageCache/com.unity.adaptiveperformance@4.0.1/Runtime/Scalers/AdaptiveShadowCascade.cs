namespace UnityEngine.AdaptivePerformance
{
    /// <summary>
    /// A scaler used by <see cref="AdaptivePerformanceIndexer"/> to adjust the number of shadow cascades to be used.
    /// </summary>
    public class AdaptiveShadowCascade : AdaptivePerformanceScaler
    {
        int m_DefaultCascadeCount;

        /// <summary>
        /// Ensures settings are applied during startup.
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            if (m_Settings == null)
                return;
            ApplyDefaultSetting(m_Settings.scalerSettings.AdaptiveShadowCascade);
        }

        /// <summary>
        /// Callback when scaler gets disabled and removed from indexer
        /// </summary>
        protected override void OnDisabled()
        {
            AdaptivePerformanceRenderSettings.MainLightShadowCascadesCountBias = m_DefaultCascadeCount;
        }

        /// <summary>
        /// Callback when scaler gets enabled and added to the indexer
        /// </summary>
        protected override void OnEnabled()
        {
            m_DefaultCascadeCount = AdaptivePerformanceRenderSettings.MainLightShadowCascadesCountBias;
        }

        /// <summary>
        /// Callback for any level change.
        /// </summary>
        protected override void OnLevel()
        {
            if (ScaleChanged())
                AdaptivePerformanceRenderSettings.MainLightShadowCascadesCountBias = (int)(2 * Scale);
        }
    }
}
