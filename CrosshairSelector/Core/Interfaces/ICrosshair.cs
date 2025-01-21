using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CrosshairSelector
{
    public enum CrosshairShape
    {
        Cross,
        Cross2,
        Triangle,
        Circle,
        X
    }

    public interface ICrosshair : ICloneable
    {
        int Thickness { get; set; }
        int Opacity { get; set; }
        int OutlineOpacity { get; set; }
        int Gap { get; set; }
        int Size { get; set; }
        bool Outline { get; set; }
        bool CenterDot { get; set; }
        CrosshairShape Shape { get; set; }
        Key AssignedKey { get; set; }
        System.Windows.Media.Color CrosshairColor { get; set; }
        System.Windows.Media.Color OutlineColor { get; set; }
        int OutlineThickness { get; set; }
        ICrosshairView View { get; set; }
        void ModifyCrossView(ICrosshair crosshair);
        void ChangeCrosshairShape(CrosshairShape shape);
    }
}
