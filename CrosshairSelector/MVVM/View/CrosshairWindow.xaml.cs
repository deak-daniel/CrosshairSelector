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
                (crosshair.View as CrosshairView).Up.Stroke = new SolidColorBrush(Colors.Black);
                (crosshair.View as CrosshairView).Down.Stroke = new SolidColorBrush(Colors.Black);
                (crosshair.View as CrosshairView).Left.Stroke = new SolidColorBrush(Colors.Black);
                (crosshair.View as CrosshairView).Right.Stroke = new SolidColorBrush(Colors.Black);
            }
            else
            {
                 (crosshair.View as CrosshairView).Up.Stroke = new SolidColorBrush(crosshair.CrosshairColor);
                 (crosshair.View as CrosshairView).Down.Stroke = new SolidColorBrush(crosshair.CrosshairColor);
                 (crosshair.View as CrosshairView).Left.Stroke = new SolidColorBrush(crosshair.CrosshairColor);
                 (crosshair.View as CrosshairView).Right.Stroke = new SolidColorBrush(crosshair.CrosshairColor);
            }
             (crosshair.View as CrosshairView).Up.Fill = new SolidColorBrush(crosshair.CrosshairColor);
            this.Left = width - ( (crosshair.View as CrosshairView).Up.Width / 2) - (this.Width / 2);
            Canvas.SetLeft( (crosshair.View as CrosshairView).Up, this.ActualWidth / 2);
            Canvas.SetTop( (crosshair.View as CrosshairView).Up, this.ActualHeight / 2 -  (crosshair.View as CrosshairView).Up.Height);
            canvas.Children.Add( (crosshair.View as CrosshairView).Up);
            #endregion

            #region Down
             (crosshair.View as CrosshairView).Down.Fill = new SolidColorBrush(crosshair.CrosshairColor);
            Canvas.SetLeft( (crosshair.View as CrosshairView).Down, this.ActualWidth / 2);
            Canvas.SetTop( (crosshair.View as CrosshairView).Down, this.ActualHeight / 2 +  (crosshair.View as CrosshairView).Left.Height);
            canvas.Children.Add( (crosshair.View as CrosshairView).Down);
            #endregion

            #region Left
             (crosshair.View as CrosshairView).Left.Fill = new SolidColorBrush(crosshair.CrosshairColor);
            this.Top = height - ( (crosshair.View as CrosshairView).Left.Height / 2) - (this.Height / 2);
            Canvas.SetLeft( (crosshair.View as CrosshairView).Left, this.ActualWidth / 2 -  (crosshair.View as CrosshairView).Left.Width);
            Canvas.SetTop( (crosshair.View as CrosshairView).Left, this.ActualHeight / 2);
            canvas.Children.Add( (crosshair.View as CrosshairView).Left);
            #endregion

            #region Right
             (crosshair.View as CrosshairView).Right.Fill = new SolidColorBrush(crosshair.CrosshairColor);
            Canvas.SetLeft( (crosshair.View as CrosshairView).Right, this.ActualWidth / 2 +  (crosshair.View as CrosshairView).Right.Height);
            Canvas.SetTop( (crosshair.View as CrosshairView).Right, this.ActualHeight / 2);
            canvas.Children.Add( (crosshair.View as CrosshairView).Right);
            #endregion

            this.Show();
        }
    }
}
