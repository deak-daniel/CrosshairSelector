using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Runtime.Serialization;
using System.ComponentModel.Design;
using System.IO;


namespace CrosshairSelector
{
    public enum CrosshairShape
    {
        Cross,
        Cross2
        //Circle
    }

    [DataContract]
    public class Crosshair : ICrosshair
    {
        #region Properties

        [DataMember]
        public int Thickness { get; set; }

        [DataMember]
        public int Opacity { get; set; }

        [DataMember]
        public int Gap { get; set; }

        [DataMember]
        public int Size { get; set; }

        [DataMember]
        public bool Outline { get; set; }

        [DataMember]
        public System.Windows.Media.Color CrosshairColor { get; set; }
        public ICrosshairView View { get; set; }
        #endregion // Properties

        #region Constructor
        /// <summary>
        /// Default constructor
        /// </summary>
        public Crosshair()
        {
            View = new CrossView();
            Thickness = 1;
            Opacity = 1;
            Gap = 1;
            Size = 1;
        }
        #endregion // Default Constructor

        #region ICrosshair interface implementation
        public void ModifyCrossView(int size, int thickness, int gap, int opacity, int red, int green, int blue, bool outline)
        {
            Size = size == 0 ? 1 : size;
            Opacity = opacity == 0 ? 255 : opacity;
            Thickness = thickness == 0 ? 1 : thickness;
            Gap = gap == 0 ? 1 : gap;
            CrosshairColor = System.Windows.Media.Color.FromArgb((byte)opacity, (byte)red, (byte)green, (byte)blue);
            Outline = outline;
            View.Modify(this);
        }
        public void ModifyCrossView(ICrosshair crosshair)
        {
            Size = crosshair.Size == 0 ? 1 : crosshair.Size;
            Opacity = crosshair.Opacity == 0 ? 255 : crosshair.Opacity;
            Thickness = crosshair.Thickness == 0 ? 1 : crosshair.Thickness;
            //Gap = gap == 0 ? 1 : gap;
            CrosshairColor = crosshair.CrosshairColor;
            Outline = crosshair.Outline;
            View.Modify(this);
        }
        public void ChangeCrosshairShape(CrosshairShape newShape)
        {
            switch (newShape)
            {
                case CrosshairShape.Cross:
                    View = new CrossView();
                    break;
                case CrosshairShape.Cross2:
                    View = new Cross2View();
                    break;
                //case CrosshairShape.Circle:
                //    View = new CircleView();
                //    break;
                default:
                    break;
            }
        }
        #endregion // ICrosshair interface implementation

        public static Crosshair Load(string xmlPath)
        {
            DataContractSerializer DataContract = new DataContractSerializer(typeof(Crosshair));
            Crosshair readIn;
            Stream reader = File.OpenRead(xmlPath);
            readIn = (Crosshair)DataContract.ReadObject(reader);
            readIn.View = new CrossView();
            return readIn;
        }
        public void Save(string xmlPath)
        {
            DataContractSerializer DataContract = new DataContractSerializer(typeof(Crosshair));
            Stream stream = File.Create(xmlPath);
            DataContract.WriteObject(stream, this);
            stream.Close();
        }
    }

}
