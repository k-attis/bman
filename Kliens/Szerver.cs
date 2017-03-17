using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Kliens
{
    public class Szerver
    {
        public String Neve;
        public bool Jatekbane;
        public int JatekosokSzama;
        public DateTime UtolsoPingIdeje;
        public IPAddress IPCim;

        public String ToString()
        {
            return String.Format("{0}({1}) - {2}", Neve, IPCim.ToString(), UtolsoPingIdeje);
        }
    }
}