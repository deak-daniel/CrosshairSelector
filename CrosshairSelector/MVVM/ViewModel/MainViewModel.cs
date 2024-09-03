using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace CrosshairSelector
{
    public class MainViewModel : NotifyPropertyChanged
    {
        private Model Model;

        private Crosshair _crosshair;

        private int _thickness;
		public int Thickness
		{
			get {
                return _thickness;
            }
			set { _thickness = value;
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
        private int _red;

        public int Red
        {
            get
            {
                return _red;
            }
            set { _red = value;
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
            set { _green = value;
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
            set { _blue = value;
                RaisePropertyChanged();
                Modify();
            }
        }

        private bool _outline;
                
        public bool Outline
        {
            get {
                return _outline;
            }
            set { _outline = value;
                RaisePropertyChanged();
                Modify();
            }
        }

        private CrosshairShape _shape;

        public CrosshairShape Shape
        {
            get { return _shape; }
            set { _shape = value; 
                RaisePropertyChanged();
                Modify();
            }
        }

        public MainViewModel()
        {
            try
            {
                _crosshair = Model.LoadCrosshair();
            }
            catch (FileNotFoundException e)
            {
                _crosshair = new Crosshair();
            }
            Model = new Model(ref _crosshair);

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
            Modify();
            Size = _crosshair.Size;
            Gap = _crosshair.Gap;
            Thickness = _crosshair.Thickness;
            Outline = _crosshair.Outline;
            Opacity = _crosshair.Opacity;
            Red = _crosshair.CrosshairColor.R;
            Green = (int)_crosshair.CrosshairColor.G;
            Blue = (int)_crosshair.CrosshairColor.B;
        }
        public void Modify()
        {
            Model.ModifyCrosshair(Size, Thickness, Gap, Opacity, Red, Green, Blue, Outline);
        }
        public void SaveCrosshair()
        {
            string xmlPath = "crosshair.xml";
            Model.SaveCrosshair(xmlPath);
        }
    }
}
