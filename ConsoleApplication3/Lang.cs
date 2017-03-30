using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing;

namespace ConsoleApplication3
{
    class Lang
    {
        static uint Lang_ID_Szamlalo = 1;
        public uint ID = Lang_ID_Szamlalo++;
        public DateTime Meddig = 
            DateTime.Now.AddMilliseconds(1000);
        public uint Jatekos_ID;
        public uint x;
        public uint y;

        public Lang(uint Jatekos_ID, uint x, uint y)
        {
            this.Jatekos_ID = Jatekos_ID;
            this.x = x;
            this.y = y;
        }

        
    }
}
