using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing;

namespace ConsoleApplication3
{
    class Bomba
    {
        static uint Bomba_ID_Szamlalo = 1;
        public uint ID = Bomba_ID_Szamlalo++;
        //public Color Szin;
        public uint Rendzs;
        public DateTime Mikor_Robban =
            DateTime.Now.AddMilliseconds(3000);
        public uint Jatekos_ID;
        public uint x;
        public uint y;

        public Bomba(Jatekos jatekos, uint x, uint y)
        {
            this.Rendzs = jatekos.Rendzs;
            this.Jatekos_ID = jatekos.ID;
            this.x = x;
            this.y = y;
        }
    }
}
