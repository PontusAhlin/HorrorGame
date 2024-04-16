using System;
using System.IO;
using UnityEngine;

using UnityEditor;
using UnityEngine.AdaptivePerformance;
using UnityEditor.AdaptivePerformance.Simulator.Editor;
using UnityEditor.AdaptivePerformance.Editor;
using UnityEditor.AdaptivePerformance.Editor.Metadata;

namespace UnityEditor.AdaptivePerformance.Tests
{
    public abstract class ManagementTestSetup
    {
        protected static readonly string[] s_TestGeneralSettings = { "Temp", "Test" };
        protected static readonly string[] s_TempSettingsPath = { "Temp", "Test", "Settings" };

        static protected string testPathToGeneralSettings;
        static protected string testPathToSettings;
        static protected string testPathToLoader;

        static private UnityEngine.Object currentSettings = null;

        static protected AdaptivePerformanceManagerSettings testManager = null;
        static protected AdaptivePerformanceGeneralSettings adaptivePerformanceGeneralSettings = null;
        static protected AdaptivePerformanceGeneralSettingsPerBuildTarget buildTargetSettings = null;
        static protected SimulatorProviderLoader loader = null;
        static protected SimulatorProviderSettings settings = null;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        public static void SetupTest()
        {
            testManager = ScriptableObject.CreateInstance<AdaptivePerformanceManagerSettings>();

            adaptivePerformanceGeneralSettings = ScriptableObject.CreateInstance<AdaptivePerformanceGeneralSettings>() as AdaptivePerformanceGeneralSettings;
            adaptivePerformanceGeneralSettings.Manager = testManager;


            testPathToSettings = GetAssetPathForComponents(s_TempSettingsPath);
            testPathToSettings = Path.Combine(testPathToSettings, $"Test_{ typeof(AdaptivePerformanceGeneralSettings).Name}.asset");
            if (!string.IsNullOrEmpty(testPathToSettings))
            {
                AssetDatabase.CreateAsset(adaptivePerformanceGeneralSettings, testPathToSettings);
                AssetDatabase.AddObjectToAsset(testManager, adaptivePerformanceGeneralSettings);

                AssetDatabase.SaveAssets();
            }

            testPathToGeneralSettings = GetAssetPathForComponents(s_TestGeneralSettings);
            testPathToGeneralSettings = Path.Combine(testPathToGeneralSettings, $"Test_{typeof(AdaptivePerformanceGeneralSettingsPerBuildTarget).Name}.asset");

            buildTargetSettings = ScriptableObject.CreateInstance<AdaptivePerformanceGeneralSettingsPerBuildTarget>();
            buildTargetSettings.SetSettingsForBuildTarget(BuildTargetGroup.Standalone, adaptivePerformanceGeneralSettings);
            if (!string.IsNullOrEmpty(testPathToSettings))
            {
                AssetDatabase.CreateAsset(buildTargetSettings, testPathToGeneralSettings);
                AssetDatabase.SaveAssets();

                EditorBuildSettings.TryGetConfigObject(AdaptivePerformanceGeneralSettings.k_SettingsKey, out currentSettings);
                EditorBuildSettings.AddConfigObject(AdaptivePerformanceGeneralSettings.k_SettingsKey, buildTargetSettings, true);
            }

            testPathToLoader = GetAssetPathForComponents(s_TempSettingsPath);
            // Setup Loader
            loader = ScriptableObject.CreateInstance(typeof(SimulatorProviderLoader)) as SimulatorProviderLoader;
            AssetDatabase.CreateAsset(loader, Path.Combine(testPathToLoader, $"Test_{typeof(SimulatorProviderLoader).Name}.asset"));
            testManager.loaders.Add(loader);

            // Setup Settings
            settings = ScriptableObject.CreateInstance(typeof(SimulatorProviderSettings)) as SimulatorProviderSettings;
            AssetDatabase.CreateAsset(settings, Path.Combine(testPathToLoader, $"Test_{typeof(SimulatorProviderSettings).Name}.asset"));
            //settings.logging = false;
            EditorBuildSettings.AddConfigObject(SimulatorProviderConstants.k_SettingsKey, settings, true);


            // Due to the Settings menu, we have to manually assigned the Simulator loader in tests.
            AdaptivePerformancePackageMetadataStore.AssignLoader(AdaptivePerformanceGeneralSettings.Instance.Manager, typeof(SimulatorProviderLoader).Name, BuildTargetGroup.Standalone);
        }

        public virtual void TearDownTest()
        {
            testManager.DeinitializeLoader();
            adaptivePerformanceGeneralSettings.Manager.loaders.Remove(loader);
            loader = null;

            EditorBuildSettings.RemoveConfigObject(AdaptivePerformanceGeneralSettings.k_SettingsKey);

            if (!string.IsNullOrEmpty(testPathToGeneralSettings))
            {
                AssetDatabase.DeleteAsset(testPathToGeneralSettings);
            }

            if (!string.IsNullOrEmpty(testPathToSettings))
            {
                AssetDatabase.DeleteAsset(testPathToSettings);
            }

            adaptivePerformanceGeneralSettings.Manager = null;
            UnityEngine.Object.DestroyImmediate(adaptivePerformanceGeneralSettings);
            adaptivePerformanceGeneralSettings = null;

            UnityEngine.Object.DestroyImmediate(testManager);
            testManager = null;

            UnityEngine.Object.DestroyImmediate(buildTargetSettings);
            buildTargetSettings = null;

            if (currentSettings != null)
                EditorBuildSettings.AddConfigObject(AdaptivePerformanceGeneralSettings.k_SettingsKey, currentSettings, true);
            else
                EditorBuildSettings.RemoveConfigObject(AdaptivePerformanceGeneralSettings.k_SettingsKey);

            AssetDatabase.DeleteAsset(Path.Combine("Assets", "Temp"));
        }

        public static string GetAssetPathForComponents(string[] pathComponents, string root = "Assets")
        {
            if (pathComponents.Length <= 0)
                return null;

            string path = root;
            foreach (var pc in pathComponents)
            {
                string subFolder = Path.Combine(path, pc);
                bool shouldCreate = true;
                foreach (var f in AssetDatabase.GetSubFolders(path))
                {
                    if (String.Compare(Path.GetFullPath(f), Path.GetFullPath(subFolder), true) == 0)
                    {
                        shouldCreate = false;
                        break;
                    }
                }

                if (shouldCreate)
                    AssetDatabase.CreateFolder(path, pc);
                path = subFolder;
            }

            return path;
        }
    }
}
