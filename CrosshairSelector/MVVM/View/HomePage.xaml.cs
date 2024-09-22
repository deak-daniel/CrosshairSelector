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
        HomePageViewModel viewModel = new HomePageViewModel();
        public HomePage()
        {
            InitializeComponent();
            this.DataContext = viewModel;
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            string selectedItem = CrosshairListbox.SelectedItem.ToString();
            viewModel.DeleteCrosshair(selectedItem);
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            string selectedItem = CrosshairListbox.SelectedItem.ToString();
            viewModel.EditCrosshair(selectedItem!);
        }

        private void Save_click(object sender, RoutedEventArgs e)
        {
            viewModel.SaveConfig();
            MessageBox.Show("Crosshair saved!");
        }

        private void Add_click(object sender, RoutedEventArgs e)
        {
            viewModel.AddEmptyCrosshair();
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Cross2View bottomLeft = new Cross2View();
            Cross2View topLeft = new Cross2View();
            CrossView bottomRight = new CrossView();
            Cross2View topRight = new Cross2View();

            bottomLeft.Modify(10, 30, true, Colors.Red);
            bottomLeft.PutCrosshairOnCanvas(BottomLeft.ActualWidth, BottomLeft.ActualHeight, ref BottomLeft);

            topLeft.Modify(3, 30, true, Colors.Yellow);
            topLeft.PutCrosshairOnCanvas(TopLeft.ActualWidth, TopLeft.ActualHeight, ref TopLeft);

            bottomRight.Modify(7, 9, 8, true, Colors.White);
            bottomRight.PutCrosshairOnCanvas(BottomRight.ActualWidth, BottomRight.ActualHeight, ref BottomRight);

            topRight.Modify(4, 30, true, Colors.SteelBlue);
            topRight.PutCrosshairOnCanvas(TopRight.ActualWidth, TopRight.ActualHeight, ref TopRight);

        }
    }
}
