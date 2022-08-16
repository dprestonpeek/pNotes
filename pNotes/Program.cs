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
        
        static string[] internalArgs;
        static string singularFile = "";
        static void Main(string[] externalArgs)
        {
            string input;
            bool initial = true;
            Commands.Initialize();
            AddCommands();
            Notes notes = new Notes();
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
                    if (singularFile.Equals(""))
                    {
                        Output.WriteToConsole("All Files ~ ");
                    }
                    else
                    {
                        string[] split = singularFile.Split('\\');
                        Output.Write(split[split.Length - 1] + " ~ ");
                    }
                    input = Console.ReadLine();
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
            //Commands.AddCommand("Print Notes Sorted", new string[2] { "sort", "rec" }, "Print files sorted by recent", "Print sorted notes containing keyword", PrintRecentNotes);
            Commands.AddCommand("Find Excerpt", new string[2] { "find", "fnd" }, "", "Search files for specified excerpt, print excerpt if found", FindExcerpt);
            Commands.AddCommand("Read Note", new string[2] { "read", "red" }, "Read selected file", "Search for filenames containing specified keyword, read if found", ReadNote);
            Commands.AddCommand("Open Note", new string[2] { "open", "opn" }, "Open selected file", "Search for filenames containing specified keyword, open if found", OpenNote);
            Commands.AddCommand("Hone in on Note ", new string[2] { "hone", "hn" }, "Set context to everything", "Set context to file matching specified keyword", HoneInOnNote);
            Commands.AddCommand("New Note", new string[1] { "new" }, "Create new file", "", NewNote);
            Commands.AddCommand("Open Directory", new string[1] { "dir" }, "Open directory of files in explorer", "", OpenDirectory);
            Commands.AddCommand("Clear Console", new string[2] { "clear", "cls" }, "Clear console text", "", ClearConsole);
            Commands.AddCommand("Print History", new string[3] { "his", "uncls", "unclear" }, "Print collected history", "", PrintHistory);
            Commands.AddCommand("Open Saved Note", new string[2] { "log", "note" }, "Open presaved note", "", OpenSavedNote);

            Commands.AddCommand("Help menu", new string[2] { "help", "hlp" }, "Open this help menu", "", PrintHelp);
            Commands.AddCommand("Exit program", new string[1] { "exit" }, "Exit the program", "", null);
        }

        static void PrintHelp()
        {
            Output.WriteToConsole("Commands"  + "\t" + "Functions" + "\t\t" + "Desc (no args)" + "\t\t\t" + "Desc (w/ args)");
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
            bool logNote = false;
            bool recursive = false;
            if (internalArgs.Length == 0)
            {
                return;
            }
            if (internalArgs.Length > 2)
            {
                if (internalArgs[2].Equals("log") || internalArgs[2].Equals("note"))
                {
                    logNote = true;
                }
                if (internalArgs[2].Equals("-r"))
                {
                    recursive = true;
                }
            }
            List<string> toWrite = new List<string>();

            Task<List<string>> findExcerptTask = Task.Factory.StartNew(() => {
                if (logNote)
                {
                    foreach (string line in Notes.FindErrorInNote(internalArgs[1], Properties.Resources.PresetNote))
                    {
                        toWrite.Add(line + "\n");
                    }
                }
                else if (singularFile.Equals(""))
                {
                    List<string> dirs = new List<string>();
                    dirs.Add(Notes.GetPath());
                    if (recursive)
                    {
                        dirs.AddRange(BuildDirectoriesList(Notes.GetPath()));
                    }

                    foreach (string dir in dirs)
                    {
                        foreach (string line in Notes.FindExcerpt(internalArgs[1], dir))
                        {
                            toWrite.Add(line + "\n");
                        }
                    }
                }
                else
                {
                    foreach (string line in Notes.FindExcerptInNote(internalArgs[1], singularFile))
                    {
                        toWrite.Add(line + "\n");
                    }
                }
                return toWrite;
            });

            Console.Write("Searching for excerpt. ");
            int i = 0;
            while (!findExcerptTask.IsCompleted)
            {
                if (i < 4)
                {
                    Console.Write("|");
                }
                else
                {
                    Console.Write("\b\b\b\b");
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.Write("||||");
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.Write(" ");
                }
                i++;
                findExcerptTask.Wait(1000);
            }
            Console.WriteLine();
            foreach(string line in findExcerptTask.Result)
            {
                Output.WriteToConsole(line);
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
                HoneInOnNote();
            }

            if (!singularFile.Equals(""))
            {
                if (Notes.NoteExists(singularFile))
                {
                    Process.Start("notepad.exe", singularFile);
                }
            }
        }

        static void OpenSavedNote()
        {
            if (Notes.NoteExists(Properties.Resources.PresetNote))
            {
                Process.Start("notepad.exe", Properties.Resources.PresetNote);
            }
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
