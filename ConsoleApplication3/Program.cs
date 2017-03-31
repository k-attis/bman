using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using System.Collections.Concurrent;
using Bomberman.KozosKod;

namespace ConsoleApplication3
{
    class Program
    {
        static Random r = new Random();

        static Palya palya = new Palya(20, 20, 0.7);

        static Dictionary<uint, Jatekos> Jatekosok = new Dictionary<uint, Jatekos>();
        static Dictionary<uint, Bomba> Bombak = new Dictionary<uint, Bomba>();
        static Dictionary<uint, Lang> Langok = new Dictionary<uint, Lang>();

        static void jatekos_pozicio_generalas()
        {
            uint x_db = (palya.Szelesseg - 2 - 1) / 2;
            uint y_db = (palya.Magassag - 2 - 1) / 2;

            for (int i = 0; i < Jatekosok.Count; i++)
            {
                while (true)
                {
                    uint x = (uint)(1 + r.Next((int)x_db) * 2);
                    uint y = (uint)(1 + r.Next((int)y_db) * 2);

                    bool talaltunke = false;

                    for (int j = 0; j < i; j++)
                    {
                        Jatekos jj = Jatekosok.Values.ElementAt(j);
                        if (jj.x == x && jj.y == y)
                        {
                            talaltunke = true;
                            break;
                        }
                    }

                    if (!talaltunke)
                    {
                        Jatekos jj = Jatekosok.Values.ElementAt(i);
                        jj.x = x;
                        jj.y = y;

                        palya.Cellak[x, y].Tipus = CellaTipus.Ures;
                        palya.Cellak[x + 1, y].Tipus = CellaTipus.Ures;
                        palya.Cellak[x, y + 1].Tipus = CellaTipus.Ures;
                        break;
                    }
                }

            }
        }

        static void bomba_telepites(uint jatekos_ID, uint bomba_x, uint bomba_y)
        {
            Jatekos j;
            if (!Jatekosok.TryGetValue(jatekos_ID, out j))
                return;

            if (j.Actbombaszam >= j.Maxbombaszam)
                return;

            if (!palya.uresE(bomba_x, bomba_y))
                return;

            Bomba b = new Bomba(j, bomba_x, bomba_y);

            Bombak.Add(b.ID, b);

            palya.bomba_telepit(b);

            j.Actbombaszam++;
        }

        static void bomba_check()
        {
            foreach (Bomba b in Bombak.Values.ToList())
                if (b.Mikor_Robban <= DateTime.Now)
                    bomba_robban(b.ID);
        }

        static void bomba_robban(uint bomba_id)
        {
            Bomba b;
            if (!Bombak.TryGetValue(bomba_id, out b))
                return;

            Bombak.Remove(b.ID);

            Jatekos j;
            if (Jatekosok.TryGetValue(b.Jatekos_ID, out j))
            {
                if (j.Actbombaszam <= 0)
                    j.Actbombaszam = 0;

                else
                    j.Actbombaszam--;
            }

            palya.cellaTorol(b.x, b.y);

            lang_telepit(b.x, b.y, b);

            uint x = b.x;
            uint y = b.y;

            bool felmehete = true;
            bool jobbramehet = true;
            bool lemehet = true;
            bool balramehet = true;

            for (uint i = 1; i <= b.Rendzs; i++)
            {
                if (felmehete)
                    felmehete = lang_telepit(b.x, b.y - i, b);
                if (jobbramehet)
                    jobbramehet = lang_telepit(b.x + i, b.y, b);
                if (lemehet)
                    lemehet = lang_telepit(b.x, b.y + i, b);
                if (balramehet)
                    balramehet = lang_telepit(b.x - i, b.y, b);
            }
        }

        static bool lang_telepit(uint lang_x, uint lang_y, Bomba b)
        {
            if (lang_x >= palya.Szelesseg || lang_y >= palya.Magassag)
                return false;

            foreach (Jatekos j in Jatekosok.Values)
                if (j.x == lang_x && j.y == lang_y)
                    jatekosMeghal(j, b.Jatekos_ID);

            switch (palya.Cellak[lang_x, lang_y].Tipus)
            {
                case CellaTipus.Ures:
                    {
                        Lang l = new Lang(b.Jatekos_ID, lang_x, lang_y);

                        Langok.Add(l.ID, l);

                        palya.lang_telepit(l);

                        return true;
                    }
                case CellaTipus.Fal:
                    {
                        return false;
                    }
                case CellaTipus.Lang:
                    {
                        Langok.Remove(palya.Cellak[lang_x, lang_y].Lang_ID);

                        Lang l = new Lang(b.Jatekos_ID, lang_x, lang_y);

                        Langok.Add(l.ID, l);

                        palya.lang_telepit(l);

                        return true;
                    }
                case CellaTipus.Bomba:
                    {
                        bomba_robban(palya.Cellak[lang_x, lang_y].Bomba_ID);
                        return false;
                    }
                case CellaTipus.Robbanthato_Fal:
                    {
                        palya.cellaTorol(lang_x, lang_y);
                        kartya_telepit(lang_x, lang_y, false);
                        return false;
                    }
                default:
                    {
                        palya.cellaTorol(lang_x, lang_y);
                        return false;
                    }
            }
        }

