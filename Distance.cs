using System;
using System.Windows;

namespace _15cm_abuse
{
    public class Distance
    {
        private readonly Values values;

        public double Dist => Calculate().Item1;
        public double Azimuth => Calculate().Item2;

        public Distance(Values val)
        {
            values = val ?? throw new ArgumentNullException(nameof(val));
        }

        private (double, double) Calculate()
        {
            if (values.Points.Count < 2)
                return (0, 0);

            Point p1 = values.Points[0];
            Point p2 = values.Points[1];

            double pixelDist = Math.Sqrt(Math.Pow(p2.X - p1.X, 2) + Math.Pow(p2.Y - p1.Y, 2));
            double dist = (pixelDist / (values.ImageDefinition / values.SquareCount)) * values.Scale;

            double deltaX = p2.X - p1.X;
            double deltaY = p2.Y - p1.Y;

            double azimuth = Math.Atan2(deltaY, deltaX) * (180.0 / Math.PI);

            azimuth = (azimuth + 90) % 360;
            if (azimuth < 0)
                azimuth += 360;

            return (Math.Round(dist, 2), Math.Round(azimuth, 2));
        }
    }
}
