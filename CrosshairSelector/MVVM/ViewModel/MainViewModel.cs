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
        private Frame _currentPage;
        public Frame CurrentPage
        {
            get { return _currentPage; }
            set { _currentPage = value;
                RaisePropertyChanged();
            }
        }
        private Model model;
        #endregion // Properties

        #region Constructor
        public MainViewModel()
        {
            model = Model.Instance;
            model.Initialize();
            AddHomePage();
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
            Frame frame = WrapCrosshairConfigPage(c);
            if ((Crosshair)e.Crosshair != null)
            {
                model.AddCrosshair(new Crosshair(), frame);
            }
            else
            {
                model.AddCrosshair(new Crosshair(), frame);
            }
            if (e.Flag == CrosshairEventFlags.NewCrosshairRequested)
            {
                model.AddCrosshair(new Crosshair(), frame);
            }
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
            OnCrosshairDeleted?.Invoke(this, new PageChangedEventArgs(model.Pages[e.Crosshair.Name].Content as CrosshairConfigPage));
            model.Pages.Remove(e.Crosshair.Name);
            model.DeleteCrosshair((Crosshair)e.Crosshair);
                   
            CurrentPage = model.Pages[model.Crosshairs.list.Last().Name];
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
            frame.Content = model.HomePage;
            CurrentPage = frame;
        }
        private Frame WrapCrosshairConfigPage(CrosshairConfigPage page)
        {
            Frame frame = new Frame();
            frame.Content = page;
            frame.NavigationService.Navigated += (sender, e) =>
            {
                Debug.WriteLine(e.Content.GetType());
                CurrentPage = model.Pages[model.Crosshairs.Last().Name];
                if (CurrentPage.Content != null)
                {
                    (CurrentPage.Content as CrosshairConfigPage).viewModel.Crosshair = model.Crosshairs.Last();
                }
                OnCrosshairAdded?.Invoke(this, new PageChangedEventArgs(e.Content as CrosshairConfigPage));
            };
            return frame;
        }
        #endregion // Private methods

        #region Public methods
        public void LoadCrosshairConfig()
        {
            bool res = OnLoadRequest.Invoke("crosshair.xml");
            if (res)
            {
                for (int i = 0; i < model.Crosshairs.Count; i++)
                {
                    model.AddCrosshair(model.Crosshairs[i], WrapCrosshairConfigPage(new CrosshairConfigPage()));
                }
            }
        }
        public void UpdateCrosshairConfig()
        {
            //if (model.Crosshairs.Count == 0) return;
            //int index = 0;
            //for (int i = 0; i < _pages.Count; i++)
            //{
            //    if (_pages[i].Content.GetType().Name == typeof(CrosshairConfigPage).Name)
            //    {
            //        GetViewModel(i).Crosshair = model.Crosshairs[index];
            //        index++;
            //    }
            //}
        }
        public void ChangePage(HomePage page)
        {
            Frame frame = new Frame();
            frame.Content = page;
            CurrentPage = frame;
        }
        public void ChangePage(string pageName)
        {
            if (pageName == null || pageName == "")
            {
                return;
            }
            CurrentPage = model.Pages[pageName];
            (model.Pages[pageName].Content as CrosshairConfigPage).viewModel.Show();
            
        }
        public void SendCrosshairs()
        {
            OnCrosshairRequested?.Invoke(this, new CrosshairsRequestedEventArgs(model.Crosshairs.list));
        }
        #endregion // Public methods
    }
}
