﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using CrosshairSelector.Model;

namespace CrosshairSelector.ViewModel
{
    public class CrosshairConfigViewModel : NotifyPropertyChanged, IViewModelBase
    {
        #region Events
        public static event Action<Crosshair?>? OnTabRequested;
        public static event Action<Crosshair>? OnDeleteCrosshair;
        #endregion // Events

        #region Fields
        private ModelClass model = ModelClass.Instance;
        #endregion // Fields

        #region Properties

        private Crosshair _crosshair;
        public Crosshair Crosshair
        {
            get { return _crosshair; }
            set 
            { 
                _crosshair = value;
                LoadCrosshair();
            }
        }

        private string _assignedKey;
        public string AssignedKey
        {
            get { return _assignedKey; }
            set { _assignedKey = value;
                RaisePropertyChanged();
                Modify();
            }
        }

        private int _thickness;
        public int Thickness
        {
            get
            {
                return _thickness;
            }
            set
            {
                _thickness = value;
                RaisePropertyChanged();
                Modify();
            }
        }

        private int _size;
        public int Size
        {
            get
            {
                return _size;
            }
            set
            {
                _size = value;
                RaisePropertyChanged();
                Modify();
            }
        }

        private int _gap;
        public int Gap
        {
            get
            {
                return _gap;
            }
            set
            {
                _gap = value;
                RaisePropertyChanged();
                Modify();
            }
        }

        #region outline
        private bool _outline;
        public bool Outline
        {
            get
            {
                return _outline;
            }
            set
            {
                _outline = value;
                RaisePropertyChanged();
                Modify();
            }
        }
        private int _outlineThickness;
        public int OutlineThickness
        {
            get { return _outlineThickness; }
            set { _outlineThickness = value;
                RaisePropertyChanged();
                Modify();
            }
        }

        private int _outlineRed;
        public int OutlineRed
        {
            get { return _outlineRed; }
            set { _outlineRed = value;
                RaisePropertyChanged();
                Modify();
            }
        }

        private int _outlineGreen;
        public int OutlineGreen
        {
            get { return _outlineGreen; }
            set { _outlineGreen = value;
                RaisePropertyChanged();
                Modify();
            }
        }

        private int _outlineBlue;
        public int OutlineBlue
        {
            get { return _outlineBlue; }
            set { _outlineBlue = value;
                RaisePropertyChanged();
                Modify();
            }
        }

        private int _outlineOpacity = 255;
        public int OutlineOpacity
        {
            get { return _outlineOpacity; }
            set { _outlineOpacity = value;
                RaisePropertyChanged();
                Modify();
            }
        }
        #endregion

        #region crosshair color
        private int _red;
        public int Red
        {
            get
            {
                return _red;
            }
            set
            {
                _red = value;
                RaisePropertyChanged();
                Modify();
            }
        }

        private int _green;
        public int Green
        {
            get
            {
                return _green;
            }
            set
            {
                _green = value;
                RaisePropertyChanged();
                Modify();
            }
        }

        private int _blue;
        public int Blue
        {
            get
            {
                return _blue;
            }
            set
            {
                _blue = value;
                RaisePropertyChanged();
                Modify();
            }
        }
        private int _opacity = 255;
        public int Opacity
        {
            get
            {
                return _opacity;
            }
            set
            {
                _opacity = value;
                RaisePropertyChanged();
                Modify();
            }
        }
        #endregion

        private ObservableCollection<CrosshairShape> _crosshairTypes;
        public ObservableCollection<CrosshairShape> CrosshairTypes
        {
            get { return _crosshairTypes; }
            set { _crosshairTypes = value;
                RaisePropertyChanged();
                Modify();
            }
        }


        private CrosshairShape _shape;
        public CrosshairShape Shape
        {
            get { return _shape; }
            set
            {
                _shape = value;
                RaisePropertyChanged();
                ChangeShape();
                Modify();
            }
        }

        #endregion // Properties

        #region Commands
        private RelayCommand addCrosshair;
        public ICommand AddCrosshairCommand
        {
            get { return addCrosshair; }
        }

        private RelayCommand saveCrosshair;

        public ICommand SaveCrosshairCommand
        {
            get { return saveCrosshair; }
        }

        private RelayCommand deleteCrosshair;

        public ICommand DeleteCrosshairCommand
        {
            get { return deleteCrosshair; }
        }


