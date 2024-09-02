using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CrosshairSelector
{
    public interface ICrosshair
    {
        int Thickness { get; set; }
        int Opacity { get; set; }
        public int Gap { get; set; }
        public int Size { get; set; }
        public bool Outline { get; set; }
        public System.Windows.Media.Color CrosshairColor { get; set; }
        void ModifyCrossView(int size, int thickness, int gap, int opacity, int red, int green, int blue, bool outline);
        void ModifyCrossView(ICrosshair crosshair);
    }
}
