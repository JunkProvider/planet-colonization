namespace SpaceLogistic.Core.Services.WorldGeneration
{
    using System;

    using JPV.RocketScience;

    public sealed class WorldSettings
    {
        public static WorldSettings Realistic { get; } = new WorldSettings { GravitationalConstant = Physics.GravitationalConstant };

        public double GravitationalConstant { get; set; } = Physics.GravitationalConstant;

        public double OrbitFactor { get; set; } = 1;

        public double SizeFactor { get; set; } = 1;

        public double DensityFactor { get; set; } = 1;


        public double ShipEmptyMass { get; set; } = 50e3;

        public double ShipSpecificImpulse { get; set; } = 100000;

        public double TravelSpeedFactor { get; set; } = TimeSpan.FromDays(50).TotalMilliseconds / TimeSpan.FromSeconds(1).TotalMilliseconds;

        public double ItemMass { get; set; } = 1e3;
    }
}