using UnityEngine;
using System.Linq;
using UnityEditor.Build;
using UnityEngine.AdaptivePerformance;
using System.Runtime.CompilerServices;
using UnityEditor.Build.Reporting;
using UnityEditor.PackageManager.Requests;
using UnityEditor.PackageManager;

[assembly: InternalsVisibleTo("Unity.AdaptivePerformance.Editor.Tests")]
namespace UnityEditor.AdaptivePerformance.Editor
{
    class AdaptivePerformanceGeneralBuildProcessor : IPreprocessBuildWithReport, IPostprocessBuildWithReport
    {
        static string s_ProviderPackageNotFound = L10n.Tr("No Adaptive Performance provider package installed. Adaptive Performance requires a provider to get information during runtime. Please install a provider such as Adaptive Performance Samsung (Android) from the Adaptive Performance Settings.");
        static string s_Title = L10n.Tr("No Adaptive Performance provider found");
        static string s_Ok = L10n.Tr("Go to Settings");
        static string s_Cancel = L10n.Tr("Ignore");

        public int callbackOrder
        {
            get { return 0;  }
        }

        void CleanOldSettings()
        {
            BuildHelpers.CleanOldSettings<AdaptivePerformanceGeneralSettings>();
        }

        public void OnPreprocessBuild(BuildReport report)
        {
            CheckInstalledProvider();

            // Always remember to clean up preloaded assets after build to make sure we don't
            // dirty later builds with assets that may not be needed or are out of date.
            CleanOldSettings();

            AdaptivePerformanceGeneralSettingsPerBuildTarget buildTargetSettings = null;
            EditorBuildSettings.TryGetConfigObject(AdaptivePerformanceGeneralSettings.k_SettingsKey, out buildTargetSettings);
            if (buildTargetSettings == null)
                return;

            AdaptivePerformanceGeneralSettings settings = buildTargetSettings.SettingsForBuildTarget(report.summary.platformGroup);
            if (settings == null)
                return;

            UnityEngine.Object[] preloadedAssets = PlayerSettings.GetPreloadedAssets();

            if (!preloadedAssets.Contains(settings))
            {
                var assets = preloadedAssets.ToList();
                assets.Add(settings);
                PlayerSettings.SetPreloadedAssets(assets.ToArray());
            }
        }

        public void OnPostprocessBuild(BuildReport report)
        {
            // Always remember to clean up preloaded assets after build to make sure we don't
            // dirty later builds with assets that may not be needed or are out of date.
            CleanOldSettings();
        }

        static ListRequest Request;

        /// <summary>
        /// Requests a list of all installed packages from Package Manager which are processed in CheckInstalledPackages.
        /// </summary>
        static void CheckInstalledProvider()
        {
            Request = Client.List();    // List packages installed for the Project
            EditorApplication.update += CheckInstalledPackages;
        }

        /// <summary>
        /// Processes a list of all installed packages prints a message to the console if no Adaptive Performance Provider package is installed.
        /// </summary>
        static void CheckInstalledPackages()
        {
            if (Request.IsCompleted)
            {
                if (Request.Status == StatusCode.Success)
                {
                    var installedPackageCount = 0;

                    foreach (var package in Request.Result)
                        if (package.name.StartsWith("com.unity.adaptiveperformance."))
                            installedPackageCount++;

                    if (installedPackageCount == 0)
                    {
                        if (EditorUtility.DisplayDialog(s_Title, s_ProviderPackageNotFound, s_Ok, s_Cancel))
                        {
                            PackageManager.UI.Window.Open("com.unity.adaptiveperformance.samsung.android");
                            SettingsService.OpenProjectSettings("Project/Adaptive Performance");
                        }
                        else
                        {
                            Debug.LogWarning(s_ProviderPackageNotFound);
                        }
                    }
                }
                else if (Request.Status >= StatusCode.Failure)
                    Debug.Log(Request.Error.message);

                EditorApplication.update -= CheckInstalledPackages;
            }
        }
    }
}
