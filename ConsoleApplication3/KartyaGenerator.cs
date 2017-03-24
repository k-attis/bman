using Bomberman.KozosKod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplication3
{
    public class KartyaGenerator
    {
        struct KartyaSuly
        {
            public CellaTipus KartyaTipus;
            public double Suly;
        };

        static List<KartyaSuly> KartyaSulyok = new List<KartyaSuly>
        {
            new KartyaSuly() {
                KartyaTipus = CellaTipus.Bomba_Kartya,
                Suly = 0.3
            },
            new KartyaSuly() {
                KartyaTipus = CellaTipus.Lang_Kartya,
                Suly = 0.3
            },
                        new KartyaSuly() {
                KartyaTipus = CellaTipus.Halalfej_Kartya,
                Suly = 0.1
            },
                                    new KartyaSuly() {
                KartyaTipus = CellaTipus.Sebesseg_Kartya,
                Suly = 0.1
            },
                                                new KartyaSuly() {
                KartyaTipus = CellaTipus.Lab_Kartya,
                Suly = 0.1
            },
                                                            new KartyaSuly() {
                KartyaTipus = CellaTipus.Kesztyu_Kartya,
                Suly = 0.1
            }
        };

        CellaTipus general()
        {

        }
    }
}