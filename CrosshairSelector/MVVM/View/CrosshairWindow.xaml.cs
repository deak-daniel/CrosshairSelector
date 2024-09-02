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
                (crosshair.View as CrossView).Up.Stroke = new SolidColorBrush(Colors.Black);
                (crosshair.View as CrossView).Down.Stroke = new SolidColorBrush(Colors.Black);
                (crosshair.View as CrossView).Left.Stroke = new SolidColorBrush(Colors.Black);
                (crosshair.View as CrossView).Right.Stroke = new SolidColorBrush(Colors.Black);
            }
            else
            {
                 (crosshair.View as CrossView).Up.Stroke = new SolidColorBrush(crosshair.CrosshairColor);
                 (crosshair.View as CrossView).Down.Stroke = new SolidColorBrush(crosshair.CrosshairColor);
                 (crosshair.View as CrossView).Left.Stroke = new SolidColorBrush(crosshair.CrosshairColor);
                 (crosshair.View as CrossView).Right.Stroke = new SolidColorBrush(crosshair.CrosshairColor);
            }
             (crosshair.View as CrossView).Up.Fill = new SolidColorBrush(crosshair.CrosshairColor);
            this.Left = width - ( (crosshair.View as CrossView).Up.Width / 2) - (this.Width / 2);
            Canvas.SetLeft( (crosshair.View as CrossView).Up, this.ActualWidth / 2);
            Canvas.SetTop( (crosshair.View as CrossView).Up, this.ActualHeight / 2 -  (crosshair.View as CrossView).Up.Height);
            canvas.Children.Add( (crosshair.View as CrossView).Up);
            #endregion

            #region Down
             (crosshair.View as CrossView).Down.Fill = new SolidColorBrush(crosshair.CrosshairColor);
            Canvas.SetLeft( (crosshair.View as CrossView).Down, this.ActualWidth / 2);
            Canvas.SetTop( (crosshair.View as CrossView).Down, this.ActualHeight / 2 +  (crosshair.View as CrossView).Left.Height);
            canvas.Children.Add( (crosshair.View as CrossView).Down);
            #endregion

            #region Left
             (crosshair.View as CrossView).Left.Fill = new SolidColorBrush(crosshair.CrosshairColor);
            this.Top = height - ( (crosshair.View as CrossView).Left.Height / 2) - (this.Height / 2);
            Canvas.SetLeft( (crosshair.View as CrossView).Left, this.ActualWidth / 2 -  (crosshair.View as CrossView).Left.Width);
            Canvas.SetTop( (crosshair.View as CrossView).Left, this.ActualHeight / 2);
            canvas.Children.Add( (crosshair.View as CrossView).Left);
            #endregion

            #region Right
             (crosshair.View as CrossView).Right.Fill = new SolidColorBrush(crosshair.CrosshairColor);
            Canvas.SetLeft( (crosshair.View as CrossView).Right, this.ActualWidth / 2 +  (crosshair.View as CrossView).Right.Height);
            Canvas.SetTop( (crosshair.View as CrossView).Right, this.ActualHeight / 2);
            canvas.Children.Add( (crosshair.View as CrossView).Right);
            #endregion

            this.Show();
        }
    }
}
