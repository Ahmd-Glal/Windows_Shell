using os;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace OS_Project
{
   class Virtual_disk
    {
        
        public  void intialize()
        {
            string path = Directory.GetCurrentDirectory()+ @"\\Virtual_disk.txt";
                    
            
            
            FileInfo Virtual_disk_txt = new FileInfo(path);
            directory root = new directory("H", 1, 5,0, null);
            Fat_Tabel.setNext(5, -1);
            Program.c_dic = root;

           
            if (File.Exists(path))
            {

                Fat_Tabel.fatTabel = Fat_Tabel.Read_fat();
                
                root.Read_directory();
                
                
            }
            else
            {
               
                FileStream wt = Virtual_disk_txt.Open(FileMode.Create, FileAccess.ReadWrite);
                for (int i = 0; i < 1024; i++)
                {
                    wt.WriteByte(0);
                }
                for (int i = 0; i < 4 * 1024; i++)
                {
                    wt.WriteByte((byte)'*');
                }
                for (int i = 0; i < 1019 * 1024; i++)
                {
                    wt.WriteByte((byte)'#');
                }
                wt.Close();
                 Fat_Tabel.intialize();
                 root.write_directory();
                 Fat_Tabel.write_fat();
                
               
            }
            
            
        }
        public static void write_block(byte[] data, int index)
        {
           
            string path = Directory.GetCurrentDirectory() + @"\\Virtual_disk.txt";
            
            FileStream Virtual_disk_text  = new FileStream(path, FileMode.Open, FileAccess.ReadWrite);
            Virtual_disk_text.Seek(1024*index, SeekOrigin.Begin);
            Virtual_disk_text.Write(data, 0, data.Length);
            Virtual_disk_text.Close();

        }
        public static byte[] read_block( int index)
        {
            string path = Directory.GetCurrentDirectory() + @"\\Virtual_disk.txt";
            FileStream Virtual_disk_text = new FileStream(path, FileMode.Open, FileAccess.ReadWrite);
            Virtual_disk_text.Seek(1024 * index, SeekOrigin.Begin);
            Byte[] bt = new Byte[1024];
            Virtual_disk_text.Read(bt, 0, bt.Length);
            Virtual_disk_text.Close();
            return bt;
        }


    }
}
