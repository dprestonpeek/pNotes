using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace pNotes
{
    class Output
    {
        public static List<string> History;

        /// <summary>
        /// Print entire history to output
        /// </summary>
        public static void PrintHistory()
        {
            List<string> toWrite = new List<string>();
            Task<List<string>> GetAllContent = Task.Factory.StartNew(() => {
                toWrite.AddRange(History);
                return toWrite;
            });

            while (!GetAllContent.IsCompleted) { }
            foreach (string line in toWrite)
            {
                Console.WriteLine(line);
            }
        }

        /// <summary>
        /// Do Console.WriteLine and don't log content in the history
        /// </summary>
        public static void NewLine()
        {
            //AddToHistory("\n");
            Console.WriteLine();
        }

        /// <summary>
        /// Do Console.WriteLine and log content in the history
        /// </summary>
        /// <param name="content"></param>
        public static void WriteLine(string content)
        {
            AddToHistory(content);
            Console.WriteLine(content);
        }

        /// <summary>
        /// Do Console.Write and log content in the history
        /// </summary>
        /// <param name="content"></param>
        public static void Write(string content)
        {
            AddToHistory(content);
            Console.Write(content);
        }

        public static void ClearConsole()
        {
            Console.Clear();
        }

        public static void PrintHorizontalBarrier()
        {
            WriteLine("________________________________________________\n");
        }

        public static void PrintHelp()
        {
            PrintHorizontalBarrier();
            WriteLine(Program.version);
            PrintHorizontalBarrier();
            WriteLine("Enter 'help {command}' for more details on a specific command.\n");
            WriteLine("Commands" + "\t" + "Functions" + "\t\t" + "Desc (no args)" + "\t\t\t" + "Desc (w/ args)");
            foreach (Command command in Commands.List)
            {
                string commands = command.GetCommandsAsString();
                string tab1 = GetTab1(commands);
                string tab2 = GetTab2(command);
                string tab3 = GetTab3(command);

                WriteLine(commands + tab1 + command.Name + tab2 + command.NoArgsDesc + tab3 + command.ArgsDesc);
            }
            NewLine();
        }

        public static void PrintFindHelp()
        {
            Command findCommand = Commands.List[1];
            
            PrintHorizontalBarrier();
            WriteLine("Find Command Help Page\n");
            WriteLine("Command " + "\t" + "Function " + "\t\t" + "Desc (no args)" + "\t\t\t" + "Desc (w/ args)");
            
            string commands = findCommand.GetCommandsAsString();
            string tab1 = GetTab1(commands);
            string tab2 = GetTab2(findCommand);
            string tab3 = GetTab3(findCommand);
            WriteLine(commands + tab1 + findCommand.Name + tab2 + findCommand.NoArgsDesc + tab3 + findCommand.ArgsDesc);
            NewLine();
            WriteLine("Argument" + "\t" + "Function " + "\t\t" + "Description");

            string arg = "-r", name = "Recurse", desc = "Search all root and child directories";
            tab1 = GetTab1(arg);
            tab2 = GetTab2(name);
            WriteLine(arg + tab1 + name + tab2 + desc);

            arg = "-p";
            name = "Prompt";
            desc = "Prompt for directories to search in (non-recursive)";
            tab1 = GetTab1(arg);
            tab2 = GetTab2(name);
            WriteLine(arg + tab1 + name + tab2 + desc);

            arg = "-rp, -pr";
            name = "Recurse & Prompt";
            desc = "Prompt which directories to search (recursive)";
            tab1 = GetTab1(arg);
            tab2 = GetTab2(name);
            WriteLine(arg + tab1 + name + tab2 + desc);

            arg = "-l";
            name = "Large Data";
            desc = "Search (non-recursive) and show hits when complete";
            tab1 = GetTab1(arg);
            tab2 = GetTab2(name);
            WriteLine(arg + tab1 + name + tab2 + desc);

            arg = "-pl, -lp";
            name = "Large Data & Prompt";
            desc = "Prompt which directories to search (non-recursive) and show hits when complete";
            tab1 = GetTab1(arg);
            tab2 = GetTab2(name);
            WriteLine(arg + tab1 + name + tab2 + desc);

            arg = "-rl, -lr";
            name = "Large Data & Recurse";
            desc = "Search (recursive) and show hits when complete";
            tab1 = GetTab1(arg);
            tab2 = GetTab2(name);
            WriteLine(arg + tab1 + name + tab2 + desc);

            arg = "-rlp, -plr...";
            name = "Large Data, P & R";
            desc = "Prompt which directories to search (recursive) and show hits when complete";
            tab1 = GetTab1(arg);
            tab2 = GetTab2(name);
            WriteLine(arg + tab1 + name + tab2 + desc);
        }



        private static string GetTab1(Command command)
        {
            return GetTab1(command.GetCommandsAsString());
        }

        private static string GetTab1(string commands)
        {
            string tab1 = "\t";

            if (commands.Length < 8)
            {
                tab1 = "\t\t";
            }
            return tab1;
        }

        private static string GetTab2(string commandName)
        {
            string tab2 = "\t";

            if (commandName.Length < 8)
            {
                tab2 = "\t\t\t";
            }
            else if (commandName.Length < 16)
            {
                tab2 = "\t\t";
            }
            return tab2;
        }

        private static string GetTab2(Command command)
        {
            string tab2 = "\t";

            if (command.Name.Length < 8)
            {
                tab2 = "\t\t\t";
            }
            else if (command.Name.Length < 16)
            {
                tab2 = "\t\t";
            }
            return tab2;
        }

        private static string GetTab3(string commandNoArgsDesc)
        {

            string tab3 = "\t";

            if (commandNoArgsDesc.Length == 0)
            {
                tab3 = "\t\t\t\t";
            }
            else if (commandNoArgsDesc.Length < 8)
            {
                tab3 = "\t\t\t\t";
            }
            else if (commandNoArgsDesc.Length < 16)
            {
                tab3 = "\t\t\t";
            }
            else if (commandNoArgsDesc.Length < 24)
            {
                tab3 = "\t\t";
            }
            return tab3;
        }

        private static string GetTab3(Command command)
        {

            string tab3 = "\t";

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
            return tab3;
        }


        private static void AddToHistory(string content)
        {
            if (History == null)
            {
                History = new List<string>();
            }
            History.Add(content);
        }
    }
}
