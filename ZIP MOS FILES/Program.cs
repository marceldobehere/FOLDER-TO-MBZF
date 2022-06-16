using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ZIP_MOS_FILES
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("No Folder given!");
                Console.ReadLine();
                return;
            }

            string[] files = Directory.GetFiles(args[0]);

            long size = 12;

            byte[][] filedata = new byte[files.Length][];
            string[] names = new string[files.Length];
            {
                int i = 0;
                foreach (string file in files)
                {
                    string name = Path.GetFileName(file);
                    size += 4 + name.Length;
                    names[i] = name;

                    filedata[i] = File.ReadAllBytes(file);
                    size += 8 + filedata[i].LongLength;
                    i++;
                }
            }

            Console.WriteLine($"Filesize: {size} Bytes.");

            using (BinaryWriter writer = new BinaryWriter(new FileStream($"{args[0]}.mbzf", FileMode.Create))) // marcel binary zip format
            {
                writer.Write(size); //filesize
                writer.Write(files.Length); // amount of files
                for (int i = 0; i < files.Length; i++)
                {
                    writer.Write(names[i].Length);
                    foreach (char c in names[i])
                        writer.Write((byte)c);

                    writer.Write(filedata[i].LongLength);
                    writer.Write(filedata[i]);
                }
            }

            Console.WriteLine("\n\nEnd.");
            Console.ReadLine();
            return;
        }
    }
}
