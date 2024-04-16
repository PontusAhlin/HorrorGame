using System;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.AdaptivePerformance.Simulator.Editor;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.AdaptivePerformance;

namespace UnityEditor.AdaptivePerformance.Tests
{
    public class AdaptivePerformanceSimulation : ManagementTestSetup
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Setup()
        {
            if (!AdaptivePerformanceGeneralSettings.Instance || !AdaptivePerformanceGeneralSettings.Instance.Manager || !AdaptivePerformanceGeneralSettings.Instance.Manager.isInitializationComplete)
                return;

            IAdaptivePerformanceSettings settings = AdaptivePerformanceGeneralSettings.Instance.Manager.activeLoader.GetSettings();
            if (settings == null)
                return;

            settings.automaticPerformanceMode = false;
            settings.logging = false;
        }

        [OneTimeSetUp]
        public void SetupAdaptivePerformanceManagerTest()
        {
            var ap = Holder.Instance;
            ap.DevelopmentSettings.Logging = false;
            ap.DevicePerformanceControl.AutomaticPerformanceControl = false;
        }

        [OneTimeTearDown]
        public void TeardownAdaptivePerformanceManagerTest()
        {
            base.TearDownTest();
        }

        [UnityTest]
        public IEnumerator Applies_Cpu_Level()
        {
            var subsystem = AdaptivePerformanceGeneralSettings.Instance.Manager.activeLoader.GetLoadedSubsystem<SimulatorAdaptivePerformanceSubsystem>();
            var ap = Holder.Instance;

            subsystem.AcceptsPerformanceLevel = true;

            var level = ap.DevicePerformanceControl.MaxCpuPerformanceLevel;

            ap.DevicePerformanceControl.CpuLevel = level;

            yield return null;

            Assert.AreEqual(level, ap.DevicePerformanceControl.CpuLevel);
            Assert.AreEqual(level, ap.PerformanceStatus.PerformanceMetrics.CurrentCpuLevel);
        }

        [UnityTest]
        public IEnumerator Applies_Gpu_Level()
        {
            var subsystem = AdaptivePerformanceGeneralSettings.Instance.Manager.activeLoader.GetLoadedSubsystem<SimulatorAdaptivePerformanceSubsystem>();
            var ap = Holder.Instance;

            subsystem.AcceptsPerformanceLevel = true;

            var level = ap.DevicePerformanceControl.MaxGpuPerformanceLevel;

            ap.DevicePerformanceControl.GpuLevel = level;

            yield return null;

            Assert.AreEqual(level, ap.DevicePerformanceControl.GpuLevel);
            Assert.AreEqual(level, ap.PerformanceStatus.PerformanceMetrics.CurrentGpuLevel);
        }

        [UnityTest]
        public IEnumerator Unknown_GpuLevel_In_Throttling_State()
        {
            var subsystem = AdaptivePerformanceGeneralSettings.Instance.Manager.activeLoader.GetLoadedSubsystem<SimulatorAdaptivePerformanceSubsystem>();
            var ap = Holder.Instance;

            subsystem.AcceptsPerformanceLevel = false;

            ap.DevicePerformanceControl.GpuLevel = ap.DevicePerformanceControl.MaxCpuPerformanceLevel;

            yield return null;

            Assert.AreEqual(PerformanceControlMode.System, ap.DevicePerformanceControl.PerformanceControlMode);

            Assert.AreEqual(Constants.UnknownPerformanceLevel, ap.PerformanceStatus.PerformanceMetrics.CurrentGpuLevel);
        }

        [UnityTest]
        public IEnumerator Unknown_CpuLevel_In_Throttling_State()
        {
            var subsystem = AdaptivePerformanceGeneralSettings.Instance.Manager.activeLoader.GetLoadedSubsystem<SimulatorAdaptivePerformanceSubsystem>();
            var ap = Holder.Instance;

            subsystem.AcceptsPerformanceLevel = false;

            ap.DevicePerformanceControl.CpuLevel = ap.DevicePerformanceControl.MaxCpuPerformanceLevel;

            yield return null;

            Assert.AreEqual(PerformanceControlMode.System, ap.DevicePerformanceControl.PerformanceControlMode);
            Assert.AreEqual(Constants.UnknownPerformanceLevel, ap.PerformanceStatus.PerformanceMetrics.CurrentCpuLevel);
        }

