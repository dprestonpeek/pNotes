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
        public static List<string> forbiddenFileTypes = new List<string>() { ".db", ".lib", ".pri", ".exe", ".zip", "svn.base", ".ipch", ".tex", ".fsb", ".pdb", ".ilk" };

        public static string version = "pNotes v0.91";

        static string[] internalArgs;
        static string singularFile = "";

        static string logFile = "log.txt";
        static void Main(string[] externalArgs)
        {
            currentDir = Directory.GetCurrentDirectory();
            string input;
            bool initial = true;
            bool extArgs = false;
            Commands.Initialize();
            AddCommands();
            Notes notes = new Notes();
            Task task = Task.CompletedTask;
            //Console.SetWindowSize(200, 20);

            if (externalArgs.Length > 0 && !externalArgs[0].Equals(""))
            {
                extArgs = true;
            }
            Output.WriteLine("Enter a command or \"help\".");

            while (true)
            {
                if (surfing)
                {
                    while (!task.IsCompleted) { }
                    if (!initial)
                    {
                        Output.PrintHorizontalBarrier();
                    }
                    initial = false;
                    if (singularFile.Equals("") && !searching)
                    {
                        Output.Write("pCmd ~ ");
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
                        Output.WriteLine(input);
                        extArgs = false;
                    }
                    else
                    {
                        input = Console.ReadLine();
                    }
                    Output.NewLine();
                    string[] commands = input.Split(new string[] { "+++" }, StringSplitOptions.None);
                    foreach (string cmd in commands)
                    {
                        while (!task.IsCompleted)
                        {

                        }
                        input = cmd.Trim();
                        if (input.Contains("help"))
                        {
                            Command command = Command.GetCommandByString("help", Commands.List);
                            if (command.EqualsFullCommand(input))
                            {
                                internalArgs = new string[0];
                            }
                            else
                            {
                                internalArgs = input.Split(' ');
                            }
                            task = Task.Factory.StartNew(() => { command.Action.Invoke(); });
                        }
                        else
                        {
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
                                        task = Task.Factory.StartNew(() => { command.Action.Invoke(); });
                                        break;
                                    }
                                    catch (Exception e)
                                    {
                                        Output.WriteLine(e.Message);
                                        File.WriteAllText(logFile, e.Message);
                                    }
                                }
                                if (input == "exit")
                                {
                                    Environment.Exit(0);
                                }
                            }
                        }
                        Output.NewLine();
                        if (input == "exit")
                        {
                            Environment.Exit(0);
                        }
                    }
                }
            }
        }

        static void AddCommands()
        {
            //add commands here
            Commands.AddCommand("Print File List", new string[2] { "list", "ls" }, "Print names of available files", "Print files containing specified keyword", Help.listHelp, PrintFiles);
            Commands.AddCommand("Find Excerpt", new string[2] { "find", "fnd" }, "Open \"Find Command Help Page\"", "Search files for specified excerpt, print", Help.findHelp, FindExcerpt);
            Commands.AddCommand("Search Partial Filename", new string[1] { "search" }, "", "Search file names for specified excerpt, print", Help.searchHelp, SearchFilenames);
            Commands.AddCommand("Read File", new string[2] { "read", "red" }, "Read honed-in file", "Print file text in cmd window", Help.readHelp, ReadFile);
            Commands.AddCommand("Open File", new string[2] { "open", "opn" }, "Open selected file", "Open file in notepad", Help.openHelp, OpenFileInNotepad);
            Commands.AddCommand("Open File in Code", new string[1] { "code" }, "Open file in VS Code", "Open file in VS Code", Help.codeHelp, OpenFileInCode);
            Commands.AddCommand("Hone in on File ", new string[2] { "hone", "hn" }, "Set context to everything", "Set context to file matching specified keyword", Help.honeHelp, HoneInOnFile);
            Commands.AddCommand("New File", new string[1] { "new" }, "Create new file", "Create new file with name", Help.newHelp, NewFile);
            Commands.AddCommand("Open Directory", new string[1] { "dir" }, "Open directory of files in explorer", "", Help.dirHelp, OpenDirectory);
            Commands.AddCommand("Clear Console", new string[2] { "clear", "cl" }, "Clear console text", "", Help.clearHelp, Output.ClearConsole);
            Commands.AddCommand("Print History", new string[2] { "his", "uncl" }, "Print collected history", "", Help.hisHelp, PrintHistory);
            //Commands.AddCommand("Open Saved File", new string[2] { "log", "note" }, "Open presaved file", "", OpenSavedFile);
            Commands.AddCommand("Change Path", new string[2] { "path", "where" }, "Print the current path", "Change the path a given directory", Help.pathHelp, DirPath);

            Commands.AddCommand("Help menu", new string[2] { "help", "hlp" }, "Open this help menu", "Open command help page", Help.helpHelp, PrintHelp);
            Commands.AddCommand("Exit program", new string[1] { "exit" }, "Exit the program", "", Help.exitHelp, null);
        }

        static string PromptUser(string question)
        {
            Output.WriteLine(question);
            string answer = Console.ReadLine();
            return answer;
        }

        static void PrintHelp()
        {
            if (internalArgs.Length > 0)
            {
                Help.PrintCommandHelp(internalArgs[1]);
            }
            else
            {
                Output.PrintHelp();
            }
        }

        static void DirPath()
        {
            if (internalArgs.Length > 0 && !internalArgs[1].Equals(""))
            {
                string newPath = internalArgs[1];

                if (internalArgs.Length > 2)
                {
                    newPath = "";
                    for (int i = 1; i < internalArgs.Length; i++)
                    {
                        newPath += internalArgs[i];
                        if (i < internalArgs.Length - 1)
                        {
                            newPath += " ";
                        }
                    }
                }

                if (newPath.Equals("..") || newPath.Equals("../"))
                {
                    newPath = Directory.GetParent(newPath).ToString();
                }

                if (Directory.Exists(newPath))
                {
                    currentDir = newPath;
                    Output.WriteLine("Path changed to " + currentDir);
                }
                else
                {
                    Output.WriteLine("Not a valid directory.");
                }
            }
            else
            {
                Output.WriteLine(currentDir);
            }
        }

        static void DirPath(string externalArgs)
        {
            if (!externalArgs.Equals(""))
            {
                if (Directory.Exists(externalArgs))
                {
                    currentDir = externalArgs;
                    Output.WriteLine("Path changed to " + currentDir);
                }
                else
                {
                    Output.WriteLine("Not a valid directory.");
                }
            }
        }

        static void PrintFiles()
        {
            bool usingArgs = false;
            if (internalArgs.Length > 0)
            {
                usingArgs = true;
            }
            List<string> notes;
            if (usingArgs)
            {
                notes = GetFilesAndDirectories(internalArgs[1], false);
            }
            else
            {
                notes = GetFilesAndDirectories("", false);
            }
            foreach (string note in notes)
            {
                Output.WriteLine(note);
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
                Output.WriteLine(note);
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

        static List<string> GetFilesAndDirectories(string filename, bool withFilePaths)
        {
            List<string> filesAndDirs = new List<string>();
            bool usingArgs = false;
            if (internalArgs.Length > 0)
            {
                usingArgs = true;
            }

            foreach (string file in Directory.GetFileSystemEntries(currentDir))
            {
                if (filename.Equals(""))
                {
                    filesAndDirs.Add(file);
                }
                else
                {
                    if (!usingArgs || (usingArgs && file.Contains(filename)))
                    {
                        filesAndDirs.Add(file);
                    }
                }
            }
            return filesAndDirs;
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
            bool recursive = false;
            bool prompt = false;
            bool large = false;
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
                //we actually want to display the help page here
                Output.PrintFindHelp();
                return;
            }
            path = currentDir;
            if (internalArgs.Length > 2)
            {
                for (int i = 1; i < internalArgs.Length; i++)
                {
                    if (internalArgs[i] == "")
                    {
                        continue;
                    }
                    if (i < internalArgs.Length - 1)
                        term += internalArgs[i];
                    if (i < internalArgs.Length - 2)
                        term += " ";
                    if (i == internalArgs.Length - 1 && internalArgs[i][0] == '-')
                        modifier = internalArgs[i];
                    else if (i == internalArgs.Length - 1)
                        term += " " + internalArgs[i];
                }

                if (modifier.Equals("-r"))
                {
                    recursive = true;
                }
                if (modifier.Equals("-p"))
                {
                    prompt = true;
                }
                if (modifier.Equals("-l"))
                {
                    large = true;
                }
                if (modifier.Equals("-lp") || modifier.Equals("-pl"))
                {
                    large = true;
                    prompt = true;
                }
                if (modifier.Equals("-rl") || modifier.Equals("-lr"))
                {
                    recursive = true;
                    large = true;
                }
                if (modifier.Equals("-rp") || modifier.Equals("-pr"))
                {
                    recursive = true;
                    prompt = true;
                }
                if (modifier.Equals("-rpl") || modifier.Equals("-rlp") || modifier.Equals("-lrp") || 
                    modifier.Equals("-lpr") || modifier.Equals("-plr") || modifier.Equals("-prl"))
                {
                    recursive = true;
                    prompt = true;
                    large = true;
                }
            }
            else
            {
                term = internalArgs[1];
            }
            Dictionary<int, string> rootDirs = new Dictionary<int, string>();
            rootDirs.Add(0, currentDir);
            if (prompt)
            {
                string[] args = internalArgs;
                int j = 1;
                foreach (string dir in Directory.GetDirectories(currentDir))
                {
                    if (dir == currentDir/* || dir.Contains("netcoreapp3.1")*/)
                        continue;
                    rootDirs.Add(j, dir);
                    j++;
                }

                for (int i = 0; i < rootDirs.Count; i++)
                {
                    Output.WriteLine(i + ":" + "\t" + rootDirs[i]);
                }
                Output.WriteLine("enter X to cancel");

                string input = "";

                input = PromptUser("Choose a 1 or more root directories to search in. (ex. \"3\", \"5-10\")");
                input = input.Replace(" ", "");
                string[] inputs = input.Split(',');
                int rootDir;
                foreach (string i in inputs)
                {
                    exclude = true;
                    int min = 0; int max = 0;
                    if (i.Contains("-"))
                    {
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
                    else if (int.TryParse(i, out rootDir))
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
            else
            {
                dirsToSearch.Add(0);
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
                            break;
                        }
                    }
                    rootDirs.TryGetValue(k, out path);
                }
                Task<List<string>> findExcerptTask;
                List<string> toWrite = new List<string>();
                findExcerptTask = Task<List<string>>.Factory.StartNew(() =>
                {
                    bool wasRecursive = recursive;
                    if (path == currentDir)
                    {
                        recursive = false;
                    }
                    toWrite = DoFindExcerpt(term, large, recursive, path);
                    if (path == currentDir)
                    {
                        recursive = wasRecursive;
                    }
                    return toWrite;
                });

                if (dirsToSearch.Contains(k))
                {
                    Console.WriteLine("Searching for \"" + term + "\" in " + path);
                    Output.PrintHorizontalBarrier();
                }
                while (!findExcerptTask.IsCompleted)
                {

                }
                //if (large)
                //{
                //    foreach (string line in findExcerptTask.Result)
                //    {
                //        Output.WriteLine(line);
                //    }
                //}
                searching = false;
                k++;
            } while (exclude);

        }

        static List<string> DoFindExcerpt(string term, bool large, bool recursive, string path)
        {
            List<string> toWrite = new List<string>();

            if (singularFile.Equals(""))
            {
                List<string> dirs = new List<string>();
                dirs.Add(path);
                if (recursive)
                {
                    dirs.AddRange(BuildDirectoriesList(path, false));
                }

                foreach (string dir in dirs)
                {
                    foreach (string line in Notes.FindExcerpt(term, dir))
                    {
                        if (large)
                        {
                            toWrite.Add(line + "\n");
                        }
                        else
                        {
                            Output.WriteLine("\n" + line);
                        }
                    }
                }
            }
            else
            {
                foreach (string line in Notes.FindExcerptInNote(term, singularFile))
                {
                    if (large)
                    {
                        toWrite.Add(line + "\n");
                    }
                    else
                    {
                        Output.WriteLine(line);
                    }
                }
            }
            return toWrite;
        }

        static void SearchFilenames()
        {
            string term = "";
            string modifier = "";
            bool recursive = false;
            if (internalArgs.Length == 0 || internalArgs.Length > 3)
            {
                Output.WriteLine("Please enter a valid argument");
                return;
            }
            else if (internalArgs.Length == 3)
            {
                modifier = internalArgs[2];
                if (!modifier.Equals("-r"))
                {
                    Output.WriteLine("Please enter a valid argument");
                    return;
                }
                else
                {
                    recursive = true;
                }
            }
            term = internalArgs[1];

            if (recursive)
            {
                foreach (string dir in BuildDirectoriesList(currentDir, true))
                {
                    foreach (string file in Directory.GetFiles(dir))
                    {
                        if (file.ToLower().Contains(term.ToLower()))
                        {
                            Output.WriteLine(file);
                        }
                    }
                }
            }
            else
            {
                foreach (string file in Directory.GetFiles(currentDir))
                {
                    if (file.ToLower().Contains(term.ToLower()))
                    {
                        Output.WriteLine(file);
                    }
                }
            }
        }

        static List<string> BuildDirectoriesList(string path, bool includeRoot)
        {
            List<string> dirs = new List<string>();
            if (includeRoot)
            {
                dirs.Add(path);
            }
            foreach (string dir in Directory.GetDirectories(path))
            {
                if (Directory.GetDirectories(dir).Length > 0)
                {
                    dirs.AddRange(BuildDirectoriesList(dir, false));
                }
                dirs.Add(dir);
            }

            return dirs;
        }

        static void HoneBackOut()
        {
            SetSingularFile("");
        }

        static void HoneInOnFile()
        {
            if (internalArgs.Length == 0)
            {
                HoneBackOut();
                return;
            }
            List<string> files = GetFilesAndDirectories(internalArgs[1], true);
            if (files.Count > 1)
            {
                Output.WriteLine("Please specify further, these are all the files matching that keyword.");
                foreach (string note in files)
                {
                    Output.WriteLine(note);
                }
            }
            else
            {
                Output.WriteLine(files[0]);
                SetSingularFile(files[0]);
            }
        }

        static void SetSingularFile(string note)
        {
            singularFile = note;
        }

        static void ReadFile()
        {
            bool usingArgs = false;
            if (internalArgs.Length > 0)
            {
                usingArgs = true;
            }
            if (usingArgs)
            {
                HoneInOnFile();
            }

            if (!singularFile.Equals(""))
            {
                foreach (string line in Notes.GetNote(singularFile))
                {
                    Output.WriteLine(line);
                }
            }
        }

        static void OpenFileInNotepad()
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

        static void OpenFileInCode()
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
                    Process.Start(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"..\Local\Programs\Microsoft VS Code\Code.exe"), singularFile);
                }
            }
        }

        static void NewFile()
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

            string fullFileName = Notes.GetFilename(newFileName);
            if (Notes.NoteExists(fullFileName))
            {
                Output.WriteLine("A file by the name " + newFileName + " already exists.");
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
    }
}
