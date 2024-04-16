namespace UnityEngine.AdaptivePerformance.Tests.Standalone
{
    namespace Providing
    {
        public class StandaloneSubsystemParams
        {
            public string id { get; set;}
            public System.Type subsystemImplementationType { get; set; }

            public StandaloneSubsystemParams(string id, System.Type subsystemImplType)
            {
                this.id = id;
                this.subsystemImplementationType = subsystemImplType;
            }
        }
    }

#pragma warning disable CS0618
    public class StandaloneSubsystemDescriptor : SubsystemDescriptor<StandaloneSubsystem>
    {
        public static void Create(Providing.StandaloneSubsystemParams parms)
        {
            SubsystemRegistration.CreateDescriptor(new StandaloneSubsystemDescriptor(parms.id, parms.subsystemImplementationType));
        }

        public StandaloneSubsystemDescriptor(string id, System.Type subsystemImplType)
        {
            this.id = id;
            this.subsystemImplementationType = subsystemImplType;
        }
    }
#pragma warning restore CS0618
}
