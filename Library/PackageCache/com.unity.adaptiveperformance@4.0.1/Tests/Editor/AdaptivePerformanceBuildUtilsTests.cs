using System.IO;
using NUnit.Framework;
using UnityEngine;

namespace UnityEditor.AdaptivePerformance.Editor.Tests
{
    public class AdaptivePerformanceBuildUtilsTests
    {
        string testFolder;
        string bootConfig;

        [SetUp]
        public void SetUp()
        {
            testFolder = Path.Combine(Application.temporaryCachePath, "APTestBoot");
            if (Directory.Exists(testFolder))
                Directory.Delete(testFolder, true);
            Directory.CreateDirectory(testFolder);

            string testBuildPath = Path.Combine(testFolder, "src/main/assets/bin/Data");
            Directory.CreateDirectory(testBuildPath);
            bootConfig = Path.Combine(testBuildPath, "boot.config");
            File.WriteAllLines(bootConfig, new[]
            {
                "gfx-disable-mt-rendering=1",
                "wait-for-native-debugger=0",
                "hdr-display-enabled=0",
            });
        }

        [TearDown]
        public void TearDown()
        {
            if (testFolder != null && Directory.Exists(testFolder))
                Directory.Delete(testFolder, true);
        }

        void CheckBootConfigContains(string key, string value)
        {
            var lines = File.ReadAllLines(bootConfig);
            bool found = false;
            foreach (var line in lines)
            {
                if (line.StartsWith(key))
                {
                    Assert.IsFalse(found);  // no duplicates allowed
                    Assert.AreEqual($"{key}={value}", line);
                    found = true;
                }
            }

            Assert.IsTrue(found);
        }

        [Test]
        public void UpdateBootConfigBoostSetting_WithoutSetting_AddsSetting()
        {
            AdaptivePerformanceBuildUtils.UpdateBootConfigBoostSetting(testFolder, "adaptive-performance-test", "1");
            CheckBootConfigContains("adaptive-performance-test", "1");
        }

        [Test]
        public void UpdateBootConfigBoostSetting_WithCorrectSetting_DoesntChangeFile()
        {
            File.AppendAllLines(bootConfig, new[] { "adaptive-performance-test=1" });
            AdaptivePerformanceBuildUtils.UpdateBootConfigBoostSetting(testFolder, "adaptive-performance-test", "1");
            CheckBootConfigContains("adaptive-performance-test", "1");
        }

        [Test]
        public void UpdateBootConfigBoostSetting_WithInorrectSetting_ReplacesSetting()
        {
            File.AppendAllLines(bootConfig, new[] { "adaptive-performance-test=0" });
            AdaptivePerformanceBuildUtils.UpdateBootConfigBoostSetting(testFolder, "adaptive-performance-test", "1");
            CheckBootConfigContains("adaptive-performance-test", "1");
        }
    }
}
