using os;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace OS_Project
{
    public class Fat_Tabel
    {
        public static int[] fatTabel = new int[1024];
        public  static void intialize()
        {
            fatTabel[0] = -1;
            fatTabel[1] = 2;
            fatTabel[2] = 3;
            fatTabel[3] = 4;
            fatTabel[4] = -1;
          

        }
        public static void testing()
        {
            int[] F = Read_fat();

            Console.WriteLine("index" + "            " + "Next");
            for (int i = 0; i < 1024; i++)
            {
                Console.WriteLine(i + "            " + F[i]);
            }
        }
        public static void write_fat()
        {
            string path = Directory.GetCurrentDirectory() + @"\\Virtual_disk.txt";
            FileStream wt = new FileStream(path, FileMode.Open, FileAccess.ReadWrite);
            wt.Seek(1024, SeekOrigin.Begin);
            Byte[] bt = new Byte[1024 * 4];
            Buffer.BlockCopy(fatTabel, 0, bt, 0, bt.Length);
            wt.Write(bt, 0, bt.Length);
            wt.Close();


        }
        public static int[] Read_fat()
        {
            string path = Directory.GetCurrentDirectory() + @"\\Virtual_disk.txt";
            FileStream rd = new FileStream(path, FileMode.Open, FileAccess.ReadWrite);
            rd.Seek(1024, SeekOrigin.Begin);
            Byte[] bt = new Byte[1024 * 4];
            rd.Read(bt, 0, bt.Length);
            rd.Close();
            Buffer.BlockCopy(bt, 0, fatTabel, 0, fatTabel.Length);
            
            
            return fatTabel;
        }

        public static int getNext(int index)
        {
            return fatTabel[index];
        }
        public static void setNext(int index, int value)
        {
            fatTabel[index] = value;

        }
        public static int getAvaliablIndex()
        {
            int[] S = Read_fat();
            for (int i = 0; i < 1024; i++)
            {
                if (S[i] == 0)
                {
                    return i;
                }

            }
            return -1;


        }
        public  static int getAvaliableBlock()
        {
            int[] S = Read_fat();
            int no = 0;
            for (int i = 0; i < 1024; i++)
            {
                if (S[i] == 0)
                {
                    no++;
                }

            }
            return (no==0 ? -1 : no);
        }

        public static int get_free_space()
        {
            
            return getAvaliableBlock()*1024;
        }
    }


}



