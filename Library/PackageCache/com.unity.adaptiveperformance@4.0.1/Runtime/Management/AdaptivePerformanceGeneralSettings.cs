using System;
using System.Collections;

using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UnityEngine.AdaptivePerformance
{
    /// <summary>
    /// General settings container used to house the instance of the active settings, as well as the manager
    /// instance used to load the loaders with.
    /// </summary>
    public class AdaptivePerformanceGeneralSettings : ScriptableObject
    {
        /// <summary>The key used to query to get the current loader settings.</summary>
        public static string k_SettingsKey = "com.unity.adaptiveperformance.loader_settings";
        internal static AdaptivePerformanceGeneralSettings s_RuntimeSettingsInstance = null;

        [SerializeField]
        internal AdaptivePerformanceManagerSettings m_LoaderManagerInstance = null;

        [SerializeField]
        [Tooltip("Enable this to automatically start up Adaptive Performance at runtime.")]
        internal bool m_InitManagerOnStart = true;

        /// <summary>The current active manager used to manage the Adaptive Performance lifetime.</summary>
        public AdaptivePerformanceManagerSettings Manager
        {
            get { return m_LoaderManagerInstance; }
            set { m_LoaderManagerInstance = value; }
        }

        private AdaptivePerformanceManagerSettings m_AdaptivePerformanceManager = null;

#pragma warning disable 414 // Suppress warning for needed variables.
        private bool m_ProviderIntialized = false;
        private bool m_ProviderStarted = false;
#pragma warning restore 414

        /// <summary>The current settings instance.</summary>
        public static AdaptivePerformanceGeneralSettings Instance
        {
            get
            {
                return s_RuntimeSettingsInstance;
            }
#if UNITY_EDITOR
            set
            {
                s_RuntimeSettingsInstance = value;
            }
#endif
        }

        /// <summary>
        /// The current active manager used to manage the Adaptive Performance lifetime.
        /// </summary>
        public AdaptivePerformanceManagerSettings AssignedSettings
        {
            get
            {
                return m_LoaderManagerInstance;
            }
#if UNITY_EDITOR
            set
            {
                m_LoaderManagerInstance = value;
            }
#endif
        }

        /// <summary>
        /// Used to set if the manager is activated and initialized on startup.
        /// </summary>
        public bool InitManagerOnStart
        {
            get
            {
                return m_InitManagerOnStart;
            }
            #if UNITY_EDITOR
            set
            {
                m_InitManagerOnStart = value;
            }
            #endif
        }


#if !UNITY_EDITOR
        void Awake()
        {
            s_RuntimeSettingsInstance = this;
            Application.quitting += Quit;
            DontDestroyOnLoad(s_RuntimeSettingsInstance);
        }

#endif

#if UNITY_EDITOR
        /// <summary>For internal use only.</summary>
        public void InternalPlayModeStateChanged(PlayModeStateChange state)
        {
            switch (state)
            {
                case PlayModeStateChange.ExitingPlayMode:
                    Quit();
                    break;
                case PlayModeStateChange.ExitingEditMode:
                case PlayModeStateChange.EnteredPlayMode:
                case PlayModeStateChange.EnteredEditMode:
                    break;
            }
        }

#endif
        static void Quit()
        {
            AdaptivePerformanceGeneralSettings instance = AdaptivePerformanceGeneralSettings.Instance;
            if (instance == null)
                return;

            instance.DeInitAdaptivePerformance();
        }

        void Start()
        {
            StartAdaptivePerformance();
        }

        void OnDestroy()
        {
            DeInitAdaptivePerformance();
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        internal static void AttemptInitializeAdaptivePerformanceOnLoad()
        {
            AdaptivePerformanceGeneralSettings instance = AdaptivePerformanceGeneralSettings.Instance;
            if (instance == null || !instance.InitManagerOnStart)
                return;

            instance.InitAdaptivePerformance();
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        internal static void AttemptStartAdaptivePerformanceOnBeforeSplashScreen()
        {
            AdaptivePerformanceGeneralSettings instance = AdaptivePerformanceGeneralSettings.Instance;
            if (instance == null || !instance.InitManagerOnStart)
                return;

            instance.StartAdaptivePerformance();
        }

        private void InitAdaptivePerformance()
        {
            if (AdaptivePerformanceGeneralSettings.Instance == null || AdaptivePerformanceGeneralSettings.Instance.m_LoaderManagerInstance == null || AdaptivePerformanceGeneralSettings.Instance.m_InitManagerOnStart == false)
                return;

            m_AdaptivePerformanceManager = AdaptivePerformanceGeneralSettings.Instance.m_LoaderManagerInstance;
            if (m_AdaptivePerformanceManager == null)
            {
                Debug.LogError("Assigned GameObject for Adaptive Performance Management loading is invalid. No Adaptive Performance Providers will be automatically loaded.");
                return;
            }

            m_AdaptivePerformanceManager.automaticLoading = false;
            m_AdaptivePerformanceManager.automaticRunning = false;
            m_AdaptivePerformanceManager.InitializeLoaderSync();
            m_ProviderIntialized = true;
        }

        private void StartAdaptivePerformance()
        {
            if (m_AdaptivePerformanceManager != null && m_AdaptivePerformanceManager.activeLoader != null)
            {
                m_AdaptivePerformanceManager.StartSubsystems();
                m_ProviderStarted = true;
            }
        }

        private void StopAdaptivePerformance()
        {
            if (m_AdaptivePerformanceManager != null && m_AdaptivePerformanceManager.activeLoader != null)
            {
                m_AdaptivePerformanceManager.StopSubsystems();
                m_ProviderStarted = false;
            }
        }

        private void DeInitAdaptivePerformance()
        {
            if (m_AdaptivePerformanceManager != null && m_AdaptivePerformanceManager.activeLoader != null)
            {
                m_AdaptivePerformanceManager.DeinitializeLoader();
                m_AdaptivePerformanceManager = null;
                m_ProviderIntialized = false;
            }
        }
    }
}
