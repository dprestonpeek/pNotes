using System;
using System.Collections.Generic;
using System.Text;

namespace pNotes
{
    class Editor
    {
        public static bool editing = true;
        public static bool cursorAtLatest = true;
        public static List<string> fileContents;
        public static int index = 0;
        public static string noteName;
        public static void BeginEditing(string file)
        {
            editing = true;
            noteName = file;
            fileContents = new List<string>();
            while (true)
            {
                Loop();
            }
            //CenterStrings(new string[4] { "Func1 []", "Func2 []", "Func3 []", "Func4 []" });
        }

        public static void Loop()
        {
            PrintHeader(noteName);
            PrintContent();
            WaitForInput(noteName);
        }

        public static void WaitForInput(string filename)
        {
            Console.WriteLine();
            string input = "";
            input += GetFirstKey();
            input += Console.ReadLine();
            if (!cursorAtLatest)
            {
                index = fileContents.Count;
            }
            AddContent(input);
        }

        private static void AddContent(string line)
        {
            if (line.Equals(string.Empty) || line.Equals("\n"))
            {
                return;
            }
            if (index == fileContents.Count)
            {
                fileContents.Add(line);
            }
            else
            {
                EditLine(line);
            }
            index++;
        }

        private static void EditLine(string line)
        {
            fileContents[index] = line;
            index = fileContents.Count;
        }

        private static void NewNote(string filename)
        {
            Notes.WriteNote(filename);
        }

        private static char GetFirstKey()
        {
            ConsoleKeyInfo key = Console.ReadKey();
            if (!CheckForArrows(key))
            {
                return key.KeyChar;
            }
            return ' ';
        }

        private static bool CheckForArrows(ConsoleKeyInfo key)
        {
            if (key.Key == ConsoleKey.UpArrow)
            {
                //up
                if (index > 0)
                {
                    index--;
                    Loop();
                }
                cursorAtLatest = false;
                return true;
            }
            else if (key.Key == ConsoleKey.DownArrow)
            {
                //down
                if (index < fileContents.Count)
                {
                    index++;
                    Loop();
                }
                cursorAtLatest = false;
                return true;
            }
            else if (key.Key == ConsoleKey.LeftArrow)
            {
                //left
                return true;
            }
            else if (key.Key == ConsoleKey.RightArrow)
            {
                //right
                return true;
            }
            return false;
        }

        private static void PrintHeader(string noteName)
        {
            Console.Clear();
            CenterString(FilenameOnly(noteName));
            //Console.WriteLine();
        }

        private static void PrintContent()
        {
            int i = 0;
            for (i = 0; i < index; i++)
            {
                Console.WriteLine(fileContents[i]);
            }
            if (index < fileContents.Count && i < fileContents.Count)
            {
                Console.Write(fileContents[i]);
            }
        }

        private static string FilenameOnly(string toAbridge)
        {
            string[] split = toAbridge.Split('\\');
            return split[split.Length - 1];
        }

        private static void CenterString(string toCenter)
        {
            CenterString(toCenter, 48, "_", "|");
        }

        private static void CenterString(string toCenter, int totalWidth, string space, string barrier)
        {
            int whle = totalWidth - toCenter.Length;
            int half = whle / 2;

            Console.Write(barrier);
            for (int i = 0; i < whle; i++)
            {
                if (i == half)
                {
                    Console.Write(toCenter);
                }
                Console.Write(space);
            }
            Console.WriteLine(barrier);
        }

        private static void CenterStrings(string[] toCenter)
        {
            CenterStrings(toCenter, 48, "_", "|");
        }

        private static void CenterStrings(string[] toCenter, int totalWidth, string space, string barrier)
        {
            int totalChars = 0;
            foreach (string str in toCenter)
            {
                totalChars += str.Length;
            }
            int whle = totalWidth;
            int half = whle / 2;
            int oChars = 0;
            int oLines = 0;
            List<string> overflow = new List<string>();

            if (totalChars > totalWidth)
            {
                float div = totalChars / totalWidth;
                string divStr = div.ToString();
                string[] split = divStr.Split('.');
                divStr = split[0];
                int divInt = int.Parse(divStr);
                int rem = totalChars % totalWidth;
                oLines = divInt;
                oChars = rem;
            }

            int maxChars = totalWidth > totalChars ? totalWidth : totalChars;
            int perWord = 48 - totalChars;

            int between = perWord / toCenter.Length;
            int extra = perWord % 2;
            int before = between / 2;
            int after = 0;
            int k = 0;

            Console.Write(barrier);
            int j = 0;
            while (true)
            {
                for (int i = 0; i < 48; i++)
                {
                    if (k == before)
                    {
                        Console.Write(toCenter[j]);
                        i += toCenter[j].Length;
                        if (j == toCenter.Length - 1)
                        {
                            break;
                        }
                        k = 0;
                        after = before + toCenter[j].Length + extra;
                        continue;
                    }
                    else if (k == after)
                    {
                        k = 0;
                        j++;
                    }
                    else
                    {
                        Console.Write(space);
                        k++;
                    }
                }
                if (j == toCenter.Length - 1)
                {
                    break;
                }
            }
            Console.Write(barrier);
            Console.WriteLine();
        }
    }
}
