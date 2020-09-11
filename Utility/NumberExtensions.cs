namespace SpaceLogistic.Utility
{
    using System.Globalization;

    public static class NumberExtensions
    {
        public static string ToStringInvariant(this double value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }
    }
}
