﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
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
        private static Tuple<int, int> screenRes = PcInformations.GetResolution();
        private static double width = (screenRes.Item1 / 2);
        private static double height = (screenRes.Item2 / 2);
        public static Action<ICrosshair> DisplayCrosshair;
        public static Action Closed;
        public CrosshairWindow()
        {
            InitializeComponent();
            this.Width = 480.0;
            this.Height = 270.0;
            DisplayCrosshair = changeCrosshair;
            Closed = CloseWindow;
        }
        private void CloseWindow()
        {
            this.Close();
        }
        public void changeCrosshair(ICrosshair crosshair)
        {
            canvas.Children.Clear();
            crosshair.View.PutCrosshairOnCanvas(this.ActualWidth, this.ActualHeight ,ref canvas);
            this.Left = width - (crosshair.View.Width / 2) - (this.Width / 2);
            this.Top = height - (crosshair.View.Height / 2) - (this.Height / 2);
            this.Show();
        }
    }
}
