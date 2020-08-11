namespace SpaceLogistic.WpfView.View.Map
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Windows.Shapes;

    public class MapControl : Canvas
    {
        public static double GetDistance(DependencyObject obj)
        {
            return (double)obj.GetValue(DistanceProperty);
        }

        public static void SetDistance(DependencyObject obj, double value)
        {
            obj.SetValue(DistanceProperty, value);
        }

        public static readonly DependencyProperty DistanceProperty = DependencyProperty.RegisterAttached(
            "Distance", 
            typeof(double), 
            typeof(MapControl),
            new PropertyMetadata(0d));

        public static double GetSize(DependencyObject obj)
        {
            return (double)obj.GetValue(SizeProperty);
        }

        public static void SetSize(DependencyObject obj, double value)
        {
            obj.SetValue(SizeProperty, value);
        }

        public static readonly DependencyProperty SizeProperty = DependencyProperty.RegisterAttached(
            "Size",
            typeof(double),
            typeof(MapControl),
            new PropertyMetadata(0d));

        public static double GetPeriodStart(DependencyObject obj)
        {
            return (double)obj.GetValue(PeriodStartProperty);
        }

        public static void SetPeriodStart(DependencyObject obj, double value)
        {
            obj.SetValue(PeriodStartProperty, value);
        }

        public static readonly DependencyProperty PeriodStartProperty = DependencyProperty.RegisterAttached(
            "PeriodStart",
            typeof(double),
            typeof(MapControl),
            new PropertyMetadata(0d));

        public static double GetPeriod(DependencyObject obj)
        {
            return (double)obj.GetValue(PeriodProperty);
        }

        public static void SetPeriod(DependencyObject obj, double value)
        {
            obj.SetValue(PeriodProperty, value);
        }

        public static readonly DependencyProperty PeriodProperty = DependencyProperty.RegisterAttached(
            "Period",
            typeof(double),
            typeof(MapControl),
            new PropertyMetadata(0d));

        static MapControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MapControl), new FrameworkPropertyMetadata(typeof(MapControl)));
        }

        private double Zoom { get; set; } = 1;

        protected override void OnInitialized(EventArgs e)
        {
        }

        protected override void OnVisualChildrenChanged(DependencyObject visualAdded, DependencyObject visualRemoved)
        {
            base.OnVisualChildrenChanged(visualAdded, visualRemoved);
            this.InvalidateVisual();
        }
        
        protected override Size ArrangeOverride(Size finalSize)
        {
            var now = Environment.TickCount / 1000;
            
            var viewportSize = Math.Min(finalSize.Width, finalSize.Height);
            var viewportCenter = new Point(viewportSize / 2, viewportSize / 2);
            var viewportOffset = new Point((finalSize.Width - viewportSize) / 2, (finalSize.Height - viewportSize) / 2);

            var contentSize = 0d;
            
            for (var i = 0; i < this.InternalChildren.Count; i++)
            {
                var child = this.InternalChildren[i];
                var childDistance = GetDistance(child);
                var childSize = GetSize(child);
                
                contentSize = Math.Max(contentSize, 2 * childDistance + childSize );
            }

            var scale = contentSize == 0 ? 0 : viewportSize / contentSize;

            for (var i = 0; i < this.InternalChildren.Count; i++)
            {
                var child = this.InternalChildren[i];
                var childDistance = GetDistance(child);
                var childSize = GetSize(child);
                var scaledChildSize = childSize * scale;
                var childPeriodStart = GetPeriodStart(child);
                var childPeriod = GetPeriod(child);
                var periodCount = childPeriod == 0 ? 0 : now / childPeriod;

                var periodCountRadian = (periodCount % 1d) * 2 * Math.PI;
                var scaledDistance = childDistance * scale;
                var x = scaledDistance * Math.Cos(periodCountRadian);
                var y = scaledDistance * Math.Sin(periodCountRadian);

                child.Arrange(new Rect(
                    viewportOffset.X + viewportCenter.X + x - scaledChildSize / 2,
                    viewportOffset.Y + viewportCenter.Y + y - scaledChildSize / 2,
                    scaledChildSize,
                    scaledChildSize));
            }
            
            return finalSize;
        }

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);

            var borderBrush = new SolidColorBrush(Colors.DarkSlateBlue);
            var borderPen = new Pen(borderBrush, 1);

            var viewportSize = Math.Min(this.RenderSize.Width, this.RenderSize.Height);
            var viewportCenter = new Point(viewportSize / 2, viewportSize / 2);
            var viewportOffset = new Point((this.RenderSize.Width - viewportSize) / 2, (this.RenderSize.Height - viewportSize) / 2);

            var contentSize = 0d;

            for (var i = 0; i < this.InternalChildren.Count; i++)
            {
                var child = this.InternalChildren[i];
                var childDistance = GetDistance(child);
                var childSize = GetSize(child);

                contentSize = Math.Max(contentSize, 2 * childDistance + childSize);
            }

            var scale = contentSize == 0 ? 0 : viewportSize / contentSize;

            for (var i = 0; i < this.InternalChildren.Count; i++)
            {
                var child = this.InternalChildren[i];
                var childDistance = GetDistance(child);

                var scaledDistance = childDistance * scale;

                dc.DrawEllipse(
                    null,
                    borderPen,
                    new Point(viewportOffset.X + viewportCenter.X, viewportOffset.Y + viewportCenter.Y),
                    scaledDistance,
                    scaledDistance);
            }
        }

        private static Ellipse CreateCircle(double diameter, Color color)
        {
            return new Ellipse { Fill = new SolidColorBrush(color) };
        }
    }
}
