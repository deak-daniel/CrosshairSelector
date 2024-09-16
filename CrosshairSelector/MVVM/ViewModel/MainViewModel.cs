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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace CrosshairSelector
{
    public class MainViewModel : NotifyPropertyChanged
    {
        public static event EventHandler OnCrosshairAdded;

        private Frame _currentPage;

        public Frame CurrentPage
        {
            get { return _currentPage; }
            set { _currentPage = value;
                RaisePropertyChanged();
            }
        }
        private Model model;
        private CrosshairList _crosshairConfig;
        private int _last_index;
        public int lastIndex
        {
            get { return _last_index; }
            set { _last_index = value;
                RaisePropertyChanged();
            }
        }

        private List<Frame> _pages;

        #region Constructor
        public MainViewModel()
        {
            _crosshairConfig = new CrosshairList();
            _pages = new List<Frame>();
            _pages.Add(WrapPage(new HomePage()));
            CurrentPage = _pages.First();
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

        private Frame WrapPage(Page page)
        {
            Frame frame = new Frame();
            frame.Content = page;
            return frame;
        }
        public void CrosshairModifedHandler(object sender, CrosshairModifiedEventArgs e)
        {
            model.ModifyCrosshair(e.Crosshair);
        }
        public void ChangeShapeHandler(object sender, CrosshairModifiedEventArgs e)
        {
            model.ChangeShape(e.Crosshair.Shape);
        }
        public void AddTab(object sender, CrosshairModifiedEventArgs e)
        {
            Frame frame = new Frame();
            frame.Content = new Frame() { Content = new CrosshairConfigPage() };
            _pages.Add(frame);
            _crosshairConfig.Add((Crosshair)e.Crosshair);
        }
        public void ChangeCrosshair(Key key)
        {
            int index = 0;
            for (int i = 0; i < _pages.Count; i++)
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
        public CrosshairConfigViewModel GetViewModel(int index) => (_pages[index].Content as CrosshairConfigPage).viewModel;
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
            _crosshairConfig = Model.LoadCrosshair("crosshair.xml");
            for (int i = 0; i < _crosshairConfig.Count; i++)
            {
                Frame frame = new Frame();
                frame.Content = new CrosshairConfigPage();
                _pages.Add(frame);
            }
        }
        public void UpdateCrosshairConfig()
        {
            if (_crosshairConfig.Count > 0)
            {
                for (int i = 0; i < _crosshairConfig.Count; i++)
                {
                    if (_pages[i].Content.GetType().Name == typeof(CrosshairConfigPage).Name)
                    {
                        GetViewModel(i).Crosshair = _crosshairConfig.list[i];
                        (_pages[i].Content as CrosshairConfigPage).Name = "Crosshair" + i;
                        OnCrosshairAdded?.Invoke(this, EventArgs.Empty);
                    }
                }
            }
            else
            {
                throw new Exception("No crosshairconfig file");
            }
        }
        public void ChangePage(Page page)
        {
            Type type = page.GetType();
            Frame frame = new Frame();
            frame.Content = page;
            for (int i = 0; i < _pages.Count; i++)
            {
                if (_pages[i].Content != null)
                {
                    if (_pages[i].Content.GetType().Name == type.Name)
                    {
                        CurrentPage = _pages[i];
                    }
                    else
                    {
                        _pages.Add(frame);
                        CurrentPage = frame;
                    }
                }
            }
        }
        public void ChangePage(string pageName)
        {
            for (int i = 0; i < _pages.Count; i++)
            {
                if (_pages[i].Content != null && _pages[i].Content.GetType().Name == typeof(CrosshairConfigPage).Name)
                {
                    if ((_pages[i].Content as CrosshairConfigPage).Name == pageName)
                    {
                        CurrentPage = _pages[i];
                    }
                }
            }
        }
    }
}
