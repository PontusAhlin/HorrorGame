using System;
using UnityEngine.Scripting;
using UnityEngine;
using UnityEngine.AdaptivePerformance;
using UnityEngine.AdaptivePerformance.Provider;
using Provider = UnityEngine.AdaptivePerformance.Provider;

namespace UnityEditor.AdaptivePerformance.Simulator.Editor
{
    /// <summary>
    /// The subsystem is used for simulating Adaptive Performance in the Editor with the <see href="https://docs.unity3d.com/Manual/DeviceSimulator.html">Device Simulator</see>.
    /// It is also used for Adaptive Performance tests and to simulate Adaptive Performance when it is not available on the hardware you work with.
    /// </summary>
    [Preserve]
    public class SimulatorAdaptivePerformanceSubsystem : AdaptivePerformanceSubsystem, IApplicationLifecycle, IDevicePerformanceLevelControl
    {
        /// <summary>
        /// This property is a wrapper around an internal PerformanceDataRecord object. For more details, see <see cref="PerformanceDataRecord.ChangeFlags"/>.
        /// </summary>
        public Feature ChangeFlags
        {
            get { return updateResult.ChangeFlags; }
            set { updateResult.ChangeFlags = value; }
        }

        /// <summary>
        /// This property is a wrapper around an internal PerformanceDataRecord object. For more details, see <see cref="PerformanceDataRecord.TemperatureLevel"/>.
        /// </summary>
        public float TemperatureLevel
        {
            get { return updateResult.TemperatureLevel; }
            set
            {
                updateResult.TemperatureLevel = value;
                updateResult.ChangeFlags |= Provider.Feature.TemperatureLevel;
            }
        }

        /// <summary>
        /// This property is a wrapper around an internal PerformanceDataRecord object. For more details, see <see cref="PerformanceDataRecord.TemperatureTrend"/>.
        /// </summary>
        public float TemperatureTrend
        {
            get { return updateResult.TemperatureTrend; }
            set
            {
                updateResult.TemperatureTrend = value;
                updateResult.ChangeFlags |= Provider.Feature.TemperatureTrend;
            }
        }

        /// <summary>
        /// This property is a wrapper around an internal PerformanceDataRecord object. For more details, see <see cref="PerformanceDataRecord.WarningLevel"/>.
        /// </summary>
        public WarningLevel WarningLevel
        {
            get { return updateResult.WarningLevel; }
            set
            {
                updateResult.WarningLevel = value;
                updateResult.ChangeFlags |= Provider.Feature.WarningLevel;
            }
        }

        /// <summary>
        /// This property is a wrapper around an internal PerformanceDataRecord object. For more details, see <see cref="PerformanceDataRecord.CpuPerformanceLevel"/>.
        /// </summary>
        public int CpuPerformanceLevel
        {
            get { return updateResult.CpuPerformanceLevel; }
            set
            {
                updateResult.CpuPerformanceLevel = value;
                updateResult.ChangeFlags |= Provider.Feature.CpuPerformanceLevel;
            }
        }

        /// <summary>
        /// This property is a wrapper around an internal PerformanceDataRecord object. For more details, see <see cref="PerformanceDataRecord.GpuPerformanceLevel"/>.
        /// </summary>
        public int GpuPerformanceLevel
        {
            get { return updateResult.GpuPerformanceLevel; }
            set
            {
                updateResult.GpuPerformanceLevel = value;
                updateResult.ChangeFlags |= Provider.Feature.GpuPerformanceLevel;
            }
        }

        /// <summary>
        /// This property is a wrapper around an internal PerformanceDataRecord object. For more details, see <see cref="PerformanceDataRecord.CpuPerformanceBoost"/>.
        /// </summary>
        public bool CpuPerformanceBoost
        {
            get { return updateResult.CpuPerformanceBoost; }
            set
            {
                updateResult.CpuPerformanceBoost = value;
                updateResult.ChangeFlags |= Provider.Feature.CpuPerformanceBoost;
            }
        }

        /// <summary>
        /// This property is a wrapper around an internal PerformanceDataRecord object. For more details, see <see cref="PerformanceDataRecord.GpuPerformanceBoost"/>.
        /// </summary>
        public bool GpuPerformanceBoost
        {
            get { return updateResult.GpuPerformanceBoost; }
            set
            {
                updateResult.GpuPerformanceBoost = value;
                updateResult.ChangeFlags |= Provider.Feature.GpuPerformanceBoost;
            }
        }