        #endregion // Commands

        #region Constructor
        public CrosshairConfigViewModel()
        {
            _crosshair = new Crosshair();
            
            if (FillCrosshairTypes(out ObservableCollection<CrosshairShape> tempList))
            {
                CrosshairTypes = tempList;
            }
            addCrosshair = new RelayCommand(AddConfig);
            saveCrosshair = new RelayCommand(SaveCrosshair);
            deleteCrosshair = new RelayCommand(DeleteCrosshair);
        }
        #endregion // Constructor

        #region Public methods
        public void LoadCrosshair()
        {
            _gap = _crosshair.Gap;
            _size = _crosshair.Size;
            _thickness = _crosshair.Thickness;
            _outline = _crosshair.Outline;
            _opacity = _crosshair.Opacity;
            _red = (int)_crosshair.CrosshairColor.R;
            _green = (int)_crosshair.CrosshairColor.G;
            _blue = (int)_crosshair.CrosshairColor.B;
            _outlineRed = (int)_crosshair.OutlineColor.R;
            _outlineGreen = (int)_crosshair.OutlineColor.G;
            _outlineBlue = (int)_crosshair.OutlineColor.B;
            _outlineOpacity = _crosshair.OutlineOpacity;
            _outlineThickness = _crosshair.OutlineThickness;
            _assignedKey = _crosshair.AssignedKey.ToStringFromKey();
            _shape = _crosshair.Shape;
            Modify();
            Shape = _crosshair.Shape;
            Size = _crosshair.Size;
            Gap = _crosshair.Gap;
            Thickness = _crosshair.Thickness;
            Outline = _crosshair.Outline;
            Opacity = _crosshair.Opacity;
            Red = _crosshair.CrosshairColor.R;
            Green = _crosshair.CrosshairColor.G;
            Blue = _crosshair.CrosshairColor.B;
            OutlineOpacity = _crosshair.OutlineOpacity;
            OutlineRed = _crosshair.OutlineColor.R;
            OutlineGreen = _crosshair.OutlineColor.G;
            OutlineBlue = _crosshair.OutlineColor.B;
            OutlineThickness = _crosshair.OutlineThickness;
            AssignedKey = _crosshair.AssignedKey.ToStringFromKey();
        }
        public void Modify()
        {
            _crosshair.Gap = Gap;
            _crosshair.Shape = Shape;
            _crosshair.Size = Size;
            _crosshair.Thickness = Thickness;
            _crosshair.Outline = Outline;
            _crosshair.Opacity = Opacity;
            _crosshair.OutlineOpacity = OutlineOpacity;
            _crosshair.CrosshairColor = System.Windows.Media.Color.FromArgb((byte)Opacity, (byte)Red, (byte)Green, (byte)Blue);
            _crosshair.OutlineColor = System.Windows.Media.Color.FromArgb((byte)OutlineOpacity, (byte)OutlineRed, (byte)OutlineGreen, (byte)OutlineBlue);
            _crosshair.OutlineThickness = OutlineThickness;
            _crosshair.AssignedKey = AssignedKey.ToKey();

            model.ModifyCrosshair(_crosshair);
        }
        public void ChangeShape()
        {
            _crosshair.Shape = Shape;
            model.ChangeShape(_crosshair);
        }
        public bool Show(Crosshair previous = null)
        {
            bool res = false;
            try
            {
                model.ModifyCrosshair(previous ?? _crosshair);
                res = true;
            }
            catch (Exception)
            {
                res = false;
            }
            return res;
        }
        #endregion // Public methods

        #region Private methods
        private bool FillCrosshairTypes(out ObservableCollection<CrosshairShape> list)
        {
            bool res = false;
            list = new ObservableCollection<CrosshairShape>();
            try
            {
                foreach (CrosshairShape item in Enum.GetValues<CrosshairShape>())
                {
                    list.Add(item);
                }
                res = true;
            }
            catch (NullReferenceException)
            {
                res = false;
            }
            return res;
        }
        private void SaveCrosshair()
        {
            model.SaveCrosshairConfig(_crosshair);
        }
        private void AddConfig()
        {
            OnTabRequested?.Invoke(null);
        }
        private void DeleteCrosshair()
        {
            OnDeleteCrosshair?.Invoke(_crosshair);
        }
        #endregion // Private methods
    }
}
