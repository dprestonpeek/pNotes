using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pNotes
{
    public class Help
    {
        public static string listHelp = "Similar to 'ls' command in bash. Lists some or all files at the current directory depending on args given.\n\n" +
            "Modifiers:\n'-i' only lists files.\n" +
            "'-o' only lists folders.\n" +
            "Examples:\n'list text' or 'ls text' prints all files containing the 'text' keyword. \n" + 
            "'list' or 'ls' prints all files at the current directory.";
        public static string findHelp = "Crawl through all specified text files for a specific keyword, and print that file name along with an exerpt containing the keyword.\n\n" +
            "Modifiers:\n'-r' toggles recursive searching through folders.\n" +
            "'-p' toggles prompting for directories to search in.\n\n" + 
            "Examples:\n'find text' or 'fnd text' searches through all text files in the current directory for a specified keyword.\n" + 
            "'find text -rp' searches through all text files recursively and prompts the user for specification of the directories to be searched.";
        public static string searchHelp = "Search for filenames in the current directory. Functions as a replacement for Windows search when it's acting up.\n\n" + 
            "Modifiers:\n'-r' toggles recursive searching through folders.\n\n" +
            "Examples:\n'search text' searches all files in the current directory for file names containing a specific keyword.\n" +
            "'search text -r' searches all files and folders recursively for file names containing a specific keyword.";
        public static string readHelp = "Print out the text of the specified file in the cmd window.\n\n" + 
            "Examples:\n'read test.txt' prints the file text in the cmd window.\n" + 
            "'read' prints the file text only if there is a selected (honed-in) file.";
        public static string openHelp = "Open the file in notepad.exe.\n\n" + 
            "Examples:\n'open test.txt' opens the file in notepad.\n" + 
            "'open' opens the file in notepad only if there is a selected (honed-in) file.";
        public static string codeHelp = "Open the file in Visual Studio Code. This function is (experimental, requires an instance of this program to use VS Code)\n\n" + 
            "Examples:\n'code test.lua' opens the file in Visual Studio Code." +
            "'code' opens the file in Visual Studio Code only if there is a selected (honed-in) file.";
        public static string honeHelp = "Hone in on (select) a file for context. Use this when you want to execute commands on a file without typing out the filename each time.\n\n" +
            "Examples:\n'hone test.txt' selects test.txt for context. Now you can run the 'open', 'read', or 'code' command and assume this file will open.\n" +
            "'hone' deselects the file for reference. The context is now 'all files' instead of one singular file.";
        public static string newHelp = "Create a new file at the current directory.\n\n" +
            "Examples:\n'new test.txt' creates a new file called test.txt.\n" +
            "'new' creates a new file and prompts for a filename,";
        public static string dirHelp = "Open the current directory in File Explorer.\n\n" +
            "Examples:\n'open' opens the current directory in File Explorer.";
        public static string clearHelp = "Clear the console of all text.\n\n" + 
            "Examples:\n'clear' clears the current console view of all text.";
        public static string hisHelp = "Print the output history of this current session.\n\n" +
            "Examples:\n'his' prints the collected history.";
        public static string pathHelp = "View or Change the current working directory.\n\n" +
            "Examples:\n'path' prints the current directory.\n" +
            "'path C:\\Users\\user\\Documents' changes the current working directory to C:\\Users\\user\\Documents.";
        public static string diffHelp = "Determine whether 2 or more files are identical.\n\n" +
            "Modifiers:\n'-d' indicates the arguments provided are directories, not files.\n" +
            "'-m' indicates there are more than 2 files or directories to search.\n\n" +
            "Examples:\n'diff' will prompt for files to compare.\n" +
            "'diff C:\\Users\\user\\Documents\\file1.txt C:\\Users\\user\\Documents\\file2.txt' will compare file1.txt and file2.txt and print the differences.\n" +
            "'diff C:\\Users\\user\\Documents\\dir1 C:\\Users\\user\\Documents\\dir2 -d' will compare all files in both directories (assuming they should be identical) and print which files differ.";
        public static string helpHelp = "Opens the Help Overview page.\n\n" +
            "Examples:\n'help' opens the Help Overview page.\n" +
            "'help help' opens this Help page for the Help command.";
        public static string exitHelp = "Exits the program.\n\n" + 
            "Examples:\n'exit' exits this program.";

        public static void PrintCommandHelp(string commandInput)
        {
            Command cmd = Command.GetCommandByString(commandInput);
            Output.PrintHorizontalBarrier();
            Output.WriteLine(cmd.Name + " - HELP\n");
            if (cmd.CmdHelp != "")
            {
                Output.WriteLine(cmd.CmdHelp);
            }
            Output.WriteLine("\nAlternate Commands: ");
            for (int i = 0; i < cmd.CommandAlts.Length; i++)
            {
                Output.Write(cmd.CommandAlts[i]);
                if (i < cmd.CommandAlts.Length - 1)
                {
                    Output.Write(", ");
                }
            }
            Output.WriteLine("\n\nCommand function with args: ");
            Output.WriteLine(cmd.NoArgsDesc);
            Output.WriteLine("\nCommand function without args: ");
            Output.WriteLine(cmd.ArgsDesc + "\n");
        }
    }
}
