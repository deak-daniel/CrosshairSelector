using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace CrosshairSelector
{
    public class MainViewModel : NotifyPropertyChanged
    {
        private ObservableCollection<TabItem> _tabs;

        public ObservableCollection<TabItem> Tabs
        {
            get { return _tabs; }
            set { _tabs = value;
                RaisePropertyChanged();
            }
        }
        private int _last_index;

        public int lastIndex
        {
            get { return _last_index; }
            set { _last_index = value;
                RaisePropertyChanged();
            }
        }

        public MainViewModel(Page userControl)
        {
            Tabs = new ObservableCollection<TabItem>();
        }
        public void AddTab(Page usercontrol, string header = "Crosshair")
        {
            TabItem tabItem = new TabItem();
            tabItem.Header = header + (Tabs.Count+1);
            tabItem.Content = new Frame() { Content = usercontrol };
            Tabs.Add(tabItem);
        }

    }
}
