using System;
using System.Collections.Generic;
using System.Text;


namespace OS_Project
{
    class File_Entry : DirectoryEntry
    {
        public string content;
        public directory parent;
       public File_Entry()
        {

        }
		
        public File_Entry(string n, byte attr, int fc, string c,directory p) : base(n, attr, fc,c.Length)
        {

            content = c;
            if (p != null)
                parent = p;

        }
       
        public void writeFileContent()
        {
            byte[] b = new byte[content.Length];
            List<byte[]> blocks = new List<byte[]>();
            for (int i = 0; i < content.Length; i++)
            {

                b[i] = Convert.ToByte(content[i]);
            }
            int num_of_req_block = (int)Math.Ceiling(b.Length / 1024.0);
            int num_no_full_size_block = (b.Length / 1024);
            int remainder = b.Length % 1024;
            byte[] temp = new byte[1024];
            for (int i = 0; i < num_no_full_size_block; i++)
            {

                for (int j = 0; j < 1024; j++)
                {
                    temp[j] = b[j + i * 1024];
                }
                blocks.Add(temp);
            }
            int indexR = (num_no_full_size_block * 1024);

            for (int i = 0; i < remainder; i++, indexR++)
            {

                temp[i] = b[indexR];


            }
            if (remainder > 0)
            {
                blocks.Add(temp);
            }
           
            
            int clusterFATIndex;
            if (this.firstCluster != 0)
            {
                clusterFATIndex = this.firstCluster;
            }
            else
            {
                clusterFATIndex = Fat_Tabel.getAvaliablIndex();
                this.firstCluster = clusterFATIndex;
            }
            int lastCluster = -1;
            for (int i = 0; i < blocks.Count; i++)
            {
                if (clusterFATIndex != -1)
                {
                    Virtual_disk.write_block(blocks[i], clusterFATIndex);
                    Fat_Tabel.setNext(clusterFATIndex, -1);
                    if (lastCluster != -1)
                    {
                        Fat_Tabel.setNext(lastCluster, clusterFATIndex);
                        clusterFATIndex = Fat_Tabel.getAvaliablIndex();

                    }
                    lastCluster = clusterFATIndex;
                }
            }
            Fat_Tabel.write_fat();
        }
        public void readFileContent()
        {
            if (this.firstCluster != 0)
            {
                content = string.Empty;
                int cluster = this.firstCluster;
                int next = Fat_Tabel.getNext(cluster);
                List<byte> ls = new List<byte>();
                do
                {
                    ls.AddRange(Virtual_disk.read_block(cluster));
                    cluster = next;
                    if (cluster != -1)
                        next = Fat_Tabel.getNext(cluster);
                }
                while (next != -1);
                List<byte> temp = new List<byte>();
                for (int i = 0; i < ls.Count; i++)
                {
                    if(ls[i]!=0)
                    {
                        temp.Add(ls[i]);
                    }

                }
                content = Encoding.ASCII.GetString(temp.ToArray());
            }
        }
        public void deleteFile()
        {
            if (this.firstCluster != 0)
            {
                int cluster = this.firstCluster;
                int next = Fat_Tabel.getNext(cluster);
                do
                {
                    Fat_Tabel.setNext(cluster, 0);
                    cluster = next;
                    if (cluster != -1)
                        next = Fat_Tabel.getNext(cluster);
                }
                while (cluster != -1);
            }
            if (parent != null)
            {
                parent.Directory_Table.Clear();
                string ss = "";
                for (int i = 0; i < this.Fname.Length && this.Fname[i] != '\0'; i++)
                {
                    ss += this.Fname[i];
                }
                int index = parent.Search_directory(ss);
                if (index != -1)
                {
                    parent.Directory_Table.RemoveAt(index);
                    parent.write_directory();
                    Fat_Tabel.write_fat();
                }
            }
        }
    }






}

