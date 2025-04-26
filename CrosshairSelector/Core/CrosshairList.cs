using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

using CrosshairSelector.Model;

namespace CrosshairSelector.Core
{
    public sealed class CrosshairList : List<Crosshair>
    {
        public new void Add(Crosshair item)
        {
            if (base.Count == 0)
            {
                if(item.Name == "")
                {
                    item.Name = "Crosshair1";
                }
                base.Add(item);
            }
            else
            {
                int index = 0;
                while (index < base.Count && base[index].Name != item.Name)
                {
                    index++;
                }
                if (index < base.Count)
                {
                    return;
                }
                else
                {
                    string name = this.Last().Name;
                    item.Name = "Crosshair" + (name.GetNumberFromString() + 1);
                    base.Add(item);
                }
            }
        }
        public Crosshair this[string name]
        {
            get 
            {
                int index = 0;
                while (index < base.Count && base[index].Name != name )
                {
                    index++;
                }
                return index < base.Count ? base[index] : default;
            }
        }
    }
    public sealed class Parameters
    {
        private bool keyboardSwitch;

        public bool KeyboardSwitch
        {
            get { return keyboardSwitch; }
            set { 
                keyboardSwitch = value;
            }
        }
        private bool mouseSwitch;

        public bool MouseSwitch
        {
            get { return mouseSwitch; }
            set
            {
                mouseSwitch = value;
            }
        }
        private bool controllerSwitch;

        public bool ControllerSwitch
        {
            get { return controllerSwitch; }
            set
            {
                controllerSwitch = value;
            }
        }
    }
}
