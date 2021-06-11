namespace SpaceLogistic.Persistence
{
    using System;

    public static class ConversionExtensions
    {
        public static TOut ConvertNullable<TIn, TOut>(this TIn value, Func<TIn, TOut> selector)
            where TIn : class
            where TOut : class
        {
            return value != null ? selector(value) : null;
        }
    }
}