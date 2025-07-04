﻿using CrosshairSelector.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace CrosshairSelector.Model
{
    public sealed class CircleView : CrosshairViewBase
    {
        #region Fields
        private const int Scalar = 1;
        #endregion // Fields

        #region Properties
        public Ellipse Ellipse { get; private set; }
        #endregion // Properties

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="thickness">optional</param>
        /// <param name="size">optional</param>
        public CircleView()
        {
            Ellipse = new Ellipse();
        }
        #endregion // Constructor

        #region CrosshairViewBase implementation
        public override void PutCrosshairOnCanvas(double ActualWidth,  double ActualHeight, ref Canvas canvas)
        {
            Canvas.SetLeft(Ellipse, ActualWidth / 2 );
            Canvas.SetTop(Ellipse, ActualHeight / 2 );
            canvas.Children.Add(Ellipse);
        }
        public override void RemoveCrosshairFromCanvas(ref Canvas canvas)
        {
            canvas.Children.Remove(Ellipse);
        }
        public override void SetSize(int thickness, int size, int gap)
        {
            Height = size * Scalar;
            Width = size * Scalar;
            Thickness = thickness * Scalar;
            Ellipse.Width = Width;
            Ellipse.Height = Height;
        }
        public override void SetStyle(bool outline, Color crosshairColor, Color outlineColor, int outlineThickness)
        {
            base.SetStyle(outline, crosshairColor, outlineColor, outlineThickness);
            if (Outline)
            {
                Ellipse.Stroke = new SolidColorBrush(Colors.Black);
            }
            else
            {
                Ellipse.Stroke = new SolidColorBrush(CrosshairColor);
            }
            Ellipse.StrokeThickness = Thickness;
            Ellipse.Fill = new SolidColorBrush(CrosshairColor);
        }
        #endregion // CrosshairViewBase implementation

        #region ICloneable implementation
        public override object Clone()
        {
            return new CircleView()
            {
                Ellipse = new Ellipse() { Fill = this.Ellipse.Fill, Stroke = this.Ellipse.Stroke, Height = this.Ellipse.Height, Width = this.Ellipse.Width, StrokeThickness = this.Ellipse.StrokeThickness},
                Thickness = this.Thickness,
                Width = this.Width,
                Height = this.Height,
                Size = this.Size,
                Gap = this.Gap,
                Outline = this.Outline,
                CrosshairColor = this.CrosshairColor,
                OutlineColor = this.OutlineColor,
                OutlineThickness = this.OutlineThickness
            };
        }
        #endregion
    }
}
