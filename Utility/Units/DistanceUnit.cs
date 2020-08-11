namespace SpaceLogistic.Utility.Units
{
    public struct DistanceUnit
    {
        public DistanceUnit(string symbol, double factor)
        {
            this.Symbol = symbol;
            this.Factor = factor;
        }

        public static DistanceUnit Kilometer { get; } = new DistanceUnit("K", 1);

        public static DistanceUnit MillionMeter { get; } = new DistanceUnit("M", 1e3);

        public static DistanceUnit BillionMeter { get; } = new DistanceUnit("G", 1e6);
        
        public string Symbol { get; }

        public double Factor { get; }
    }
}