        /// <summary>
        /// This property is a wrapper around an internal PerformanceDataRecord object. For more details, see <see cref="PerformanceDataRecord.GpuFrameTime"/>.
        /// </summary>
        public float NextGpuFrameTime
        {
            get { return updateResult.GpuFrameTime; }
            set
            {
                updateResult.GpuFrameTime = value;
                updateResult.ChangeFlags |= Provider.Feature.GpuFrameTime;
            }
        }

        /// <summary>
        /// This property is a wrapper around an internal PerformanceDataRecord object. For more details, see <see cref="PerformanceDataRecord.CpuFrameTime"/>.
        /// </summary>
        public float NextCpuFrameTime
        {
            get { return updateResult.CpuFrameTime; }
            set
            {
                updateResult.CpuFrameTime = value;
                updateResult.ChangeFlags |= Provider.Feature.CpuFrameTime;
            }
        }

        /// <summary>
        /// This property is a wrapper around an internal PerformanceDataRecord object. For more details, see <see cref="PerformanceDataRecord.OverallFrameTime"/>.
        /// </summary>
        public float NextOverallFrameTime
        {
            get { return updateResult.OverallFrameTime; }
            set
            {
                updateResult.OverallFrameTime = value;
                updateResult.ChangeFlags |= Provider.Feature.OverallFrameTime;
            }
        }

        /// <summary>
        /// Required to simulate performance changes. To change AutomaticPerformanceControl, you have to set AcceptsPerformanceLevel to `true`. See <see cref="PerformanceDataRecord.PerformanceLevelControlAvailable"/>.
        /// </summary>
        public bool AcceptsPerformanceLevel
        {
            get { return updateResult.PerformanceLevelControlAvailable; }
            set
            {
                updateResult.PerformanceLevelControlAvailable = value;
                updateResult.ChangeFlags |= Provider.Feature.PerformanceLevelControl;
            }
        }

        /// <summary>
        /// Helper for the device simulator to change cluster info settings. Those settings are usually changed by a device directly.
        /// </summary>
        /// <param name="clusterInfo">New Cluster Info values.</param>
        public void SetClusterInfo(ClusterInfo clusterInfo)
        {
            updateResult.ClusterInfo = clusterInfo;
            updateResult.ChangeFlags |= Provider.Feature.ClusterInfo;
        }

        /// <summary>
        /// The current version of the Device Simulator Adaptive Performance Subsystem. Matches the version of the Adaptive Performance Subsystem. See <see cref="AdaptivePerformanceSubsystem.Version"/>.
        /// </summary>
        public override Version Version
        {
            get
            {
                return new Version(4, 0, 0);
            }
        }

        /// <summary>
        /// See <see cref="IDevicePerformanceLevelControl.MaxCpuPerformanceLevel"/>.
        /// </summary>
        public int MaxCpuPerformanceLevel
        {
            get
            {
                return 6;
            }
        }

        /// <summary>
        /// See <see cref="IDevicePerformanceLevelControl.MaxGpuPerformanceLevel"/>.
        /// </summary>
        public int MaxGpuPerformanceLevel
        {
            get
            {
                return 4;
            }
        }

        /// <summary>
        /// Main constructor, not used in the subsystem specifically.
        /// </summary>
        public SimulatorAdaptivePerformanceSubsystem()
        {
            Capabilities = Feature.CpuPerformanceLevel | Feature.GpuPerformanceLevel | Feature.PerformanceLevelControl |
                Feature.TemperatureLevel | Feature.WarningLevel | Feature.TemperatureTrend | Feature.CpuFrameTime | Feature.GpuFrameTime | Feature.OverallFrameTime | Feature.CpuPerformanceBoost | Feature.GpuPerformanceBoost | Feature.ClusterInfo;
            updateResult.PerformanceLevelControlAvailable = true;
        }

        /// <summary>
        /// Perform initialization of the subsystem.
        /// </summary>
        public override void Start()
        {
            initialized = true;
        }

        /// <summary>
        /// Stop running the subsystem.
        /// </summary>
        public override void Stop()
        {
        }

        /// <summary>
        /// Cleanup when the subsystem object is destroyed.
        /// </summary>
        protected override void OnDestroy() {}

        private PerformanceDataRecord updateResult = new PerformanceDataRecord();
        private float resetCpuBoostMode = 0;
        private float resetGpuBoostMode = 0;

