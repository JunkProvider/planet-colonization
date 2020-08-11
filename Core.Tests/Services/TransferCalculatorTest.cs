namespace SpaceLogistic.Core.Tests.Services
{
    using System.Linq;

    using Microsoft.Extensions.DependencyInjection;

    using SpaceLogistic.Core.Model.Celestials;
    using SpaceLogistic.Core.Services;
    using SpaceLogistic.Core.Services.WorldGeneration;

    using Xunit;

    public sealed class TransferCalculatorTest
    {
        [Fact]
        public void Test_EarthLowOrbit_To_MarsLowOrbit()
        {
            this.Setup(out var calculator, out var starSystem);

            var origin = starSystem.GetDescendantWithName("Earth System").OrbitalLocations.First();
            var destination = starSystem.GetDescendantWithName("Mars System").OrbitalLocations.First();

            calculator.CalculateTransferOrbit(
                origin,
                destination,
                out var deltaV,
                out var period);

            Assert.Equal(5712, deltaV, 0);
            Assert.Equal(259, period.TotalDays, 0);
        }

        // [Fact]
        public void Test_EarthLowOrbit_To_LunaLowOrbit()
        {
            this.Setup(out var calculator, out var starSystem);

            var origin = starSystem.GetDescendantWithName("Earth System").OrbitalLocations.First();
            var destination = starSystem.GetDescendantWithName("Luna System").OrbitalLocations.First();

            calculator.CalculateTransferOrbit(
                origin,
                destination,
                out var deltaV,
                out var period);

            Assert.Equal(5712, deltaV, 0);
            Assert.Equal(259, period.TotalDays, 0);
        }

        [Fact]
        public void Test_LunaLowOrbit_To_MarsLowOrbit()
        {
            this.Setup(out var calculator, out var starSystem);

            var origin = starSystem.GetDescendantWithName("Luna System").OrbitalLocations.First();
            var destination = starSystem.GetDescendantWithName("Mars System").OrbitalLocations.First();

            calculator.CalculateTransferOrbit(
                origin,
                destination,
                out var deltaV,
                out var period);

            Assert.Equal(5038, deltaV, 0);
            Assert.Equal(259, period.TotalDays, 0);
        }

        private void Setup(out TransferCalculator calculator, out CelestialSystem starSystem)
        {
            var services = new ServiceCollection()
                .AddSingleton(WorldSettings.Realistic)
                .AddSingleton<TransferCalculator>()
                .BuildServiceProvider();

            starSystem = TestHelper.GetSolarSystem();

            calculator = services.GetRequiredService<TransferCalculator>();
        }
    }
}
