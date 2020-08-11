namespace SpaceLogistic.Utility.Units
{
    public struct Distance
    {
        public Distance(double value)
        {
            this.Value = value;
        }

        public double Value { get; }

        public static Distance Kilometers(double value)
        {
            return new Distance(value);
        }

        public double InMeter()
        {
            return this.Value * 1e3;
        }

        public double InKilometer()
        {
            return this.Value;
        }

        public double InThousandKilometer()
        {
            return this.Value / 1e3;
        }

        public double InMillionKilometer()
        {
            return this.Value / 1e6;
        }

        public double InBillionKilometer()
        {
            return this.Value / 1e9;
        }

        public double In(DistanceUnit unit)
        {
            return this.Value / unit.Factor;
        }

        public string Format(DistanceUnit unit, int decimalPlaces)
        {
            var unitSuffix = string.IsNullOrEmpty(unit.Symbol) ? string.Empty : $" {unit.Symbol}";
            var numberFormat = decimalPlaces < 1 ? "0" : "0." + new string('0', decimalPlaces);
            return $"{this.In(unit).ToString(numberFormat)}{unitSuffix}";
        }
    }
}
