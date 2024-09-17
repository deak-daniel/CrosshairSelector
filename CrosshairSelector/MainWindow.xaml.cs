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
        int index = 1;
        private GlobalKeyboardHook _globalKeyboardHook;
        public static Action<Key> HandleKeyboard;
        CrosshairWindow crosshairWindow = new CrosshairWindow();
        MainViewModel viewModel = new MainViewModel();
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = viewModel;
            crosshairWindow.Topmost = true;
            HandleKeyboard = HandleKeys;
            _globalKeyboardHook = new GlobalKeyboardHook();
            _globalKeyboardHook.SetHook();
            viewModel.LoadCrosshairConfig();
            MainViewModel.OnCrosshairAdded += OnCrosshairAddedHandler;
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
                Debug.WriteLine(ex.Message);
            }
        }

        private void Settings_click(object sender, RoutedEventArgs e)
        {
            Page page = new SettingsPage();
            viewModel.ChangePage(page);
        }

        private void HomePage_click(object sender, RoutedEventArgs e)
        {
            viewModel.ChangePage(new HomePage());
        }
        private void Crosshair_click(object sender, RoutedEventArgs e)
        {
            viewModel.ChangePage((sender as RadioButton).Content.ToString());
        }
        private void Crosshair_lostfocus(object sender, RoutedEventArgs e)
        {
            viewModel.SaveCurrentCrosshair((sender as RadioButton).Content.ToString());
        }
        private void OnCrosshairAddedHandler(object sender, PageChangedEventArgs e)
        {
            for (int i = 0; i < SidePanel.Children.Count; i++)
            {
                (SidePanel.Children[i] as RadioButton).IsChecked = false;
            }
            RadioButton radioButton = new RadioButton();
            e.Source.Name = "Crosshair" + index;
            radioButton.LostFocus += Crosshair_lostfocus;
            radioButton.Click += Crosshair_click;
            radioButton.Style = (Style)FindResource("SideButton");
            radioButton.Content = "Crosshair" + index;
            SidePanel.Children.Add(radioButton);
            (SidePanel.Children[viewModel.IndexOfCurrentPage] as RadioButton).IsChecked = true;
            index++;
        }
    }
}