        [UnityTest]
        public IEnumerator Ignores_Invalid_Cpu_Level()
        {
            var subsystem = AdaptivePerformanceGeneralSettings.Instance.Manager.activeLoader.GetLoadedSubsystem<SimulatorAdaptivePerformanceSubsystem>();
            var ap = Holder.Instance;

            subsystem.AcceptsPerformanceLevel = true;
            subsystem.WarningLevel = WarningLevel.NoWarning;

            ap.DevicePerformanceControl.CpuLevel = 100;

            yield return null;

            Assert.AreEqual(Constants.UnknownPerformanceLevel, ap.PerformanceStatus.PerformanceMetrics.CurrentCpuLevel);
        }

        [UnityTest]
        public IEnumerator Ignores_Invalid_Gpu_Level()
        {
            var subsystem = AdaptivePerformanceGeneralSettings.Instance.Manager.activeLoader.GetLoadedSubsystem<SimulatorAdaptivePerformanceSubsystem>();
            var ap = Holder.Instance;

            subsystem.AcceptsPerformanceLevel = true;
            subsystem.WarningLevel = WarningLevel.NoWarning;

            ap.DevicePerformanceControl.GpuLevel = -2;

            yield return null;

            Assert.AreEqual(Constants.UnknownPerformanceLevel, ap.PerformanceStatus.PerformanceMetrics.CurrentGpuLevel);
        }

        [UnityTest]
        public IEnumerator TemperatureChangeEvent_Values_Are_Applied()
        {
            var subsystem = AdaptivePerformanceGeneralSettings.Instance.Manager.activeLoader.GetLoadedSubsystem<SimulatorAdaptivePerformanceSubsystem>();
            var ap = Holder.Instance;

            subsystem.TemperatureLevel = 0.0f;
            subsystem.TemperatureTrend = 1.0f;

            yield return null;

            Assert.AreEqual(0.0f, ap.ThermalStatus.ThermalMetrics.TemperatureLevel);
            Assert.AreEqual(1.0f, ap.ThermalStatus.ThermalMetrics.TemperatureTrend);
        }

        [UnityTest]
        public IEnumerator WarningLevel_Is_Applied()
        {
            var subsystem = AdaptivePerformanceGeneralSettings.Instance.Manager.activeLoader.GetLoadedSubsystem<SimulatorAdaptivePerformanceSubsystem>();
            var ap = Holder.Instance;

            subsystem.WarningLevel = WarningLevel.ThrottlingImminent;

            yield return null;

            Assert.AreEqual(WarningLevel.ThrottlingImminent, ap.ThermalStatus.ThermalMetrics.WarningLevel);

            subsystem.WarningLevel = WarningLevel.Throttling;

            yield return null;

            Assert.AreEqual(WarningLevel.Throttling, ap.ThermalStatus.ThermalMetrics.WarningLevel);

            subsystem.WarningLevel = WarningLevel.NoWarning;

            yield return null;

            Assert.AreEqual(WarningLevel.NoWarning, ap.ThermalStatus.ThermalMetrics.WarningLevel);
        }

        [UnityTest]
        public IEnumerator Provider_FrameTimes_Work()
        {
            var subsystem = AdaptivePerformanceGeneralSettings.Instance.Manager.activeLoader.GetLoadedSubsystem<SimulatorAdaptivePerformanceSubsystem>();
            var ap = Holder.Instance;

            subsystem.NextGpuFrameTime = 0.033f;
            subsystem.NextCpuFrameTime = 0.015f;
            subsystem.NextOverallFrameTime = 0.042f;

            yield return null;

            var ft = ap.PerformanceStatus.FrameTiming;

            Assert.IsTrue(Mathf.Abs(ft.CurrentFrameTime - subsystem.NextOverallFrameTime) < 0.001f);
            Assert.IsTrue(Mathf.Abs(ft.CurrentCpuFrameTime - subsystem.NextCpuFrameTime) < 0.001f);
            Assert.IsTrue(Mathf.Abs(ft.CurrentGpuFrameTime - subsystem.NextGpuFrameTime) < 0.001f);
        }

