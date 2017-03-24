using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Bomberman.KozosKod;

namespace ConsoleApplication3
{
    class JatekosokPoziciojaCsomi : Csomi
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
                bw.Write(j.ID);
                bw.Write(j.Ele);
                bw.Write(j.x);
                bw.Write(j.y);
            }
        }
    }
}