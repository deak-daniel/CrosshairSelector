using CrosshairSelector.ViewModel;
using CrosshairSelector.Windows;
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
    /// Interaction logic for CrosshairConfigControl.xaml
    /// </summary>
    public partial class CrosshairConfigControl : UserControl
    {
        public CrosshairConfigViewModel viewModel;
        public static Action<ICrosshair> ChangeCrosshair;
        public CrosshairConfigControl()
        {
            InitializeComponent();
            viewModel = new CrosshairConfigViewModel();
            this.DataContext = viewModel;
            ChangeCrosshair = CrosshairWindow.DisplayCrosshair;
        }
    }
}
