using CrosshairSelector.Core;
using CrosshairSelector.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace CrosshairSelector.Model
{
    public sealed class CrossView : CrosshairViewBase
    {
        #region Properties
        public Rectangle Up { get; private set; }
        public Rectangle Down { get; private set; }
        public Rectangle Left { get; private set; }
        public Rectangle Right { get; private set; }
        #endregion // Properties

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="thickness">Optional</param>
        /// <param name="size">Optional</param>
        public CrossView(int thickness = 1, int size = 1)
        {
            Outline = false;
            Gap = 0;
            Up = new Rectangle();
            Up.Width = thickness;
            Up.Height = size;
            Down = new Rectangle();
            Down.Width = thickness;
            Down.Height = size;
            Left = new Rectangle();
            Left.Width = size;
            Left.Height = thickness;
            Right = new Rectangle();
            Right.Width = size;
            Right.Height = thickness;
            Width = thickness;
            Height = size;
            CrosshairColor = new Color();
        }
        #endregion // constructor

        #region CrosshairViewBase Implementation
        public override void PutCrosshairOnCanvas(double ActualWidth, double ActualHeight, ref Canvas canvas)
        {
            Canvas.SetLeft(Up, ActualWidth / 2);
            Canvas.SetTop(Up, ActualHeight / 2 - Up.Height - Gap / 4);
            canvas.Children.Add(Up);
            
            Canvas.SetLeft(Down, ActualWidth / 2);
            Canvas.SetTop(Down, ActualHeight / 2 + Left.Height + Gap / 4);
            canvas.Children.Add(Down);

            Canvas.SetLeft(Left, ActualWidth / 2 - Left.Width - Gap / 4);
            Canvas.SetTop(Left, ActualHeight / 2);
            canvas.Children.Add(Left);

            Canvas.SetLeft(Right, ActualWidth / 2 + Right.Height + Gap / 4);
            Canvas.SetTop(Right, ActualHeight / 2);
            canvas.Children.Add(Right);
        }
        public override void RemoveCrosshairFromCanvas(ref Canvas canvas)
        {
            canvas.Children.Remove(Up);
            canvas.Children.Remove(Down);
            canvas.Children.Remove(Left);
            canvas.Children.Remove(Right);
        }
        public override void SetSize(int thickness, int size, int gap)
        {
            Thickness = thickness * scalar;
            Size = size * scalar;
            Gap = (gap * scalar) + gapOffset;
            Up.Width = Thickness;
            Up.Height = Size;
            Down.Width = Thickness;
            Down.Height = Size;
            Left.Width = Size;
            Left.Height = Thickness;
            Right.Width = Size;
            Right.Height = Thickness;
            Width = Thickness;
            Height = Thickness;
        }
        public override void SetStyle(bool outline, Color crosshairColor, Color outlineColor, int outlineThickness)
        {
            base.SetStyle(outline, crosshairColor, outlineColor, outlineThickness);
            if (Outline)
            {
                Up.Stroke = new SolidColorBrush(OutlineColor);
                Down.Stroke = new SolidColorBrush(OutlineColor);
                Left.Stroke = new SolidColorBrush(OutlineColor);
                Right.Stroke = new SolidColorBrush(OutlineColor);
                Up.StrokeThickness = OutlineThickness;
                Down.StrokeThickness = OutlineThickness;
                Left.StrokeThickness = OutlineThickness;
                Right.StrokeThickness = OutlineThickness;
            }
            else
            {
                Up.Stroke = new SolidColorBrush(CrosshairColor);
                Down.Stroke = new SolidColorBrush(CrosshairColor);
                Left.Stroke = new SolidColorBrush(CrosshairColor);
                Right.Stroke = new SolidColorBrush(CrosshairColor);
            }

            Up.Fill = new SolidColorBrush(CrosshairColor);
            Down.Fill = new SolidColorBrush(CrosshairColor);
            Left.Fill = new SolidColorBrush(CrosshairColor);
            Right.Fill = new SolidColorBrush(CrosshairColor);
        }
        #endregion // CrosshairViewBase Implementation


        #region ICloneable implementation
        public override object Clone()
        {
            return new CrossView()
            {
                Up = new Rectangle() { Width = this.Up.Width, Height = this.Up.Height, Stroke = this.Up.Stroke, Fill = this.Up.Fill },
                Down = new Rectangle() { Width = this.Down.Width, Height = this.Down.Height, Stroke = this.Down.Stroke, Fill = this.Down.Fill },
                Left = new Rectangle() { Width = this.Left.Width, Height = this.Left.Height, Stroke = this.Left.Stroke, Fill = this.Left.Fill },
                Right = new Rectangle() { Width = this.Right.Width, Height = this.Right.Height, Stroke = this.Right.Stroke, Fill = this.Right.Fill },
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
