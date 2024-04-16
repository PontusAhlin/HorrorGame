using NUnit.Framework;

using System;
using System.IO;

using UnityEngine;
using UnityEngine.AdaptivePerformance;


namespace UnityEditor.AdaptivePerformance.Editor.Tests
{
    class AdaptivePerformanceGeneralSettingsTests
    {
        internal static readonly string[] s_TempSettingsPath = {"Temp", "Test", "Settings"};

        string testPathToSettings;

        UnityEngine.Object currentSettings = null;

        AdaptivePerformanceManagerSettings testManager = null;
        AdaptivePerformanceGeneralSettings testSettings = null;

        [SetUp]
        public void SetupTest()
        {
            testManager = ScriptableObject.CreateInstance<AdaptivePerformanceManagerSettings>();

            testSettings = ScriptableObject.CreateInstance<AdaptivePerformanceGeneralSettings>() as AdaptivePerformanceGeneralSettings;
            testSettings.Manager = testManager;

            testPathToSettings = AdaptivePerformanceGeneralSettingsTests.GetAssetPathForComponents(AdaptivePerformanceGeneralSettingsTests.s_TempSettingsPath);
            if (!string.IsNullOrEmpty(testPathToSettings))
            {
                testPathToSettings = Path.Combine(testPathToSettings, "Test_AdaptivePerformanceGeneralSettings.asset");
                AssetDatabase.CreateAsset(testSettings, testPathToSettings);
                AssetDatabase.SaveAssets();
            }

            EditorBuildSettings.TryGetConfigObject(AdaptivePerformanceGeneralSettings.k_SettingsKey, out currentSettings);
            EditorBuildSettings.AddConfigObject(AdaptivePerformanceGeneralSettings.k_SettingsKey, testSettings, true);
        }

        [TearDown]
        public void TearDownTest()
        {
            EditorBuildSettings.RemoveConfigObject(AdaptivePerformanceGeneralSettings.k_SettingsKey);

            if (!string.IsNullOrEmpty(testPathToSettings))
            {
                AssetDatabase.DeleteAsset(testPathToSettings);
            }

            testSettings.Manager = null;
            UnityEngine.Object.DestroyImmediate(testSettings);
            testSettings = null;

            UnityEngine.Object.DestroyImmediate(testManager);
            testManager = null;

            if (currentSettings != null)
                EditorBuildSettings.AddConfigObject(AdaptivePerformanceGeneralSettings.k_SettingsKey, currentSettings, true);

            AssetDatabase.DeleteAsset(Path.Combine("Assets", "Temp"));
        }

        internal static string GetAssetPathForComponents(string[] pathComponents, string root = "Assets")
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
