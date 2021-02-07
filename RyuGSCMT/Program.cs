using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace RyuGSCMT
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("RyuGSCMT by MHVuze v1.0");
            Console.WriteLine("Guest Shader Cache Manifest Tool for Ryujinx");
            Console.WriteLine("==========");

            if (!File.Exists("cache.zip"))
            {
                Console.WriteLine("Error: cache.zip file doesn't exist.");
                Console.WriteLine("Make sure to place your target guest cache.zip in the same directory as this tool.");
            }
            else
            {
                ulong shaderCacheVersion = 0;
                Console.WriteLine("Enter Guest Cache Version and press Enter:");
                try
                {
                    shaderCacheVersion = ulong.Parse(Console.ReadLine());
                    Console.WriteLine("==========");
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error: " + e.Message);
                    Console.WriteLine("==========");
                    Console.WriteLine("Press Enter to exit...");
                    Console.Read();
                }

                ulong count = 0;
                List<string> entries = new List<string>();

                using (ZipArchive archive = ZipFile.OpenRead("cache.zip"))
                {
                    foreach (ZipArchiveEntry entry in archive.Entries)
                    {
                        if (entry.FullName.Length == 32)
                        {
                            //Console.WriteLine(entry.FullName);
                            entries.Add(entry.FullName);
                            count++;
                        }
                    }
                }

                Console.WriteLine($"{count} potential shaders found in cache.zip.");

                byte[] entriesData = new byte[count * 16];
                for (int i = 0; i < entries.Count; i++)
                {
                    byte[] entry = new byte[16];
                    for (int j = 0; j < 32; j += 2)
                        entry[j / 2] = Convert.ToByte(entries[i].Substring(j, 2), 16);
                    Array.Reverse(entry);
                    entry.CopyTo(entriesData, i * 16);
                }

                using (BinaryWriter writer = new BinaryWriter(File.Open("cache.info", FileMode.Create)))
                {
                    writer.Write(shaderCacheVersion); // version
                    writer.Write((byte)5); // graphics api -> Guest
                    writer.Write((byte)0); // hash type -> XxHash128
                    writer.Write(CalculateCrc16(entriesData)); // crc16
                    writer.Write((uint)0); // padding
                    writer.Write(entriesData);
                }

                Console.WriteLine("New cache.info manifest file written.");
                Console.WriteLine("Don't forget to delete the OpenGL (or other Graphics API) shader cache(s) for the game.");
            }
            Console.WriteLine("==========");
            Console.WriteLine("Press Enter to exit...");
            Console.Read();
        }

        /// <summary>
        /// Calculate a CRC-16 over data.
        /// </summary>
        /// <param name="data">The data to perform the CRC-16 on</param>
        /// <returns>A CRC-16 over data</returns>
        private static ushort CalculateCrc16(byte[] data)
        {
            int crc = 0;

            const ushort poly = 0x1021;

            for (int i = 0; i < data.Length; i++)
            {
                crc ^= data[i] << 8;

                for (int j = 0; j < 8; j++)
                {
                    crc <<= 1;

                    if ((crc & 0x10000) != 0)
                    {
                        crc = (crc ^ poly) & 0xFFFF;
                    }
                }
            }

            return (ushort)crc;
        }
    }
}
