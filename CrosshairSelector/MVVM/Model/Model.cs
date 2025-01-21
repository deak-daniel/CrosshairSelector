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
using System.Windows.Controls;


namespace CrosshairSelector
{
    public class Model
    {
        #region Events
        #endregion // Events

        #region Fields
        private int currentCrosshairIndex = 1;
        private bool keyboardSwitching = true;
        private bool controllerSwitching = false;
        private bool mouseWheelSwitching = false;
        #endregion // Fields

        #region Properties
        public CrosshairList Crosshairs 
        { 
            get => _crosshairConfig; 
        }
        public bool KeyboardSwitch
        {
            get => keyboardSwitching;
        }
        public bool ControllerSwitch
        {
            get => controllerSwitching;
        }
        public bool ScrollSwitch
        {
            get => mouseWheelSwitching;
        }
        #endregion // Properties

        private static Model instance;
        public static Model Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (typeof(MainWindow))
                    {
                        instance = new Model();
                    }
                }
                return instance;
            }
        }
        private CrosshairList _crosshairConfig;
        private Model()
        {
            _crosshairConfig = new CrosshairList();
            GlobalMouseWheelHook.MouseWheelScrolled += ScrollCrosshair!;
            HomePageViewModel.SwitchingTypeUpdated += SwitchingTypeUpdatedHandler!;
            MainWindow.OnControllerSwitch += ControllerSwitching!;
            MainViewModel.OnLoadRequest += LoadCrosshair;
        }
        ~Model()
        {
            GlobalMouseWheelHook.MouseWheelScrolled -= ScrollCrosshair!;
            HomePageViewModel.SwitchingTypeUpdated -= SwitchingTypeUpdatedHandler!;
            MainWindow.OnControllerSwitch -= ControllerSwitching!;
            MainViewModel.OnLoadRequest -= LoadCrosshair;
        }
        public void Initialize()
        {
            _crosshairConfig = new CrosshairList();
        }
        public void ModifyCrosshair(ICrosshair crosshair)
        {
            if (_crosshairConfig.Contains(crosshair))
            {
                _crosshairConfig[_crosshairConfig.list.IndexOf((Crosshair)crosshair)].ModifyCrossView(crosshair);
                CrosshairConfigPage.ChangeCrosshair(crosshair);
            }
        }
        public bool LoadCrosshair(string xmlPath)
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
                        case CrosshairShape.Triangle:
                            readIn[i].View = new TriangularCrossView();
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
            _crosshairConfig = readIn;
            return true;
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
        public void ChangeShape(ICrosshair crosshair)
        {
            if (_crosshairConfig.Contains(crosshair))
            {
                _crosshairConfig[_crosshairConfig.list.IndexOf((Crosshair)crosshair)].ChangeCrosshairShape(crosshair.Shape);
            }
        }
        public bool DeleteCrosshair(ICrosshair crosshair)
        {
            if (_crosshairConfig.Contains(crosshair))
            {
                _crosshairConfig.list.Remove((Crosshair)crosshair);
                return true;
            }
            return false;
        }
        private void ScrollCrosshair(object sender, MouseWheelEventArgs e)
        {
            if (!mouseWheelSwitching) return;
            if (currentCrosshairIndex >= 0 && currentCrosshairIndex <= _crosshairConfig.Count - 1)
            {
                if (e.Delta > 0 && (currentCrosshairIndex + 1 <= _crosshairConfig.Count - 1)) // UP
                {
                    currentCrosshairIndex++;
                }
                if (e.Delta < 0 && (currentCrosshairIndex - 1 >= 0)) // DOWN
                {
                    currentCrosshairIndex--;
                }
                ModifyCrosshair(_crosshairConfig.list[currentCrosshairIndex]);
            }
        }
        private void ControllerSwitching(byte button)
        {
            string buttonName = "";
            switch (button)
            {
                case 9:
                    buttonName = "LB";
                    break;
                case 10:
                    buttonName = "RB";
                    break;

                default:
                    break;
            }
            if (!controllerSwitching) return;
            if (currentCrosshairIndex >= 0 && currentCrosshairIndex <= _crosshairConfig.Count - 1)
            {
                if (buttonName == "LB" && (currentCrosshairIndex + 1 <= _crosshairConfig.Count - 1)) // UP
                {
                    currentCrosshairIndex++;
                }
                if (buttonName == "RB" && (currentCrosshairIndex - 1 >= 0)) // DOWN
                {
                    currentCrosshairIndex--;
                }
                ModifyCrosshair(_crosshairConfig.list[currentCrosshairIndex]);
            }
        }
        public void AddCrosshair(ICrosshair crosshair)
        {
            _crosshairConfig.Add((Crosshair)crosshair);
        }
        private void SwitchingTypeUpdatedHandler(bool keyboard, bool mousewheel, bool controller)
        {
            mouseWheelSwitching = mousewheel;
            controllerSwitching = controller;
            keyboardSwitching = keyboard;
        }
    }
}
