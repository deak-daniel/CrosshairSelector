using CrosshairSelector.MVVM.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace CrosshairSelector
{
    public class MainViewModel : NotifyPropertyChanged
    {
        private ObservableCollection<TabItem> _tabs;
        private Model model;
        private CrosshairList _crosshairConfig;
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

        #region Constructor
        public MainViewModel()
        {
            _crosshairConfig = new CrosshairList();
            Tabs = new ObservableCollection<TabItem>();
            model = new Model(new Crosshair());
            CrosshairConfigViewModel.OnCrosshairModifed += CrosshairModifedHandler;
            CrosshairConfigViewModel.OnChangeShape += ChangeShapeHandler;
            CrosshairConfigViewModel.OnTabRequested += AddTab;
            CrosshairConfigViewModel.OnSaveConfig += SaveCrosshairConfig;
        }
        #endregion // Constructor

        #region Destructor
        ~MainViewModel()
        {
            CrosshairConfigViewModel.OnCrosshairModifed -= CrosshairModifedHandler;
            CrosshairConfigViewModel.OnChangeShape -= ChangeShapeHandler;
            CrosshairConfigViewModel.OnTabRequested -= AddTab;
            CrosshairConfigViewModel.OnSaveConfig -= SaveCrosshairConfig;
        }
        #endregion // Destructor

        public void CrosshairModifedHandler(object sender, CrosshairModifiedEventArgs e)
        {
            model.ModifyCrosshair(e.Crosshair);
        }
        public void ChangeShapeHandler(object sender, CrosshairModifiedEventArgs e)
        {
            model.ChangeShape(e.Crosshair.Shape);
        }
        public void AddEmptyPage()
        {
            TabItem tabItem = new TabItem();
            tabItem.Header = "Crosshair" + (Tabs.Count + 1);
            tabItem.Content = new Frame() { Content = new CrosshairConfigPage() };
            Tabs.Add(tabItem);
        }
        public void AddTab(object sender, CrosshairModifiedEventArgs e)
        {
            TabItem tabItem = new TabItem();
            tabItem.Header = "Crosshair" + (Tabs.Count+1);
            tabItem.Content = new Frame() { Content = new CrosshairConfigPage() };
            Tabs.Add(tabItem);
            _crosshairConfig.Add((Crosshair)e.Crosshair);
        }
        public void ChangeCrosshair(Key key)
        {
            int index = 0;
            for (int i = 0; i < Tabs.Count; i++)
            {
                if (GetViewModel(i).AssignedKey != "None")
                {
                    if (GetViewModel(i).Crosshair.AssignedKey == key)
                    {
                        GetViewModel(i).Modify();
                    }
                }
            }
        }
        public CrosshairConfigViewModel GetViewModel(int index) => (((Tabs[index] as TabItem).Content as Frame).Content as CrosshairConfigPage).viewModel;
        public void SaveCrosshairConfig(object sender, CrosshairModifiedEventArgs e)
        {
            string xmlPath = "crosshair.xml";
            if (e.Crosshair != null && !_crosshairConfig.list.Contains(e.Crosshair))
            {
                _crosshairConfig.Add((Crosshair)e.Crosshair);
            }
            Model.SaveCrosshair(xmlPath, _crosshairConfig);
        }
        public void LoadCrosshairConfig()
        {
            Tabs = new ObservableCollection<TabItem>();
            _crosshairConfig = Model.LoadCrosshair("crosshair.xml");
            for (int i = 0; i < _crosshairConfig.Count; i++)
            {
                TabItem tabItem = new TabItem();
                Frame frame = new Frame();
                frame.Content = new CrosshairConfigPage();
                tabItem.Header = "Crosshair" + (Tabs.Count + 1);
                tabItem.Content = frame;
                Tabs.Add(tabItem);
            }
        }
        public void UpdateCrosshairConfig()
        {
            if (_crosshairConfig.Count > 0)
            {
                for (int i = 0; i < _crosshairConfig.Count; i++)
                {
                    GetViewModel(i).Crosshair = _crosshairConfig.list[i];
                }
            }
            else
            {
                throw new Exception("No crosshairconfig file");
            }
        }
    }
}
