using CrosshairSelector.MVVM.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Windows.Threading;

namespace CrosshairSelector
{
    public class MainViewModel : NotifyPropertyChanged
    {
        public static event EventHandler<PageChangedEventArgs>? OnCrosshairAdded;
        public static event EventHandler<PageChangedEventArgs>? OnCrosshairDeleted;
        public static event EventHandler<CrosshairsRequestedEventArgs>? OnCrosshairRequested;

        public int IndexOfCurrentPage
        {
            get
            {
                return _pages.FindIndex(x => x == CurrentPage);
            }
        }
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
            AddHomePage();
            model = new Model(new Crosshair());
            CrosshairConfigViewModel.OnCrosshairModifed += CrosshairModifedHandler!;
            CrosshairConfigViewModel.OnChangeShape += ChangeShapeHandler!;
            CrosshairConfigViewModel.OnTabRequested += AddTab!;
            CrosshairConfigViewModel.OnSaveConfig += SaveCrosshairConfig!;
            HomePageViewModel.CrosshairDeleted += CrosshairDeletedHandler!;
            HomePageViewModel.ConfigSaved += SaveCrosshairConfig!;
            HomePageViewModel.CrosshairAdded += AddTab!;
            HomePageViewModel.CrosshairEdited += EditCrosshair!;
        }
        #endregion // Constructor

        #region Destructor
        ~MainViewModel()
        {
            CrosshairConfigViewModel.OnCrosshairModifed -= CrosshairModifedHandler!;
            CrosshairConfigViewModel.OnChangeShape -= ChangeShapeHandler!;
            CrosshairConfigViewModel.OnTabRequested -= AddTab!;
            CrosshairConfigViewModel.OnSaveConfig -= SaveCrosshairConfig!;
            HomePageViewModel.CrosshairDeleted -= CrosshairDeletedHandler!;
            HomePageViewModel.ConfigSaved -= SaveCrosshairConfig!;
            HomePageViewModel.CrosshairAdded -= AddTab!;
            HomePageViewModel.CrosshairEdited -= EditCrosshair!;
        }
        #endregion // Destructor

        #region Eventhandlers
        private void CrosshairModifedHandler(object sender, CrosshairModifiedEventArgs e)
        {
            model.ModifyCrosshair(e.Crosshair);
        }
        private void ChangeShapeHandler(object sender, CrosshairModifiedEventArgs e)
        {
            model.ChangeShape(e.Crosshair.Shape);
        }
        private void AddTab(object sender, CrosshairModifiedEventArgs e)
        {
            CrosshairConfigPage c = new CrosshairConfigPage();
            Frame frame = WrapCrosshairConfigPage(c);
            _pages.Add(frame);
            if ((Crosshair)e.Crosshair != null)
            {
                _crosshairConfig.Add((Crosshair)e.Crosshair);
            }
            else
            {
                _crosshairConfig.Add(new Crosshair());
            }
        }
        private void SaveCrosshairConfig(object sender, CrosshairModifiedEventArgs e)
        {
            string xmlPath = "crosshair.xml";
            if (e.Crosshair != null)
            {
                _crosshairConfig.Add((Crosshair)e.Crosshair);
            }
            Model.SaveCrosshair(xmlPath, _crosshairConfig);
        }
        private void CrosshairDeletedHandler(object sender, CrosshairModifiedEventArgs e)
        {
            for (int i = 0; i < _pages.Count; i++)
            {
                if (_pages[i].Content.GetType().Name == typeof(CrosshairConfigPage).Name)
                {
                    if (GetViewModel(i).Crosshair == e.Crosshair)
                    {
                        OnCrosshairDeleted?.Invoke(this, new PageChangedEventArgs(_pages[i].Content as CrosshairConfigPage));
                        _pages.RemoveAt(i);
                        _crosshairConfig.list.Remove((Crosshair)e.Crosshair);
                    }
                }
            }
        }
        private void EditCrosshair(object sender, CrosshairEditedEventArgs e)
        {
            ChangePage(e.CrosshairName ?? "");
        }
        #endregion // Eventhandlers
        private void AddHomePage()
        { 
            Frame frame = new Frame();
            frame.Content = new HomePage();
            _pages.Add(frame);
            CurrentPage = _pages.First();
        }
        public void SaveCurrentCrosshair(string pageName)
        {
            for (int i = 0; i < _pages.Count; i++)
            {
                if (_pages[i].Content != null && _pages[i].Content.GetType().Name == typeof(CrosshairConfigPage).Name)
                {
                    if ((_pages[i].Content as CrosshairConfigPage).Name == pageName)
                    {
                        GetViewModel(i).SaveCrosshair();
                    }
                }
            }
        }
        private Frame WrapCrosshairConfigPage(CrosshairConfigPage page)
        {
            Frame frame = new Frame();
            frame.Content = page;
            frame.NavigationService.Navigated += (sender, e) =>
            {
                Debug.WriteLine(e.Content.GetType());
                CurrentPage = _pages.Last();
                if (CurrentPage.Content != null)
                {
                    (CurrentPage.Content as CrosshairConfigPage).viewModel.Crosshair = _crosshairConfig.list.Last();
                }
                OnCrosshairAdded?.Invoke(this, new PageChangedEventArgs(e.Content as CrosshairConfigPage));
            };
            return frame;
        }
        private bool ShowActivatedCrosshair(Key key, int index)
        {
            bool res = false;
            if (GetViewModel(index).AssignedKey != "None")
            {
                if (GetViewModel(index).Crosshair.AssignedKey == key)
                {
                    GetViewModel(index).Modify();
                    res = true;
                }
            }
            return res;
        }
        public void ChangeCrosshair(Key key)
        {
            for (int i = 0; i < _pages.Count; i++)
            {
                if (_pages[i].Content.GetType().Name == typeof(CrosshairConfigPage).Name)
                {
                    ShowActivatedCrosshair(key, i);
                }
            }
        }
        public CrosshairConfigViewModel GetViewModel(int index) => (_pages[index].Content as CrosshairConfigPage).viewModel;
        public void LoadCrosshairConfig()
        {
            _crosshairConfig = Model.LoadCrosshair("crosshair.xml");
            for (int i = 0; i < _crosshairConfig.Count; i++)
            {
                _pages.Add(WrapCrosshairConfigPage(new CrosshairConfigPage()));
            }
        }
        public void UpdateCrosshairConfig()
        {
            int index = 0;
            if (_crosshairConfig.Count > 0)
            {
                for (int i = 0; i < _pages.Count; i++)
                {
                    if (_pages[i].Content.GetType().Name == typeof(CrosshairConfigPage).Name)
                    {
                        GetViewModel(i).Crosshair = _crosshairConfig[index];
                        index++;
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
                }
            }
        }
        public void ChangePage(string pageName)
        {
            if (pageName != null && pageName != "")
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
        public void SendCrosshairs()
        {
            OnCrosshairRequested?.Invoke(this, new CrosshairsRequestedEventArgs(_crosshairConfig.list));
        }
    }
}
