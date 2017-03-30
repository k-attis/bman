using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Bomberman.KozosKod;

namespace ConsoleApplication3
{
    sealed class JatekosokPoziciojaCsomi : Csomi
    {
        private List<Jatekos> JatekosLista;

        public JatekosokPoziciojaCsomi(List<Jatekos> JatekosLista)
            : base(Server_Uzi_Tipusok.Jatekosok_Pozicioja)
        {
            this.JatekosLista = JatekosLista;
        }

        public override void becsomagol(BinaryWriter bw)
        {
            base.becsomagol(bw);

            bw.Write((UInt32)JatekosLista.Count);

            foreach (Jatekos j in JatekosLista)
            {
                bw.Write((UInt32)j.ID);
                bw.Write((bool)j.Ele);
                bw.Write((UInt32)j.x);
                bw.Write((UInt32)j.y);
            }

            bw.Flush();
        }
    }
}