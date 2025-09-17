using System;
using System.Collections.Generic;
using System.Windows;

namespace _15cm_abuse
{
    public class Values
    {
        public delegate void Notify();
        public event Notify? ScaleChanged;

        private double _imageDefinition = 325;
        public double ImageDefinition { get => _imageDefinition; set => _imageDefinition = value; }

        private int _squareCount = 7;
        public int SquareCount { get => _squareCount; set => _squareCount = value; }

        private double scale;
        public double Scale
        {
            get => scale;
            set
            {
                scale = value;
                ScaleChanged?.Invoke();
            }
        }

        public List<Point> Points { get; set; } = new();
    }
}
