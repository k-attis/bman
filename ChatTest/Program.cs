﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

using Bomberman.KozosKod;
using System.IO;

namespace ChatTest
{
    class Program
    {
        static void Main(string[] args)
        {
            TcpClient tcpc = new TcpClient();
            tcpc.Connect("sanyi", 60000);

            using (BinaryWriter bw = new BinaryWriter(tcpc.GetStream()))
            {
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
                        if (Console.KeyAvailable)
                        {
                            String s = Console.ReadLine();
                            bw.Write((byte)Jatekos_Uzi_Tipusok.Chat);
                            bw.Write(s);
                            bw.Flush();
                        }

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
                                case Server_Uzi_Tipusok.Chat:
                                    String s = br.ReadString();
                                    Console.WriteLine(s);
                                    break;
                            }
                        }
                    }
                }
            }
        }
    }
}
