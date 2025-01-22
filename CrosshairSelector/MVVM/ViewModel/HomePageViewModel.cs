using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrosshairSelector
{
    public class HomePageViewModel : NotifyPropertyChanged
    {

        #region Events
        public static event EventHandler<CrosshairModifiedEventArgs>? CrosshairDeleted;
        public static event EventHandler<CrosshairModifiedEventArgs>? ConfigSaved;
        public static event EventHandler<CrosshairEditedEventArgs>? CrosshairEdited;
        public static event EventHandler<CrosshairModifiedEventArgs>? CrosshairAdded;
        public static event Action<bool, bool, bool>? SwitchingTypeUpdated;
        #endregion // Events

        #region Fields
        private List<Crosshair> _crosshairList;
        Dictionary<string, Crosshair> dict;
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
            _crosshairList = new List<Crosshair>();
            Crosshairs = new ObservableCollection<string>();
            dict = new Dictionary<string, Crosshair>();
        }
        #endregion // Constructor

        #region Destructor
        ~HomePageViewModel()
        {
            MainViewModel.OnCrosshairRequested -= CrosshairRequestedHandler!;
        }
        #endregion // Destructor

        #region Public methods
        public void DeleteCrosshair(string selectedItem)
        {
            CrosshairDeleted?.Invoke(this, new CrosshairModifiedEventArgs(dict[selectedItem]));
            _crosshairList.Remove(dict[selectedItem]);
            dict.Remove(selectedItem);
            Crosshairs.Remove(selectedItem);
        }
        public void SaveConfig()
        {
            ConfigSaved?.Invoke(this, new CrosshairModifiedEventArgs());
        }
        public void AddEmptyCrosshair()
        {
            CrosshairAdded?.Invoke(this, new CrosshairModifiedEventArgs());
        }
        public void AddCrosshair(Crosshair crosshair)
        {
            CrosshairAdded?.Invoke(this, new CrosshairModifiedEventArgs(crosshair));
            if (!_crosshairList.Contains(crosshair))
            {
                int number = 1;
                _crosshairList.Add(crosshair);
                if (Crosshairs.Count > 0)
                {
                    string subs = Crosshairs.Last().Remove(0, "Crosshair".Length);
                    number = int.Parse(subs) + 1;
                }
                Crosshairs.Add($"Crosshair{number}");
                dict.Add($"Crosshair{number}", crosshair);
            }
        }
        public void EditCrosshair(string selectedItem)
        {
            CrosshairEdited?.Invoke(this, new CrosshairEditedEventArgs(selectedItem));
        }
        #endregion // Public methods

        #region Eventhandlers
        private void CrosshairRequestedHandler(object sender, CrosshairsRequestedEventArgs e)
        {
            dict = new Dictionary<string, Crosshair>();
            Crosshairs = new ObservableCollection<string>();
            for (int i = 0; i < e.Crosshairs.Count; i++)
            {
                _crosshairList.Add(e.Crosshairs[i]);
                Crosshairs.Add($"Crosshair{i + 1}");
                dict.Add($"Crosshair{i + 1}", e.Crosshairs[i]);
            }
        }
        #endregion // Eventhandlers
    }
}
