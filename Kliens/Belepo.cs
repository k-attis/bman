using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kliens
{
    public partial class Belepo : Form
    {
        Thread info_fogado;

        public Belepo()
        {
            InitializeComponent();

            info_fogado = new Thread(new ThreadStart(info_fogado_szal));
            info_fogado.Start();
        }

        List<Szerver> SzerverLista = new List<Szerver>();

        void info_fogado_szal()
        {
            UdpClient c = new UdpClient(60001);
            IPEndPoint ep = null;

            while (true)
            {
                if (c.Available > 0)
                {
                    byte[] info_csomag = c.Receive(ref ep);

                    Szerver s = null;

                    foreach (Szerver sz in SzerverLista)
                        if (ep.Address.Equals(sz.IPCim))
                        {
                            s = sz;
                            break;
                        }

                    if (s == null)
                    {
                        s = new Szerver();
                        s.IPCim = ep.Address;
                        SzerverLista.Add(s);
                    }

                    using (BinaryReader br = new BinaryReader(new MemoryStream(info_csomag)))
                    {
                        s.Neve = br.ReadString();
                        s.Jatekbane = br.ReadUInt16() > 0;
                        s.JatekosokSzama = br.ReadUInt16();
                        s.UtolsoPingIdeje = DateTime.Now;
                    }
                }

                Thread.Sleep(500);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            foreach(Szerver s in SzerverLista)
            {
                listBox1.Items.Add(s.ToString());
            }
        }
    }
}