        [UnityTest]
        public IEnumerator GpuBound_When_GpuTime_Is_High()
        {
            var subsystem = AdaptivePerformanceGeneralSettings.Instance.Manager.activeLoader.GetLoadedSubsystem<SimulatorAdaptivePerformanceSubsystem>();
            var ap = Holder.Instance;

            for (int i = 0; i < Constants.DefaultAverageFrameCount; ++i)
            {
                subsystem.NextGpuFrameTime = 0.040f;
                subsystem.NextCpuFrameTime = 0.015f;
                subsystem.NextOverallFrameTime = 0.042f;
                yield return null;
            }

            Assert.AreEqual(PerformanceBottleneck.GPU, ap.PerformanceStatus.PerformanceMetrics.PerformanceBottleneck);
        }

        [UnityTest]
        public IEnumerator CpuBound_When_CpuTime_Is_High()
        {
            var subsystem = AdaptivePerformanceGeneralSettings.Instance.Manager.activeLoader.GetLoadedSubsystem<SimulatorAdaptivePerformanceSubsystem>();
            var ap = Holder.Instance;

            for (int i = 0; i < Constants.DefaultAverageFrameCount; ++i)
            {
                subsystem.NextGpuFrameTime = 0.033f;
                subsystem.NextCpuFrameTime = 0.038f;
                subsystem.NextOverallFrameTime = 0.042f;
                yield return null;
            }

            Assert.AreEqual(PerformanceBottleneck.CPU, ap.PerformanceStatus.PerformanceMetrics.PerformanceBottleneck);
        }

        [UnityTest]
        public IEnumerator Unknown_Bottleneck_When_GpuTime_And_CpuTime_Are_Equal()
        {
            var subsystem = AdaptivePerformanceGeneralSettings.Instance.Manager.activeLoader.GetLoadedSubsystem<SimulatorAdaptivePerformanceSubsystem>();
            var ap = Holder.Instance;

            for (int i = 0; i < Constants.DefaultAverageFrameCount; ++i)
            {
                subsystem.NextGpuFrameTime = 0.033f;
                subsystem.NextCpuFrameTime = subsystem.NextGpuFrameTime;
                subsystem.NextOverallFrameTime = 0.042f;
                yield return null;
            }

            Assert.AreEqual(PerformanceBottleneck.Unknown, ap.PerformanceStatus.PerformanceMetrics.PerformanceBottleneck);
        }

        [UnityTest]
        public IEnumerator Bottleneck_TargetFrameRate_Works()
        {
            var subsystem = AdaptivePerformanceGeneralSettings.Instance.Manager.activeLoader.GetLoadedSubsystem<SimulatorAdaptivePerformanceSubsystem>();
            var ap = Holder.Instance;
            // There is a bug in the editor where target framerate can be 0 and the test will fail and we skip the test. -1 is default in Editor and is not supported either.
            if (AdaptivePerformanceManager.EffectiveTargetFrameRate() <= 0)
            {
                Assert.AreEqual(null, null);
                yield break;
            }

            // very low frame numbers, to avoid failing test
            for (int i = 0; i < Constants.DefaultAverageFrameCount; ++i)
            {
                subsystem.NextCpuFrameTime = 0.0001f;
                subsystem.NextGpuFrameTime = 0.0002f;
                subsystem.NextOverallFrameTime = 0.001f;
                yield return null;
            }

            Assert.AreEqual(PerformanceBottleneck.TargetFrameRate, ap.PerformanceStatus.PerformanceMetrics.PerformanceBottleneck);
        }

