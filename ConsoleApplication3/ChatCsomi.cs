using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bomberman.KozosKod;
using System.IO;

namespace ConsoleApplication3
{
    public sealed class ChatCsomi : Csomi
    {
        private int JatekosID;
        private String ChatUzi;

        public ChatCsomi(int JatekosID, String ChatUzi) 
            : base(Server_Uzi_Tipusok.Chat)
        {
            this.JatekosID = JatekosID;
            this.ChatUzi = ChatUzi;
        }

        public override void becsomagol(BinaryWriter bw)
        {
            base.becsomagol(bw);
            bw.Write((UInt32)JatekosID);
            bw.Write(ChatUzi);
        }
    }
}