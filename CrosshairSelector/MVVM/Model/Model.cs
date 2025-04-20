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

using CrosshairSelector.ViewModel;

namespace CrosshairSelector
{
    public class Model
    {
        #region Constants
        private readonly string controllerLb = "LB";
        private readonly string controllerRb = "RB";
        private readonly string saveName = "crosshair.xml";
        private readonly string xmlConfig = "Config";
        private readonly string xmlCrosshair = "Crosshair";
        private readonly string xmlName = "Name";
        private readonly string xmlAssignedKey = "AssignedKey";
        private readonly string xmlThickness = "Thickness";
        private readonly string xmlOpacity = "Opacity";
        private readonly string xmlGap = "Gap";
        private readonly string xmlSize = "Size";
        private readonly string xmlOutlineOpacity = "OutlineOpacity";
        private readonly string xmlOutline = "Outline";
        private readonly string xmlCenterDot = "CenterDot";
        private readonly string xmlOutlineThickness = "OutlineThickness";
        private readonly string xmlCrosshairShape = "CrosshairShape";
        private readonly string xmlColor = "Color";
        private readonly string xmlOutlineColor = "OutlineColor";
        private readonly string xmlKeyboard = "Keyboard";
        private readonly string xmlController = "Controller";
        private readonly string xmlScroll = "Scroll";
        #endregion // Constants

        #region Fields
        private HomeControl homePage;
        private int currentCrosshairIndex = 1;
        private CrosshairList _crosshairConfig;
        private static Model instance;
        private Dictionary<string, UserControl> pageDict;
        #endregion // Fields

        #region Properties
        public Parameters ParameterClass { get; private set; }
        public HomeControl HomePage { get => HomeControl.Instance; }
        public CrosshairList Crosshairs { get => _crosshairConfig; }
        public Dictionary<string, UserControl> Pages { get => pageDict; }
        public bool KeyboardSwitch { get => ParameterClass.KeyboardSwitch; }
        public bool ControllerSwitch { get => ParameterClass.ControllerSwitch; }
        public bool ScrollSwitch { get => ParameterClass.MouseSwitch; }
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
        #endregion // Properties

        #region Constructor
        private Model()
        {
            ParameterClass = new Parameters();
            _crosshairConfig = new CrosshairList();
            pageDict = new Dictionary<string, UserControl>();
            GlobalMouseWheelHook.MouseWheelScrolled += ScrollCrosshair!;
            HomePageViewModel.SwitchingTypeUpdated += SwitchingTypeUpdatedHandler!;
            MainWindow.OnControllerSwitch += ControllerSwitching!;
            GlobalKeyboardHook.OnKeyPressed += KeyboardSwitching;
        }
        #endregion // Constructor

