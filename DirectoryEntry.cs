using System;
using System.Collections.Generic;
using System.Text;

namespace OS_Project
{
	public class DirectoryEntry
	{

		public char[] Fname = new char[11];
		public byte attribute;     //0 file		//1 folder
		public byte[] file_Empty = new byte[12];
		public int firstCluster;
		public int fileSize;

		public DirectoryEntry()
        {

        }
		public  DirectoryEntry(string n, byte attr, int fc,int size)
		{
			string name = n;
			attribute = attr;
			fileSize = size;
			if (fc == 0)
			{
				fc = Fat_Tabel.getAvaliablIndex();
				firstCluster = fc;
			}
			else
            {
				firstCluster = fc;
            }
			
			// check that the file name contains .
			if (attribute == 0)
			{
				if (n.Length > 11)
				{
					name = n.Substring(0, 7) + n.Substring(n.Length - 4);
				}
				else
				{
					name = n;
				}

			}
			else
			{
				
				name = n.Substring(0, Math.Min(11, n.Length));
			}
            for (int i = 0; i < name.Length; i++)
            {
				Fname[i] = name[i];


			}
		}

		public byte[] getBytes()
		{
			byte[] b = new byte[32];
			for (int i = 0; i <Fname.Length; i++)
			{
				
				b[i] = Convert.ToByte(Fname[i]);
			}
			b[11] = attribute ;
			for (int i = 12; i < 24; i++)
			{
				b[i] = file_Empty[i - 12];
			}
			Byte[] bt = new Byte[4];
			bt = BitConverter.GetBytes(firstCluster);
			for (int i = 24; i < 28; i++)
			{
				b[i] = bt[i - 24];
			}
			bt = BitConverter.GetBytes(fileSize);
			for (int i = 28; i < 32; i++)
			{
				b[i] = bt[i - 28];
			}

			return b;
		}
		public  DirectoryEntry GetDirectory (byte[]b)
        {
			List<byte[]> bt = new List<byte[]>();
			List<byte> bs = new List<byte>();
			for (int i = 0; i < 11; i++)
			{

				bs.Add(b[i]) ;
			}
			bt.Add(bs.ToArray());
			bs.Clear();
			bs.Add(b[11]);
			bt.Add(bs.ToArray());
			bs.Clear();
			for (int i = 12; i < 24; i++)
			{
				bs.Add(b[i]);
			}
			bt.Add(bs.ToArray());
			bs.Clear();
			for (int i = 24; i < 28; i++)
			{
				bs.Add(b[i]);
			}
			bt.Add(bs.ToArray());
			bs.Clear();
			for (int i = 28; i < 32; i++)
			{
				bs.Add(b[i]);
			}
			bt.Add(bs.ToArray());
			DirectoryEntry c = new DirectoryEntry();
			c.Fname = Encoding.ASCII.GetString(bt[0]).ToCharArray();
			c.attribute = bt[1][0];
			c.file_Empty = bt[2];
			byte[] ba = new byte[4];
			ba = bt[3];
			c.firstCluster = BitConverter.ToInt32(ba, 0);
			ba = bt[4];
			c.fileSize = BitConverter.ToInt32(ba, 0);


			return c;
        }

	}




}
