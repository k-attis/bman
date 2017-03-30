using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bomberman.KozosKod;
using System.IO;

namespace ConsoleApplication3
{
    sealed class PalyakepCsomi : Csomi
    {
        Palya palya;

        public PalyakepCsomi(Palya palya) :
            base(Server_Uzi_Tipusok.Palyakep)
        {
            this.palya = palya;
        }

        public override void becsomagol(BinaryWriter bw)
        {
            base.becsomagol(bw);

            bw.Write(palya.Szelesseg);
            bw.Write(palya.Magassag);

            byte[] t = new byte[palya.Szelesseg * palya.Magassag];

            for (int y = 0, tidx = 0; y < palya.Magassag; y++)
                for (int x = 0; x < palya.Szelesseg; x++)
                    t[tidx++] = (byte)palya.Cellak[x, y].Tipus;

            bw.Write(t);
            bw.Flush();
        }
    }
}