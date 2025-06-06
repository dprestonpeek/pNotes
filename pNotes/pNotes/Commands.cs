using System;
using System.Collections.Generic;
using System.Text;

namespace pNotes
{
    public class Commands
    {
        public static List<Command> List;

        public static void Initialize()
        {
            List = new List<Command>();
        }

        public static void AddCommand(string name, string[] commands, string noArgsDesc, string argsDesc, string cmdHelp, Action action)
        {
            List.Add(new Command(name, commands, noArgsDesc, argsDesc, cmdHelp, action));
        }

        public static void AddCommand(string name, string[] commands, string noArgsDesc, string argsDesc, Action action)
        {
            List.Add(new Command(name, commands, noArgsDesc, argsDesc, action));
        }
    }

    public class Command
    {
        public string Name;
        public string[] CommandAlts;
        public string NoArgsDesc;
        public string ArgsDesc;
        public string CmdHelp;
        public Action Action;

        public Command(string name, string[] commands, string noArgsDesc, string argsDesc, Action action)
        {
            Name = name;
            CommandAlts = commands;
            NoArgsDesc = noArgsDesc;
            ArgsDesc = argsDesc;
            CmdHelp = "";
            Action = action;
        }

        public Command(string name, string[] commands, string noArgsDesc, string argsDesc, string cmdHelp, Action action)
        {
            Name = name;
            CommandAlts = commands;
            NoArgsDesc = noArgsDesc;
            ArgsDesc = argsDesc;
            CmdHelp = cmdHelp;
            Action = action;
        }

        public string GetCommandsAsString()
        {
            string commands = "";
            foreach (string command in CommandAlts)
            {
                commands += command + ", ";
            }
            commands = commands.Substring(0, commands.Length - 2);
            return commands;
        }

        public static bool ContainsCommand(string commandInput)
        {
            foreach (Command command in Commands.List)
            {
                foreach (string cmd in command.CommandAlts)
                {
                    if (commandInput.Contains(cmd.Split(' ')[0]))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool ContainsCommandAlt(string commandInput)
        {
            foreach (string command in CommandAlts)
            {
                if (commandInput.Contains(command))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool EqualsFullCommand(string commandInput)
        {
            string typedCommand = commandInput.Split(' ')[0];
            foreach (Command command in Commands.List)
            {
                if (command.ContainsCommandAlt(typedCommand))
                {
                    foreach (string cmd in command.CommandAlts)
                    {
                        if (cmd == typedCommand)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public static Command GetCommandByString(string commandInput)
        {
            foreach (Command command in Commands.List)
            {
                foreach(string cmd in command.CommandAlts)
                {
                    if (cmd.Contains(commandInput))
                    {
                        return command;
                    }
                }
            }
            return null;
        }
    }
}
