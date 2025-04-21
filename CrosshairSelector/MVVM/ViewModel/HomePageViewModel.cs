using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using CrosshairSelector.Core;
using CrosshairSelector.Model;

namespace CrosshairSelector.ViewModel
{
    public class HomePageViewModel : NotifyPropertyChanged, IViewModelBase
    {
        #region Events
        public static event Action<Crosshair>? CrosshairDeleted;
        public static event Action<Crosshair>? ConfigSaved;
        public static event Action<string>? CrosshairEdited;
        public static event Action<Crosshair?>? CrosshairAdded;
        public static event Action<bool, bool, bool>? SwitchingTypeUpdated;
        #endregion // Events

        #region Fields
        private CrosshairList crosshairList;
        private ModelClass model = ModelClass.Instance;
        #endregion // Fields

        #region Properties
        private ObservableCollection<string> _crosshairs;

		public ObservableCollection<string> Crosshairs
		{
			get { return _crosshairs; }
			set { _crosshairs = value;
                RaisePropertyChanged();
            }
		}

        private bool _keyboardSwitch = true;
        public bool KeyboardSwitch
        {
            get { return _keyboardSwitch; }
            set { _keyboardSwitch = value;
                RaisePropertyChanged();
                SwitchingTypeUpdated?.Invoke(_keyboardSwitch, _mouseSwitch, _controllerSwitch);
            }
        }

        private bool _mouseSwitch;
        public bool MouseSwitch
        {
            get { return _mouseSwitch; }
            set
            {
                _mouseSwitch = value;
                RaisePropertyChanged();
                SwitchingTypeUpdated?.Invoke(_keyboardSwitch, _mouseSwitch, _controllerSwitch);
            }
        }

        private bool _controllerSwitch;

        public bool ControllerSwitch
        {
            get { return _controllerSwitch; }
            set { 
                _controllerSwitch = value;
                RaisePropertyChanged();
                SwitchingTypeUpdated?.Invoke(_keyboardSwitch, _mouseSwitch, _controllerSwitch);
            }
        }
        #endregion // Properties

        #region Constructor
        public HomePageViewModel()
        {
            MainViewModel.OnCrosshairRequested += CrosshairRequestedHandler!;
            MainViewModel.OnLoadCompleted += SwitchingLoaded;
            crosshairList = new CrosshairList();
            Crosshairs = new ObservableCollection<string>();
        }
        #endregion // Constructor

        #region Destructor
        ~HomePageViewModel()
        {
            MainViewModel.OnCrosshairRequested -= CrosshairRequestedHandler!; 
            MainViewModel.OnLoadCompleted -= SwitchingLoaded;
        }
        #endregion // Destructor

        #region Public methods
        public void DeleteCrosshair(string selectedItem)
        {
            CrosshairDeleted?.Invoke(crosshairList[selectedItem]);
        }
        public void SaveConfig()
        {
            model.SaveCrosshairConfig();
        }
        public void AddEmptyCrosshair()
        {
            CrosshairAdded?.Invoke(null);
        }
        public void AddCrosshair(Crosshair crosshair)
        {
            CrosshairAdded?.Invoke(crosshair);
            if (!crosshairList.Contains(crosshair))
            {
                int number = 1;
                crosshairList.Add(crosshair);
                if (Crosshairs.Count > 0)
                {
                    string subs = Crosshairs.Last().Remove(0, "Crosshair".Length);
                    number = int.Parse(subs) + 1;
                }
                Crosshairs.Add($"Crosshair{number}");
            }
        }
        public void EditCrosshair(string selectedItem)
        {
            CrosshairEdited?.Invoke(selectedItem);
        }
        #endregion // Public methods

        #region Eventhandlers
        private void CrosshairRequestedHandler(List<Crosshair> crosshairs)
        { 
            Crosshairs = new ObservableCollection<string>();
            crosshairList = new CrosshairList();
            for (int i = 0; i < crosshairs.Count; i++)
            {
                crosshairList.Add(crosshairs[i]);
                Crosshairs.Add(crosshairs[i].Name);
            }
        }
        private void SwitchingLoaded(bool keyboard, bool mouse, bool controller)
        {
            KeyboardSwitch = keyboard;
            MouseSwitch = mouse;
            ControllerSwitch = controller;
        }
        #endregion // Eventhandlers
    }
}
