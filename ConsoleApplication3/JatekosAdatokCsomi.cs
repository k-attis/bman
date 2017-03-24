using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Bomberman.KozosKod;

namespace ConsoleApplication3
{
    class JatekosAdatokCsomi : Csomi
    {
        private Jatekos Jatekos;

        public JatekosAdatokCsomi(Jatekos Jatekos) 
            : base(Server_Uzi_Tipusok.Jatekos_Adatok)
        {
            this.Jatekos = Jatekos;
        }

        public override void becsomagol(BinaryWriter bw)
        {
            base.becsomagol(bw);
            bw.Write((UInt32)Jatekos.ID);
            bw.Write(Jatekos.Nev);
            bw.Write(Jatekos.Arc.Length);
            bw.Write(Jatekos.Arc);
        }
    }
}