﻿namespace SpaceLogistic.Core.Services
{
    using System;

    using SpaceLogistic.Core.Model.Celestials;

    public interface ITransferCalculator
    {
        void Calculate(
            OrbitalLocation origin,
            OrbitalLocation destination,
            double dryMass,
            out double fuelCosts,
            out TimeSpan travelTime);
    }
}