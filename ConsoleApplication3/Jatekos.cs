using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing;
using System.Net.Sockets;
using System.Threading;
using System.Collections.Concurrent;

namespace ConsoleApplication3
{
    class Jatekos
    {
        public uint ID;
        public String Nev = "";
        public byte[] Arc;        
        public uint x = 0;
        public uint y = 0;
        public uint Sebesseg = 1;
        public bool Ele = true;
        public uint Maxbombaszam = 1;
        public uint Actbombaszam = 0;
        public uint Rendzs = 1;
        public uint Lab = 0;

        public ConcurrentQueue<Csomi> CsomiSor = new ConcurrentQueue<Csomi>();

        public TcpClient tcp;
        public Thread thread;

        public Jatekos()
        {
            ID = Jatekos_ID_Szamlalo++;
        }

        static uint Jatekos_ID_Szamlalo = 1;
    }
}
