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


namespace CrosshairSelector
{
    public class Model
    {
        #region Constants
        private string xmlConfig = "Config";
        private string xmlCrosshair = "Crosshair";
        private string xmlName = "Name";
        private string xmlAssignedKey = "AssignedKey";
        private string xmlThickness = "Thickness";
        private string xmlOpacity = "Opacity";
        private string xmlGap = "Gap";
        private string xmlSize = "Size";
        private string xmlOutlineOpacity = "OutlineOpacity";
        private string xmlOutline = "Outline";
        private string xmlCenterDot = "CenterDot";
        private string xmlOutlineThickness = "OutlineThickness";
        private string xmlCrosshairShape = "CrosshairShape";
        private string xmlColor = "Color";
        private string xmlOutlineColor = "OutlineColor";
        private string xmlKeyboard = "Keyboard";
        private string xmlController = "Controller";
        private string xmlScroll = "Scroll";
        #endregion // Constants

        #region Events
        public static Action<Key> OnKeyPressed;
        #endregion // Events

        #region Fields
        private HomePage homePage;
        private int currentCrosshairIndex = 1;
        private bool keyboardSwitching = true;
        private bool controllerSwitching = false;
        private bool mouseWheelSwitching = false;
        private CrosshairList _crosshairConfig;
        private static Model instance;
        private Dictionary<string, Frame> pageDict;
        #endregion // Fields

        #region Properties
        public Parameters ParameterClass { get; set; }
        public HomePage HomePage { get => homePage; }
        public CrosshairList Crosshairs { get => _crosshairConfig; }
        public Dictionary<string, Frame> Pages { get => pageDict; }
        public bool KeyboardSwitch { get => keyboardSwitching; }
        public bool ControllerSwitch { get => controllerSwitching; }
        public bool ScrollSwitch { get => mouseWheelSwitching; }
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
            pageDict = new Dictionary<string, Frame>();
            GlobalMouseWheelHook.MouseWheelScrolled += ScrollCrosshair!;
            HomePageViewModel.SwitchingTypeUpdated += SwitchingTypeUpdatedHandler!;
            MainWindow.OnControllerSwitch += ControllerSwitching!;
            OnKeyPressed = KeyboardSwitching;
        }
        #endregion // Constructor

        #region Destructor
        ~Model()
        {
            GlobalMouseWheelHook.MouseWheelScrolled -= ScrollCrosshair!;
            HomePageViewModel.SwitchingTypeUpdated -= SwitchingTypeUpdatedHandler!;
            MainWindow.OnControllerSwitch -= ControllerSwitching!;
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
                _crosshairConfig[_crosshairConfig.IndexOf((Crosshair)crosshair)].ModifyCrossView(crosshair);
                CrosshairConfigPage.ChangeCrosshair(crosshair);
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
        public void AddCrosshair(ICrosshair crosshair, Frame page)
        {
            _crosshairConfig.Add((Crosshair)crosshair);
            if (!pageDict.ContainsKey(crosshair.Name))
            {
                pageDict.Add(crosshair.Name, page);
            }
        }
        public void KeyboardSwitching(Key key)
        {
            if (!keyboardSwitching)
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
        public void SaveCrosshairConfig(Crosshair crosshair = null)
        {
            string xmlPath = "crosshair.xml";
            if (crosshair != null)
            {
                _crosshairConfig.Add(crosshair);
            }
            SaveXml(xmlPath);
        }
        public bool LoadCrosshairList(string xmlPath)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(xmlPath);
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
                    ParameterClass.ControllerSwitch = bool.Parse(root.ChildNodes[root.ChildNodes.Count - 3].SelectSingleNode(xmlController)?.InnerText);
                    ParameterClass.KeyboardSwitch = bool.Parse(root.ChildNodes[root.ChildNodes.Count - 2].SelectSingleNode(xmlKeyboard)?.InnerText);
                    ParameterClass.MouseSwitch = bool.Parse(root.ChildNodes[root.ChildNodes.Count - 1].SelectSingleNode(xmlScroll)?.InnerText);
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
                ModifyCrosshair(_crosshairConfig[currentCrosshairIndex]);
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
                ModifyCrosshair(_crosshairConfig[currentCrosshairIndex]);
            }
        }
        private void SwitchingTypeUpdatedHandler(bool keyboard, bool mousewheel, bool controller)
        {
            mouseWheelSwitching = mousewheel;
            controllerSwitching = controller;
            keyboardSwitching = keyboard;
            ParameterClass.ControllerSwitch = controllerSwitching;
            ParameterClass.KeyboardSwitch = keyboardSwitching;
            ParameterClass.MouseSwitch = mouseWheelSwitching;
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
