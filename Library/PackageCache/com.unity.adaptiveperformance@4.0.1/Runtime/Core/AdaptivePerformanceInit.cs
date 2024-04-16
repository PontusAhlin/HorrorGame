using System;
using System.Reflection;

namespace UnityEngine.AdaptivePerformance
{
    internal class AdaptivePerformanceManagerSpawner : ScriptableObject
    {
        public GameObject m_ManagerGameObject;

        void OnEnable()
        {
            if (m_ManagerGameObject == null)
            {
                m_ManagerGameObject = new GameObject("AdaptivePerformanceManager");
                var ap = m_ManagerGameObject.AddComponent<AdaptivePerformanceManager>();

                // if no provider was found we can disable AP and destroy the game object, otherwise continue with initialization.
                if (ap.Indexer == null)
                {
                    Destroy(m_ManagerGameObject);
                    return;
                }

                Holder.Instance = ap;
                InstallScalers();
                DontDestroyOnLoad(m_ManagerGameObject);

                var settings = ap.Settings;
                var scalerProfiles = settings.GetAvailableScalerProfiles();
                if (scalerProfiles.Length <= 0)
                {
                    APLog.Debug("No Scaler Profiles available. Did you remove all profiles manually from the provider Settings?");
                    return;
                }
                settings.LoadScalerProfile(scalerProfiles[settings.defaultScalerProfilerIndex]);
            }
        }

        void InstallScalers()
        {
            Type ti = typeof(AdaptivePerformanceScaler);
            foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type t in asm.GetTypes())
                {
                    if (ti.IsAssignableFrom(t) && !t.IsAbstract)
                    {
                        ScriptableObject.CreateInstance(t);
                    }
                }
            }
        }
    }

    internal static class AdaptivePerformanceInitializer
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Initialize()
        {
            ScriptableObject.CreateInstance<AdaptivePerformanceManagerSpawner>();
        }
    }
}