        static void lang_check()
        {
            foreach (Lang l in Langok.Values.ToList())
                if (l.Meddig < DateTime.Now)
                {
                    Langok.Remove(l.ID);
                    palya.cellaTorol(l.x, l.y);
                }
        }


        static void kartya_telepit(uint kartya_x, uint kartya_y, bool force)
        {
            if (kartya_x >= palya.Szelesseg || kartya_y >= palya.Magassag)
                return;

            if (palya.Cellak[kartya_x, kartya_y].Tipus != CellaTipus.Ures)
                return;

            palya.Cellak[kartya_x, kartya_y].Tipus =
                KartyaGenerator.general(force);
        }

        static Thread info;

        static void info_szal()
        {
            UdpClient c = new UdpClient();
            c.EnableBroadcast = true;

            IPEndPoint ep = new IPEndPoint(IPAddress.Broadcast, 60001);

            while (true)
            {
                using (MemoryStream ms = new MemoryStream())
                using (BinaryWriter bw = new BinaryWriter(ms))
                {
                    bw.Write(szerverneve);
                    UInt16 tmp = (UInt16)((szerverjatekban) ? (1) : (0));
                    bw.Write(tmp);
                    tmp = (UInt16)Jatekosok.Count;
                    bw.Write(tmp);
                    c.Send(ms.ToArray(), (int)ms.Length, ep);
                }

                Thread.Sleep(500);
            }
        }

        static bool szerverjatekban = false;
        static String szerverneve = "";

        static void Main(string[] args)
        {
            //szerverneve = Console.ReadLine();
            szerverneve = "Szerver";
            Console.WriteLine("szerver elindult {0} néven", szerverneve);
            info = new Thread(new ThreadStart(info_szal));
            info.Start();

            TcpListener tl = new TcpListener(60000);
            tl.Start();

            while (true) // Csatalakozós ciklus
            {
                if (tl.Pending())
                {
                    Jatekos j = new Jatekos()
                    {
                        tcp = tl.AcceptTcpClient(),
                        thread = new Thread(new ParameterizedThreadStart(jatekos_szal))
                    };
                    j.CsomiSor.Enqueue(new ChatCsomi(0, "Üdv a világomban!"));

                    foreach (Jatekos tmpj in Jatekosok.Values.ToList())
                        j.CsomiSor.Enqueue(new JatekosAdatokCsomi(tmpj));

                    Jatekosok.Add(j.ID, j);

                    j.thread.Start(j);
                }

                if (Console.KeyAvailable)
                    if (Console.ReadKey().KeyChar == 's')
                        break;
            }

            szerverjatekban = true;

            jatekos_pozicio_generalas();

            while (true)
            {
                bomba_check();
                lang_check();
                palya.kirajzol();
                System.Threading.Thread.Sleep(50);
            }
        }

        static bool bombaMozgat(Bomba b, Jatekos_Uzi_Tipusok irany)
        {
            uint uj_x = b.x;
            uint uj_y = b.y;

            switch (irany)
            {
                case Jatekos_Uzi_Tipusok.Lep_Jobbra: uj_x++; break;
                case Jatekos_Uzi_Tipusok.Lep_Le: uj_y++; break;
                case Jatekos_Uzi_Tipusok.Lep_Balra: uj_x--; break;
                case Jatekos_Uzi_Tipusok.Lep_Fel: uj_y--; break;
            }

            if (
                uj_y < 0
                ||
                uj_x < 0
                ||
                uj_y >= palya.Magassag
                ||
                uj_x >= palya.Szelesseg
                )
                return false;

            switch (palya.Cellak[uj_x, uj_y].Tipus)
            {
                case CellaTipus.Fal:
                case CellaTipus.Robbanthato_Fal:
                case CellaTipus.Bomba: return false;
                case CellaTipus.Lang:
                    b.x = uj_x;
                    b.y = uj_y;
                    bomba_robban(b.ID);
                    return true;
                default:
                    b.x = uj_x;
                    b.y = uj_y;
                    return true;
            }
        }

