namespace SpaceLogistic.Core.Tests.Services
{
    using SpaceLogistic.Core.Model.Celestials;
    using SpaceLogistic.Core.Services;

    using Xunit;

    public sealed class TransferPlanTest
    {
        private readonly CelestialSystem solarSystem;

        public TransferPlanTest()
        {
            this.solarSystem = TestHelper.GetSolarSystem();
        }

        [Fact]
        public void Test_Earth_To_Luna()
        {
            var plan = this.CreateTransferPlan("Low Earth Orbit", "Low Luna Orbit");

            Assert.Equal("Earth System", plan.PrimarySystem);
            Assert.Equal("Earth System", plan.OriginSystem);
            Assert.Equal(string.Empty, plan.OriginSubSystems);
            Assert.Equal("Luna System", plan.DestinationSystem);
            Assert.Equal(string.Empty, plan.DestinationSubSystems);
        }

        [Fact]
        public void Test_Luna_To_Earth()
        {
            var plan = this.CreateTransferPlan("Low Luna Orbit", "Low Earth Orbit");

            Assert.Equal("Earth System", plan.PrimarySystem);
            Assert.Equal("Luna System", plan.OriginSystem);
            Assert.Equal(string.Empty, plan.OriginSubSystems);
            Assert.Equal("Earth System", plan.DestinationSystem);
            Assert.Equal(string.Empty, plan.DestinationSubSystems);
        }

        [Fact]
        public void Test_Earth_To_Mars()
        {
            var plan = this.CreateTransferPlan("Low Earth Orbit", "Low Mars Orbit");

            Assert.Equal("Sun System", plan.PrimarySystem);
            Assert.Equal("Earth System", plan.OriginSystem);
            Assert.Equal(string.Empty, plan.OriginSubSystems);
            Assert.Equal("Mars System", plan.DestinationSystem);
            Assert.Equal(string.Empty, plan.DestinationSubSystems);
        }

        [Fact]
        public void Test_Luna_To_Mars()
        {
            var plan = this.CreateTransferPlan("Low Luna Orbit", "Low Mars Orbit");

            Assert.Equal("Sun System", plan.PrimarySystem);
            Assert.Equal("Earth System", plan.OriginSystem);
            Assert.Equal("Luna System", plan.OriginSubSystems);
            Assert.Equal("Mars System", plan.DestinationSystem);
            Assert.Equal(string.Empty, plan.DestinationSubSystems);
        }

        private TransferPlanResult CreateTransferPlan(string origin, string destination)
        {
            var plan = TransferPlan.Create(
                this.solarSystem.GetOrbitalLocationWithName(origin),
                this.solarSystem.GetOrbitalLocationWithName(destination));

            return new TransferPlanResult(
                plan.PrimarySystem.Name,
                plan.OriginSystem.Name,
                string.Join(", ", plan.OriginSubsystems),
                plan.OriginOrbit,
                plan.DestinationSystem.Name,
                string.Join(", ", plan.DestinationSubsystems),
                plan.DestinationOrbit);
        }

        private sealed class TransferPlanResult
        {
            public TransferPlanResult(string primarySystem, string originSystem, string originSubSystems, double originOrbit, string destinationSystem, string destinationSubSystems, double destinationOrbit)
            {
                this.PrimarySystem = primarySystem;
                this.OriginSystem = originSystem;
                this.OriginSubSystems = originSubSystems;
                this.OriginOrbit = originOrbit;
                this.DestinationSystem = destinationSystem;
                this.DestinationSubSystems = destinationSubSystems;
                this.DestinationOrbit = destinationOrbit;
            }

            public string PrimarySystem { get; }

            public string OriginSystem { get; }

            public string OriginSubSystems { get; }

            public double OriginOrbit { get; }
            
            public string DestinationSystem { get; }

            public string DestinationSubSystems { get; }

            public double DestinationOrbit { get; }
        }
    }
}
