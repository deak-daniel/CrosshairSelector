using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;


namespace CrosshairSelector
{
    public class Model
    {
        Crosshair Crosshair { get; set; }
        public Model(ref Crosshair crosshair)
        {
            Crosshair = crosshair;
        }
        public void ModifyCrosshair(Crosshair crosshair)
        {
            this.Crosshair = crosshair;
            Crosshair.ModifyCrosshairView(crosshair);
            MainWindow.ChangeCrosshair(crosshair);
        }
        public void ModifyCrosshair(int size, int thickness, int gap, int opacity, int red, int green, int blue, bool outline)
        {
            Crosshair.ModifyCrosshairView(size, thickness, gap, opacity, red, green, blue, outline);
            MainWindow.ChangeCrosshair(Crosshair);
        }
        public static Crosshair LoadCrosshair()
        {
            Crosshair crosshair = Crosshair.Load("crosshair.xml");
            return crosshair;
        }
        public void SaveCrosshair(string xmlPath)
        {
            this.Crosshair.Save(xmlPath);
        }

    }
}
