using Bomberman.KozosKod;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kliens
{
    class Jatekter : Control
    {
        Bitmap buffer;
        Graphics bufferg;

        uint palya_szelesseg;
        uint palya_magassag;

        uint cell_size;
        uint offset_x;
        uint offset_y;

        uint CellaX2PixelX(uint CellaX)
        {
            return offset_x + CellaX * cell_size;
        }

        uint CellaY2PixelY(uint CellaY)
        {
            return offset_y + CellaY * cell_size;
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            try
            {
                arcom = new Bitmap(@"c:\zzz\turtle.jpg");
                arcom.MakeTransparent(Color.FromArgb(255, 255, 255));
            }
            catch { }            
        }

        public byte[] getArcom()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                arcom.Save(ms, ImageFormat.Png);
                return ms.ToArray();
            }
        }

        public CellaTipus[,] Palya;

        public Dictionary<uint, Jatekos> JatekosLista = new Dictionary<uint, Jatekos>();

        /*public void palya_init(uint szelesseg, uint magassag)
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
        }*/

        Bitmap arcom;

        public void Palyakep(uint Szelesseg, uint Magassag, byte[] Adatok)
        {
            if (Magassag != palya_magassag
                ||
                Szelesseg != palya_szelesseg)
            {
                Palya = new CellaTipus[Szelesseg, Magassag];
                palya_szelesseg = Szelesseg;
                palya_magassag = Magassag;
            }

            for (int i = 0, y = 0; y < palya_magassag; y++)
                for (int x = 0; x < palya_szelesseg; x++)
                    Palya[x, y] = (CellaTipus)Adatok[i++];
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (buffer == null)
                return;

            bufferg.Clear(Color.Green);

            if (palya_szelesseg > 0 && palya_magassag > 0)
            {
                try
                {
                    uint cell_width = ((uint)buffer.Width - 10) / palya_szelesseg;
                    uint cell_height = ((uint)buffer.Height - 10) / palya_magassag;
                    cell_size = (cell_width < cell_height) ? (cell_width) : (cell_height);

                    offset_x = ((uint)buffer.Width - cell_size * palya_szelesseg) / 2;
                    offset_y = ((uint)buffer.Height - cell_size * palya_magassag) / 2;

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
                                case CellaTipus.Bomba_Kartya:
                                    {
                                        bufferg.FillRectangle(Brushes.Cyan,
                                          CellaX2PixelX(j),
                                          CellaY2PixelY(i),
                                          cell_size,
                                          cell_size);

                                        string ss = Encoding.UTF32.GetString(BitConverter.GetBytes(0x1F4A3));

                                        Font f = new Font("Segoe UI Symbol", cell_size * 0.6f, FontStyle.Bold);

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
                                case CellaTipus.Lang_Kartya:
                                    {
                                        bufferg.FillRectangle(Brushes.Cyan,
                                          CellaX2PixelX(j),
                                          CellaY2PixelY(i),
                                          cell_size,
                                          cell_size);

                                        string ss = Encoding.UTF32.GetString(BitConverter.GetBytes(0x1f525));

                                        Font f = new Font("Segoe UI Symbol", cell_size * 0.6f, FontStyle.Bold);

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
                                case CellaTipus.Lang:
                                    {
                                        string ss = Encoding.UTF32.GetString(
                                          BitConverter.GetBytes(0x1F525)
                                          );

                                        Font f = new Font("Segoe UI Symbol",
                                            cell_size * 0.6f,
                                            FontStyle.Bold);

                                        SizeF s = bufferg.MeasureString(ss, f);

                                        int sox = ((int)cell_size - (int)s.Width) / 2;
                                        int soy = ((int)cell_size - (int)s.Height) / 2;

                                        bufferg.DrawString(ss,
                                            f,
                                            Brushes.Red,
                                            CellaX2PixelX(j) + sox,
                                            CellaY2PixelY(i) + soy);

                                        break;
                                    }
                            }

                    foreach (Jatekos j in JatekosLista.Values.ToList())
                    {
                        string ss;

                        if (j.Ele)
                            ss = Encoding.UTF32.GetString(BitConverter.GetBytes(0x1F410));//1F410,
                        else
                            ss = Encoding.UTF32.GetString(BitConverter.GetBytes(0x271D));

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

                    bufferg.DrawImage(arcom, 0, 0, cell_size, cell_size);
                }
                catch { }
            }

            e.Graphics.DrawImage(buffer, 0, 0);
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            if (buffer != null)
                buffer.Dispose();

            buffer = new Bitmap(Width, Height);
            bufferg = Graphics.FromImage(buffer);

            Invalidate();
        }
    }
}
