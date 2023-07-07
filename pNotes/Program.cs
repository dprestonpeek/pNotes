using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace pNotes
{
    class Program
    {
        public static bool surfing = true;
        public static bool searching = false;
        public static string currentDir;

        static string[] internalArgs;
        static string singularFile = "";
        static void Main(string[] externalArgs)
        {
            currentDir = Directory.GetCurrentDirectory();
            string input;
            bool initial = true;
            bool extArgs = false;
            Commands.Initialize();
            AddCommands();
            Notes notes = new Notes();

            if (externalArgs.Length > 0 && !externalArgs[0].Equals(""))
            {
                extArgs = true;
            }
            Output.WriteToConsole("Enter a command or \"help\".");

            while (true)
            {
                if (surfing)
                {
                    if (!initial)
                    {
                        PrintHorizontalBarrier();
                    }
                    initial = false;
                    if (singularFile.Equals("") && !searching)
                    {
                        Output.Write("All Files ~ ");
                    }
                    else
                    {
                        string[] split = singularFile.Split('\\');
                        Output.Write(split[split.Length - 1] + " ~ ");
                    }
                    if (extArgs)
                    {
                        input = "";
                        for (int i = 0; i < externalArgs.Length; i++)
                        {
                            input += externalArgs[i];
                            if (i < externalArgs.Length - 1)
                            {
                                input += " ";
                            }
                        }
                        Output.WriteToConsole(input);
                        extArgs = false;
                    }
                    else
                    {
                        input = Console.ReadLine();
                    }
                    Output.WriteToConsole();

                    foreach (Command command in Commands.List)
                    {
                        if (command.ContainsCommand(input))
                        {
                            try
                            {
                                if (command.EqualsFullCommand(input))
                                {
                                    internalArgs = new string[0];
                                }
                                else
                                {
                                    internalArgs = input.Split(' ');
                                }
                                command.Action.Invoke();
                                break;
                            }
                            catch (Exception e)
                            {
                                Output.WriteToConsole(e.Message);
                            }
                        }
                        if (input == "exit")
                        {
                            break;
                        }
                    }
                    Output.WriteToConsole();
                    if (input == "exit")
                    {
                        break;
                    }
                }
            }
        }

        static void AddCommands()
        {
            //add commands here
            Commands.AddCommand("Print Notes List", new string[2] { "list", "ls" }, "Print names of available files", "Print notes containing specified keyword", PrintNotes);
            Commands.AddCommand("Find Excerpt", new string[2] { "find", "fnd" }, "", "Search files for specified excerpt, print excerpt if found", FindExcerpt);
            Commands.AddCommand("Search Partial Filename", new string[1] { "search" }, "", "Search file names for specified excerpt, print filename if found", SearchFilenames);
            Commands.AddCommand("Read Note", new string[2] { "read", "red" }, "Read selected file", "Print file text in cmd window", ReadNote);
            Commands.AddCommand("Open Note", new string[2] { "open", "opn" }, "Open selected file", "Open file in notepad", OpenNote);
            Commands.AddCommand("Open Note in Code", new string[1] { "code" }, "Open file in VS Code", "Open file in VS Code", OpenNoteInCode);
            Commands.AddCommand("Hone in on Note ", new string[2] { "hone", "hn" }, "Set context to everything", "Set context to file matching specified keyword", HoneInOnNote);
            Commands.AddCommand("New Note", new string[1] { "new" }, "Create new file", "", NewNote);
            Commands.AddCommand("Open Directory", new string[1] { "dir" }, "Open directory of files in explorer", "", OpenDirectory);
            Commands.AddCommand("Clear Console", new string[2] { "clear", "cls" }, "Clear console text", "", ClearConsole);
            Commands.AddCommand("Print History", new string[2] { "his", "uncls" }, "Print collected history", "", PrintHistory);
            Commands.AddCommand("Open Saved Note", new string[2] { "log", "note" }, "Open presaved note", "", OpenSavedNote);
            Commands.AddCommand("Change Path", new string[2] { "path", "where" }, "Print the current path", "Change the path a given directory", DirPath);

            Commands.AddCommand("Help menu", new string[2] { "help", "hlp" }, "Open this help menu", "", PrintHelp);
            Commands.AddCommand("Exit program", new string[1] { "exit" }, "Exit the program", "", null);
        }

        static void PrintHelp()
        {
            Output.WriteToConsole("Commands" + "\t" + "Functions" + "\t\t" + "Desc (no args)" + "\t\t\t" + "Desc (w/ args)");
            foreach (Command command in Commands.List)
            {
                string commands = command.GetCommandsAsString();
                string tab1 = "\t";
                string tab2 = "\t";
                string tab3 = "\t";
                if (commands.Length < 8)
                {
                    tab1 = "\t\t";
                }
                if (command.Name.Length < 8)
                {
                    tab2 = "\t\t\t";
                }
                else if (command.Name.Length < 16)
                {
                    tab2 = "\t\t";
                }
                if (command.NoArgsDesc.Length == 0)
                {
                    tab3 = "\t\t\t\t";
                }
                else if (command.NoArgsDesc.Length < 8)
                {
                    tab3 = "\t\t\t\t";
                }
                else if (command.NoArgsDesc.Length < 16)
                {
                    tab3 = "\t\t\t";
                }
                else if (command.NoArgsDesc.Length < 24)
                {
                    tab3 = "\t\t";
                }
                Output.WriteToConsole(commands + tab1 + command.Name + tab2 + command.NoArgsDesc + tab3 + command.ArgsDesc);
            }
            Output.WriteToConsole();
        }

        static string PromptUser(string question)
        {
            Output.WriteToConsole(question);
            string answer = Console.ReadLine();
            return answer;
        }

        static void DirPath()
        {
            if (internalArgs.Length > 0 && !internalArgs[1].Equals(""))
            {
                if (Directory.Exists(internalArgs[1]))
                {
                    currentDir = internalArgs[1];
                    Output.WriteToConsole("Path changed to " + currentDir);
                }
                else
                {
                    Output.WriteToConsole("Not a valid directory.");
                }
            }
            else
            {
                Output.WriteToConsole(currentDir);
            }
        }

        static void DirPath(string externalArgs)
        {
            if (!externalArgs.Equals(""))
            {
                if (Directory.Exists(externalArgs))
                {
                    currentDir = externalArgs;
                    Output.WriteToConsole("Path changed to " + currentDir);
                }
                else
                {
                    Output.WriteToConsole("Not a valid directory.");
                }
            }
        }

        static void PrintNotes()
        {
            bool usingArgs = false;
            if (internalArgs.Length > 0)
            {
                usingArgs = true;
            }
            List<string> notes;
            if (usingArgs)
            {
                notes = GetNotes(internalArgs[1], false);
            }
            else
            {
                notes = GetNotes("", false);
            }
            foreach (string note in notes)
            {
                Output.WriteToConsole(note);
            }
        }

        static void PrintRecentNotes()
        {
            bool usingArgs = false;
            if (internalArgs.Length > 0)
            {
                usingArgs = true;
            }
            List<string> notes;
            if (usingArgs)
            {
                notes = GetSortedNotes(internalArgs[1], false);
            }
            else
            {
                notes = GetSortedNotes("", false);
            }

            foreach (string note in notes)
            {
                Output.WriteToConsole(note);
            }
        }

        static void PrintHistory()
        {
            if (internalArgs.Length > 1)
            {
                return;
            }
            Output.PrintHistory();
        }

        static List<string> GetNotes(string filename, bool withFilePaths)
        {
            List<string> notes = new List<string>();
            bool usingArgs = false;
            if (internalArgs.Length > 0)
            {
                usingArgs = true;
            }
            foreach (string note in Notes.GetNotes(withFilePaths))
            {
                if (filename.Equals(""))
                {
                    notes.Add(note);
                }
                else
                {
                    if (!usingArgs || (usingArgs && note.Contains(filename)))
                    {
                        notes.Add(note);
                    }
                }
            }
            return notes;
        }

        static List<string> GetSortedNotes(string filename, bool withFilePaths)
        {
            List<string> notes = new List<string>();
            bool usingArgs = false;
            if (internalArgs.Length > 0)
            {
                usingArgs = true;
            }
            foreach (string note in Notes.GetNotesSortedByDate(withFilePaths))
            {
                if (filename.Equals(""))
                {
                    notes.Add(note);
                }
                else
                {
                    if (!usingArgs || (usingArgs && note.Contains(filename)))
                    {
                        notes.Add(note);
                    }
                }
            }
            return notes;
        }

        static void FindExcerpt()
        {
            bool exclude = false;
            bool logNote = false;
            bool recursive = false;
            bool prompt = false;
            string path = "";
            string modifier = "";
            string term = "";
            //int min = 0; int max = 0;
            List<int> mins = new List<int>();
            List<int> maxs = new List<int>();
            List<int> dirsToSearch = new List<int>();
            searching = true;
            if (internalArgs.Length == 0)
            {
                return;
            }
            path = currentDir;
            if (internalArgs.Length > 2)
            {
                for (int i = 1; i < internalArgs.Length; i++)
                {
                    if (i < internalArgs.Length - 1)
                        term += internalArgs[i];
                    if (i < internalArgs.Length - 2)
                        term += " ";
                    if (i == internalArgs.Length - 1 && internalArgs[i][0] == '-')
                        modifier = internalArgs[i];
                    else if (i == internalArgs.Length - 1)
                        term += " " + internalArgs[i];
                }

                //if (modifier.Equals("-log") || internalArgs[2].Equals("-note"))
                //{
                //    logNote = true;
                //}
                if (modifier.Equals("-r"))
                {
                    recursive = true;
                }
                if (modifier.Equals("-rp"))
                {
                    recursive = true;
                    prompt = true;
                }
            }
            else
            {
                term = internalArgs[1];
            }
            Dictionary<int, string> rootDirs = new Dictionary<int, string>();
            if (prompt)
            {
                string[] args = internalArgs;
                int j = 0;
                foreach (string dir in Directory.GetDirectories(currentDir))
                {
                    if (dir == currentDir/* || dir.Contains("netcoreapp3.1")*/)
                        continue;
                    rootDirs.Add(j, dir);
                    j++;
                }

                for (int i = 0; i < rootDirs.Count; i++)
                {
                    Output.WriteToConsole(i + ":" + "\t" + rootDirs[i]);
                }
                Output.WriteToConsole("enter X to cancel");

                string input = "";

                input = PromptUser("Choose a 1 or more root directories to search in. (ex. \"3\", \"5-10\")");
                input = input.Replace(" ", "");
                string[] inputs = input.Split(',');
                foreach (string i in inputs)
                {
                    int min = 0; int max = 0;
                    if (i.Contains("-"))
                    {
                        exclude = true;
                        string[] split = i.Split('-');
                        if (int.TryParse(split[0], out min)) 
                        {
                            mins.Add(min);
                        }
                        if (int.TryParse(split[1], out max))
                        {
                            maxs.Add(max);
                        }
                    }
                    else if (int.TryParse(i, out int rootDir))
                    {
                        prompt = true;
                        min = rootDir;
                        max = rootDir;
                    }

                    if (i.Contains("x") || i.Contains("X"))
                    {
                        prompt = false;
                        exclude = false;
                        searching = false;
                        return;
                    }
                    for (int l = min; l <= max; l++)
                    {
                        dirsToSearch.Add(l);
                    }
                }
            }

            int k = dirsToSearch[0];
            int minDir = int.MaxValue;
            int maxDir = 0;
            foreach (int val in dirsToSearch)
            {
                if (val < minDir)
                    minDir = val;
            }
            foreach (int val in dirsToSearch)
            {
                if (val > maxDir)
                    maxDir = val;
            }
            do
            {
                if (exclude)
                {
                    while (!dirsToSearch.Contains(k))
                    {
                        if (k < maxDir)
                        {
                            k++;
                        }
                        else
                        {
                            exclude = false;
                        }
                    }
                    rootDirs.TryGetValue(k, out path);
                }
                //if (prompt)
                //{
                //    rootDirs.TryGetValue(minDir, out path);
                //}
                Task<List<string>> findExcerptTask = Task<List<string>>.Factory.StartNew(() =>
                {
                    return DoFindExcerpt(term, logNote, recursive, path);
                });

                if (exclude || prompt)
                {
                    Console.WriteLine("Searching for excerpt in " + path);
                }
                else
                {
                    Console.WriteLine("Searching for excerpt ");
                }
                PrintHorizontalBarrier();
                while (!findExcerptTask.IsCompleted)
                {

                }
                searching = false;
                k++;
            } while (exclude);
        }

        static List<string> DoFindExcerpt(string term, bool logNote, bool recursive, string path)
        {
            List<string> toWrite = new List<string>();
            //if (logNote)
            //{
            //    foreach (string line in Notes.FindErrorInNote(term, Properties.Resources.PresetNote))
            //    {
            //        Output.WriteToConsole(line);
            //        //toWrite.Add(line + "\n");
            //    }
            //}
            if (singularFile.Equals(""))
            {
                List<string> dirs = new List<string>();
                dirs.Add(path);
                if (recursive)
                {
                    dirs.AddRange(BuildDirectoriesList(path));
                }

                foreach (string dir in dirs)
                {
                    foreach (string line in Notes.FindExcerpt(term, dir))
                    {
                        Output.WriteToConsole("\n" + line);
                        //toWrite.Add(line + "\n");
                    }
                }
            }
            else
            {
                foreach (string line in Notes.FindExcerptInNote(term, singularFile))
                {
                    Output.WriteToConsole(line);
                    //toWrite.Add(line + "\n");
                }
            }
            return toWrite;
        }

        static void SearchFilenames()
        {
            string term = "";
            if (internalArgs.Length == 0 || internalArgs.Length > 2)
            {
                Output.WriteToConsole("Please enter a valid argument");
                return;
            }
            else
            {
                term = internalArgs[1];
            }
            foreach (string dir in BuildDirectoriesList(currentDir))
            {
                foreach (string file in Directory.GetFiles(dir))
                {
                    if (file.ToLower().Contains(term.ToLower()))
                    {
                        Output.WriteToConsole(file);
                    }
                }
            }
        }

        static List<string> BuildDirectoriesList(string path)
        {
            List<string> dirs = new List<string>();
            foreach (string dir in Directory.GetDirectories(path))
            {
                if (!dir.Contains("netcoreapp3.1"))
                {
                    if (Directory.GetDirectories(dir).Length > 0)
                    {
                        dirs.AddRange(BuildDirectoriesList(dir));
                    }
                    dirs.Add(dir);
                }
            }

            return dirs;
        }

        static void HoneBackOut()
        {
            SetSingularFile("");
        }

        static void HoneInOnNote()
        {
            if (internalArgs.Length == 0)
            {
                HoneBackOut();
                return;
            }
            List<string> notes = GetNotes(internalArgs[1], true);
            if (notes.Count > 1)
            {
                Output.WriteToConsole("Please specify further, these are all the files matching that keyword.");
                foreach (string note in notes)
                {
                    Output.WriteToConsole(note);
                }
            }
            else
            {
                Output.WriteToConsole(notes[0]);
                SetSingularFile(notes[0]);
            }
        }

        static void SetSingularFile(string note)
        {
            singularFile = note;
        }

        static void ReadNote()
        {
            bool usingArgs = false;
            if (internalArgs.Length > 0)
            {
                usingArgs = true;
            }
            if (usingArgs)
            {
                HoneInOnNote();
            }

            if (!singularFile.Equals(""))
            {
                foreach (string line in Notes.GetNote(singularFile))
                {
                    Output.WriteToConsole(line);
                }
            }
        }

        static void OpenNote()
        {
            bool usingArgs = false;
            if (internalArgs.Length > 0)
            {
                usingArgs = true;
            }
            if (usingArgs)
            {
                //HoneInOnNote();
                SetSingularFile(internalArgs[1]);
            }

            if (!singularFile.Equals(""))
            {
                if (Notes.NoteExists(singularFile))
                {
                    Process.Start("notepad.exe", singularFile);
                }
            }
        }

        static void OpenNoteInCode()
        {
            bool usingArgs = false;
            if (internalArgs.Length > 0)
            {
                usingArgs = true;
            }
            if (usingArgs)
            {
                //HoneInOnNote();
                SetSingularFile(internalArgs[1]);
            }

            if (!singularFile.Equals(""))
            {
                if (Notes.NoteExists(singularFile))
                {
                    Process.Start(@"C:\Users\Skymap\AppData\Local\Programs\Microsoft VS Code\Code.exe", singularFile);
                }
            }
        }

        static void OpenSavedNote()
        {
            //if (Notes.NoteExists(Properties.Resources.PresetNote))
            //{
            //    Process.Start("notepad.exe", Properties.Resources.PresetNote);
            //}
        }

        static void NewNote()
        {
            bool usingArgs = false;
            if (internalArgs.Length > 0)
            {
                usingArgs = true;
            }
            string newFileName = "";
            if (usingArgs)
            {
                newFileName = internalArgs[1];
            }
            else
            {
                newFileName = PromptUser("Enter new file name: ");
            }

            if (newFileName.Length < 5)
            {
                newFileName += ".txt";
            }
            else
            {
                if (newFileName.Substring(newFileName.Length - 5) != ".txt")
                {
                    newFileName += ".txt";
                }
            }

            string fullFileName = Notes.GetFilename(newFileName);
            if (Notes.NoteExists(fullFileName))
            {
                Output.WriteToConsole("A file by this name already exists.");
            }
            else
            {
                Notes.AddAndOpenNote(fullFileName);
            }
        }

        static void OpenDirectory()
        {
            Notes.OpenDirectory();
        }

        static void ClearConsole()
        {
            Console.Clear();
        }

        static void PrintHorizontalBarrier()
        {
            Output.WriteToConsole("________________________________________________\n");
        }
    }
}
