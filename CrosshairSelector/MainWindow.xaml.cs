using SDL2;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

using CrosshairSelector.MVVM.View;
using CrosshairSelector.Windows;
using CrosshairSelector.ViewModel;

namespace CrosshairSelector
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private GlobalKeyboardHook _globalKeyboardHook;
        private GlobalMouseWheelHook _globalMouseWheelHook;
        public static event Action<byte> OnControllerSwitch;
        CrosshairWindow crosshairWindow = new CrosshairWindow();
        MainViewModel viewModel;
        public MainWindow()
        {
            InitializeComponent();
            InitializeSDL();
            viewModel = new MainViewModel();
            this.DataContext = viewModel;
            crosshairWindow.Topmost = true;
            _globalKeyboardHook = new GlobalKeyboardHook();
            _globalKeyboardHook.SetHook();
            _globalMouseWheelHook = new GlobalMouseWheelHook();
            //_globalMouseWheelHook.Start();
            MainViewModel.OnCrosshairAdded += OnCrosshairAddedHandler!;
            MainViewModel.OnCrosshairDeleted += OnCrosshairDeletedHandler!;
            MainViewModel.OnCrosshairChanged += OnCrosshairChanged!;
            viewModel.LoadCrosshairConfig();
        }
        ~MainWindow()
        {
            MainViewModel.OnCrosshairChanged -= OnCrosshairChanged!;
            MainViewModel.OnCrosshairDeleted -= OnCrosshairDeletedHandler!;
            MainViewModel.OnCrosshairAdded -= OnCrosshairAddedHandler!;
        }
        private void InitializeSDL()
        {
            if (SDL.SDL_Init(SDL.SDL_INIT_GAMECONTROLLER) < 0)
            {
                Application.Current.Shutdown();
            }

            int joystickCount = SDL.SDL_NumJoysticks();
            for (int i = 0; i < joystickCount; i++)
            {
                if (SDL.SDL_IsGameController(i) == SDL.SDL_bool.SDL_TRUE)
                {
                    IntPtr controller = SDL.SDL_GameControllerOpen(i);
                    if (controller != IntPtr.Zero)
                    {
                        Debug.WriteLine($"Controller {i} connected: {SDL.SDL_GameControllerName(controller)}");
                    }
                }
            }

            // Start polling for events
            CompositionTarget.Rendering += OnRender;
        }
        private void OnRender(object sender, EventArgs ev)
        {
            while (SDL.SDL_PollEvent(out SDL.SDL_Event e) != 0)
            {
                if (e.type == SDL.SDL_EventType.SDL_CONTROLLERBUTTONDOWN)
                {
                    OnControllerSwitch?.Invoke(e.cbutton.button);
                }
            }
        }
        protected override void OnClosed(EventArgs e)
        {
            _globalKeyboardHook.Unhook();
            _globalMouseWheelHook.Stop();
            CrosshairWindow.Closed.Invoke();
            base.OnClosed(e);
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                viewModel.SendCrosshairs();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        private void HomePage_click(object sender, RoutedEventArgs e)
        {
            viewModel.ChangePage(HomeControl.Instance);
        }
        private void Crosshair_click(object sender, RoutedEventArgs e)
        {
            viewModel.ChangePage((sender as RadioButton).Content.ToString());
        }
        private void Crosshair_lostfocus(object sender, RoutedEventArgs e)
        {
            //viewModel.SaveCurrentCrosshair((sender as RadioButton).Content.ToString());
        }
        private void OnCrosshairChanged(string name)
        {
            for (int i = 0; i < SidePanel.Children.Count; i++)
            {
                (SidePanel.Children[i] as RadioButton).IsChecked = false;
            }
            int index = 0;
            while (index < SidePanel.Children.Count && (SidePanel.Children[index] as RadioButton).Content.ToString() != name)
            {
                index++;
            }
            if (index < SidePanel.Children.Count)
            {
                (SidePanel.Children[index] as RadioButton).IsChecked = true;
            }
        }
        private void OnCrosshairAddedHandler(string crosshairName)
        {
            for (int i = 0; i < SidePanel.Children.Count; i++)
            {
                (SidePanel.Children[i] as RadioButton).IsChecked = false;
            }
            RadioButton radioButton = new RadioButton();
            radioButton.LostFocus += Crosshair_lostfocus;
            radioButton.Click += Crosshair_click;
            radioButton.Style = (Style)FindResource("SideButton");
            radioButton.Content = crosshairName;
            radioButton.IsChecked = true;
            SidePanel.Children.Add(radioButton);
        }
        private void OnCrosshairDeletedHandler(string crosshairName)
        {
            for (int i = 0; i < SidePanel.Children.Count; i++)
            {
                if (crosshairName == (SidePanel.Children[i] as RadioButton).Content.ToString())
                {
                    SidePanel.Children.RemoveAt(i);
                    (SidePanel.Children[i - 1] as RadioButton).IsChecked = true;
                    break;
                }
            }
        }
    }
}