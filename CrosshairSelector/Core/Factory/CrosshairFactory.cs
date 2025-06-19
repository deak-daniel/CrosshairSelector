using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

using CrosshairSelector.Model;

namespace CrosshairSelector.Core
{
    public static class CrosshairFactory
    {
        public static CrosshairList CreateCrosshairs()
        {
            CrosshairList crosshairs = new CrosshairList();
            Parameters parameters = new Parameters();
            (crosshairs, parameters) = XmlHandler.LoadCrosshairList(XmlNames.defaultXmlConfigName);
            return crosshairs;
        }
    }
}
