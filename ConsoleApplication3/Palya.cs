﻿using Bomberman.KozosKod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplication3
{
    class Palya
    {
        public readonly uint Szelesseg;
        public readonly uint Magassag;
        public Cella[,] Cellak;

        public Palya(uint Szelesseg, uint Magassag, double kezdeti_cellak_telitettseg_faktor)
        {
            this.Szelesseg = Szelesseg;
            this.Magassag = Magassag;

            Cellak = new Cella[Szelesseg, Magassag];

            for (uint i = 0; i < Szelesseg; i++)
                for (uint j = 0; j < Magassag; j++)
                    Cellak[i, j].Tipus = CellaTipus.Ures;

            for (uint i = 0; i < Szelesseg; i++)
            {
                Cellak[i, 0].Tipus = CellaTipus.Fal;
                Cellak[i, Magassag - 1].Tipus = CellaTipus.Fal;
            }

            for (uint i = 0; i < Magassag; i++)
            {
                Cellak[0, i].Tipus = CellaTipus.Fal;
                Cellak[Szelesseg - 1, i].Tipus = CellaTipus.Fal;
            }

            for (uint i = 2; i < Szelesseg; i += 2)
                for (uint j = 2; j < Magassag; j += 2)
                    Cellak[i, j].Tipus = CellaTipus.Fal;

            for (uint y = 0; y < Magassag; y++)
                for (uint x = 0; x < Szelesseg; x++)
                    if (Cellak[x, y].Tipus == CellaTipus.Ures)
                        if (r.NextDouble() < kezdeti_cellak_telitettseg_faktor)
                            Cellak[x, y].Tipus = CellaTipus.Robbanthato_Fal;
        }

        public bool uresE(uint x, uint y)
        {
            return
                x < Szelesseg
                &&
                y < Magassag
                &&
                Cellak[x, y].Tipus == CellaTipus.Ures;
        }

        public void bomba_telepit(Bomba b)
        {
            Cellak[b.x, b.y].Tipus = CellaTipus.Bomba;
            Cellak[b.x, b.y].Bomba_ID = b.ID;
        }

        public void lang_telepit(Lang l)
        {
            Cellak[l.x, l.y].Tipus = CellaTipus.Lang;
            Cellak[l.x, l.y].Lang_ID = l.ID;
        }

        void kirajzol()
        {
            for (uint y = 0; y < Magassag; y++)
            {
                for (uint x = 0; x < Szelesseg; x++)
                {
                    Console.SetCursorPosition((int)x, (int)y);
                    Console.ForegroundColor = ConsoleColor.White;
                    switch (Cellak[x, y].Tipus)
                    {
                        case CellaTipus.Ures: Console.ForegroundColor = ConsoleColor.DarkGreen; Console.Write(' '); break;
                        case CellaTipus.Fal: Console.ForegroundColor = ConsoleColor.Gray; Console.Write('█'); break; //219
                        case CellaTipus.Robbanthato_Fal: Console.ForegroundColor = ConsoleColor.Cyan; Console.Write('▒'); break; //177
                        case CellaTipus.Bomba: Console.ForegroundColor = ConsoleColor.Magenta; Console.Write('☼'); break;
                        case CellaTipus.Lang: Console.ForegroundColor = ConsoleColor.Red; Console.Write('x'); break;
                        default: Console.Write('?'); break;
                    }
                }
            }

            /*foreach (Jatekos j in Jatekosok.Values.ToList())
            {
                Console.SetCursorPosition((int)j.x, (int)j.y);
                Console.Write(j.Nev);
            }*/
        }

        public void cellaTorol(uint x, uint y)
        {
            Cellak[x, y].Tipus = CellaTipus.Ures;
        }
    }
}