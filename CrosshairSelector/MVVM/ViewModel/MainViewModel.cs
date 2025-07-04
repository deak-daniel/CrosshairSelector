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

using CrosshairSelector.Model;
using CrosshairSelector.MVVM.View;

namespace CrosshairSelector.ViewModel
{
    public class MainViewModel : NotifyPropertyChanged
    {
        #region Events
        public static event Action<string>? OnCrosshairAdded;
        public static event Action<string>? OnCrosshairDeleted;
        public static event Action<string>? OnCrosshairChanged;
        public static event Action<bool, bool, bool>? OnLoadCompleted;
        public static event Action<List<Crosshair>>? OnCrosshairRequested;
        #endregion // Events

        #region Fields
        private ModelClass model;
        #endregion // Fields

        #region Properties
        private UserControl _currentPage;
        public UserControl CurrentPage
        {
            get { return _currentPage; }
            set { _currentPage = value;
                RaisePropertyChanged();
            }
        }
        #endregion // Properties

        #region Constructor
        public MainViewModel()
        {
            model = ModelClass.Instance;
            model.Initialize();
            AddHomePage();
            CrosshairConfigViewModel.OnDeleteCrosshair += CrosshairDeletedHandler;
            CrosshairConfigViewModel.OnTabRequested += AddTab!;
            HomePageViewModel.CrosshairDeleted += CrosshairDeletedHandler!;
            HomePageViewModel.CrosshairAdded += AddTab!;
            HomePageViewModel.CrosshairEdited += EditCrosshair!;
        }
        #endregion // Constructor

        #region Destructor
        ~MainViewModel()
        {
            CrosshairConfigViewModel.OnDeleteCrosshair -= CrosshairDeletedHandler;
            CrosshairConfigViewModel.OnTabRequested -= AddTab!;
            HomePageViewModel.CrosshairDeleted -= CrosshairDeletedHandler!;
            HomePageViewModel.CrosshairAdded -= AddTab!;
            HomePageViewModel.CrosshairEdited -= EditCrosshair!;
        }
        #endregion // Destructor

        #region Eventhandlers
        private void AddTab(Crosshair crosshair)
        {
            CrosshairConfigControl c = new CrosshairConfigControl();
            c.viewModel.Crosshair = crosshair;
            if (model.AddCrosshair(c.viewModel.Crosshair, c))
            {
                OnCrosshairAdded?.Invoke(model.Crosshairs.Last().Name);
                ChangePage(model.Crosshairs.Last().Name);
                SendCrosshairs();
            }
        }
        private void CrosshairDeletedHandler(Crosshair crosshair)
        {
            model.DeleteCrosshair(crosshair);
            model.Pages.Remove(crosshair.Name);
            if (model.Pages.Count > 0)
            {
                ChangePage(model.Crosshairs.Last().Name);
            }
            else if (model.Pages.Count == 0)
            {
                ChangePage(HomeControl.Instance);
            }
            OnCrosshairDeleted?.Invoke(crosshair.Name);
            SendCrosshairs();
        }
        private void EditCrosshair(string crosshairName)
        {
            ChangePage(crosshairName ?? "");
        }
        #endregion // Eventhandlers

        #region Private methods
        private void AddHomePage()
        {
            CurrentPage = model.HomePage;
        }
        #endregion // Private methods

        #region Public methods
        public void LoadCrosshairConfig()
        {
            bool res = model.LoadCrosshairList();
            if (res && model.Crosshairs.Count > 0)
            {
                for (int i = 0; i < model.Crosshairs.Count; i++)
                {
                    CrosshairConfigControl control = new CrosshairConfigControl();
                    control.viewModel.Crosshair = model.Crosshairs[i];
                    model.Pages.Add(model.Crosshairs[i].Name, control);
                    OnCrosshairAdded?.Invoke(model.Crosshairs[i].Name);
                }
                ChangePage(model.Crosshairs.Last().Name);
                OnLoadCompleted?.Invoke(model.KeyboardSwitch, model.ScrollSwitch, model.ControllerSwitch);
            }
        }
        public void ChangePage(HomeControl control)
        {
            CurrentPage = control;
        }
        public void ChangePage(string pageName)
        {
            if (pageName == null || pageName == "")
            {
                return;
            }
            CurrentPage = model.Pages[pageName];
            (model.Pages[pageName] as CrosshairConfigControl).viewModel.Show();
            OnCrosshairChanged?.Invoke(pageName);
        }
        public void SendCrosshairs()
        {
            OnCrosshairRequested?.Invoke(model.Crosshairs);
        }
        #endregion // Public methods
    }
}
