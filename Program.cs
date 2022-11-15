using OS_Project;
using System;
using System.Collections.Generic;
using System.IO;

namespace os
{
    // check quit 
    class Program
    {
        public static directory c_dic = new directory();
        public static string c_pos = "" ;

        static void Main(string[] args)
        {
            string[] commands = new string[13];
            string[] details = new string[13];  
            List<KeyValuePair<int, int>> number_of_arguments = new List<KeyValuePair<int, int>>();
            intialize(commands, details, number_of_arguments);
            Virtual_disk disk1 = new Virtual_disk();
            disk1.intialize();
            for (int i = 0; i < c_dic.Fname.Length&&c_dic.Fname[i] != '\0'; i++)
            {
                c_pos += c_dic.Fname[i];
            }
            

           

            

            while (true)
            {

              get_dir();
              check(commands, details, number_of_arguments);
               
            
            }
             
        }
       public  static string return_path()
        {
            return Directory.GetCurrentDirectory();
        }

        static void get_dir()
        {
            

                Console.Write($"{return_path()}\\\\{c_pos}:\\>");
           
        }

        
        static void intialize(string[] commands, string[] details, List<KeyValuePair<int, int>> number_of_arguments)
        {
            commands[0] = "cd";
            details[0] = "Change the current default directory to . If the argument is not present, report the current directory. \n             If the directory does not exist an appropriate error should be reported.";
            number_of_arguments.Add(new KeyValuePair<int, int>(0, 1));

            commands[1] = "cls";//done 
            details[1] = "Clear the screen.";
            number_of_arguments.Add(new KeyValuePair<int, int>(0, 0));


            commands[2] = "dir";
            details[2] = "List the contents of directory.";
            number_of_arguments.Add(new KeyValuePair<int, int>(0, 1));


            commands[3] = "quit";//done 
            details[3] = "Quit the shell.";
            number_of_arguments.Add(new KeyValuePair<int, int>(0,0));


            commands[4] = "copy";
            details[4] = "Copies one or more files to another location";
            number_of_arguments.Add(new KeyValuePair<int, int>(1, 2));

            commands[5] = "del";
            details[5] = "Deletes one or more files.";
            number_of_arguments.Add(new KeyValuePair<int, int>(1, 1));


            commands[6] = "help";//done
            details[6] = "Provides Help information for commands.";
            number_of_arguments.Add(new KeyValuePair<int, int>(0, 1));


            commands[7] = "md";
            details[7] = "Creates a directory.";
            number_of_arguments.Add(new KeyValuePair<int, int>(1, 1000000));


            commands[8] = "rd";
            details[8] = "Removes a directory.";
            number_of_arguments.Add(new KeyValuePair<int, int>(1, 1000000));


            commands[9] = "rename";
            details[9] = "Renames a file.";
            number_of_arguments.Add(new KeyValuePair<int, int>(2, 2));

            commands[10] = "type";
            details[10] = "Displays the contents of a text file.";
            number_of_arguments.Add(new KeyValuePair<int, int>(1, 1));
            commands[11] = "import";
            details[11] = "import text file from your computer .";
            number_of_arguments.Add(new KeyValuePair<int, int>(1,1));
            commands[12] = "export";
            details[12] = "export text file from your computer .";
            number_of_arguments.Add(new KeyValuePair<int, int>(2, 2));


        }
        static void help(string[] commands, string[] details,string s="")
        {
            if (s=="")
            {
                for (int i = 0; i < 13; i++)
                {
                    Console.Write(commands[i]);
                    for (int j = commands[i].Length; j <= 12; j++) Console.Write(" ");
                    Console.WriteLine(details[i] + "\n");
                }
            }
            else
            {
                bool che = false;
                for (int j = 0; j < 11; j++)
                {
                    if (s==commands[j])
                    {
                        che = true;
                        Console.WriteLine(details[j]);
                        break;
                    }
                }
                if (!che)
                {
                    Console.WriteLine("not vaild command");
                }
            }
        }
        static bool cd(string name)
        {
            bool che = false;

            int pos = c_dic.Search_directory(name);
            if (pos != -1)
            {
                if (c_dic.Directory_Table[pos].attribute == 1)
                {
                    int fc = c_dic.Directory_Table[pos].firstCluster;
                    directory d = new directory(name, 1, fc, 0, c_dic);
                    d.write_directory();


                    c_dic = d;
                    
                    che = true;
                }

                else
                {
                    Console.WriteLine("Can't change current directory ");
                }

            }
        
                else
                {
                    string r = "";
                    if (c_dic.parent != null)
                    {

                        for (int j = 0; j < c_dic.parent.Fname.Length && c_dic.parent.Fname[j] != '\0'; j++)
                        {
                            r += c_dic.parent.Fname[j];
                        }

                    }

                    if (name == r || name == "..")
                    {
                        directory d = c_dic.parent;
                        d.Read_directory();
                        c_dic = d;
                        while (c_pos[c_pos.Length - 1] != '\\')
                        {

                            c_pos = c_pos.Remove(c_pos.Length - 1);
                        }
                        c_pos = c_pos.Remove(c_pos.Length - 1);
                        che = true;
                    }
                

                    
               
                }
            return che;


        }
        static void dir()
        {
               c_dic.Directory_Table.Clear();
                                c_dic.Read_directory();
                                int fl_no = 0, dir_no = 0, fl_size = 0;

                                for (int j = 0; j < c_dic.Directory_Table.Count; j++)
                                {
                                    string s = "";

                                    for (int c = 0; c < c_dic.Directory_Table[j].Fname.Length; c++)
                                    {
                                        s += c_dic.Directory_Table[j].Fname[c];

                                    }
                                    if (c_dic.Directory_Table[j].attribute == 1)
                                    {
                                        Console.WriteLine("     <DIR>    " + s);
                                        dir_no++;
                                    }
                                    else
                                    {
                                        Console.WriteLine("      " + c_dic.Directory_Table[j].fileSize + "        " + s );
                                        fl_size += c_dic.Directory_Table[j].fileSize;
                                        fl_no++;
                                    }


                                }
                                Console.WriteLine("     " + fl_no + " File(s)" + "     " + fl_size + " bytes");
                                Console.WriteLine("     " + dir_no + " Dir(s)" + "     " + (Fat_Tabel.get_free_space() - fl_size) + " Free bytes");
                                
        }
        static void check(string[] commands, string[] details, List<KeyValuePair<int, int>> number_of_arguments)
        {
            string order = Console.ReadLine();
            int i = 0;                                  
            List<string> arguments = new List<string>();
            string temp;
            
            while (i < order.Length)
            {
                temp = "";
                while (i < order.Length && order[i] == ' ')
                {
                    i++;
                }
                while (i < order.Length && order[i] != ' ')
                {
                    temp += order[i];
                    i++;
                }
              if(temp.Length>0)arguments.Add(temp);
               

            }
            if (arguments.Count == 0)
            {
                
            }
            else
            {
                bool che = false;
                int index = 0;
                 

                for (int j = 0; j < 13; j++)
                {
                    if (commands[j] == arguments[0] && (arguments.Count - 1 >= number_of_arguments[j].Key && arguments.Count - 1 <= number_of_arguments[j].Value))
                    {
                        che = true;
                        index = j;
                        break;
                    }
                }
                if (che)
                {

                    if (commands[index] == "quit")
                    {
                        Environment.Exit(0);
                    }
                    else if (commands[index] == "cls")
                    {
                        Console.Clear();
                    }

                    else if (commands[index] == "help")
                    {
                        if (arguments.Count == 1)
                            help(commands, details);
                        else
                            help(commands, details, arguments[1]);

                    }

                    else if (commands[index] == "md")
                    {

                        int pos = c_dic.Search_directory(arguments[1]);
                        c_dic.Directory_Table.Clear();
                        c_dic.Read_directory();

                        if (pos == -1)
                        {

                            DirectoryEntry d = new DirectoryEntry(arguments[1], 1, 0, 0);
                            c_dic.Directory_Table.Add(d);
                            c_dic.write_directory();
                            Fat_Tabel.setNext(d.firstCluster, -1);

                            Fat_Tabel.write_fat();

                        }
                        else
                        {
                            Console.WriteLine("Already Exist");
                        }
                        if (c_dic.parent != null)
                        {
                            c_dic.parent.Update_content(c_dic.Getinf());
                            c_dic.write_directory();
                        }

                    }

                    else if (commands[index] == "dir")
                    {
                        if (arguments.Count > 1)
                        {
                            c_dic.Directory_Table.Clear();
                            c_dic.Read_directory();
                            directory old = c_dic;
                            string old_pos = c_pos;
                            string s = arguments[1];
                            string[] d = s.Split('\\');

                            bool z = false;
                            if (d.Length == 1)
                            {
                                z = cd(d[0]);
                            }
                            else
                            {
                                for (int indx = 1; indx < d.Length; indx++)
                                {

                                    z = cd(d[indx]);
                                    if (z == false)
                                    {
                                        break;
                                    }

                                }


                            }
                            if (z)
                            {
                                dir();
                                c_dic = old;
                                c_pos = old_pos;

                            }
                            else
                            {
                                Console.WriteLine("Error : this path \"{0}\" is not exist", arguments[1]);
                            }

                        }
                        else
                        {
                            dir();
                        }
                    }

                    else if (commands[index] == "cd")
                    {

                        if (arguments.Count > 1)
                        {
                            string s = arguments[1];

                            string[] d = s.Split('\\');

                            bool z = false;
                            if (d.Length == 1)
                            {
                                z = cd(d[0]);
                                if (d[0] != "..")
                                {
                                    c_pos += '\\' + d[0];
                                }
                            }
                            else
                            {

                                for (int indx = 1; indx < d.Length; indx++)
                                {

                                    z = cd(d[indx]);
                                    if (z == false)
                                    {
                                        break;
                                    }

                                }
                                string dd = "", ddp = " ";
                                for (int j = 0; j < c_dic.Fname.Length && c_dic.Fname[j] != '\0'; j++)
                                {
                                    dd += c_dic.Fname[j];
                                }
                                for (int j = 0; j < c_dic.parent.Fname.Length && c_dic.parent.Fname[j] != '\0'; j++)
                                {
                                    ddp += c_dic.parent.Fname[j];
                                }
                                if (ddp != dd)
                                { c_pos = s; }

                            }
                            if (z)
                            {

                            }
                            else
                            {
                                Console.WriteLine("Error : this path \"{0}\" is not exist", arguments[1]);
                            }


                        }
                        else
                        {
                            Console.WriteLine(c_pos + ':');
                        }


                    }
                    else if (commands[index] == "rd")
                    {
                        c_dic.Directory_Table.Clear();
                        int pos = c_dic.Search_directory(arguments[1]);
                        if (pos != -1)
                        {
                            if (c_dic.Directory_Table[pos].attribute == 1)
                            {
                                int fc = c_dic.Directory_Table[pos].firstCluster;
                                directory d = new directory(arguments[1], 1, fc, 0, c_dic);
                                d.write_directory();
                                d.delete_directory();




                            }
                        }
                        else
                        {
                            Console.WriteLine("Not Exist");
                        }
                    }
                    else if (commands[index] == "rename")
                    {

                        int pos = c_dic.Search_directory(arguments[1]);
                        if (pos != -1)
                        {
                            if (c_dic.Directory_Table[pos].attribute == 0)
                            {
                                c_dic.Directory_Table.Clear();
                                if (c_dic.Search_directory(arguments[2]) == -1)
                                {
                                    string s = "";
                                    File_Entry f = new File_Entry(arguments[1], 0, c_dic.Directory_Table[pos].firstCluster, s, c_dic);
                                    f.readFileContent();
                                    File_Entry nw = new File_Entry(arguments[2], 0, f.firstCluster, f.content, f.parent);
                                    f.deleteFile();
                                    nw.writeFileContent();
                                    DirectoryEntry d = new DirectoryEntry(arguments[2], 0, nw.firstCluster, nw.fileSize);
                                    c_dic.Directory_Table.Add(d);
                                    c_dic.write_directory();
                                }

                                else
                                {
                                    Console.WriteLine($" that file : {arguments[2]} Already Exist");

                                }



                            }
                        }
                        else
                        {
                            Console.WriteLine($"that file: {arguments[1]} doesn’t exist on your disk.");
                        }
                    }
                    else if (commands[index] == "type")
                    {
                        c_dic.Directory_Table.Clear();
                        int pos = c_dic.Search_directory(arguments[1]);
                        if (pos != -1)
                        {
                            if (c_dic.Directory_Table[pos].attribute == 0)
                            {
                                string s = "";
                                File_Entry f = new File_Entry(arguments[1], 0, c_dic.Directory_Table[pos].firstCluster, s, c_dic);
                                f.readFileContent();
                                for (int x = 0; x < f.content.Length; x++)
                                {
                                    if (f.content[x] != '\n')
                                    {
                                        Console.Write(f.content[x]);
                                    }
                                    else
                                    {
                                        Console.WriteLine();
                                    }

                                }
                                Console.WriteLine();


                            }
                        }
                        else
                        {
                            Console.WriteLine($"that file: {arguments[1]} doesn’t exist on your disk.");
                        }
                    }
                    else if (commands[index] == "del")
                    {

                        int pos = c_dic.Search_directory(arguments[1]);
                        if (pos != -1)
                        {
                            if (c_dic.Directory_Table[pos].attribute == 0)
                            {

                                string s = "";
                                File_Entry f = new File_Entry(arguments[1], 0, c_dic.Directory_Table[pos].firstCluster, s, c_dic);
                                f.readFileContent();
                                f.deleteFile();



                            }
                            else
                            {
                                Console.Write("Error  : del Can't Remove Directory  use  rd To Remove it  ");

                            }
                        }
                        else
                        {
                            Console.WriteLine($"that file: {arguments[1]} doesn’t exist on your disk.");
                        }
                    }
                    else if (commands[index] == "import")
                    {
                        if (File.Exists(arguments[1]))
                        {
                            string cds = arguments[1];
                            string[] d = cds.Split('\\');
                            string s = File.ReadAllText(arguments[1]);
                            c_dic.Directory_Table.Clear();
                            if (c_dic.Search_directory(d[d.Length - 1]) == -1)
                            {
                                File_Entry f = new File_Entry(d[d.Length - 1], 0, 0, s, c_dic);
                                f.writeFileContent();
                                f.readFileContent();
                                DirectoryEntry nw = new DirectoryEntry(d[d.Length - 1], 0, f.firstCluster, f.fileSize);
                                c_dic.Directory_Table.Add(nw);
                                c_dic.write_directory();
                            }
                            else
                            {
                                Console.WriteLine($" that file : {d[d.Length - 1]} Already Exist");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Error : this  file in  path \"{0}\" is not exist", arguments[1]);
                        }

                    }
                    else if (commands[index] == "export")
                    {
                        int pos = c_dic.Search_directory(arguments[1]);
                        if (pos != -1)
                        {
                            string s = "";
                            File_Entry f = new File_Entry(arguments[1], 0, c_dic.Directory_Table[pos].firstCluster, s, c_dic);
                            f.readFileContent();
                            string path = arguments[2] + '\\' + arguments[1];
                            if (!File.Exists(path))
                            {
                                File.WriteAllText(path, f.content);
                            }
                            else
                            {
                                Console.WriteLine($" that file : {arguments[1]} Already Exist");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Error : this  file  \"{0}\" is not exist", arguments[1]);
                        }
                    }
                    else if (commands[index] == "copy")
                    { 
                        if(arguments.Count==2)
                        {

                            c_dic.Directory_Table.Clear();
                            c_dic.Read_directory();
                            directory old = c_dic;
                            string old_pos = c_pos;
                            string s = arguments[1];
                            string[] d = s.Split('\\');
                            if (d.Length==1)
                            {
                                c_dic.Directory_Table.Clear();
                                if (c_dic.Search_directory(d[d.Length - 1])!=-1)
                                {
                                    Console.WriteLine("that the file cannot be copied onto itself");
                                    Console.WriteLine(" 0  file(s)  copied ");

                                }
                                else
                                {
                                    Console.WriteLine($"that file: {arguments[1]} doesn’t exist on your disk.");
                                }
                            }
                            else 
                            {
                                c_dic.Directory_Table.Clear();
                                if (c_dic.Search_directory(d[d.Length - 1]) == -1)
                                {

                                    bool z = false;
                                    for (int indx = 1; indx < d.Length - 1; indx++)
                                    {

                                        z = cd(d[indx]);
                                        if (z == false)
                                        {
                                            break;
                                        }

                                    }
                                    if (z)
                                    {

                                        c_dic.Directory_Table.Clear();
                                        int pos = c_dic.Search_directory(d[d.Length - 1]);
                                        if (pos != -1)
                                        {
                                            string r = "";
                                            File_Entry f = new File_Entry(d[d.Length - 1], 0, c_dic.Directory_Table[pos].firstCluster, r, c_dic);
                                            f.readFileContent();
                                            File_Entry nwf = new File_Entry(d[d.Length - 1], 0, 0, f.content, old);
                                            nwf.writeFileContent();
                                            nwf.readFileContent();
                                            DirectoryEntry nwd = new DirectoryEntry(d[d.Length - 1], 0, nwf.firstCluster, nwf.fileSize);

                                            old.Directory_Table.RemoveRange((old.Directory_Table.Count / 2), old.Directory_Table.Count / 2);
                                            old.Directory_Table.Add(nwd);
                                            c_dic = old;
                                            c_pos = old_pos;
                                            c_dic.write_directory();
                                            Console.WriteLine(" 1  file(s)  copied ");
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("Error : this path \"{0}\" is not exist", arguments[1]);
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("that the file cannot be copied onto itself");
                                    Console.WriteLine(" 0  file(s)  copied ");
                                }
                                

                            }

                        }
                        else
                        {
                            string s = arguments[1];
                            string[] d = s.Split('\\');
                            string s2 = arguments[2];
                            string[] d2 = s2.Split('\\');
                            File_Entry f = new File_Entry();
                            if(d2.Length==1)
                            {
                                Console.WriteLine("Error : this  file  \"{0}\" is not exist", arguments[1]);

                            }
                            else
                            {
                                if(d.Length==1)
                                {
                                    c_dic.Directory_Table.Clear();
                                    int posd = c_dic.Search_directory(d[d.Length - 1]);
                                    if ( posd != -1)
                                    {
                                        c_dic.Directory_Table.Clear();
                                        c_dic.Read_directory();
                                        directory old = c_dic;
                                        string old_pos = c_pos;
                                        string e = "";
                                        File_Entry cp1 = new File_Entry(d[d.Length - 1], 0, c_dic.Directory_Table[posd].firstCluster, e, c_dic);
                                        cp1.readFileContent();
                                        f = cp1;
                                        c_dic.Directory_Table.Clear();
                                        if (c_dic.Search_directory(d2[d2.Length - 1]) != -1)
                                        {

                                            bool z = false;
                                            for (int indx = 1; indx < d2.Length ; indx++)
                                            {

                                                z = cd(d2[indx]);
                                                if (z == false)
                                                {
                                                    break;
                                                }

                                            }
                                            if (z)
                                            {

                                                c_dic.Directory_Table.Clear();
                                                int pos = c_dic.Search_directory(d[d.Length - 1]);
                                                if (pos == -1)
                                                {
                                                    string r = "";
                                                    
                                                    File_Entry nwf = new File_Entry(d[d.Length-1], 0, 0, f.content, old);
                                                    nwf.writeFileContent();
                                                    nwf.readFileContent();
                                                    DirectoryEntry nwd = new DirectoryEntry(d[d.Length - 1], 0, nwf.firstCluster, nwf.fileSize);

                                                    old.Directory_Table.RemoveRange((old.Directory_Table.Count / 2), old.Directory_Table.Count / 2);
                                                    c_dic.Directory_Table.Add(nwd);
                                                    c_dic.write_directory();
                                                    c_dic = old;
                                                    c_pos = old_pos;
                                                    Console.WriteLine(" 1  file(s)  copied ");
                                                }
                                                else
                                                {
                                                    Console.WriteLine("that the file cannot be copied onto itself");
                                                    Console.WriteLine(" 0  file(s)  copied ");
                                                }
                                            }
                                            else
                                            {
                                                Console.WriteLine("Error : this path \"{0}\" is not exist", arguments[1]);
                                            }
                                        }
                                        

                                    }
                                    else
                                    {
                                        Console.WriteLine($"that file: {arguments[1]} doesn’t exist.");
                                    }

                                }
                            }

                        }
                        
                                
                             
                            
                        




                    }     









                    
                }
                else
                    Console.WriteLine("not vaild command");

            }
        }
    }

}
