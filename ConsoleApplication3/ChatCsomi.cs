using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bomberman.KozosKod;
using System.IO;

namespace ConsoleApplication3
{
    sealed class ChatCsomi : Csomi
    {
        private uint JatekosID;
        private String ChatUzi;

        public ChatCsomi(uint JatekosID, String ChatUzi) 
            : base(Server_Uzi_Tipusok.Chat)
        {
            this.JatekosID = JatekosID;
            this.ChatUzi = ChatUzi;
        }

        public override void becsomagol(BinaryWriter bw)
        {
            base.becsomagol(bw);

            bw.Write(JatekosID);
            bw.Write(ChatUzi);

            bw.Flush();
        }
    }
}