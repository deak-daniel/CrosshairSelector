using CrosshairSelector.MVVM.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.PortableExecutable;
using System.Runtime.Serialization;
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
        #region Events
        public static event EventHandler<PageChangedEventArgs>? OnCrosshairAdded;
        public static event EventHandler<PageChangedEventArgs>? OnCrosshairDeleted;
        public static event EventHandler<CrosshairsRequestedEventArgs>? OnCrosshairRequested;
        public static event Func<string, bool>? OnLoadRequest;
        #endregion // Events

        #region Properties
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
        private List<Frame> _pages;
        #endregion // Properties

        #region Constructor
        public MainViewModel()
        {
            _pages = new List<Frame>();
            AddHomePage();
            model = Model.Instance;
            model.Initialize();
            CrosshairConfigViewModel.OnCrosshairModifed += CrosshairModifedHandler!;
            CrosshairConfigViewModel.OnShowRequested += ShowCrosshairHandler!;
            CrosshairConfigViewModel.OnChangeShape += ChangeShapeHandler!;
            CrosshairConfigViewModel.OnTabRequested += AddTab!;
            CrosshairConfigViewModel.OnSaveConfig += SaveCrosshairConfig!;
            CrosshairConfigViewModel.OnDeleteCrosshair += CrosshairDeletedHandler!;
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
            CrosshairConfigViewModel.OnShowRequested -= ShowCrosshairHandler!;
            CrosshairConfigViewModel.OnChangeShape -= ChangeShapeHandler!;
            CrosshairConfigViewModel.OnTabRequested -= AddTab!;
            CrosshairConfigViewModel.OnSaveConfig -= SaveCrosshairConfig!;
            CrosshairConfigViewModel.OnDeleteCrosshair -= CrosshairDeletedHandler!;
            HomePageViewModel.CrosshairDeleted -= CrosshairDeletedHandler!;
            HomePageViewModel.ConfigSaved -= SaveCrosshairConfig!;
            HomePageViewModel.CrosshairAdded -= AddTab!;
            HomePageViewModel.CrosshairEdited -= EditCrosshair!;
        }
        #endregion // Destructor

        #region Eventhandlers
        private void CrosshairModifedHandler(object sender, CrosshairModifiedEventArgs e)
        {
            if (e.Crosshair != null)
            {
                model.ModifyCrosshair(e.Crosshair);
            }
        }
        private void ChangeShapeHandler(object sender, CrosshairModifiedEventArgs e)
        {
            model.ChangeShape(e.Crosshair);
        }
        private void AddTab(object sender, CrosshairModifiedEventArgs e)
        {
            CrosshairConfigPage c = new CrosshairConfigPage();
            if ((Crosshair)e.Crosshair != null)
            {
                model.AddCrosshair((Crosshair)e.Crosshair);
            }
            else
            {
                model.AddCrosshair(new Crosshair());
            }
            if (e.Flag == CrosshairEventFlags.NewCrosshairRequested)
            {
                model.AddCrosshair(new Crosshair());
            }
            Frame frame = WrapCrosshairConfigPage(c);
            _pages.Add(frame);
            SendCrosshairs();
        }
        private void SaveCrosshairConfig(object sender, CrosshairModifiedEventArgs e)
        {
            string xmlPath = "crosshair.xml";
            if (e.Crosshair != null)
            {
                model.AddCrosshair((Crosshair)e.Crosshair);
            }
            Model.SaveCrosshair(xmlPath, model.Crosshairs);
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
                        model.DeleteCrosshair((Crosshair)e.Crosshair);
                    }
                }
            }

            CurrentPage = _pages.Last();
            if (CurrentPage.Content.GetType().Name == typeof(CrosshairConfigPage).Name)
            {
                ((CurrentPage.Content as CrosshairConfigPage).DataContext as CrosshairConfigViewModel).Show();
            }
            SendCrosshairs();
        }
        private void EditCrosshair(object sender, CrosshairEditedEventArgs e)
        {
            ChangePage(e.CrosshairName ?? "");
        }
        private void ShowCrosshairHandler(object sender, CrosshairModifiedEventArgs e)
        {
            if (e.Crosshair != null)
            {
                model.ModifyCrosshair(e.Crosshair);
            }
        }
        #endregion // Eventhandlers

        #region Private methods
        private void AddHomePage()
        {
            Frame frame = new Frame();
            frame.Content = new HomePage();
            _pages.Add(frame);
            CurrentPage = _pages.First();
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
                    (CurrentPage.Content as CrosshairConfigPage).viewModel.Crosshair = model.Crosshairs.list.Last();
                }
                OnCrosshairAdded?.Invoke(this, new PageChangedEventArgs(e.Content as CrosshairConfigPage));
            };
            return frame;
        }
        #endregion // Private methods

        #region Public methods
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
        public void ChangeCrosshair(Key key)
        {
            if (!model.KeyboardSwitch)
            {
                return;
            }
            for (int i = 0; i < _pages.Count; i++)
            {
                if (_pages[i].Content.GetType().Name == typeof(CrosshairConfigPage).Name)
                {
                    CrosshairConfigViewModel viewModel = GetViewModel(i);
                    model.KeyboardSwitching(viewModel.Crosshair, key);
                }
            }
        }
        public CrosshairConfigViewModel GetViewModel(int index) => (_pages[index].Content as CrosshairConfigPage).viewModel;
        public void LoadCrosshairConfig()
        {
            bool res = OnLoadRequest.Invoke("crosshair.xml");
            if (res)
            {
                for (int i = 0; i < model.Crosshairs.Count; i++)
                {
                    _pages.Add(WrapCrosshairConfigPage(new CrosshairConfigPage()));
                }
            }
        }
        public void UpdateCrosshairConfig()
        {
            if (model.Crosshairs.Count == 0) return;
            int index = 0;
            for (int i = 0; i < _pages.Count; i++)
            {
                if (_pages[i].Content.GetType().Name == typeof(CrosshairConfigPage).Name)
                {
                    GetViewModel(i).Crosshair = model.Crosshairs[index];
                    index++;
                }
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
            if (pageName == null || pageName == "")
            {
                return;
            }
            for (int i = 0; i < _pages.Count; i++)
            {
                if (_pages[i].Content != null && _pages[i].Content.GetType().Name == typeof(CrosshairConfigPage).Name)
                {
                    if ((_pages[i].Content as CrosshairConfigPage)!.Name == pageName)
                    {
                        CurrentPage = _pages[i];
                        GetViewModel(i).Show();
                    }
                }
            }
            
        }
        public void SendCrosshairs()
        {
            OnCrosshairRequested?.Invoke(this, new CrosshairsRequestedEventArgs(model.Crosshairs.list));
        }
        #endregion // Public methods
    }
}
