using System;
using UnityEngine.Scripting;
using System.Collections.Generic;

[assembly: AlwaysLinkAssembly]
namespace UnityEngine.AdaptivePerformance.Provider
{
    using AdaptivePerformanceSubsystemDescriptorBase = UnityEngine.SubsystemDescriptor<AdaptivePerformanceSubsystem>;


    [Preserve]
    internal static class AdaptivePerformanceSubsystemRegistry
    {
        /// <summary>
        /// Only for internal use.
        /// </summary>
        /// <param name="cinfo"></param>
        /// <returns></returns>
        public static AdaptivePerformanceSubsystemDescriptor RegisterDescriptor(AdaptivePerformanceSubsystemDescriptor.Cinfo cinfo)
        {
            var desc = new AdaptivePerformanceSubsystemDescriptor(cinfo);
            if (SubsystemRegistration.CreateDescriptor(desc))
            {
                return desc;
            }
            else
            {
                var registeredDescriptors = GetRegisteredDescriptors();
                foreach (var d in registeredDescriptors)
                {
                    if (d.subsystemImplementationType == cinfo.subsystemImplementationType)
                        return d;
                }
            }
            return null;
        }

        /// <summary>
        /// Only for internal use.
        /// </summary>
        /// <returns></returns>
        public static List<AdaptivePerformanceSubsystemDescriptor> GetRegisteredDescriptors()
        {
            var perfDescriptors = new List<AdaptivePerformanceSubsystemDescriptor>();
            SubsystemManager.GetSubsystemDescriptors<AdaptivePerformanceSubsystemDescriptor>(perfDescriptors);
            return perfDescriptors;
        }
    }

    /// <summary>
    /// The Adaptive Performance Subsystem Descriptor is used for describing the subsystem so it can be picked up by the subsystem management system.
    /// </summary>
#pragma warning disable CS0618
    [Preserve]
    public sealed class AdaptivePerformanceSubsystemDescriptor : AdaptivePerformanceSubsystemDescriptorBase
    {
        /// <summary>
        /// Cinfo stores the ID and subsystem implementation type which is used to identify the subsystem during subsystem initialization.
        /// </summary>
        public struct Cinfo
        {
            /// <summary>
            /// The ID stores the name of the subsystem used to identify it in the subsystem registry.
            /// </summary>
            public string id { get; set; }
            /// <summary>
            /// The subsystem implementation type stores the the type used for initialization in the subsystem registry.
            /// </summary>
            public Type subsystemImplementationType { get; set; }
        }

        /// <summary>
        /// Constructor to fill the subsystem descriptor with all information to register the subsystem successfully.
        /// </summary>
        /// <param name="cinfo">Pass in the information about the subsystem.</param>
        public AdaptivePerformanceSubsystemDescriptor(Cinfo cinfo)
        {
            id = cinfo.id;
            subsystemImplementationType = cinfo.subsystemImplementationType;
        }

        /// <summary>
        /// Register the subsystem with the subsystem registry and make it available to use during runtime.
        /// </summary>
        /// <param name="cinfo">Pass in the information about the subsystem.</param>
        /// <returns>Returns an active subsystem descriptor.</returns>
        public static AdaptivePerformanceSubsystemDescriptor RegisterDescriptor(Cinfo cinfo)
        {
            return AdaptivePerformanceSubsystemRegistry.RegisterDescriptor(cinfo);
        }
    }
#pragma warning restore CS0618
}
