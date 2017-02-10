using Bomberman.KozosKod;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
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

        uint palya_szelesseg;
        uint palya_magassag;
        uint cell_size;
        uint offset_x;
        uint offset_y;

        CellaTipus[,] Palya;

        Dictionary<uint, Jatekos> JatekosLista = new Dictionary<uint, Jatekos>();

        void palya_init(uint szelesseg, uint magassag)
        {
            palya_szelesseg = szelesseg;
            palya_magassag = magassag;

            Palya = new CellaTipus[szelesseg, magassag];

            Palya[0, 0] = CellaTipus.Fal;
            Palya[5, 6] = CellaTipus.Fal;
            Palya[1, 1] = CellaTipus.Robbanthato_Fal;
            Palya[2, 2] = CellaTipus.Lab_Kartya;
            Palya[7, 3] = CellaTipus.Bomba;

            JatekosLista[1] = new Jatekos()
            {
                ID = 1,
                Nev = "Test jatekos",
                Szin = Color.FromArgb(0xFF, 0xFF, 0x00),
                x = 2,
                y = 5
            };
        }

        uint CellaX2PixelX(uint CellaX)
        {
            return offset_x + CellaX * cell_size;
        }

        uint CellaY2PixelY(uint CellaY)
        {
            return offset_y + CellaY * cell_size;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            if (buffer == null)
                return;

            uint cell_width = ((uint)buffer.Width - 150) / palya_szelesseg;
            uint cell_height = ((uint)buffer.Height - 150) / palya_magassag;
            cell_size = (cell_width < cell_height) ? (cell_width) : (cell_height);

            offset_x = ((uint)buffer.Width - cell_size * palya_szelesseg) / 2;
            offset_y = ((uint)buffer.Height - cell_size * palya_magassag) / 2;
            /*
            // vízszintes
            for (uint y = 0; y < (palya_magassag + 1); y++)
                bufferg.DrawLine(Pens.Red,
                    offset_x,
                    CellaY2PixelY(y),
                    CellaX2PixelX(palya_szelesseg),
                    CellaY2PixelY(y));

            for (uint x = 0; x < (palya_szelesseg + 1); x++)
                bufferg.DrawLine(Pens.Red,
                    CellaX2PixelX(x),
                    offset_y,
                    CellaX2PixelX(x),
                    CellaY2PixelY(palya_magassag));
                    */
            bufferg.Clear(Color.Green);
                               
            for (uint i = 0; i < palya_magassag; i++)
                for (uint j = 0; j < palya_szelesseg; j++)
                    switch (Palya[j, i])
                    {
                        case CellaTipus.Fal:
                            {
                                bufferg.FillRectangle(Brushes.Gray,
                                    CellaX2PixelX(j),
                                    CellaY2PixelY(i),
                                    cell_size,
                                    cell_size);
                                break;
                            }
                        case CellaTipus.Robbanthato_Fal:
                            {
                                bufferg.FillRectangle(new HatchBrush(HatchStyle.DiagonalBrick, Color.Gray, Color.Red),
                                    CellaX2PixelX(j),
                                    CellaY2PixelY(i),
                                    cell_size,
                                    cell_size);
                                break;
                            }
                        case CellaTipus.Lab_Kartya:
                            {
                                bufferg.FillRectangle(Brushes.Cyan,
                                    CellaX2PixelX(j),
                                    CellaY2PixelY(i),
                                    cell_size,
                                    cell_size);

                                string ss = Encoding.UTF32.GetString(
                                    BitConverter.GetBytes(0x1f463)
                                    );

                                Font f = new Font("Segoe UI Symbol",
                                    cell_size * 0.6f,
                                    FontStyle.Bold);

                                SizeF s = bufferg.MeasureString(ss, f);

                                int sox = ((int)cell_size - (int)s.Width) / 2;
                                int soy = ((int)cell_size - (int)s.Height) / 2;

                                bufferg.DrawString(ss,
                                    f,
                                    Brushes.Black,
                                    CellaX2PixelX(j) + sox,
                                    CellaY2PixelY(i) + soy);

                                break;
                            }
                        case CellaTipus.Bomba:
                            {
                                /* bufferg.FillRectangle(Brushes.Cyan,
                                     CellaX2PixelX(j,
                                     CellaY2PixelY(i,
                                     cell_size,
                                     cell_size);*/

                                Font f = new Font("Wingdings", cell_size * 0.6f, FontStyle.Bold);

                                SizeF s = bufferg.MeasureString("M", f);

                                int sox = ((int)cell_size - (int)s.Width) / 2;
                                int soy = ((int)cell_size - (int)s.Height) / 2;

                                bufferg.DrawString("M",
                                    f,
                                    new SolidBrush(Color.FromArgb(0xFF, 0xFF, 0xFF)),
                                    CellaX2PixelX(j) + sox,
                                    CellaY2PixelY(i) + soy);

                                break;
                            }


                    }

            foreach (Jatekos j in JatekosLista.Values.ToList())
            {
                string ss = Encoding.UTF32.GetString(
                                                   BitConverter.GetBytes(0x1F603)
                                                   );

                Font f = new Font("Segoe UI Symbol",
                    cell_size * 0.6f,
                    FontStyle.Bold);

                SizeF s = bufferg.MeasureString(ss, f);

                int sox = ((int)cell_size - (int)s.Width) / 2;
                int soy = ((int)cell_size - (int)s.Height) / 2;

                bufferg.DrawString(ss,
                    f,
                    new SolidBrush(j.Szin),
                    CellaX2PixelX(j.x) + sox,
                    CellaY2PixelY(j.y) + soy);
            }

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
            palya_init(10, 10);
            Thread t = new Thread(new ThreadStart(fogadoszal));
            t.Start();
            panel1.Refresh();
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
            tcpc.Connect("10.0.1.166", 60000);
            //tcpc.Connect("10.7.51.141", 60000);
            //tcpc.Connect("localhost", 60000);

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

                                    Jatekos j;

                                    if (!JatekosLista.TryGetValue(id, out j))
                                        j = new Jatekos();

                                    j.Nev = br.ReadString(); // Nev

                                    byte r = br.ReadByte(); // R
                                    byte g = br.ReadByte(); // G
                                    byte b = br.ReadByte();  // B

                                    j.Szin = Color.FromArgb(r, g, b);
                                    j.x = br.ReadUInt32(); // x
                                    j.y = br.ReadUInt32(); // y

                                    JatekosLista[id] = j;
                                }
                                break;
                            case Server_Uzi_Tipusok.Palyakep:

                                uint tmp_palya_szelesseg = br.ReadUInt32();
                                uint tmp_palya_magassag = br.ReadUInt32();

                                if (tmp_palya_magassag != palya_magassag
                                    ||
                                    tmp_palya_szelesseg != palya_szelesseg)
                                {
                                    Palya = new CellaTipus[tmp_palya_szelesseg, tmp_palya_magassag];
                                    palya_szelesseg = tmp_palya_szelesseg;
                                    palya_magassag = tmp_palya_magassag;
                                }

                                byte[] t = br.ReadBytes((int)(palya_magassag * palya_szelesseg));

                                for (int i = 0, y = 0; y < palya_magassag; y++)
                                    for (int x = 0; x < palya_szelesseg; x++)
                                        Palya[x, y] = (CellaTipus)t[i++];

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
                textBox1.Text = "";
            }
        }

        private void Form1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            panel1.Refresh();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
           
        }

        private void panel1_Click(object sender, EventArgs e)
        {
            panel1.Focus();
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
    }
}
