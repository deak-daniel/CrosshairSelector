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
using CrosshairSelector.Model;
using CrosshairSelector.ViewModel;

namespace CrosshairSelector.MVVM.View
{
    /// <summary>
    /// Interaction logic for HomeControl.xaml
    /// </summary>
    public partial class HomeControl : UserControl
    {
        private const string crosshair1 = "Crosshair101";
        private const string crosshair2 = "Crosshair102";
        private const string crosshair3 = "Crosshair103";
        private const string crosshair4 = "Crosshair104";
        private static HomeControl instance;
        public static HomeControl Instance
        {
            get
            {
                lock (typeof(MainWindow))
                {
                    if (instance == null)
                    {
                        instance = new HomeControl();
                    }
                }

                return instance;
            }
        }
        int loaded = 0;
        private HomePageViewModel viewModel;
        public HomePageViewModel ViewModel { get => viewModel; }
        public HomeControl()
        {
            InitializeComponent();
            viewModel = new HomePageViewModel();
            this.DataContext = viewModel;
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (CrosshairListbox.SelectedItem != null)
            {
                string selectedItem = CrosshairListbox.SelectedItem.ToString();
                viewModel.DeleteCrosshair(selectedItem);
            }
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (CrosshairListbox.SelectedItem != null)
            {
                string selectedItem = CrosshairListbox.SelectedItem.ToString();
                viewModel.EditCrosshair(selectedItem!);
            }
        }

        private void Save_click(object sender, RoutedEventArgs e)
        {
            viewModel.SaveConfig();
        }

        private void Add_click(object sender, RoutedEventArgs e)
        {
            viewModel.AddEmptyCrosshair();
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            string name = ((sender as Border)?.Child as Canvas)?.Name ?? "";
            string requestedCrosshair = "";
            switch (name)
            {
                case "TopLeft":
                    requestedCrosshair = crosshair3;
                    break;
                case "TopRight":
                    requestedCrosshair = crosshair4;
                    break;
                case "BottomRight":
                    requestedCrosshair = crosshair1;
                    break;
                case "BottomLeft":
                    requestedCrosshair = crosshair2;
                    break;
                default:
                    break;
            }
            Crosshair c = (Crosshair)viewModel.GetDefaultCrosshair(requestedCrosshair).Clone();
            viewModel.AddCrosshair(c);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (loaded == 0)
            {
                viewModel.AddDefaultCrosshair(ref BottomRight, crosshair1);
                viewModel.AddDefaultCrosshair(ref BottomLeft, crosshair2);
                viewModel.AddDefaultCrosshair(ref TopLeft, crosshair3);
                viewModel.AddDefaultCrosshair(ref TopRight, crosshair4);
                loaded++;
            }
        }
    }
}
