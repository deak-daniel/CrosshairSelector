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
        int loaded = 0;
        Crosshair bottomLeft = new Crosshair();
        Crosshair topLeft = new Crosshair();
        Crosshair bottomRight = new Crosshair();
        Crosshair topRight = new Crosshair();
        HomePageViewModel viewModel = new HomePageViewModel();
        public HomePage()
        {
            InitializeComponent();
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
                bottomLeft.View = new Cross2View();
                bottomLeft.Thickness = 10;
                bottomLeft.Shape = CrosshairShape.Cross2;
                bottomLeft.Size = 30;
                bottomLeft.Outline = true;
                bottomLeft.Opacity = 255;
                bottomLeft.CrosshairColor = Colors.Red;
                bottomLeft.ModifyCrossView(bottomLeft);
                bottomLeft.View.PutCrosshairOnCanvas(BottomLeft.ActualWidth, BottomLeft.ActualHeight, ref BottomLeft);

                topLeft.View = new Cross2View();
                topLeft.Thickness = 3;
                topLeft.Size = 30;
                topLeft.Outline = true;
                topLeft.Shape = CrosshairShape.Cross2;
                topLeft.Opacity = 255;
                topLeft.CrosshairColor = Colors.Yellow;
                topLeft.ModifyCrossView(topLeft);
                topLeft.View.PutCrosshairOnCanvas(TopLeft.ActualWidth, TopLeft.ActualHeight, ref TopLeft);

                bottomRight.View = new CrossView();
                bottomRight.Thickness = 7;
                bottomRight.Size = 9;
                bottomRight.Outline = true;
                bottomRight.CrosshairColor = Colors.White;
                bottomRight.Gap = 8;
                bottomRight.Shape = CrosshairShape.Cross;
                bottomRight.Opacity = 255;
                bottomRight.ModifyCrossView(bottomRight);
                bottomRight.View.PutCrosshairOnCanvas(BottomRight.ActualWidth, BottomRight.ActualHeight, ref BottomRight);

                topRight.View = new Cross2View();
                topRight.Thickness = 4;
                topRight.Size = 30;
                topRight.Outline = true;
                topRight.Shape = CrosshairShape.Cross2;
                topRight.Opacity = 255;
                topRight.CrosshairColor = Colors.SteelBlue;
                topRight.ModifyCrossView(topRight);
                topRight.View.PutCrosshairOnCanvas(TopRight.ActualWidth, TopRight.ActualHeight, ref TopRight);

                loaded++;
            }
        }
    }
}