        #region Destructor
        ~Model()
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
            _crosshairConfig = new CrosshairList();
        }
        public void ModifyCrosshair(ICrosshair crosshair)
        {
            if (_crosshairConfig.Contains(crosshair))
            {
                _crosshairConfig[crosshair.Name].ModifyCrossView(crosshair);
                CrosshairConfigControl.ChangeCrosshair(crosshair);
            }
        }
        public void ChangeShape(ICrosshair crosshair)
        {
            if (_crosshairConfig.Contains(crosshair))
            {
                _crosshairConfig[_crosshairConfig.IndexOf((Crosshair)crosshair)].ChangeCrosshairShape(crosshair.Shape);
            }
        }
        public bool DeleteCrosshair(ICrosshair crosshair)
        {
            bool res = false;
            if (_crosshairConfig.Contains(crosshair))
            {
                _crosshairConfig.Remove((Crosshair)crosshair);
                res = true;
            }
            return res;
        }
        public void AddCrosshair(Crosshair crosshair, UserControl page)
        {
            if (!_crosshairConfig.Contains(crosshair))
            {
                _crosshairConfig.Add(crosshair);
            }
            if (!pageDict.ContainsKey(crosshair.Name))
            {
                pageDict.Add(_crosshairConfig.Last().Name, page);
            }
        }
        public void SaveCrosshairConfig(Crosshair crosshair = null)
        {
            if (crosshair != null)
            {
                _crosshairConfig.Add(crosshair);
            }
            SaveXml(saveName);
        }
        public bool LoadCrosshairList()
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(saveName);
                XmlNode root = xmlDoc.SelectSingleNode(xmlConfig);
                if (root != null)
                {
                    for (int i = 0; i < root.ChildNodes.Count - 3; i++)
                    {
                        Crosshair crosshair = new Crosshair();
                        crosshair.Name = root.ChildNodes[i].SelectSingleNode(xmlName)?.InnerText;
                        crosshair.AssignedKey = root.ChildNodes[i].SelectSingleNode(xmlAssignedKey).InnerText.ToKey();
                        crosshair.Thickness = int.Parse(root.ChildNodes[i].SelectSingleNode(xmlThickness)?.InnerText);
                        crosshair.Opacity = int.Parse(root.ChildNodes[i].SelectSingleNode(xmlOpacity)?.InnerText);
                        crosshair.Gap = int.Parse(root.ChildNodes[i].SelectSingleNode(xmlGap)?.InnerText);
                        crosshair.Size = int.Parse(root.ChildNodes[i].SelectSingleNode(xmlSize)?.InnerText);
                        crosshair.Outline = bool.Parse(root.ChildNodes[i].SelectSingleNode(xmlOutline)?.InnerText);
                        crosshair.OutlineOpacity = int.Parse(root.ChildNodes[i].SelectSingleNode(xmlOutlineOpacity)?.InnerText);
                        crosshair.CenterDot = bool.Parse(root.ChildNodes[i].SelectSingleNode(xmlCenterDot)?.InnerText);
                        crosshair.OutlineThickness = int.Parse(root.ChildNodes[i].SelectSingleNode(xmlOutlineThickness)?.InnerText);
                        crosshair.Shape = (CrosshairShape)Enum.Parse(typeof(CrosshairShape), root.ChildNodes[i].SelectSingleNode(xmlCrosshairShape)?.InnerText);
                        crosshair.CrosshairColor = (Color)ColorConverter.ConvertFromString(root.ChildNodes[i].SelectSingleNode(xmlColor)?.InnerText);
                        crosshair.OutlineColor = (Color)ColorConverter.ConvertFromString(root.ChildNodes[i].SelectSingleNode(xmlOutlineColor)?.InnerText);
                        _crosshairConfig.Add(crosshair);
                    }
                    ParameterClass.ControllerSwitch = bool.Parse(root.SelectSingleNode(xmlController)?.InnerText);
                    ParameterClass.KeyboardSwitch = bool.Parse(root.SelectSingleNode(xmlKeyboard)?.InnerText);
                    ParameterClass.MouseSwitch = bool.Parse(root.SelectSingleNode(xmlScroll)?.InnerText);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
            if (_crosshairConfig != null)
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
            for (int i = 0; i < _crosshairConfig.Count; i++)
            {
                if (_crosshairConfig[i].AssignedKey == key)
                {
                    ModifyCrosshair(_crosshairConfig[i]);
                }
            }
        }
        private void ScrollCrosshair(object sender, MouseWheelEventArgs e)
        {
            if (!ParameterClass.MouseSwitch) return;
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
                ModifyCrosshair(_crosshairConfig[currentCrosshairIndex]);
            }
        }
        private void ControllerSwitching(byte button)
        {
            string buttonName = "";
            switch (button)
            {
                case 9:
                    buttonName = controllerLb;
                    break;
                case 10:
                    buttonName = controllerRb;
                    break;

                default:
                    break;
            }
            if (!ParameterClass.ControllerSwitch) return;
            if (currentCrosshairIndex >= 0 && currentCrosshairIndex <= _crosshairConfig.Count - 1)
            {
                if (buttonName == controllerLb && (currentCrosshairIndex + 1 <= _crosshairConfig.Count - 1)) // UP
                {
                    currentCrosshairIndex++;
                }
                if (buttonName == controllerRb && (currentCrosshairIndex - 1 >= 0)) // DOWN
                {
                    currentCrosshairIndex--;
                }
                ModifyCrosshair(_crosshairConfig[currentCrosshairIndex]);
            }
        }
        private void SwitchingTypeUpdatedHandler(bool keyboard, bool mousewheel, bool controller)
        {
            ParameterClass.ControllerSwitch = controller;
            ParameterClass.KeyboardSwitch = keyboard;
            ParameterClass.MouseSwitch = mousewheel;
        }
        #endregion // Eventhandlers

        #region Private methods
        private void SaveXml(string xmlPath)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlNode root = xmlDoc.CreateElement(xmlConfig);
            xmlDoc.AppendChild(root);
            foreach (Crosshair item in _crosshairConfig)
            {
                XmlNode crosshair = xmlDoc.CreateElement(xmlCrosshair);
                XmlNode name = xmlDoc.CreateElement(xmlName);
                name.InnerText = item.Name;
                crosshair.AppendChild(name);
                XmlNode key = xmlDoc.CreateElement(xmlAssignedKey);
                key.InnerText = item.AssignedKey.ToStringFromKey();
                crosshair.AppendChild(key);
                XmlNode thickness = xmlDoc.CreateElement(xmlThickness);
                thickness.InnerText = item.Thickness.ToString();
                crosshair.AppendChild(thickness);
                XmlNode opacity = xmlDoc.CreateElement(xmlOpacity);
                opacity.InnerText = item.Opacity.ToString();
                crosshair.AppendChild(opacity);
                XmlNode gap = xmlDoc.CreateElement(xmlGap);
                gap.InnerText = item.Gap.ToString();
                crosshair.AppendChild(gap);
                XmlNode size = xmlDoc.CreateElement(xmlSize);
                size.InnerText = item.Size.ToString();
                crosshair.AppendChild(size);
                XmlNode outlineOp = xmlDoc.CreateElement(xmlOutlineOpacity);
                outlineOp.InnerText = item.OutlineOpacity.ToString();
                crosshair.AppendChild(outlineOp);
                XmlNode centerDot = xmlDoc.CreateElement(xmlCenterDot);
                centerDot.InnerText = item.CenterDot.ToString();
                crosshair.AppendChild(centerDot);
                XmlNode outline = xmlDoc.CreateElement(xmlOutline);
                outline.InnerText = item.Outline.ToString();
                crosshair.AppendChild(outline);
                XmlNode outlineTh = xmlDoc.CreateElement(xmlOutlineThickness);
                outlineTh.InnerText = item.OutlineThickness.ToString();
                crosshair.AppendChild(outlineTh);
                XmlNode shape = xmlDoc.CreateElement(xmlCrosshairShape);
                shape.InnerText = item.Shape.ToString();
                crosshair.AppendChild(shape);
                XmlNode color = xmlDoc.CreateElement(xmlColor);
                color.InnerText = item.CrosshairColor.ToString();
                crosshair.AppendChild(color);
                XmlNode outlineColor = xmlDoc.CreateElement(xmlOutlineColor);
                outlineColor.InnerText = item.OutlineColor.ToString();
                crosshair.AppendChild(outlineColor);
                root.AppendChild(crosshair);
            }
            XmlNode paramKeyboard = xmlDoc.CreateElement(xmlKeyboard);
            paramKeyboard.InnerText = ParameterClass.KeyboardSwitch.ToString();
            root.AppendChild(paramKeyboard);
            XmlNode paramController = xmlDoc.CreateElement(xmlController);
            paramController.InnerText = ParameterClass.ControllerSwitch.ToString();
            root.AppendChild(paramController);
            XmlNode paramScroll = xmlDoc.CreateElement(xmlScroll);
            paramScroll.InnerText = ParameterClass.MouseSwitch.ToString();
            root.AppendChild(paramScroll);
            xmlDoc.Save(xmlPath);
        }
        #endregion // Private methods
    }
}
