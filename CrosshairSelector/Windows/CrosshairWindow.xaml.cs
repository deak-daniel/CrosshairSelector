using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CrosshairSelector.Windows
{
    /// <summary>
    /// Interaction logic for CrosshairWindow.xaml
    /// </summary>
    public partial class CrosshairWindow : Window
    {
        public static Action<Crosshair> DisplayCrosshair;
        public CrosshairWindow()
        {
            InitializeComponent();
            this.Width = 480.0;
            this.Height = 270.0;
            DisplayCrosshair = changeCrosshair;
        }
        public void changeCrosshair(Crosshair crosshair)
        {
            canvas.Children.Clear();

            Tuple<int, int> screenRes = PcInformations.GetResolution();
            double width = (screenRes.Item1 / 2);
            double height = (screenRes.Item2 / 2);

            #region Up
            if (crosshair.Outline)
            {
                crosshair.View.Up.Stroke = new SolidColorBrush(Colors.Black);
                crosshair.View.Down.Stroke = new SolidColorBrush(Colors.Black);
                crosshair.View.Left.Stroke = new SolidColorBrush(Colors.Black);
                crosshair.View.Right.Stroke = new SolidColorBrush(Colors.Black);
            }
            else
            {
                crosshair.View.Up.Stroke = new SolidColorBrush(crosshair.CrosshairColor);
                crosshair.View.Down.Stroke = new SolidColorBrush(crosshair.CrosshairColor);
                crosshair.View.Left.Stroke = new SolidColorBrush(crosshair.CrosshairColor);
                crosshair.View.Right.Stroke = new SolidColorBrush(crosshair.CrosshairColor);
            }
            crosshair.View.Up.Fill = new SolidColorBrush(crosshair.CrosshairColor);
            this.Left = width - (crosshair.View.Up.Width / 2) - (this.Width / 2);
            Canvas.SetLeft(crosshair.View.Up, this.ActualWidth / 2);
            Canvas.SetTop(crosshair.View.Up, this.ActualHeight / 2 - crosshair.View.Up.Height);
            canvas.Children.Add(crosshair.View.Up);
            #endregion

            #region Down
            crosshair.View.Down.Fill = new SolidColorBrush(crosshair.CrosshairColor);
            Canvas.SetLeft(crosshair.View.Down, this.ActualWidth / 2);
            Canvas.SetTop(crosshair.View.Down, this.ActualHeight / 2 + crosshair.View.Left.Height);
            canvas.Children.Add(crosshair.View.Down);
            #endregion

            #region Left
            crosshair.View.Left.Fill = new SolidColorBrush(crosshair.CrosshairColor);
            this.Top = height - (crosshair.View.Left.Height / 2) - (this.Height / 2);
            Canvas.SetLeft(crosshair.View.Left, this.ActualWidth / 2 - crosshair.View.Left.Width);
            Canvas.SetTop(crosshair.View.Left, this.ActualHeight / 2);
            canvas.Children.Add(crosshair.View.Left);
            #endregion

            #region Right
            crosshair.View.Right.Fill = new SolidColorBrush(crosshair.CrosshairColor);
            Canvas.SetLeft(crosshair.View.Right, this.ActualWidth / 2 + crosshair.View.Right.Height);
            Canvas.SetTop(crosshair.View.Right, this.ActualHeight / 2);
            canvas.Children.Add(crosshair.View.Right);
            #endregion

            this.Show();
        }
    }
}
