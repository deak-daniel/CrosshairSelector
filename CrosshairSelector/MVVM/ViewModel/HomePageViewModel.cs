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
        #endregion // Properties

        #region Constructor
        public HomePageViewModel()
        {
            MainViewModel.OnCrosshairRequested += CrosshairRequestedHandler;
            _crosshairList = new List<Crosshair>();
            Crosshairs = new ObservableCollection<string>();
            dict = new Dictionary<string, Crosshair>();
        }
        #endregion // Constructor

        #region Destructor
        ~HomePageViewModel()
        {
            MainViewModel.OnCrosshairRequested -= CrosshairRequestedHandler;
        }
        #endregion // Destructor

        #region Public methods
        public void DeleteCrosshair(string selectedItem)
        {
            CrosshairDeleted?.Invoke(this, new CrosshairModifiedEventArgs(dict[selectedItem]));
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

        #region Events
        public static event EventHandler<CrosshairModifiedEventArgs>? CrosshairDeleted;
        #endregion // Events

    }
}
