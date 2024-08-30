using CrosshairSelector.Windows;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CrosshairSelector
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        CrosshairWindow crosshairWindow = new CrosshairWindow();
        MainViewModel viewModel = new MainViewModel();
        public static Action<Crosshair> ChangeCrosshair;
        public MainWindow()
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
    }
}