using System.Runtime.CompilerServices;

using UnityEngine;

using UnityEditor.AdaptivePerformance.Editor.Metadata;
using UnityEngine.AdaptivePerformance.TestPackage;

[assembly: InternalsVisibleTo("Unity.AdaptivePerformance.Editor.Tests")]
namespace UnityEditor.AdaptivePerformance.TestPackage.Editor
{
    internal class TestPackage : IAdaptivePerformancePackage
    {
        public TestPackage() {}

        public IAdaptivePerformancePackageMetadata metadata
        {
            get
            {
                return TestMetadata.CreateAndGetMetadata();
            }
        }

        public bool PopulateNewSettingsInstance(ScriptableObject obj)
        {
            TestSettings packageSettings = obj as TestSettings;
            if (packageSettings != null)
            {
                return true;
            }
            return false;
        }
    }
}
