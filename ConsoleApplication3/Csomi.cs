using Bomberman.KozosKod;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ConsoleApplication3
{
    public abstract class Csomi
    {
        private Server_Uzi_Tipusok Tipus;

        public Csomi(Server_Uzi_Tipusok Tipus)
        {
            this.Tipus = Tipus;
        }

        public virtual void becsomagol(BinaryWriter bw)
        {
            bw.Write((byte)Tipus);
        }
    }
}