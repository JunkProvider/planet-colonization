namespace SpaceLogistic.WpfView.Utility
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;

    public static class DependencyPropertyExtensions
    {
        public static DependencyProperty Register<TOwner, TValue>(string name)
        {
            return DependencyProperty.Register(name, typeof(TValue), typeof(TOwner), new PropertyMetadata());
        }

        public static DependencyProperty Register<TOwner, TValue>(string name, TValue defaultValue)
        {
            return DependencyProperty.Register(name, typeof(TValue), typeof(TOwner), new PropertyMetadata(defaultValue));
        }

        public static DependencyProperty Register<TOwner, TValue>(string name, TValue defaultValue, PropertyChangedCallback propertyChangedCallback)
        {
            return DependencyProperty.Register(name, typeof(TValue), typeof(TOwner), new PropertyMetadata(defaultValue, propertyChangedCallback));
        }

        public static DependencyProperty Register<TOwner, TValue>(string name, PropertyChangedCallback propertyChangedCallback)
        {
            return DependencyProperty.Register(name, typeof(TValue), typeof(TOwner), new PropertyMetadata(default(TValue), propertyChangedCallback));
        }

        public static DependencyProperty RegisterEnumerable<TOwner, TItem>(string name)
        {
            return DependencyProperty.Register(name, typeof(IEnumerable<TItem>), typeof(TOwner), new PropertyMetadata(Enumerable.Empty<TItem>()));
        }
    }
}
