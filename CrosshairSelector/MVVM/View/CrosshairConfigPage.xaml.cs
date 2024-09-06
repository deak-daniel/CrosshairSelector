using CrosshairSelector.Windows;
using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CrosshairSelector.MVVM.View
{
    /// <summary>
    /// Interaction logic for CrosshairConfigPage.xaml
    /// </summary>
    public partial class CrosshairConfigPage : Page
    {
        CrosshairWindow crosshairWindow = new CrosshairWindow();
        CrosshairConfigViewModel viewModel = new CrosshairConfigViewModel();
        public static Action<Crosshair> ChangeCrosshair;
        public CrosshairConfigPage()
        {
            InitializeComponent();
            this.DataContext = viewModel;
            crosshairWindow.Topmost = true;
            ChangeCrosshair = CrosshairWindow.DisplayCrosshair;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            viewModel.LoadCrosshair();
            viewModel.Modify();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            viewModel.SaveCrosshair();
            MessageBox.Show("Crosshair saved!");
        }

        private void Add_config_click(object sender, RoutedEventArgs e)
        {
            viewModel.AddConfig();
        }
    }
}
