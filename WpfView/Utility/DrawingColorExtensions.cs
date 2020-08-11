namespace SpaceLogistic.WpfView.Utility
{
    using System.Windows.Media;

    public static class DrawingColorExtensions
    {
        public static Color ToMediaColor(this System.Drawing.Color color)
        {
            return Color.FromArgb(color.A, color.R, color.G, color.B);
        }
    }
}
