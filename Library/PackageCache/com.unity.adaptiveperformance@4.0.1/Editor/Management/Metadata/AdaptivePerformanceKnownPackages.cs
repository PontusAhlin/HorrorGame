using System;
using System.Collections.Generic;

using UnityEngine;

namespace UnityEditor.AdaptivePerformance.Editor.Metadata
{
    internal class AdaptivePerformanceKnownPackages
    {
        class KnownLoaderMetadata : IAdaptivePerformanceLoaderMetadata
        {
            public string loaderName { get; set; }
            public string loaderType { get; set; }
            public List<BuildTargetGroup> supportedBuildTargets { get; set; }
        }

        class KnownPackageMetadata : IAdaptivePerformancePackageMetadata
        {
            public string packageName { get; set; }
            public string packageId { get; set; }
            public string settingsType { get; set; }
            public string licenseURL { get; set; }
            public List<IAdaptivePerformanceLoaderMetadata> loaderMetadata { get; set; }
        }

        class KnownPackage : IAdaptivePerformancePackage
        {
            public IAdaptivePerformancePackageMetadata metadata { get; set; }
            public bool PopulateNewSettingsInstance(ScriptableObject obj) { return true; }
        }

        private static Lazy<List<IAdaptivePerformancePackage>> s_KnownPackages = new Lazy<List<IAdaptivePerformancePackage>>(InitKnownPackages);

        internal static List<IAdaptivePerformancePackage> Packages => s_KnownPackages.Value;

        static List<IAdaptivePerformancePackage> InitKnownPackages()
        {
            List<IAdaptivePerformancePackage> packages = new List<IAdaptivePerformancePackage>();

            packages.Add(new KnownPackage() {
                metadata = new KnownPackageMetadata(){
                    packageName = "Adaptive Performance Samsung Android",
                    packageId = "com.unity.adaptiveperformance.samsung.android",
                    settingsType = "UnityEngine.AdaptivePerformance.Samsung.Android.SamsungAndroidProviderSettings",
                    licenseURL = "https://docs.unity3d.com/Packages/com.unity.adaptiveperformance.samsung.android@latest?subfolder=/license/LICENSE.html",
                    loaderMetadata = new List<IAdaptivePerformanceLoaderMetadata>()
                    {
                        new KnownLoaderMetadata() {
                            loaderName = "Samsung Android Provider",
                            loaderType = "UnityEngine.AdaptivePerformance.Samsung.Android.SamsungAndroidProviderLoader",
                            supportedBuildTargets = new List<BuildTargetGroup>()
                            {
                                BuildTargetGroup.Android
                            }
                        },
                    }
                }
            });

            packages.Add(new KnownPackage()
            {
                metadata = new KnownPackageMetadata()
                {
                    packageName = "Adaptive Performance",
                    packageId = "com.unity.adaptiveperformance",
                    settingsType = "UnityEditor.AdaptivePerformance.Simulator.Editor.SimulatorProviderSettings",
                    licenseURL = "https://docs.unity3d.com/Packages/com.unity.adaptiveperformance@latest?subfolder=/license/LICENSE.html",
                    loaderMetadata = new List<IAdaptivePerformanceLoaderMetadata>()
                    {
                        new KnownLoaderMetadata() {
                            loaderName = "Device Simulator Provider",
                            loaderType = "UnityEditor.AdaptivePerformance.Simulator.Editor.SimulatorProviderLoader",
                            supportedBuildTargets = new List<BuildTargetGroup>()
                            {
                                BuildTargetGroup.Standalone
                            }
                        },
                    }
                }
            });
            return packages;
        }
    }
}
