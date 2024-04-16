using NUnit.Framework;

using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.AdaptivePerformance;
using UnityEditor.AdaptivePerformance.Editor.Metadata;

using UnityEngine.AdaptivePerformance.TestPackage;
using UnityEditor.AdaptivePerformance.TestPackage.Editor;

namespace UnityEditor.AdaptivePerformance.Editor.Tests
{
    class PackageManagementTests
    {
        internal static readonly string[] s_TempSettingsPath = { "Temp", "Test" };

        AdaptivePerformanceGeneralSettingsPerBuildTarget m_TestSettingsPerBuildTarget = null;
        AdaptivePerformanceGeneralSettings m_TestSettings = null;
        AdaptivePerformanceManagerSettings m_Settings = null;

        [SetUp]
        public void SetUp()
        {
            AssetDatabase.DeleteAsset("Assets/Adaptive Performance");

            AssetDatabase.CreateFolder("Assets", "Adaptive Performance");

            m_Settings = ScriptableObject.CreateInstance<AdaptivePerformanceManagerSettings>() as AdaptivePerformanceManagerSettings;
            m_Settings.name = "Actual testable settings.";

            m_TestSettings = ScriptableObject.CreateInstance<AdaptivePerformanceGeneralSettings>() as AdaptivePerformanceGeneralSettings;
            m_TestSettings.Manager = m_Settings;
            m_TestSettings.name = "Standalone Settings Container.";

            m_TestSettingsPerBuildTarget = ScriptableObject.CreateInstance<AdaptivePerformanceGeneralSettingsPerBuildTarget>() as AdaptivePerformanceGeneralSettingsPerBuildTarget;
            m_TestSettingsPerBuildTarget.SetSettingsForBuildTarget(BuildTargetGroup.Standalone, m_TestSettings);

            var testPath = AdaptivePerformanceGeneralSettingsTests.GetAssetPathForComponents(s_TempSettingsPath);
            if (!string.IsNullOrEmpty(testPath))
            {
                AssetDatabase.CreateAsset(m_TestSettingsPerBuildTarget, Path.Combine(testPath, "Test_AdaptivePerformanceGeneralSettings.asset"));

                AssetDatabase.AddObjectToAsset(m_TestSettings, AssetDatabase.GetAssetOrScenePath(m_TestSettingsPerBuildTarget));

                AssetDatabase.CreateFolder(testPath, "Settings");
                testPath = Path.Combine(testPath, "Settings");
                AssetDatabase.CreateAsset(m_Settings, Path.Combine(testPath, "Test_AdaptivePerformanceSettingsManager.asset"));

                m_TestSettings.AssignedSettings = m_Settings;
                AssetDatabase.SaveAssets();
            }

            EditorBuildSettings.AddConfigObject(AdaptivePerformanceGeneralSettings.k_SettingsKey, m_TestSettingsPerBuildTarget, true);

            AdaptivePerformancePackageInitializationBootstrap.BeginPackageInitialization();

            TestPackage.Editor.TestPackage pkg = new TestPackage.Editor.TestPackage();
            AdaptivePerformancePackageMetadataStore.AddPluginPackage(pkg);
            AdaptivePerformancePackageInitializationBootstrap.InitPackage(pkg);

            TestLoaderBase.WasAssigned = false;
            TestLoaderBase.WasUnassigned = false;
        }

        [TearDown]
        public void Teardown()
        {
            AssetDatabase.DeleteAsset("Assets/Temp");
            AssetDatabase.DeleteAsset("Assets/Adaptive Performance");
        }

        private string LoaderTypeNameForBuildTarget(BuildTargetGroup buildTargetGroup)
        {
            var loaders = AdaptivePerformancePackageMetadataStore.GetLoadersForBuildTarget(buildTargetGroup);
            var filteredLoaders = from l in loaders where String.Compare(l.loaderType, typeof(TestLoaderOne).FullName) == 0 select l;

            if (filteredLoaders.Any())
            {
                var loaderInfo = filteredLoaders.First();
                return loaderInfo.loaderType;
            }

            return "";
        }

        private bool AssignLoaderToSettings(AdaptivePerformanceManagerSettings settings, string loaderTypeName, BuildTargetGroup buildTargetGroup = BuildTargetGroup.Standalone)
        {
            if (String.IsNullOrEmpty(loaderTypeName))
                return false;

            return AdaptivePerformancePackageMetadataStore.AssignLoader(m_Settings, loaderTypeName, buildTargetGroup);
        }

