using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace pNotes
{
    class Output
    {
        public static List<string> History;

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
        public static void WriteToConsole()
        {
            //AddToHistory("\n");
            Console.WriteLine();
        }
        public static void WriteToConsole(string content)
        {
            AddToHistory(content);
            Console.WriteLine(content);
        }
        public static void Write(string content)
        {
            AddToHistory(content);
            Console.Write(content);
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