        [UnityTest]
        public IEnumerator PerformanceBottleneckChangeEvent_Works()
        {
            var subsystem = AdaptivePerformanceGeneralSettings.Instance.Manager.activeLoader.GetLoadedSubsystem<SimulatorAdaptivePerformanceSubsystem>();
            var ap = Holder.Instance;
            int eventCounter = 0;
            var bottleneck = PerformanceBottleneck.Unknown;

            // Change to Undefined bottleneck so we can have one change (as often it's defined due to the other tests)
            for (int i = 0; i < Constants.DefaultAverageFrameCount; ++i)
            {
                subsystem.NextCpuFrameTime = 0.4f;
                subsystem.NextGpuFrameTime = 0.4f;
                subsystem.NextOverallFrameTime = 0.9f;
                yield return null;
            }
            PerformanceBottleneckChangeHandler eventHandler = delegate (PerformanceBottleneckChangeEventArgs args)
            {
                ++eventCounter;
                bottleneck = args.PerformanceBottleneck;
            };

            ap.PerformanceStatus.PerformanceBottleneckChangeEvent += eventHandler;
            // very high frame numbers, to avoid failing test on very slow machines (where targetframe rate is very high
            for (int i = 0; i < Constants.DefaultAverageFrameCount; ++i)
            {
                subsystem.NextCpuFrameTime = 0.1f;
                subsystem.NextGpuFrameTime = 0.9f;
                subsystem.NextOverallFrameTime = 0.9f;
                yield return null;
            }
            Assert.AreEqual(PerformanceBottleneck.GPU, ap.PerformanceStatus.PerformanceMetrics.PerformanceBottleneck);
            Assert.AreEqual(PerformanceBottleneck.GPU, bottleneck);
            Assert.AreEqual(1, eventCounter);
        }

        [UnityTest]
        public IEnumerator PerformanceLevelChangeEvent_Works()
        {
            var subsystem = AdaptivePerformanceGeneralSettings.Instance.Manager.activeLoader.GetLoadedSubsystem<SimulatorAdaptivePerformanceSubsystem>();
            subsystem.AcceptsPerformanceLevel = true; // boost mode disables performance level acceptance and those tests can run before this.
            var ap = Holder.Instance;

            var ctrl = ap.DevicePerformanceControl;
            ctrl.AutomaticPerformanceControl = false;
            var ps = ap.PerformanceStatus;

            ctrl.CpuLevel = 1;
            ctrl.GpuLevel = 2;

            yield return null;

            Assert.AreEqual(1, ps.PerformanceMetrics.CurrentCpuLevel);
            Assert.AreEqual(2, ps.PerformanceMetrics.CurrentGpuLevel);

            var eventArgs = new PerformanceLevelChangeEventArgs();
            PerformanceLevelChangeHandler eventHandler = delegate (PerformanceLevelChangeEventArgs args)
            {
                eventArgs = args;
            };
            ps.PerformanceLevelChangeEvent += eventHandler;

            ctrl.CpuLevel = 4;
            ctrl.GpuLevel = 0;

            yield return null;

            Assert.AreEqual(4, ps.PerformanceMetrics.CurrentCpuLevel);
            Assert.AreEqual(4, eventArgs.CpuLevel);
            Assert.AreEqual(0, ps.PerformanceMetrics.CurrentGpuLevel);
            Assert.AreEqual(0, eventArgs.GpuLevel);
            Assert.AreEqual(3, eventArgs.CpuLevelDelta);
            Assert.AreEqual(-2, eventArgs.GpuLevelDelta);
            Assert.AreEqual(false, eventArgs.ManualOverride);
            Assert.AreEqual(PerformanceControlMode.Manual, eventArgs.PerformanceControlMode);
        }

        [UnityTest]
        public IEnumerator ThermalEvent_Works()
        {
            var subsystem = AdaptivePerformanceGeneralSettings.Instance.Manager.activeLoader.GetLoadedSubsystem<SimulatorAdaptivePerformanceSubsystem>();
            var ap = Holder.Instance;
            var thermals = ap.ThermalStatus;

            var metrics = new ThermalMetrics();
            ThermalEventHandler eventHandler = delegate (ThermalMetrics args)
            {
                metrics = args;
            };
            thermals.ThermalEvent += eventHandler;

            subsystem.TemperatureLevel = 0.3f;
            subsystem.TemperatureTrend = 0.5f;

            yield return null;

            Assert.AreEqual(0.3f, metrics.TemperatureLevel, 0.0001f);
            Assert.AreEqual(0.5f, metrics.TemperatureTrend, 0.0001f);
        }

