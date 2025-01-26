using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CrosshairSelector
{
    public static class StringExtensions
    {
        private static readonly Dictionary<Key, string> KeyToLetterMap = new Dictionary<Key, string>()
        {
            {Key.D0, "0" },
            {Key.D1, "1"},
            {Key.D2, "2"},
            {Key.D3, "3"},
            {Key.D4, "4"},
            {Key.D5, "5"},
            {Key.D6, "6"},
            {Key.D7, "7"},
            {Key.D8, "8"},
            {Key.D9, "9"},
            {Key.A, "a"},
            {Key.B, "b"},
            {Key.C, "c"},
            {Key.D, "d"},
            {Key.E, "e"},
            {Key.F, "f"},
            {Key.G, "g"},
            {Key.H, "h"},
            {Key.I, "i"},
            {Key.J, "j"},
            {Key.K, "k"},
            {Key.L, "l"},
            {Key.M, "m"},
            {Key.N, "n"},
            {Key.O, "o"},
            {Key.P, "p"},
            {Key.Q, "q"},
            {Key.R, "r"},
            {Key.S, "s"},
            {Key.T, "t"},
            {Key.U, "u"},
            {Key.V, "v"},
            {Key.W, "w"},
            {Key.X, "x"},
            {Key.Y, "y"},
            {Key.Z, "z"},
            {Key.Tab, "tab"},
            {Key.None, "" },
            {Key.LeftCtrl, "leftcontrol"},
            {Key.LeftShift, "leftshift"},
            {Key.Space, "space"}
        };
        private static readonly Dictionary<string, Key> LetterToKeyMap = new Dictionary<string, Key>()
        {
            {"0", Key.D0 },
            {"1", Key.D1}, 
            {"2", Key.D2},
            {"3", Key.D3},
            {"4", Key.D4},
            {"5", Key.D5},
            {"6", Key.D6},
            {"7", Key.D7},
            {"8", Key.D8},
            {"9", Key.D9},
            {"a", Key.A},
            {"b", Key.B},
            {"c", Key.C},
            {"d", Key.D},
            {"e", Key.E},
            {"f", Key.F},
            {"g", Key.G},
            {"h", Key.H},
            {"i", Key.I},
            {"j", Key.J},
            {"k", Key.K},
            {"l", Key.L},
            {"m", Key.M},
            {"n", Key.N},
            {"o", Key.O},
            {"p", Key.P},
            {"q", Key.Q},
            {"r", Key.R},
            {"s", Key.S},
            {"t", Key.T},
            {"u", Key.U},
            {"v", Key.V},
            {"w", Key.W},
            {"x", Key.X},
            {"y", Key.Y},
            {"z", Key.Z},
            {"", Key.None },
            {"space", Key.Space },
            {"tab", Key.Tab },
            {"leftshift", Key.LeftShift },
            {"leftcontrol", Key.LeftCtrl }

        };
        public static Key ToKey(this string value)
        {
            Key res = Key.None;
            if (value != null && value != "")
            {
                res = LetterToKeyMap[value];
            }
            return res;
        }
        public static string ToStringFromKey(this Key value)
        {
            string res = "";
            if (value != null)
            {
                res = KeyToLetterMap[value];
            }
            return res;
        }
        public static int GetNumberFromString(this string value)
        {
            string output = "";
            for (int i = 0; i < value.Length; i++)
            {
                if (char.IsDigit(value[i]))
                {
                    output += value[i];
                }
            }
            return int.Parse(output);
        }
    }
}