        static void jatekos_lep(Jatekos j, Jatekos_Uzi_Tipusok irany)
        {
            uint uj_x = j.x;
            uint uj_y = j.y;

            switch (irany)
            {
                case Jatekos_Uzi_Tipusok.Lep_Jobbra: uj_x++; break;
                case Jatekos_Uzi_Tipusok.Lep_Le: uj_y++; break;
                case Jatekos_Uzi_Tipusok.Lep_Balra: uj_x--; break;
                case Jatekos_Uzi_Tipusok.Lep_Fel: uj_y--; break;
            }

            if (
                uj_y < 0
                ||
                uj_x < 0
                ||
                uj_y >= palya.Magassag
                ||
                uj_x >= palya.Szelesseg
                )
                return;

            lock (palya)
            {
                switch (palya.Cellak[uj_x, uj_y].Tipus)
                {
                    case CellaTipus.Ures: break;
                    case CellaTipus.Fal: return;
                    case CellaTipus.Robbanthato_Fal: return;
                    case CellaTipus.Bomba:
                        if (j.Lab == 0)
                            return;

                        Bomba b;

                        if (!Bombak.TryGetValue(palya.Cellak[uj_x, uj_y].Bomba_ID, out b))
                            return;

                        if (!bombaMozgat(b, irany))
                            return;

                        j.x = uj_x;
                        j.y = uj_y;

                        for (int i = (int)j.Lab - 1; i > 0; i--)
                            if (!bombaMozgat(b, irany))
                                return;
                        break;
                    case CellaTipus.Lang:
                        UInt32 langid = palya.Cellak[uj_x, uj_y].Lang_ID;

                        Lang l;

                        if (Langok.TryGetValue(langid, out l))
                            jatekosMeghal(j, l.Jatekos_ID);
                        else
                            jatekosMeghal(j, 0);

                        break;
                    case CellaTipus.Bomba_Kartya:
                        j.Maxbombaszam += 1;
                        palya.Cellak[uj_x, uj_y].Tipus = CellaTipus.Ures;
                        break;
                    case CellaTipus.Lang_Kartya:
                        j.Rendzs += 1;
                        palya.Cellak[uj_x, uj_y].Tipus = CellaTipus.Ures;
                        break;
                    case CellaTipus.Halalfej_Kartya: break;
                    case CellaTipus.Sebesseg_Kartya: break;
                    case CellaTipus.Lab_Kartya:
                        j.Lab += 1;
                        palya.cellaTorol(uj_x, uj_y);
                        break;
                    case CellaTipus.Kesztyu_Kartya: break;
                }
            }

            j.x = uj_x;
            j.y = uj_y;
        }

        static void csomiSzoras(Csomi csomi)
        {
            foreach (Jatekos j in Jatekosok.Values.ToList())
                j.CsomiSor.Enqueue(csomi);
        }

        static void jatekosMeghal(Jatekos aldozat, UInt32 tettesJatekosID)
        {
            if (!aldozat.Ele)
                return;

            Jatekos j;
            String tettesNev;

            if (!Jatekosok.TryGetValue(tettesJatekosID, out j))
                tettesNev = "Ismeretlen";
            else
                tettesNev = j.Nev;

            aldozat.Ele = false;

            csomiSzoras(
                new ChatCsomi(
                    aldozat.ID,
                    String.Format(
                        "Jajj meghaltam, Tettes:{0}",
                        tettesNev
                        )
                )
            );
        }

        static void jatekos_szal(Object param)
        {
            Jatekos j = (Jatekos)param;

            try
            {
                using (BinaryWriter bw = new BinaryWriter(j.tcp.GetStream()))
                {
                    using (BinaryReader br = new BinaryReader(j.tcp.GetStream()))
                    {
                        bool Bemutatkozott = false;

                        while (true)
                        {
                            if (j.tcp.Available > 0)
                            {
                                int uzi_tipus = br.ReadByte();
                                switch ((Jatekos_Uzi_Tipusok)uzi_tipus)
                                {
                                    case Jatekos_Uzi_Tipusok.Bemutatkozik:
                                        Bemutatkozott = true;
                                        j.Nev = br.ReadString();
                                        UInt32 hossz = br.ReadUInt32();
                                        j.Arc = br.ReadBytes((int)hossz);

                                        csomiSzoras(new JatekosAdatokCsomi(j));
                                        break;
                                    case Jatekos_Uzi_Tipusok.Lep_Fel:
                                    case Jatekos_Uzi_Tipusok.Lep_Jobbra:
                                    case Jatekos_Uzi_Tipusok.Lep_Le:
                                    case Jatekos_Uzi_Tipusok.Lep_Balra:
                                        if (!Bemutatkozott)
                                            break;
                                        if (!j.Ele)
                                            break;
                                        jatekos_lep(j, (Jatekos_Uzi_Tipusok)uzi_tipus);
                                        break;
                                    case Jatekos_Uzi_Tipusok.Bombat_rak:
                                        if (!Bemutatkozott)
                                            break;
                                        if (!j.Ele)
                                            break;
                                        bomba_telepites(j.ID, j.x, j.y);
                                        break;
                                    case Jatekos_Uzi_Tipusok.Chat:
                                        String uzike = br.ReadString();
                                        csomiSzoras(new ChatCsomi(j.ID, uzike));
                                        break;
                                }
                            }

                            j.CsomiSor.Enqueue(new JatekosokPoziciojaCsomi(Jatekosok.Values.ToList()));
                            j.CsomiSor.Enqueue(new PalyakepCsomi(palya));

                            Csomi tmp;

                            while (j.CsomiSor.TryDequeue(out tmp))
                                tmp.becsomagol(bw);

                            System.Threading.Thread.Sleep(25);
                        }
                    }
                }
            }
            catch
            {
                csomiSzoras(new ChatCsomi(j.ID, "***TCP CONNECTION KILLED***"));
            }
            finally
            {
                Jatekosok.Remove(j.ID);
                csomiSzoras(new ChatCsomi(j.ID, "***CLOSED***"));
            }
        }
    }
}
