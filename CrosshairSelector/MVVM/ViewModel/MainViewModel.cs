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
        public static Action<Crosshair> ModifyCrosshairHandler;
        public static Action<CrosshairShape> ChangeShapeHandler;
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

        public MainViewModel(Page userControl)
        {
            _crosshairConfig = new CrosshairList();
            Tabs = new ObservableCollection<TabItem>();
            model = new Model();
            ModifyCrosshairHandler = OnModifyCrosshair;
            ChangeShapeHandler = OnChangeShape;
        }
        public void OnModifyCrosshair(Crosshair crosshair)
        {
            model.ModifyCrosshair(crosshair);
        }
        public void OnChangeShape(CrosshairShape shape)
        {
            model.ChangeShape(shape);
        }
        public void AddTab(Page usercontrol, Crosshair crosshair, string header = "Crosshair")
        {
            TabItem tabItem = new TabItem();
            tabItem.Header = header + (Tabs.Count+1);
            tabItem.Content = new Frame() { Content = usercontrol };
            Tabs.Add(tabItem);
            _crosshairConfig.Add(crosshair);
        }
        public void ChangeCrosshair(Key key)
        {
            int index = 0;
            while (index < Tabs.Count && GetViewModel(index).AssignedKey.ToKey() != key)
            {
                index++;
            }
            if (index < Tabs.Count)
            {
                GetViewModel(index).Modify();
            }
        }
        public CrosshairConfigViewModel GetViewModel(int index) => (((Tabs[index] as TabItem).Content as Frame).Content as CrosshairConfigPage).viewModel;
        public void SaveCrosshairConfig(Crosshair? crosshair)
        {
            string xmlPath = "crosshair.xml";
            if (crosshair != null)
            {
                _crosshairConfig.Add(crosshair);
            }
            Model.SaveCrosshair(xmlPath, _crosshairConfig);
        }
        public void LoadCrosshairConfig(Page usercontrol)
        {
            Tabs = new ObservableCollection<TabItem>();
            _crosshairConfig = Model.LoadCrosshair("crosshair.xml");
            for (int i = 0; i < _crosshairConfig.Count; i++)
            {
                TabItem tabItem = new TabItem();
                Frame frame = new Frame();
                frame.Content = usercontrol;
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
                    GetViewModel(i).Modify();
                }
            }
            else
            {
                throw new Exception("No crosshairconfig file");
            }
        }
    }
}
