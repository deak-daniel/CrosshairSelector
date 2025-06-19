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
using System.Net.Http.Headers;
using System.Xml.Linq;

using CrosshairSelector.Core;
using CrosshairSelector.ViewModel;

namespace CrosshairSelector.Model
{
    public class ModelClass
    {

        #region Fields
        private HomeControl homePage;
        private int currentCrosshairIndex = 1;
        private CrosshairList crosshairConfig;
        private static ModelClass instance;
        private Dictionary<string, UserControl> pageDict;
        #endregion // Fields

        #region Properties
        public Parameters ParameterClass { get; private set; }
        public HomeControl HomePage { get => HomeControl.Instance; }
        public CrosshairList Crosshairs { get => crosshairConfig; }
        public Dictionary<string, UserControl> Pages { get => pageDict; }
        public bool KeyboardSwitch { get => ParameterClass.KeyboardSwitch; }
        public bool ControllerSwitch { get => ParameterClass.ControllerSwitch; }
        public bool ScrollSwitch { get => ParameterClass.MouseSwitch; }
        public static ModelClass Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (typeof(MainWindow))
                    {
                        instance = new ModelClass();
                    }
                }
                return instance;
            }
        }
        #endregion // Properties

        #region Constructor
        private ModelClass()
        {
            ParameterClass = new Parameters();
            crosshairConfig = new CrosshairList();
            pageDict = new Dictionary<string, UserControl>();
            GlobalMouseWheelHook.MouseWheelScrolled += ScrollCrosshair!;
            HomePageViewModel.SwitchingTypeUpdated += SwitchingTypeUpdatedHandler!;
            MainWindow.OnControllerSwitch += ControllerSwitching!;
            GlobalKeyboardHook.OnKeyPressed += KeyboardSwitching;
        }
        #endregion // Constructor

        #region Destructor
        ~ModelClass()
        {
            GlobalMouseWheelHook.MouseWheelScrolled -= ScrollCrosshair!;
            HomePageViewModel.SwitchingTypeUpdated -= SwitchingTypeUpdatedHandler!;
            MainWindow.OnControllerSwitch -= ControllerSwitching!;
            GlobalKeyboardHook.OnKeyPressed -= KeyboardSwitching;
        }
        #endregion // Destructor

        #region Public methods
        public void Initialize()
        {
            crosshairConfig = new CrosshairList();
        }
        public void ModifyCrosshair(ICrosshair crosshair)
        {
            if (crosshairConfig.Contains(crosshair))
            {
                crosshairConfig[crosshair.Name].ModifyCrossView(crosshair);
                CrosshairConfigControl.ChangeCrosshair(crosshair);
            }
        }
        public void ChangeShape(ICrosshair crosshair)
        {
            if (crosshairConfig.Contains(crosshair))
            {
                crosshairConfig[crosshairConfig.IndexOf((Crosshair)crosshair)].ChangeCrosshairShape(crosshair.Shape);
            }
        }
        public bool DeleteCrosshair(ICrosshair crosshair)
        {
            bool res = false;
            if (crosshairConfig.Count(x => x.Name == crosshair.Name) > 0)
            {
                crosshairConfig.Remove((Crosshair)crosshair);
                res = true;
            }
            return res;
        }
        public void AddCrosshair(Crosshair crosshair, UserControl page)
        {
            if (!crosshairConfig.Contains(crosshair) && !pageDict.ContainsKey(crosshair.Name))
            {
                crosshairConfig.Add(crosshair);
                pageDict.Add(crosshairConfig.Last().Name, page);
            }
        }
        public void SaveCrosshairConfig(Crosshair crosshair = null)
        {
            if (crosshair != null)
            {
                crosshairConfig.Add(crosshair);
            }
            bool res = XmlHandler.SaveXml(crosshairConfig, ParameterClass);
            if (!res)
            {
                throw new Exception("Could not save crosshairs.");
            }
        }
        public bool LoadCrosshairList()
        {
            (crosshairConfig, ParameterClass) = XmlHandler.LoadCrosshairList(XmlNames.saveName);
            if (crosshairConfig != null && ParameterClass != null)
            {
                return true;
            }
            return false;
        }
        #endregion // Public methods

        #region Eventhandlers
        private void KeyboardSwitching(Key key)
        {
            if (!ParameterClass.KeyboardSwitch)
            {
                return;
            }
            for (int i = 0; i < crosshairConfig.Count; i++)
            {
                if (crosshairConfig[i].AssignedKey == key)
                {
                    ModifyCrosshair(crosshairConfig[i]);
                }
            }
        }
        private void ScrollCrosshair(object sender, MouseWheelEventArgs e)
        {
            if (!ParameterClass.MouseSwitch) return;
            if (currentCrosshairIndex >= 0 && currentCrosshairIndex <= crosshairConfig.Count - 1)
            {
                if (e.Delta > 0 && (currentCrosshairIndex + 1 <= crosshairConfig.Count - 1)) // UP
                {
                    currentCrosshairIndex++;
                }
                if (e.Delta < 0 && (currentCrosshairIndex - 1 >= 0)) // DOWN
                {
                    currentCrosshairIndex--;
                }
                ModifyCrosshair(crosshairConfig[currentCrosshairIndex]);
            }
        }
        private void ControllerSwitching(byte button)
        {
            string buttonName = "";
            switch (button)
            {
                case 9:
                    buttonName = XmlNames.controllerLb;
                    break;
                case 10:
                    buttonName = XmlNames.controllerRb;
                    break;

                default:
                    break;
            }
            if (!ParameterClass.ControllerSwitch) return;
            if (currentCrosshairIndex >= 0 && currentCrosshairIndex <= crosshairConfig.Count - 1)
            {
                if (buttonName == XmlNames.controllerLb && (currentCrosshairIndex + 1 <= crosshairConfig.Count - 1)) // UP
                {
                    currentCrosshairIndex++;
                }
                if (buttonName == XmlNames.controllerRb && (currentCrosshairIndex - 1 >= 0)) // DOWN
                {
                    currentCrosshairIndex--;
                }
                ModifyCrosshair(crosshairConfig[currentCrosshairIndex]);
            }
        }
        private void SwitchingTypeUpdatedHandler(bool keyboard, bool mousewheel, bool controller)
        {
            ParameterClass.ControllerSwitch = controller;
            ParameterClass.KeyboardSwitch = keyboard;
            ParameterClass.MouseSwitch = mousewheel;
        }
        #endregion // Eventhandlers
    }
}
