using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace pNotes
{
    public class Notes
    {
        public static string AddNote(string noteName)
        {
            string filename = Path.Combine(Program.currentDir, noteName);
            File.Create(filename);
            return filename;
        }

        public static void AddAndOpenNote(string filename)
        {
            FileStream file = File.Create(filename);
            file.Close();
            file.Dispose();
            Process.Start("notepad.exe", filename);
        }

        public static void OpenDirectory()
        {
            Process.Start("explorer.exe", Program.currentDir);
        }

        public static void WriteNote(string filename)
        {
            File.Create(filename);
        }
        
        public static string GetFilename(string noteName)
        {
            string filename = Path.Combine(Program.currentDir, noteName);
            return filename;
        }

        public static bool NoteExists(string noteName)
        {
            string filename = Path.Combine(Program.currentDir, noteName);
            return File.Exists(filename);
        }

        public static string[] GetNote(string noteName)
        {
            string[] text;

            text = File.ReadAllLines(noteName);

            return text;
        }

        public static List<string> GetNotes(bool withFilePaths)
        {
            string[] split;
            string[] files;
            if (withFilePaths)
            {
                files = Directory.GetFiles(Program.currentDir);
            }
            else
            {
                split = Program.currentDir.Split('\\');
                files = Directory.GetFiles(split[split.Length - 1]);
            }
            Array.Sort(files);

            List<string> filesList = new List<string>();
            filesList.AddRange(files);
            return filesList;
        }

        public static List<string> GetNotesSortedByDate(bool withFilePaths)
        {
            string[] split;
            string[] files;
            if (withFilePaths)
            {
                files = Directory.GetFiles(Program.currentDir);
            }
            else
            {
                split = Program.currentDir.Split('\\');
                files = Directory.GetFiles(split[split.Length - 1]);
            }

            DateTime[] lastModifiedTimes = new DateTime[files.Length];
            for (int i = 0; i < files.Length; i++)
                lastModifiedTimes[i] = new FileInfo(files[i]).LastWriteTimeUtc;
            Array.Reverse(lastModifiedTimes);
            Array.Sort(lastModifiedTimes, files);

            List<string> filesList = new List<string>();
            filesList.AddRange(files);
            return filesList;
        }

        public static List<string> FindExcerpt(string excerpt)
        {
            string[] files;
            List<string> result = new List<string>();
            files = Directory.GetFiles(Program.currentDir);

            foreach (string file in files)
            {
                result.AddRange(FindExcerptInNote(excerpt, file));
            }
            return result;
        }

        public static List<string> FindExcerpt(string excerpt, string path)
        {
            string[] files;
            List<string> result = new List<string>();
            files = Directory.GetFiles(path);

            foreach (string file in files)
            {
                result.AddRange(FindExcerptInNote(excerpt, file));
            }
            return result;
        }

        public static List<string> FindExcerptInNote(string excerpt, string file)
        {
            return FindExcerptInNote(excerpt, file, 100, 200);
        }

        public static List<string> FindErrorInNote(string excerpt, string file)
        {
            return FindExcerptInNote(excerpt, file, 250, 1000);
        }

        public static List<string> FindExcerptInNote(string excerpt, string file, int head, int tail)
        {
            List<string> result = new List<string>();
            string contents = "";
            try
            {
                contents = File.ReadAllText(file);
            }
            catch (Exception e)
            {
                Output.WriteToConsole("File is in use.");
                return null;
            }
            string lowerContents = contents.ToLower();
            string lowerExcerpt = excerpt.ToLower();
            if (lowerContents.Contains(lowerExcerpt))
            {
                int index = lowerContents.IndexOf(lowerExcerpt);

                result.Add(file + "\n-");
                try
                {
                    if (index > 100)
                    {
                        if (contents.Length > index + tail + excerpt.Length)
                        {
                            int i = index - head;
                            result.Add("\"..." + contents.Substring(index - head, tail + excerpt.Length) + "...\"");
                        }
                        else
                        {
                            int i = index - head;
                            result.Add("..." + contents.Substring(index - head) + "\"");
                        }
                    }
                    else
                    {
                        if (contents.Length > index + tail + excerpt.Length)
                        {
                            result.Add(contents.Substring(index, tail + excerpt.Length) + "...\"");
                        }
                        else
                        {
                            result.Add(contents.Substring(index) + "\"");
                        }
                    }
                }
                catch (Exception e)
                {
                    result.Add(contents.Substring(index) + "\"");
                }
                result.Add(GetHorizontalBarrier());
            }
            return result;
        }

        static string GetHorizontalBarrier()
        {
            return "________________________________________________";
        }
    }
}
