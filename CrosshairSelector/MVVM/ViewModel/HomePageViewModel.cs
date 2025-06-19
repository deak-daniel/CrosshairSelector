using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
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
        private List<Crosshair> crosshairList;
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
            crosshairList = new List<Crosshair>();
            Crosshairs = new ObservableCollection<string>();
            defaultCrosshairs = CrosshairFactory.CreateCrosshairs() ?? new CrosshairList();
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
            Crosshair item = crosshairList.FirstOrDefault(x => x.Name == selectedItem)!;
            CrosshairDeleted?.Invoke(item);
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
        public void AddDefaultCrosshair(ref Canvas canvas, string crosshairName)
        {
            Crosshair crosshair = defaultCrosshairs.Find(x => x.Name == crosshairName);
            crosshair?.View.PutCrosshairOnCanvas(canvas.ActualWidth, canvas.ActualHeight,ref canvas);
        }
        public void RemoveDefaultCrosshair(ref Canvas canvas, string crosshairName)
        {
            Crosshair crosshair = defaultCrosshairs.Find(x => x.Name == crosshairName);
            crosshair?.View.RemoveCrosshairFromCanvas(ref canvas);
        }
        public void EditCrosshair(string selectedItem)
        {
            CrosshairEdited?.Invoke(selectedItem);
        }
        public Crosshair GetDefaultCrosshair(string name)
        {
            return defaultCrosshairs[name];
        }
        #endregion // Public methods

        #region Eventhandlers
        private void CrosshairRequestedHandler(List<Crosshair> crosshairs)
        { 
            Crosshairs = new ObservableCollection<string>();
            crosshairList = new List<Crosshair>();
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

        #region Fields
        private CrosshairList defaultCrosshairs;
        #endregion
    }
}
