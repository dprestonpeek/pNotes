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
        public string[] Commands;
        public string NoArgsDesc;
        public string ArgsDesc;
        public string CmdHelp;
        public Action Action;

        public Command(string name, string[] commands, string noArgsDesc, string argsDesc, Action action)
        {
            Name = name;
            Commands = commands;
            NoArgsDesc = noArgsDesc;
            ArgsDesc = argsDesc;
            CmdHelp = "";
            Action = action;
        }

        public Command(string name, string[] commands, string noArgsDesc, string argsDesc, string cmdHelp, Action action)
        {
            Name = name;
            Commands = commands;
            NoArgsDesc = noArgsDesc;
            ArgsDesc = argsDesc;
            CmdHelp = cmdHelp;
            Action = action;
        }

        public string GetCommandsAsString()
        {
            string commands = "";
            foreach (string command in Commands)
            {
                commands += command + ", ";
            }
            commands = commands.Substring(0, commands.Length - 2);
            return commands;
        }

        public bool ContainsCommand(string commandInput)
        {
            foreach (string command in Commands)
            {
                if (commandInput.Contains(command))
                {
                    return true;
                }
            }
            return false;
        }

        public bool EqualsFullCommand(string commandInput)
        {
            foreach (string command in Commands)
            {
                if (commandInput.Equals(command))
                {
                    return true;
                }
            }
            return false;
        }

        public static Command GetCommandByString(string commandInput, List<Command> Commands)
        {
            foreach (Command command in Commands)
            {
                foreach(string cmd in command.Commands)
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
