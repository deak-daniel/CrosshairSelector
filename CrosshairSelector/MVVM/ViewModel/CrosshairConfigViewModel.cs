using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CrosshairSelector
{
    public class CrosshairConfigViewModel : NotifyPropertyChanged, ICloneable
    {
        public static event EventHandler<CrosshairModifiedEventArgs?> OnCrosshairModifed;
        public static event EventHandler<CrosshairModifiedEventArgs?> OnChangeShape;
        public static event EventHandler<CrosshairModifiedEventArgs?> OnTabRequested;
        public static event EventHandler<CrosshairModifiedEventArgs?> OnSaveConfig;

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

        private int _outlineOpacity;
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
        private int _opacity;
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
        public CrosshairConfigViewModel()
        {
            _crosshair = new Crosshair();
        }
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

            OnCrosshairModifed?.Invoke(this, new CrosshairModifiedEventArgs(_crosshair));
        }
        public void ChangeShape()
        {
            _crosshair.Shape = Shape;
            OnChangeShape?.Invoke(this, new CrosshairModifiedEventArgs(_crosshair));
        }
        public void SaveCrosshair()
        {
            OnSaveConfig?.Invoke(this, new CrosshairModifiedEventArgs(_crosshair));
        }
        public void AddConfig()
        {
            if (AssignedKey != null)
            {
                OnTabRequested?.Invoke(this, new CrosshairModifiedEventArgs(_crosshair));
            }
            if (AssignedKey == null)
            {
                _crosshair.AssignedKey = Key.None;
                OnTabRequested?.Invoke(this, new CrosshairModifiedEventArgs(_crosshair));
            }
        }
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
