using System.Diagnostics;
using System.Windows.Media;
using System.IO;
using System.Xml;

using CrosshairSelector.Model;

namespace CrosshairSelector.Core
{
    public static class XmlHandler
    {
        public static (CrosshairList, Parameters) LoadCrosshairList(string config)
        {
            CrosshairList crosshairs = new CrosshairList();
            Parameters parameterClass = new Parameters();
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(config);
                XmlNode root = xmlDoc.SelectSingleNode(XmlNames.xmlConfig);
                if (root != null)
                {
                    for (int i = 0; i < root.ChildNodes.Count - 3; i++)
                    {
                        Crosshair crosshair = new Crosshair();
                        crosshair.Name = root.ChildNodes[i].SelectSingleNode(XmlNames.xmlName)?.InnerText;
                        crosshair.AssignedKey = root.ChildNodes[i].SelectSingleNode(XmlNames.xmlAssignedKey).InnerText.ToKey();
                        crosshair.Thickness = int.Parse(root.ChildNodes[i].SelectSingleNode(XmlNames.xmlThickness)?.InnerText);
                        crosshair.Opacity = int.Parse(root.ChildNodes[i].SelectSingleNode(XmlNames.xmlOpacity)?.InnerText);
                        crosshair.Gap = int.Parse(root.ChildNodes[i].SelectSingleNode(XmlNames.xmlGap)?.InnerText);
                        crosshair.Size = int.Parse(root.ChildNodes[i].SelectSingleNode(XmlNames.xmlSize)?.InnerText);
                        crosshair.Outline = bool.Parse(root.ChildNodes[i].SelectSingleNode(XmlNames.xmlOutline)?.InnerText);
                        crosshair.OutlineOpacity = int.Parse(root.ChildNodes[i].SelectSingleNode(XmlNames.xmlOutlineOpacity)?.InnerText);
                        crosshair.CenterDot = bool.Parse(root.ChildNodes[i].SelectSingleNode(XmlNames.xmlCenterDot)?.InnerText);
                        crosshair.OutlineThickness = int.Parse(root.ChildNodes[i].SelectSingleNode(XmlNames.xmlOutlineThickness)?.InnerText);
                        crosshair.Shape = (CrosshairShape)Enum.Parse(typeof(CrosshairShape), root.ChildNodes[i].SelectSingleNode(XmlNames.xmlCrosshairShape)?.InnerText);
                        crosshair.CrosshairColor = (Color)ColorConverter.ConvertFromString(root.ChildNodes[i].SelectSingleNode(XmlNames.xmlColor)?.InnerText);
                        crosshair.OutlineColor = (Color)ColorConverter.ConvertFromString(root.ChildNodes[i].SelectSingleNode(XmlNames.xmlOutlineColor)?.InnerText);
                        crosshairs.Add((Crosshair)crosshair.Clone());
                    }
                    parameterClass.ControllerSwitch = bool.Parse(root.SelectSingleNode(XmlNames.xmlController)?.InnerText);
                    parameterClass.KeyboardSwitch = bool.Parse(root.SelectSingleNode(XmlNames.xmlKeyboard)?.InnerText);
                    parameterClass.MouseSwitch = bool.Parse(root.SelectSingleNode(XmlNames.xmlScroll)?.InnerText);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            if (crosshairs != null)
            {
                return (crosshairs, parameterClass);
            }
            throw new Exception("Error during the reading of the crosshairlist.");
        }
        public static bool SaveXml(CrosshairList crosshairs, Parameters parameterClass)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlNode root = xmlDoc.CreateElement(XmlNames.xmlConfig);
            xmlDoc.AppendChild(root);
            foreach (Crosshair item in crosshairs)
            {
                XmlNode crosshair = xmlDoc.CreateElement(XmlNames.xmlCrosshair);
                XmlNode name = xmlDoc.CreateElement(XmlNames.xmlName);
                name.InnerText = item.Name;
                crosshair.AppendChild(name);
                XmlNode key = xmlDoc.CreateElement(XmlNames.xmlAssignedKey);
                key.InnerText = item.AssignedKey.ToStringFromKey();
                crosshair.AppendChild(key);
                XmlNode thickness = xmlDoc.CreateElement(XmlNames.xmlThickness);
                thickness.InnerText = item.Thickness.ToString();
                crosshair.AppendChild(thickness);
                XmlNode opacity = xmlDoc.CreateElement(XmlNames.xmlOpacity);
                opacity.InnerText = item.Opacity.ToString();
                crosshair.AppendChild(opacity);
                XmlNode gap = xmlDoc.CreateElement(XmlNames.xmlGap);
                gap.InnerText = item.Gap.ToString();
                crosshair.AppendChild(gap);
                XmlNode size = xmlDoc.CreateElement(XmlNames.xmlSize);
                size.InnerText = item.Size.ToString();
                crosshair.AppendChild(size);
                XmlNode outlineOp = xmlDoc.CreateElement(XmlNames.xmlOutlineOpacity);
                outlineOp.InnerText = item.OutlineOpacity.ToString();
                crosshair.AppendChild(outlineOp);
                XmlNode centerDot = xmlDoc.CreateElement(XmlNames.xmlCenterDot);
                centerDot.InnerText = item.CenterDot.ToString();
                crosshair.AppendChild(centerDot);
                XmlNode outline = xmlDoc.CreateElement(XmlNames.xmlOutline);
                outline.InnerText = item.Outline.ToString();
                crosshair.AppendChild(outline);
                XmlNode outlineTh = xmlDoc.CreateElement(XmlNames.xmlOutlineThickness);
                outlineTh.InnerText = item.OutlineThickness.ToString();
                crosshair.AppendChild(outlineTh);
                XmlNode shape = xmlDoc.CreateElement(XmlNames.xmlCrosshairShape);
                shape.InnerText = item.Shape.ToString();
                crosshair.AppendChild(shape);
                XmlNode color = xmlDoc.CreateElement(XmlNames.xmlColor);
                color.InnerText = item.CrosshairColor.ToString();
                crosshair.AppendChild(color);
                XmlNode outlineColor = xmlDoc.CreateElement(XmlNames.xmlOutlineColor);
                outlineColor.InnerText = item.OutlineColor.ToString();
                crosshair.AppendChild(outlineColor);
                root.AppendChild(crosshair);
            }
            XmlNode paramKeyboard = xmlDoc.CreateElement(XmlNames.xmlKeyboard);
            paramKeyboard.InnerText = parameterClass.KeyboardSwitch.ToString();
            root.AppendChild(paramKeyboard);
            XmlNode paramController = xmlDoc.CreateElement(XmlNames.xmlController);
            paramController.InnerText = parameterClass.ControllerSwitch.ToString();
            root.AppendChild(paramController);
            XmlNode paramScroll = xmlDoc.CreateElement(XmlNames.xmlScroll);
            paramScroll.InnerText = parameterClass.MouseSwitch.ToString();
            root.AppendChild(paramScroll);
            xmlDoc.Save(XmlNames.saveName);
            return true;
        }
    }
}
