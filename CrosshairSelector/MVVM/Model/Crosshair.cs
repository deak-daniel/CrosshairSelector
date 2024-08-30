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
    [DataContract]
    public class Crosshair
    {
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

        public CrosshairView View { get; set; }
        public class CrosshairView
        {
            private const int Scalar = 1;
            public System.Windows.Shapes.Rectangle Up { get; set; }
            public System.Windows.Shapes.Rectangle Down { get; set; }
            public System.Windows.Shapes.Rectangle Left { get; set; }
            public System.Windows.Shapes.Rectangle Right { get; set; }
            public Tuple<int, int> MiddlePoint { get; set; }
            public CrosshairView(int thickness = 10, int size = 30)
            {
                MiddlePoint = PcInformations.GetMiddlePoint();
                int ScreenWidth = MiddlePoint.Item1;
                int ScreenHeight = MiddlePoint.Item2;

                Up = new System.Windows.Shapes.Rectangle();
                Up.Width = thickness;
                Up.Height = size;
                Down = new System.Windows.Shapes.Rectangle();
                Down.Width = thickness;
                Down.Height = size;
                Left = new System.Windows.Shapes.Rectangle();
                Left.Width = size;
                Left.Height = thickness;
                Right = new System.Windows.Shapes.Rectangle();
                Right.Width = size;
                Right.Height = thickness;
            }
            public void Modify(Crosshair crosshair)
            {
                Up.Width = crosshair.Thickness * Scalar;
                Up.Height = crosshair.Size * Scalar;
                Down.Width = crosshair.Thickness * Scalar;
                Down.Height = crosshair.Size * Scalar;
                Left.Width = crosshair.Size * Scalar;
                Left.Height = crosshair.Thickness * Scalar;
                Right.Width = crosshair.Size * Scalar;
                Right.Height = crosshair.Thickness * Scalar;
            }
        }
        public Crosshair()
        {
            View = new CrosshairView();
            Thickness = 1;
            Opacity = 1;
            Gap = 1;
            Size = 1;
        }
        public void ModifyCrosshairView(int size, int thickness, int gap, int opacity, int red, int green, int blue, bool outline)
        {
            Size = size == 0 ? 1 : size;
            Opacity = opacity == 0 ? 255 : opacity;
            Thickness = thickness == 0 ? 1 : thickness;
            Gap = gap == 0 ? 1 : gap;
            CrosshairColor = System.Windows.Media.Color.FromArgb((byte)opacity, (byte)red, (byte)green, (byte)blue);
            Outline = outline;
            View.Modify(this);
        }
        public void ModifyCrosshairView(Crosshair crosshair)
        {
            Size = crosshair.Size == 0 ? 1 : crosshair.Size;
            Opacity = crosshair.Opacity == 0 ? 255 : crosshair.Opacity;
            Thickness = crosshair.Thickness == 0 ? 1 : crosshair.Thickness;
            //Gap = gap == 0 ? 1 : gap;
            CrosshairColor = crosshair.CrosshairColor;
            Outline = crosshair.Outline;
            View.Modify(this);
        }
        public static Crosshair Load(string xmlPath)
        {
            DataContractSerializer DataContract = new DataContractSerializer(typeof(Crosshair));
            Crosshair readIn;
            Stream reader = File.OpenRead(xmlPath);
            readIn = (Crosshair)DataContract.ReadObject(reader);
            readIn.View = new CrosshairView();
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
