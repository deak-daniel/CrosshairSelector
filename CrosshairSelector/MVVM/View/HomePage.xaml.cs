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
    }
}