        [UnityTest]
        public IEnumerator PerformanceLevels_Are_Reapplied_After_Timeout()
        {
            var subsystem = AdaptivePerformanceGeneralSettings.Instance.Manager.activeLoader.GetLoadedSubsystem<SimulatorAdaptivePerformanceSubsystem>();
            var ap = Holder.Instance;

            subsystem.AcceptsPerformanceLevel = true;

            int gpuLevel = 0;
            int cpuLevel = 0;
            ap.DevicePerformanceControl.CpuLevel = gpuLevel;
            ap.DevicePerformanceControl.GpuLevel = cpuLevel;

            yield return null;

            // Samsung Subsystem would do this when "timeout" happens (setLevels changes levels back to default after 10min)
            subsystem.GpuPerformanceLevel = Constants.UnknownPerformanceLevel;
            subsystem.CpuPerformanceLevel = Constants.UnknownPerformanceLevel;

            yield return null;

            // AdaptivePerformance is supposed to reapply the last settings
            Assert.AreEqual(cpuLevel, ap.PerformanceStatus.PerformanceMetrics.CurrentCpuLevel);
            Assert.AreEqual(gpuLevel, ap.PerformanceStatus.PerformanceMetrics.CurrentGpuLevel);
        }

        [UnityTest]
        public IEnumerator PerformanceBoostChangeEvent_Works()
        {
            var subsystem = AdaptivePerformanceGeneralSettings.Instance.Manager.activeLoader.GetLoadedSubsystem<SimulatorAdaptivePerformanceSubsystem>();
            var ap = Holder.Instance;

            var ctrl = ap.DevicePerformanceControl;
            var ps = ap.PerformanceStatus;

            ctrl.CpuPerformanceBoost = true;
            ctrl.GpuPerformanceBoost = true;

            yield return null;
            Assert.AreEqual(false, ps.PerformanceMetrics.CpuPerformanceBoost);
            Assert.AreEqual(false, ps.PerformanceMetrics.GpuPerformanceBoost);

            yield return null;
            Assert.AreEqual(true, ps.PerformanceMetrics.CpuPerformanceBoost);
            Assert.AreEqual(true, ps.PerformanceMetrics.GpuPerformanceBoost);

            var eventArgs = new PerformanceBoostChangeEventArgs();
            PerformanceBoostChangeHandler eventHandler = delegate (PerformanceBoostChangeEventArgs args)
            {
                eventArgs = args;
            };
            ps.PerformanceBoostChangeEvent += eventHandler;

            yield return null;

            // Samsung Subsystem would do this when "timeout" happens (enableBoost changes to no boost after 15 sec)
            subsystem.GpuPerformanceBoost = false;
            subsystem.CpuPerformanceBoost = false;

            Assert.AreEqual(true, ps.PerformanceMetrics.CpuPerformanceBoost);
            Assert.AreEqual(false, eventArgs.CpuBoost);
            Assert.AreEqual(true, ps.PerformanceMetrics.GpuPerformanceBoost);
            Assert.AreEqual(false, eventArgs.GpuBoost);

            yield return null;

            Assert.AreEqual(false, ps.PerformanceMetrics.CpuPerformanceBoost);
            Assert.AreEqual(false, eventArgs.CpuBoost);
            Assert.AreEqual(false, ps.PerformanceMetrics.GpuPerformanceBoost);
            Assert.AreEqual(false, eventArgs.GpuBoost);
        }

        [UnityTest]
        public IEnumerator PerformanceBoost_Disables_PerformanceLevels()
        {
            var subsystem = AdaptivePerformanceGeneralSettings.Instance.Manager.activeLoader.GetLoadedSubsystem<SimulatorAdaptivePerformanceSubsystem>();
            var ap = Holder.Instance;

            var ctrl = ap.DevicePerformanceControl;
            var ps = ap.PerformanceStatus;

            ctrl.CpuPerformanceBoost = true;
            ctrl.GpuPerformanceBoost = true;

            yield return null;
            Assert.AreEqual(false, ps.PerformanceMetrics.CpuPerformanceBoost);
            Assert.AreEqual(false, ps.PerformanceMetrics.GpuPerformanceBoost);

            yield return null;
            Assert.AreEqual(true, ps.PerformanceMetrics.CpuPerformanceBoost);
            Assert.AreEqual(true, ps.PerformanceMetrics.GpuPerformanceBoost);


            ap.DevicePerformanceControl.CpuLevel = 3;
            ap.DevicePerformanceControl.GpuLevel = 2;

            yield return null;

            // AdaptivePerformance is supposed to not apply levels when bost mode is activated
            Assert.AreEqual(Constants.UnknownPerformanceLevel, ap.PerformanceStatus.PerformanceMetrics.CurrentCpuLevel);
            Assert.AreEqual(Constants.UnknownPerformanceLevel, ap.PerformanceStatus.PerformanceMetrics.CurrentGpuLevel);
        }

