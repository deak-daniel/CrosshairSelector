using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace CrosshairSelector.Model
{
    public sealed class XCrossView : CrosshairViewBase
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
        /// <param name="thickness">optional</param>
        /// <param name="size">optional</param>
        public XCrossView(int thickness = 1, int size = 1)
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
        #endregion // Constructor

        #region CrosshairViewBase implementation
        public override void PutCrosshairOnCanvas(double ActualWidth, double ActualHeight, ref Canvas canvas)
        {
            RotateTransform rotateTransform = new RotateTransform(3*45);
            Up.RenderTransform = rotateTransform;
            Canvas.SetLeft(Up, ActualWidth / 2 - Gap / 4 + Thickness / 2 + 0.8); // upper left
            Canvas.SetTop(Up, ActualHeight / 2 - Gap / 4 /*+ Thickness / 2*/ + 0.2);
            canvas.Children.Add(Up);

            rotateTransform = new RotateTransform(45);
            Down.RenderTransform = rotateTransform;
            Canvas.SetLeft(Down, ActualWidth / 2 - Gap / 4 - Thickness / 4 + 0.5); // Lower left
            Canvas.SetTop(Down, ActualHeight / 2 + Gap / 4 + Thickness / 2 - 0.8);
            canvas.Children.Add(Down);
            
            Left.RenderTransform = rotateTransform;
            Canvas.SetLeft(Left, ActualWidth / 2 + Gap / 4 + Thickness);
            Canvas.SetTop(Left, ActualHeight / 2 + Gap / 4 + Thickness / 2 - 1); // Lower right
            canvas.Children.Add(Left);

            rotateTransform = new RotateTransform(-45); // Upper right
            Right.RenderTransform = rotateTransform;
            Canvas.SetLeft(Right, ActualWidth / 2 + Gap / 4 + Thickness/4);
            Canvas.SetTop(Right, ActualHeight / 2 - Gap / 4);
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
        #endregion // CrosshairViewBase implementation
    }
}
