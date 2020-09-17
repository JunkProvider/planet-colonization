namespace SpaceLogistic.WpfView.View.Icons
{
    using System.Collections.Generic;
    using System.Text;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    using SpaceLogistic.Utility;
    using SpaceLogistic.WpfView.Utility;

    public partial class OkIcon : UserControl
    {
        public static readonly DependencyProperty IconSizeProperty = DependencyPropertyExtensions.Register<OkIcon, double>(nameof(IconSize), 100, StaticOnIconPropertyChanged);

        public static readonly DependencyProperty IconStrokeWidthProperty = DependencyPropertyExtensions.Register<OkIcon, double>(nameof(IconStrokeWidth), 10, StaticOnIconPropertyChanged);

        private const double ArrowToIconSizeRatio = 0.5;

        private static void StaticOnIconPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((OkIcon)d).OnIconPropertyChanged();
        }

        public double IconSize
        {
            get => (double)this.GetValue(IconSizeProperty);
            set => this.SetValue(IconSizeProperty, value);
        }

        public double IconStrokeWidth
        {
            get => (double)this.GetValue(IconStrokeWidthProperty);
            set => this.SetValue(IconStrokeWidthProperty, value);
        }

        public OkIcon()
        {
            this.InitializeComponent();
            this.Update();
        }

        private void OnIconPropertyChanged()
        {
            this.Update();
        }

        private void Update()
        {
            this.Border.Width = this.IconSize;
            this.Border.Height = this.IconSize;
            this.Border.BorderThickness = new Thickness(this.IconStrokeWidth);
            this.Border.CornerRadius = new CornerRadius(this.IconSize / 2);

            this.Path.Data = Geometry.Parse(
                new PathBuilder(ArrowToIconSizeRatio * this.IconSize)
                    .MoveTo(0, 0.5)
                    .DrawLineTo(0.333, 1)
                    .DrawLineTo(1, 0)
                    .ToPathString()); 
            this.Path.StrokeThickness = this.IconStrokeWidth;
        }

        private sealed class PathBuilder
        {
            private readonly List<string> segments = new List<string>();

            private readonly double scale;

            public PathBuilder(double scale)
            {
                this.scale = scale;
            }

            public PathBuilder MoveTo(double x, double y)
            {
                this.segments.Add($"M{(x * scale).ToStringInvariant()},{(y * scale).ToStringInvariant()}");
                return this;
            }

            public PathBuilder DrawLineTo(double x, double y)
            {
                this.segments.Add($"L{(x * scale).ToStringInvariant()},{(y * scale).ToStringInvariant()}");
                return this;
            }

            public string ToPathString()
            {
                return string.Join(" ", this.segments);
            }
        }
    }
}
