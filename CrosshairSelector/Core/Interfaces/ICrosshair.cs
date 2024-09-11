using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CrosshairSelector
{
    public interface ICrosshair
    {
        int Thickness { get; set; }
        int Opacity { get; set; }
        int Gap { get; set; }
        int Size { get; set; }
        bool Outline { get; set; }
        CrosshairShape Shape { get; set; }
        System.Windows.Media.Color CrosshairColor { get; set; }
        ICrosshairView View { get; set; }
        void ModifyCrossView(int size, int thickness, int gap, int opacity, int red, int green, int blue, bool outline, Key assignedKey);
        void ModifyCrossView(ICrosshair crosshair);
        void ChangeCrosshairShape(CrosshairShape shape);
    }
}
