namespace UnityEngine.AdaptivePerformance
{
    /// <summary>
    /// A scaler used by <see cref="AdaptivePerformanceIndexer"/> for adjusting build-in fog.
    /// </summary>
    public class AdaptiveFog : AdaptivePerformanceScaler
    {
        AdaptivePerformanceScalerSettingsBase m_AdaptiveFog = new AdaptivePerformanceScalerSettingsBase
        {
            name = "Adaptive Fog",
            enabled = false,
            scale = 1.0f,
            visualImpact = ScalerVisualImpact.Medium,
            target = ScalerTarget.GPU,
            minBound = 0,
            maxBound = 1,
            maxLevel = 10
        };

        /// <summary>
        /// A scaler setting used by <see cref="AdaptivePerformanceIndexer"/> to adjust the application update rate using <see cref="Application.targetFrameRate"/>.
        /// </summary>
        public AdaptivePerformanceScalerSettingsBase AdaptiveFogSetting
        {
            get { return m_AdaptiveFog; }
            set { m_AdaptiveFog = value; }
        }

        bool m_DefaultFog;
        float m_DefaultFarClipPlane = -1;
        float m_DefaultDensity = 0;

        /// <summary>
        /// Ensures settings are applied during startup.
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            ApplyDefaultSetting(AdaptiveFogSetting);
        }

        /// <summary>
        /// Callback when scaler gets disabled and removed from indexer
        /// </summary>
        protected override void OnDisabled()
        {
            RenderSettings.fogDensity = m_DefaultDensity;
            RenderSettings.fog = m_DefaultFog;
            if (!Camera.main || m_DefaultFarClipPlane == -1)
                return;

            Camera.main.farClipPlane = m_DefaultFarClipPlane;
        }

        /// <summary>
        /// Callback when scaler gets enabled and added to the indexer
        /// </summary>
        protected override void OnEnabled()
        {
            m_DefaultFog = RenderSettings.fog;
            m_DefaultDensity = RenderSettings.fogDensity;
            if (!Camera.main)
                return;

            m_DefaultFarClipPlane = Camera.main.farClipPlane;
        }

        /// <summary>
        /// Callback for any level change.
        /// </summary>
        protected override void OnLevel()
        {
            if (ScaleChanged())
            {
                Debug.Log(Scale);

                if (RenderSettings.fogMode == FogMode.Linear)
                {
                    if (!Camera.main || Scale == 0)
                        return;

                    if (m_DefaultFarClipPlane == -1)
                        m_DefaultFarClipPlane = Camera.main.farClipPlane;

                    Camera.main.farClipPlane = Scale * 1000;
                    RenderSettings.fogEndDistance = Camera.main.farClipPlane;
                }
                else
                {
                    RenderSettings.fog = Scale == 0 ? false : true;
                    RenderSettings.fogDensity = m_DefaultDensity * Scale;
                }
            }
        }
    }
}
