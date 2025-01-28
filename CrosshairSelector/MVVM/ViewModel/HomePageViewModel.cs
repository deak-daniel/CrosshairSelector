using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CrosshairSelector
{
    public class HomePageViewModel : NotifyPropertyChanged
    {

        #region Events
        public static event Action<Crosshair>? CrosshairDeleted;
        public static event Action<Crosshair>? ConfigSaved;
        public static event Action<string>? CrosshairEdited;
        public static event Action<Crosshair?>? CrosshairAdded;
        public static event Action<bool, bool, bool>? SwitchingTypeUpdated;
        #endregion // Events

        #region Fields
        private List<Crosshair> _crosshairList;
        private Dictionary<string, Crosshair> dict;
        private Model model = Model.Instance;
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
            CrosshairDeleted?.Invoke(dict[selectedItem]);
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
            CrosshairEdited?.Invoke(selectedItem);
        }
        #endregion // Public methods

        #region Eventhandlers
        private void CrosshairRequestedHandler(List<Crosshair> crosshairs)
        { 
            dict = new Dictionary<string, Crosshair>();
            Crosshairs = new ObservableCollection<string>();
            for (int i = 0; i < crosshairs.Count; i++)
            {
                _crosshairList.Add(crosshairs[i]);
                Crosshairs.Add($"Crosshair{i + 1}");
                dict.Add($"Crosshair{i + 1}", crosshairs[i]);
            }
        }
        #endregion // Eventhandlers
    }
}