        /// <summary>
        /// Update current results and flags.
        /// </summary>
        /// <returns>The latest PerformanceDataRecord object.</returns>
        public override PerformanceDataRecord Update()
        {
            // Boost mode disables setPerformanceLevel and only lasts 15 seconds
            if ((int)resetCpuBoostMode == (int)Time.realtimeSinceStartup) // use int to avoid high precision of float
            {
                resetCpuBoostMode = 0;
                AcceptsPerformanceLevel = true;
                updateResult.CpuPerformanceBoost = false;
                updateResult.ChangeFlags |= Provider.Feature.CpuPerformanceBoost;
            }
            if ((int)resetGpuBoostMode == (int)Time.realtimeSinceStartup)
            {
                resetGpuBoostMode = 0;
                AcceptsPerformanceLevel = true;
                updateResult.GpuPerformanceBoost = false;
                updateResult.ChangeFlags |= Provider.Feature.GpuPerformanceBoost;
            }


            updateResult.ChangeFlags &= Capabilities;
            var result = updateResult;
            updateResult.ChangeFlags = Feature.None;
            return result;
        }

        /// <summary>
        /// Callback that is called when the application goes into a pause state.
        /// </summary>
        public void ApplicationPause()
        {
        }

        /// <summary>
        /// Callback that is called when the application resumes after being paused.
        /// </summary>
        public void ApplicationResume()
        {
        }

        /// <summary>
        /// See <see cref="AdaptivePerformanceSubsystem.ApplicationLifecycle"/>.
        /// </summary>
        public override IApplicationLifecycle ApplicationLifecycle { get { return null; } }

        /// <summary>
        /// See <see cref="AdaptivePerformanceSubsystem.PerformanceLevelControl"/>.
        /// </summary>
        public override IDevicePerformanceLevelControl PerformanceLevelControl { get { return this; } }

        /// <summary>
        /// Set the performance level for both the CPU and GPU.
        /// </summary>
        /// <param name="cpuLevel">The CPU performance level to request.</param>
        /// <param name="gpuLevel">The GPU performance level to request.</param>
        /// <returns>Returns if the levels were successfully set.</returns>
        public bool SetPerformanceLevel(ref int cpuLevel, ref int gpuLevel)
        {
            if (!AcceptsPerformanceLevel)
            {
                CpuPerformanceLevel = Constants.UnknownPerformanceLevel;
                GpuPerformanceLevel = Constants.UnknownPerformanceLevel;
                return false;
            }

            return cpuLevel >= 0 && gpuLevel >= 0 && cpuLevel <= MaxCpuPerformanceLevel && gpuLevel <= MaxGpuPerformanceLevel;
        }

        /// <summary>
        /// Enable the boost mode for the CPU.
        /// </summary>
        /// <returns>Returns if CPU boost mode was successfully enabled.</returns>
        public bool EnableCpuBoost()
        {
            // Boost mode disables setPerformanceLevel
            AcceptsPerformanceLevel = false;
            resetCpuBoostMode = Time.realtimeSinceStartup + 10;

            updateResult.CpuPerformanceBoost = true;
            updateResult.ChangeFlags |= Provider.Feature.CpuPerformanceBoost;
            return true;
        }

        /// <summary>
        /// Enable the boost mode for the GPU.
        /// </summary>
        /// <returns>Returns if GPU boost mode was successfully enabled.</returns>
        public bool EnableGpuBoost()
        {
            // Boost mode disables setPerformanceLevel
            AcceptsPerformanceLevel = false;
            resetGpuBoostMode = Time.realtimeSinceStartup + 10;

            updateResult.GpuPerformanceBoost = true;
            updateResult.ChangeFlags |= Provider.Feature.GpuPerformanceBoost;
            return true;
        }

        /// <summary>
        /// Register the subsystem with the subsystem registry and make it available to use during runtime.
        /// </summary>
        /// <returns>Returns an active subsystem descriptor.</returns>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static AdaptivePerformanceSubsystemDescriptor RegisterDescriptor()
        {
            return AdaptivePerformanceSubsystemDescriptor.RegisterDescriptor(new AdaptivePerformanceSubsystemDescriptor.Cinfo
            {
                id = "SimulatorAdaptivePerformanceSubsystem",
                subsystemImplementationType = typeof(SimulatorAdaptivePerformanceSubsystem)
            });
        }
    }
}
