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
        public static event Action<string>? OnCrosshairAdded;
        public static event Action<string>? OnCrosshairDeleted;
        public static event Action<List<Crosshair>>? OnCrosshairRequested;
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
        private void CrosshairModifedHandler(Crosshair crosshair)
        {
            if (crosshair != null)
            {
                model.ModifyCrosshair(crosshair);
            }
        }
        private void ChangeShapeHandler(Crosshair crosshair)
        {
            model.ChangeShape(crosshair);
        }
        private void AddTab(Crosshair crosshair)
        {
            CrosshairConfigPage c = new CrosshairConfigPage();
            Frame frame = WrapCrosshairConfigPage(c);
            model.AddCrosshair(c.viewModel.Crosshair, frame);
            SendCrosshairs();
        }
        private void SaveCrosshairConfig(Crosshair crosshair)
        {
            string xmlPath = "crosshair.xml";
            if (crosshair != null)
            {
                model.AddCrosshair(crosshair);
            }
            Model.SaveCrosshair(xmlPath, model.Crosshairs);
        }
        private void CrosshairDeletedHandler(Crosshair crosshair)
        {
            OnCrosshairDeleted?.Invoke(crosshair.Name);
            model.Pages.Remove(crosshair.Name);
            model.DeleteCrosshair(crosshair);
                   
            if (CurrentPage.Content.GetType().Name == typeof(CrosshairConfigPage).Name)
            {
                ((CurrentPage.Content as CrosshairConfigPage).DataContext as CrosshairConfigViewModel).Show();
            }
            CurrentPage = model.Pages[model.Crosshairs.Last().Name];
            SendCrosshairs();
        }
        private void EditCrosshair(string crosshairName)
        {
            ChangePage(crosshairName ?? "");
        }
        private void ShowCrosshairHandler(Crosshair crosshair)
        {
            if (crosshair != null)
            {
                model.ModifyCrosshair(crosshair);
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
        private Frame WrapCrosshairConfigPage(CrosshairConfigPage page, Crosshair crosshair = null)
        {
            Frame frame = new Frame();
            frame.Content = page;
            frame.NavigationService.Navigated += (sender, e) =>
            {
                Debug.WriteLine(e.Content.GetType());
                if (crosshair != null)
                {
                    CurrentPage = model.Pages[crosshair.Name];
                    (CurrentPage.Content as CrosshairConfigPage).viewModel.Crosshair = model.Crosshairs[crosshair.Name];
                    OnCrosshairAdded?.Invoke(crosshair.Name);
                }
                else
                {
                    CurrentPage = model.Pages.Last().Value;
                    OnCrosshairAdded?.Invoke(model.Pages.Last().Key);
                }
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
                    model.AddCrosshair(model.Crosshairs[i], WrapCrosshairConfigPage(new CrosshairConfigPage(), model.Crosshairs[i]));
                }
            }
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
            OnCrosshairRequested?.Invoke(model.Crosshairs.list);
        }
        #endregion // Public methods
    }
}
