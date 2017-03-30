using Bomberman.KozosKod;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Kliens
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        TcpClient tcpc;

        private void Form1_Load(object sender, EventArgs e)
        {
            Thread t = new Thread(new ThreadStart(fogadoszal));
            t.Start();
        }

        private void Log(String Message)
        {
            listBox1.Invoke((MethodInvoker)(() =>
            {
                listBox1.Items.Add(Message);
                listBox1.SelectedIndex = listBox1.Items.Count - 1;
            }));
        }
        public IPAddress SzerverIPCime { get; set; }
        public String JatekosNev { get; set; }


        BinaryWriter bw;

        private void fogadoszal()
        {
            tcpc = new TcpClient();
            tcpc.Connect(SzerverIPCime, 60000);

            bw = new BinaryWriter(tcpc.GetStream());
            bw.Write((byte)Jatekos_Uzi_Tipusok.Bemutatkozik);
            bw.Write(JatekosNev);
            byte[] tmp = jatekter1.getArcom();
            bw.Write((UInt32)tmp.Length);
            bw.Write(tmp);
            bw.Flush();

            using (BinaryReader br = new BinaryReader(tcpc.GetStream()))
            {
                while (true)
                {
                    if (tcpc.GetStream().DataAvailable)
                    {
                        int uzi_tipus = br.ReadByte();
                        switch ((Server_Uzi_Tipusok)uzi_tipus)
                        {
                            case Server_Uzi_Tipusok.Jatekosok_Pozicioja:

                                while (true)
                                {
                                    uint dbszam = br.ReadUInt32(); // ID
                                    for (int db = 0; db < dbszam; db++)
                                    {
                                        UInt32 id = br.ReadUInt32();

                                        Jatekos j;

                                        if (!jatekter1.JatekosLista.TryGetValue(id, out j))
                                        {
                                            j = new Jatekos();
                                            j.ID = id;
                                        }

                                        j.Ele = br.ReadBoolean();//Ele
                                        j.x = br.ReadUInt32(); // x
                                        j.y = br.ReadUInt32(); // y

                                        jatekter1.JatekosLista[id] = j;
                                    }
                                }
                                break;
                            case Server_Uzi_Tipusok.Palyakep:

                                uint tmp_palya_szelesseg = br.ReadUInt32();
                                uint tmp_palya_magassag = br.ReadUInt32();
                                byte[] t = br.ReadBytes((int)(tmp_palya_magassag * tmp_palya_szelesseg));

                                jatekter1.Palyakep(tmp_palya_szelesseg, tmp_palya_magassag, t);

                                break;
                            case Server_Uzi_Tipusok.Chat:
                                String s = br.ReadString();
                                Log(s);
                                break;
                        }
                    }
                }
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                bw.Write((byte)Jatekos_Uzi_Tipusok.Chat);
                bw.Write(textBox1.Text);
                bw.Flush();
                textBox1.Text = "";
            }
        }


        private void panel1_Click(object sender, EventArgs e)
        {
            jatekter1.Focus();
        }

        private void panel1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.A:
                    {
                        bw.Write((byte)Jatekos_Uzi_Tipusok.Lep_Balra);
                        bw.Flush();
                        break;
                    }
                case Keys.S:
                    {
                        bw.Write((byte)Jatekos_Uzi_Tipusok.Lep_Le);
                        bw.Flush();
                        break;
                    }
                case Keys.D:
                    {
                        bw.Write((byte)Jatekos_Uzi_Tipusok.Lep_Jobbra);
                        bw.Flush();
                        break;
                    }
                case Keys.W:
                    {
                        bw.Write((byte)Jatekos_Uzi_Tipusok.Lep_Fel);
                        bw.Flush();
                        break;
                    }
                case Keys.Space:
                    {
                        bw.Write((byte)Jatekos_Uzi_Tipusok.Bombat_rak);
                        bw.Flush();
                        break;
                    }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            jatekter1.Refresh();
        }
    }
}
