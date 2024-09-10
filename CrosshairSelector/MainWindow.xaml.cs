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
        private GlobalKeyboardHook _globalKeyboardHook;
        public static Action<Crosshair> AddTab;
        public static Action<Key> HandleKeyboard;
        public static Action<Crosshair?> SaveCrosshairConfig;
        CrosshairWindow crosshairWindow = new CrosshairWindow();
        MainViewModel viewModel = new MainViewModel(new CrosshairConfigPage());
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = viewModel;
            crosshairWindow.Topmost = true;
            AddTab = (crosshair) => 
            {
                viewModel.AddTab(new CrosshairConfigPage(), crosshair);
            };
            HandleKeyboard = HandleKeys;
            SaveCrosshairConfig = viewModel.SaveCrosshairConfig;
            _globalKeyboardHook = new GlobalKeyboardHook();
            _globalKeyboardHook.SetHook();
            viewModel.LoadCrosshairConfig();
        }
        protected override void OnClosed(EventArgs e)
        {
            _globalKeyboardHook.Unhook();
            base.OnClosed(e);
        }
        private void HandleKeys(Key key)
        {
            viewModel.ChangeCrosshair(key);
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                viewModel.UpdateCrosshairConfig();
            }
            catch (Exception ex)
            {
                viewModel.AddTab(new CrosshairConfigPage(), new Crosshair());
                Debug.WriteLine(ex.Message);
            }
        }
    }
}