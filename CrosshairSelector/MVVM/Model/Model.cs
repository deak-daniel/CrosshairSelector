using CrosshairSelector.MVVM.View;
using System;
using System.Collections.Generic;
using System.IO;
using System;
using System.Xml;
using System.Xml.Serialization;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Input;


namespace CrosshairSelector
{
    public class Model
    {
        ICrosshair Crosshair { get; set; }
        public Model()
        { }
        public Model(ICrosshair crosshair)
        {
            this.Crosshair = crosshair;
        }
        public void ModifyCrosshair(ICrosshair crosshair)
        {
            this.Crosshair = crosshair;
            Crosshair.ModifyCrossView(crosshair);
            CrosshairConfigPage.ChangeCrosshair(crosshair);
        }
        public static CrosshairList LoadCrosshair(string xmlPath)
        {
            DataContractSerializer serializer = new DataContractSerializer(typeof(Crosshair), new List<Type> { typeof(CrosshairList) });
            CrosshairList readIn = new CrosshairList();
            try
            {
                Stream reader = File.OpenRead(xmlPath);
                readIn = (CrosshairList)serializer.ReadObject(reader);
                for (int i = 0; i < readIn.Count; i++)
                {
                    switch (readIn[i].Shape)
                    {
                        case CrosshairShape.Cross:
                            readIn[i].View = new CrossView();
                            break;
                        case CrosshairShape.Cross2:
                            readIn[i].View = new Cross2View();
                            break;
                        default:
                            readIn[i].View = new CrossView();
                            break;
                    }
                }
            }
            catch (FileNotFoundException e)
            {
                Debug.WriteLine("Could not read crosshair config");
            }
            
            return readIn;
        }
        public static void SaveCrosshair(string xmlPath, CrosshairList crosshairs)
        {           
            DataContractSerializer serializer = new DataContractSerializer(typeof(Crosshair), new List<Type> { typeof(CrosshairList) });
            XmlWriterSettings settings = new XmlWriterSettings() { Indent = true };
            using (XmlWriter w = XmlWriter.Create(xmlPath, settings))
            {
                serializer.WriteObject(w, crosshairs);
            }
        }
        public void ChangeShape(CrosshairShape shape)
        {
            Crosshair.ChangeCrosshairShape(shape);
        }
    }
}