        [UnityTest]
        public IEnumerator PerformanceBoost_Is_Off_After_Timeout()
        {
            var subsystem = AdaptivePerformanceGeneralSettings.Instance.Manager.activeLoader.GetLoadedSubsystem<SimulatorAdaptivePerformanceSubsystem>();
            var ap = Holder.Instance;

            ap.DevicePerformanceControl.GpuPerformanceBoost = true;
            ap.DevicePerformanceControl.CpuPerformanceBoost = true;

            yield return null;

            // Samsung Subsystem would do this when "timeout" happens (enableBoost changes to no boost after 15 sec)
            subsystem.GpuPerformanceBoost = false;
            subsystem.CpuPerformanceBoost = false;

            yield return null;

            // AdaptivePerformance is supposed to reapply the last settings
            Assert.AreEqual(false, ap.PerformanceStatus.PerformanceMetrics.CpuPerformanceBoost);
            Assert.AreEqual(false, ap.PerformanceStatus.PerformanceMetrics.GpuPerformanceBoost);
        }

        [UnityTest]
        public IEnumerator Feature_ClusterInfo_Is_Supported()
        {
            var subsystem = AdaptivePerformanceGeneralSettings.Instance.Manager.activeLoader.GetLoadedSubsystem<SimulatorAdaptivePerformanceSubsystem>();
            var ap = Holder.Instance;

            var clusterInfo = ap.SupportedFeature(UnityEngine.AdaptivePerformance.Provider.Feature.ClusterInfo);
            var bigcores = ap.PerformanceStatus.PerformanceMetrics.ClusterInfo.BigCore;
            var mediumcores = ap.PerformanceStatus.PerformanceMetrics.ClusterInfo.MediumCore;
            var tinycores = ap.PerformanceStatus.PerformanceMetrics.ClusterInfo.LittleCore;

            yield return null;

            ClusterInfo newClusterInfo = new ClusterInfo
            {
                BigCore = 5,
                MediumCore = 4,
                LittleCore = -1
            };
            subsystem.SetClusterInfo(newClusterInfo);
            yield return null;

            Assert.AreEqual(true, clusterInfo);
            Assert.AreEqual(bigcores, 0); // Big core is always 0 in Simulator
            Assert.AreEqual(mediumcores, 0); // Medium core is always 0 in Simulator
            Assert.AreEqual(tinycores, 0); // Small core is always 0 in Simulator
            Assert.AreEqual(ap.PerformanceStatus.PerformanceMetrics.ClusterInfo.BigCore, 5);
            Assert.AreEqual(ap.PerformanceStatus.PerformanceMetrics.ClusterInfo.MediumCore, 4);
            Assert.AreEqual(ap.PerformanceStatus.PerformanceMetrics.ClusterInfo.LittleCore, -1);
        }

        /// <summary>
        /// Verifies the Scalers available in the assembly (e.g. created as sub-types from <see cref="AdaptivePerformanceScaler "/>
        /// are all registered and available as Scalers in the <see cref="AdaptivePerformanceIndexer"/>.
        /// </summary>
        [UnityTest]
        public IEnumerator All_Scalers_Available()
        {
            var ap = Holder.Instance;
            var apIndexer = ap.Indexer;

            var ti = typeof(AdaptivePerformanceScaler);
            var assemblyScalers = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(i => i.GetTypes())
                .Count(i => ti.IsAssignableFrom(i) && !i.IsAbstract);

            var indexedScalers = new List<AdaptivePerformanceScaler>();
            apIndexer.GetAllRegisteredScalers(ref indexedScalers);

            yield return null;

            Assert.AreEqual(assemblyScalers, indexedScalers.Count);
        }
    }
}