        private bool SettingsHasLoaderOfType(AdaptivePerformanceManagerSettings settings, string loaderTypeName)
        {
            bool wasFound = false;
            foreach (var l in m_Settings.loaders)
            {
                if (String.Compare(l.GetType().FullName, loaderTypeName) == 0)
                    wasFound = true;
            }
            return wasFound;
        }

        [UnityTest]
        public IEnumerator TestLoaderAssignment()
        {
            Assert.IsNotNull(m_Settings);

            string loaderTypeName = LoaderTypeNameForBuildTarget(BuildTargetGroup.Standalone);
            Assert.IsFalse(String.IsNullOrEmpty(loaderTypeName));

            bool wasFound = false;
            foreach (var l in m_Settings.loaders)
            {
                if (String.Compare(l.GetType().FullName, loaderTypeName) == 0)
                    wasFound = true;
            }
            Assert.IsFalse(wasFound);

            Assert.IsTrue(AdaptivePerformancePackageMetadataStore.AssignLoader(m_Settings, loaderTypeName, BuildTargetGroup.Standalone));

            yield return null;

            Assert.IsTrue(SettingsHasLoaderOfType(m_Settings, loaderTypeName));
            Assert.IsTrue(TestLoaderBase.WasAssigned);
        }

        [Test]
        public void TestLoaderAssignmentSerializes()
        {
            Assert.IsNotNull(m_Settings);
            string loaderTypeName = LoaderTypeNameForBuildTarget(BuildTargetGroup.Standalone);
            Assert.IsFalse(String.IsNullOrEmpty(loaderTypeName));
            AssignLoaderToSettings(m_Settings, loaderTypeName);
            Assert.IsTrue(SettingsHasLoaderOfType(m_Settings, loaderTypeName));

            m_Settings = null;
            var settings = EditorUtilities.GetInstanceOfTypeFromAssetDatabase<AdaptivePerformanceManagerSettings>();
            m_Settings =  settings as AdaptivePerformanceManagerSettings;
            Assert.IsNotNull(m_Settings);

            Assert.IsTrue(SettingsHasLoaderOfType(m_Settings, loaderTypeName));
            Assert.IsTrue(TestLoaderBase.WasAssigned);
        }

        [Test]
        public void TestLoaderRemoval()
        {
            Assert.IsNotNull(m_Settings);
            string loaderTypeName = LoaderTypeNameForBuildTarget(BuildTargetGroup.Standalone);
            Assert.IsFalse(String.IsNullOrEmpty(loaderTypeName));
            AssignLoaderToSettings(m_Settings, loaderTypeName);
            Assert.IsTrue(SettingsHasLoaderOfType(m_Settings, loaderTypeName));

            Assert.IsTrue(AdaptivePerformancePackageMetadataStore.RemoveLoader(m_Settings, loaderTypeName, BuildTargetGroup.Standalone));

            m_Settings = null;
            var settings = EditorUtilities.GetInstanceOfTypeFromAssetDatabase<AdaptivePerformanceManagerSettings>();
            m_Settings = settings as AdaptivePerformanceManagerSettings;
            Assert.IsNotNull(m_Settings);
            Assert.IsFalse(SettingsHasLoaderOfType(m_Settings, loaderTypeName));

            Assert.IsTrue(TestLoaderBase.WasUnassigned);
        }

        [UnityTest]
        public IEnumerator TestInvalidPackageErrorsOut()
        {
#if !UNITY_2020_2_OR_NEWER
            AdaptivePerformancePackageMetadataStore.InstallPackageAndAssignLoaderForBuildTarget("com.unity.invalid.package.id", String.Empty, BuildTargetGroup.Standalone);

            LogAssert.Expect(LogType.Error, new Regex(@"cannot be found"));

            while (AdaptivePerformancePackageMetadataStore.isDoingQueueProcessing)
            {
                yield return null;
            }
#else
            yield return null;
#endif //UNITY_2020_2_OR_NEWER
        }

        [UnityTest]
        public IEnumerator TestNoPackageIdErrorsOut()
        {
#if !UNITY_2020_2_OR_NEWER
            AdaptivePerformancePackageMetadataStore.InstallPackageAndAssignLoaderForBuildTarget("", String.Empty, BuildTargetGroup.Standalone);

            LogAssert.Expect(LogType.Error, new Regex(@"no package id"));

            while (AdaptivePerformancePackageMetadataStore.isDoingQueueProcessing)
            {
                yield return null;
            }
#else
            yield return null;
#endif //UNITY_2020_2_OR_NEWER
        }
    }
}
