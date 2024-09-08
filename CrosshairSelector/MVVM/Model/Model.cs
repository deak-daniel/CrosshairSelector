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
        Crosshair Crosshair { get; set; }
        public Model()
        { }
        public void ModifyCrosshair(Crosshair crosshair)
        {
            this.Crosshair = crosshair;
            Crosshair.ModifyCrossView(crosshair);
            CrosshairConfigPage.ChangeCrosshair(crosshair);
        }
        public void ModifyCrosshair(int size, int thickness, int gap, int opacity, int red, int green, int blue, bool outline, Key assignedKey)
        {
            Crosshair.ModifyCrossView(size, thickness, gap, opacity, red, green, blue, outline, assignedKey);
            CrosshairConfigPage.ChangeCrosshair(Crosshair);
        }
        public static CrosshairList LoadCrosshair(string xmlPath)
        {
            DataContractSerializer serializer = new DataContractSerializer(typeof(Crosshair), new List<Type> { typeof(CrosshairList) });
            CrosshairList readIn = new CrosshairList();
            try
            {
                Stream reader = File.OpenRead(xmlPath);
                readIn = (CrosshairList)serializer.ReadObject(reader);
            }
            catch (Exception)
            {
                Debug.WriteLine("Could not read crosshair config");
            }
            return readIn;
        }
        public static void SaveCrosshair(string xmlPath, CrosshairList crosshairs)
        {
            if (crosshairs.list.Any(x => x.AssignedKey == Key.None))
            {
                for (int i = 0; i < crosshairs.Count; i++)
                {
                    if (crosshairs.list[i].AssignedKey == Key.None)
                    {
                        crosshairs.list.RemoveAt(i);
                    }
                }
            }
            DataContractSerializer serializer = new DataContractSerializer(typeof(Crosshair), new List<Type> { typeof(CrosshairList) });
            XmlWriterSettings settings = new XmlWriterSettings() { Indent = true };
            using (XmlWriter w = XmlWriter.Create(xmlPath, settings))
            {
                serializer.WriteObject(w, crosshairs);
            }
            Process notepad = new Process();
            notepad.StartInfo.FileName = xmlPath;
            notepad.StartInfo.Arguments = xmlPath;
            notepad.StartInfo.UseShellExecute = true;
            notepad.Start();
        }
        public void ChangeShape(CrosshairShape shape)
        {
            Crosshair.ChangeCrosshairShape(shape);
        }
    }
}
