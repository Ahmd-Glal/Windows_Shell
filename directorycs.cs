using System;
using System.Collections.Generic;

namespace OS_Project
{
    public class directory : DirectoryEntry
	{
		public   List<DirectoryEntry> Directory_Table = new List<DirectoryEntry>();
	    public   directory parent;
       
		public directory()
        {

        }
       public directory(string n, byte attr, int fc,int size,directory p) : base( n,  attr,  fc,size)
		{
		 this.parent = p;
			
			
		}



		public void write_directory()
		{
			byte[] DTB = new byte[32 * Directory_Table.Count];
			byte[] DEB = new byte[32];
			for (int i = 0; i < Directory_Table.Count; i++)
			{
				DEB = Directory_Table[i].getBytes();
				for (int j = i * 32; j < 32 * (i + 1); j++)
				{
					DTB[j] = DEB[j % 32];
				}

			}
			int num_of_req_block = (int)Math.Ceiling(DTB.Length / 1024.0);
			int num_no_full_size_block = (DTB.Length / 1024);
			int remainder = DTB.Length % 1024;
			List<byte[]> blocks = new List<byte[]>();
			byte[] temp = new byte[1024];
			for (int i = 0; i < num_no_full_size_block; i++)
			{


				for (int j = 0; j < 1024; j++)
				{
					temp[j] = DTB[j + i * 1024];
				}
				blocks.Add(temp);
			}
			int indexR = (num_no_full_size_block * 1024);

			for (int i = 0; i < remainder; i++, indexR++)
			{

				temp[i] = DTB[indexR];


			}
			if (remainder > 0)
			{
				blocks.Add(temp);
			}


			int fc = 0, lc = -1;
			if (firstCluster != 0)
			{
				fc = firstCluster;
			}
			else
			{
				fc = Fat_Tabel.getAvaliablIndex();
				firstCluster = fc;
			}

			for (int i = 0; i < num_of_req_block; i++)
			{
				Virtual_disk.write_block(blocks[i], fc);
				Fat_Tabel.setNext(fc, -1);
				if (lc != -1)
				{
					Fat_Tabel.setNext(lc, fc);

					fc = Fat_Tabel.getAvaliablIndex();
				}
				lc = fc;

			}
			Fat_Tabel.write_fat();
		



		}
		public void Read_directory() 
		{
			List<byte> ls = new List<byte>();
			byte[] d = new byte[32];
			int fc=0, Nc;
			if (firstCluster != 0)
			{
				fc = firstCluster;
			}
			Nc = Fat_Tabel.getNext(fc);
			do
			{
				ls.AddRange(Virtual_disk.read_block(fc));
				
				if (fc != -1)
				{
					Nc = Fat_Tabel.getNext(fc);
				}
				fc = Nc;
			}
			while (fc != -1);
			bool flage = false;
			for(int i =0; i<ls.Count/32;i++)
            {
                for (int j = 0; j < 32; j++)
                {
					if(ls[j + (i * 32)]==(byte)'#' )
                    {
						flage = true;
						break;
                    }
					d[j] = ls[j +( i * 32)];
                }
				if (flage)
					break;
				if(GetDirectory(d).firstCluster!=0 )
				    Directory_Table.Add(GetDirectory(d));
            }
			
			
		}


		public int Search_directory(string name)
        {
			
			
			Read_directory();
			
            for (int i = 0; i < Directory_Table.Count; i++)
            {
				string s = "";
				for (int j = 0; j < Directory_Table[i].Fname.Length; j++)
				{
					if (Directory_Table[i].Fname[j] == '\0')
					{
						break;
					}
						 
				 s += Directory_Table[i].Fname[j];
				}

				if (s == name)
				{
					return i;
				}
				
            }
			return -1;
        }


		public void Update_content(DirectoryEntry d)
        {

			Read_directory();
			int index = Search_directory(d.Fname.ToString());
            if (index != -1)
            {
				Directory_Table.RemoveAt(index);
				Directory_Table.Insert(index, d);
            }

        }
		
		public void delete_directory()
		{
			int index, next;
			if (firstCluster != 0)
			{
				index =firstCluster;
				next = Fat_Tabel.getNext(index);


				do
				{
					Fat_Tabel.setNext(index, 0);
					index = next;
					if (index != -1)
					{
						next = Fat_Tabel.getNext(index);
					}
					
				} while (index != -1);
			}
			if(parent!=null)
            {
				parent.Directory_Table.Clear();
				//perant.Read_directory();
				string s = "";

				for (int c = 0; c < Fname.Length&& Fname[c] != '\0'; c++)
				{
					s += Fname[c];

				}
				index = parent.Search_directory(s);
				if(index!=-1)
                {
					parent.Directory_Table.RemoveAt(index);
					parent.write_directory();
                }
						 
            }
			Fat_Tabel.write_fat();

		}
	public directory Getinf()
        {
			directory d = new directory();
			d.Fname = Fname;
			d.fileSize = fileSize;
			d.file_Empty = file_Empty;
			d.firstCluster = firstCluster;
			d.attribute = attribute;
			d.parent = parent;
			return d;
        }

	}
}
