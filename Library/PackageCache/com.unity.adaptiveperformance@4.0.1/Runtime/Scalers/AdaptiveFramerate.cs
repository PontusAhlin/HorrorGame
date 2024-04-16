namespace UnityEngine.AdaptivePerformance
{
    /// <summary>
    /// A scaler used by <see cref="AdaptivePerformanceIndexer"/> to adjust the application update rate using <see cref="Application.targetFrameRate"/>.
    /// </summary>
    public class AdaptiveFramerate : AdaptivePerformanceScaler
    {
        int m_DefaultFPS;

        /// <summary>
        /// Ensures settings are applied during startup.
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            if (m_Settings == null)
                return;
            ApplyDefaultSetting(m_Settings.scalerSettings.AdaptiveFramerate);
        }

        /// <summary>
        /// Callback when scaler gets disabled and removed from indexer
        /// </summary>
        protected override void OnDisabled()
        {
            Application.targetFrameRate = m_DefaultFPS;
        }

        /// <summary>
        /// Callback when scaler gets enabled and added to the indexer
        /// </summary>
        protected override void OnEnabled()
        {
            m_DefaultFPS = Application.targetFrameRate;
            Application.targetFrameRate = (int)MaxBound;
        }

        /// <summary>
        /// Callback for when the quality level is decreased/scaler level increased.
        /// </summary>
        protected override void OnLevelIncrease()
        {
            base.OnLevelIncrease();

            var framerateDecrease = 1;

            if (Holder.Instance.Indexer.PerformanceAction == StateAction.FastDecrease)
                framerateDecrease = 5;

            var fps = Application.targetFrameRate - framerateDecrease;

            if (fps >= MinBound && fps <= MaxBound)
                Application.targetFrameRate = fps;
        }

        /// <summary>
        /// Callback for when the quality level is increased/scaler level decreased.
        /// </summary>
        protected override void OnLevelDecrease()
        {
            base.OnLevelDecrease();

            var fps = Application.targetFrameRate + 5;
            if (fps >= MinBound && fps <= MaxBound)
                Application.targetFrameRate = fps;
        }
    }
}
