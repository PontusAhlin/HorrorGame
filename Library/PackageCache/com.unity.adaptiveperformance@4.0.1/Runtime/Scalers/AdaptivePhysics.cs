namespace UnityEngine.AdaptivePerformance
{
    /// <summary>
    /// A scaler used by <see cref="AdaptivePerformanceIndexer"/> for adjusting how physics is applied.
    /// </summary>
    public class AdaptivePhysics : AdaptivePerformanceScaler
    {
        float m_fixedDeltaTimeDefault;
        /// <summary>
        /// Ensures settings are applied during startup.
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            if (m_Settings == null)
                return;
            ApplyDefaultSetting(m_Settings.scalerSettings.AdaptivePhysics);
        }

        /// <summary>
        /// Callback when scaler gets disabled and removed from indexer
        /// </summary>
        protected override void OnDisabled()
        {
            Time.fixedDeltaTime = m_fixedDeltaTimeDefault;
        }

        /// <summary>
        /// Callback when scaler gets enabled and added to the indexer
        /// </summary>
        protected override void OnEnabled()
        {
            m_fixedDeltaTimeDefault = Time.fixedDeltaTime;
        }

        /// <summary>
        /// Callback for any level change.
        /// </summary>
        protected override void OnLevel()
        {
            if (ScaleChanged())
            {
                Time.fixedDeltaTime = m_fixedDeltaTimeDefault / Scale;
            }
        }
    }
}
