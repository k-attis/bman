using Bomberman.KozosKod;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
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

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        Bitmap buffer;
        Graphics bufferg;

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            if (buffer == null)
                return;

            int cell_width = buffer.Width / 35;
            int cell_height = buffer.Height / 35;
            int cell_size = (cell_width < cell_height) ? (cell_width) : (cell_height);

            int offset_x = (buffer.Width - cell_size * 35) / 2;
            int offset_y = (buffer.Height - cell_size * 35) / 2;

            // vízszintes
            for (int y = 0; y < (35+1); y++)
                bufferg.DrawLine(Pens.Red, 
                    offset_x, 
                    offset_y + cell_size * y, 
                    offset_x + cell_size * 35, 
                    offset_y + cell_size * y);

            for (int x = 0; x < (35+1); x++)
                bufferg.DrawLine(Pens.Red,
                    offset_x + cell_size * x,
                    offset_y,
                    offset_x + cell_size * x,
                    offset_y + cell_size * 35);

            e.Graphics.DrawImage(buffer, 0, 0);
        }

        private void panel1_Resize(object sender, EventArgs e)
        {
            if (buffer != null)
                buffer.Dispose();

            buffer = new Bitmap(panel1.Width, panel1.Height);
            bufferg = Graphics.FromImage(buffer);

            panel1.Invalidate();
        }

        TcpClient tcpc;

        private void Form1_Load(object sender, EventArgs e)
        {
            Thread t = new Thread(new ThreadStart(fogadoszal));
            //t.Start();
        }

        private void Log(String Message)
        {
            listBox1.Invoke((MethodInvoker)(() =>
            {
                listBox1.Items.Add(Message);
                listBox1.SelectedIndex = listBox1.Items.Count - 1;
            }));
        }

        BinaryWriter bw;

        private void fogadoszal()
        {
            tcpc = new TcpClient();
            //tcpc.Connect("10.0.1.166", 60000);
            //tcpc.Connect("10.7.51.141", 60000);
            tcpc.Connect("localhost", 60000);

            bw = new BinaryWriter(tcpc.GetStream());


            bw.Write((byte)Jatekos_Uzi_Tipusok.Bemutatkozik);
            bw.Write("Attila");
            bw.Write((byte)255);
            bw.Write((byte)0);
            bw.Write((byte)0);
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
                                    uint id = br.ReadUInt32(); // ID
                                    if (id == 0)
                                        break;

                                    br.ReadString(); // Nev
                                    br.ReadByte(); // R
                                    br.ReadByte(); // G
                                    br.ReadByte();  // B
                                    br.ReadUInt32(); // x
                                    br.ReadUInt32(); // y
                                }

                                break;
                            case Server_Uzi_Tipusok.Palyakep:

                                uint palya_szelesseg = br.ReadUInt32();
                                uint palya_magassag = br.ReadUInt32();

                                byte[] t = br.ReadBytes((int)(palya_magassag * palya_szelesseg));




                                break;
                            case Server_Uzi_Tipusok.Meghaltal:
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
            }
        }
    }
}
