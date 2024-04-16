using UnityEngine.Rendering;

namespace UnityEngine.AdaptivePerformance
{
    /// <summary>
    /// A scaler used by <see cref="AdaptivePerformanceIndexer"/> to toggle rendering of transparent objects.
    /// </summary>
    public class AdaptiveTransparency : AdaptivePerformanceScaler
    {
        /// <summary>
        /// Ensures settings are applied during startup.
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            if (m_Settings == null)
                return;
            ApplyDefaultSetting(m_Settings.scalerSettings.AdaptiveTransparency);
        }

        /// <summary>
        /// Callback when scaler gets disabled and removed from indexer
        /// </summary>
        protected override void OnDisabled()
        {
            OnDestroy();
        }

        void OnDestroy()
        {
            AdaptivePerformanceRenderSettings.SkipTransparentObjects = false;
        }

        /// <summary>
        /// Callback for any level change
        /// </summary>
        protected override void OnLevel()
        {
            if (ScaleChanged())
                AdaptivePerformanceRenderSettings.SkipTransparentObjects = (Scale < 1);
        }
    }
}
