using CrosshairSelector.MVVM.View;
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
        public static Action AddTab;
        MainViewModel viewModel = new MainViewModel(new CrosshairConfigPage());
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = viewModel;
            viewModel.AddTab(new CrosshairConfigPage());
            AddTab = () => 
            {
                viewModel.AddTab(new CrosshairConfigPage());
            };
        }
    }
}