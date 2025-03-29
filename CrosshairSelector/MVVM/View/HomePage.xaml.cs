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
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : Page
    {
        private static HomePage instance;
        public static HomePage Instance { 
            get 
            {
                lock (typeof(MainWindow))
                {
                    if (instance == null)
                    {
                        instance = new HomePage();
                    }
                }
                
                return instance;
            }
        }
        int loaded = 0;
        Crosshair bottomLeft = new Crosshair();
        Crosshair topLeft = new Crosshair();
        Crosshair bottomRight = new Crosshair();
        Crosshair topRight = new Crosshair();
        private HomePageViewModel viewModel;
        public HomePageViewModel ViewModel { get => viewModel; } 
        public HomePage()
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
            Crosshair crosshair = new Crosshair();
            string name = ((sender as Border)?.Child as Canvas)?.Name ?? "";
            switch (name)
            {
                case "TopLeft":
                    crosshair = (Crosshair)topLeft.Clone();
                    crosshair.View.RemoveCrosshairFromCanvas(ref TopLeft);
                    break;
                case "TopRight":
                    crosshair = (Crosshair)topRight.Clone();
                    crosshair.View.RemoveCrosshairFromCanvas(ref TopRight);
                    break;
                case "BottomRight":
                    crosshair = (Crosshair)bottomRight.Clone();
                    crosshair.View.RemoveCrosshairFromCanvas(ref BottomRight);
                    break;
                case "BottomLeft":
                    crosshair = (Crosshair)bottomLeft.Clone();
                    crosshair.View.RemoveCrosshairFromCanvas(ref BottomLeft);
                    break;
                default:
                    break;
            }
            if (crosshair != default(Crosshair))
            {
                viewModel.AddCrosshair(crosshair);
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (loaded == 0)
            {
                bottomLeft = CrosshairFactory.CreateCrosshair(CrosshairShape.Cross, 10, 20, true, 255, 10, 255, 1, Colors.White, Colors.Red);
                bottomLeft.View.PutCrosshairOnCanvas(BottomLeft.ActualWidth, BottomLeft.ActualHeight, ref BottomLeft);

                topLeft = CrosshairFactory.CreateCrosshair(CrosshairShape.Cross2, 10, 20, false, 255, 10, 0, 0, Colors.White, Colors.Red);
                topLeft.View.PutCrosshairOnCanvas(TopLeft.ActualWidth, TopLeft.ActualHeight, ref TopLeft);

                bottomRight = CrosshairFactory.CreateCrosshair(CrosshairShape.Triangle, 10, 20, true, 255, 5, 255, 1, Colors.Violet, Colors.Black);
                bottomRight.View.PutCrosshairOnCanvas(BottomRight.ActualWidth, BottomRight.ActualHeight, ref BottomRight);

                topRight = CrosshairFactory.CreateCrosshair(CrosshairShape.Cross, 1, 6, false, 255, 5, 0, 0, Colors.Yellow, Colors.Black);
                topRight.View.PutCrosshairOnCanvas(TopRight.ActualWidth, TopRight.ActualHeight, ref TopRight);

                loaded++;
            }
        }
    }
}
