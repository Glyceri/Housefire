using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GXPEngine.Objects.Handlers
{
    public class BeatmapSmall
    {

        public string music = "";
        public string background = "";
        public int lanes = -1;
        public int BPM = -1;
        public int actualbpm = -1;
        public int offset = 0;
        public string name = "Song!";
        public int notesAmount = 0;
        public int sliderAmount = 0;
        public int difficulty = 0;
        public int startTime = 0;
        public string internalName = "";

        public string beatmapFile = "";

        public bool isLoaded = false;

        bool noteMode = false;
        string line = "";
        string failedValue = "";

        public BeatmapSmall(string beatmapFile)
        {
            this.beatmapFile = beatmapFile;
            Thread thread = new Thread(ReadBasicData);
            thread.Start();
        }

        

        public void ReadBasicData()
        {
            try
            {
                string[] lines = File.ReadAllLines(beatmapFile);
                for (int i = 0; i < lines.Length; i++)
                {
                    failedValue = "";
                    try
                    {
                        
                        line = lines[i];
                        if (line.Contains("//")) line = line.Split('/')[0];
                        line = line.TrimEnd(' ');
                        if (!line.EndsWith(";")) continue;
                        line = line.Replace(";", "");

                        if (!noteMode)
                        {
                            ReadLines();
                        }
                        else
                        {
                            try
                            {
                                if (line == "end")
                                {
                                    noteMode = false;
                                    continue;
                                }
                                if (line.Contains("-")) notesAmount++;
                                else                    sliderAmount++;
                            }
                            catch
                            {
                                Console.WriteLine("Something in the note section went wrong: " + line);
                            }
                        }
                    }
                    catch
                    {
                        Console.WriteLine("Loading a value failed: " + failedValue);
                    }
                }
                isLoaded = true;
            }
            catch
            {
                Console.WriteLine("Loading file failed: " + beatmapFile);
            }
        }

        public List<Note> ReadNotes()
        {
            List<Note> notes = new List<Note>();
            try
            {
                string[] lines = File.ReadAllLines(beatmapFile);
                string line = "";
                for (int i = 0; i < lines.Length; i++)
                {
                    string failedValue = "";
                    try
                    {
                        line = lines[i];
                        if (line.Contains("//")) line = line.Split('/')[0];
                        line = line.TrimEnd(' ');
                        if (!line.EndsWith(";")) continue;
                        line = line.Replace(";", "");

                        if (!noteMode)
                        {
                            if (line.Contains(failedValue = "beatmap")) { noteMode = true; }
                        }
                        else if (noteMode)
                        {
                            try
                            {
                                if (line == "end")
                                {
                                    noteMode = false;
                                    continue;
                                }
                                string[] csl = line.Replace(";", "").Split(',');
                                Note note = new Note();

                                note.lane = int.Parse(csl[0]);
                                note.hitTime = csl[1].Contains("-") ? -1 : int.Parse(csl[1]);
                                note.length = csl[2].Contains("-") ? -1 : int.Parse(csl[2]) - note.hitTime;
                                notes.Add(note);
                            }
                            catch
                            {
                                Console.WriteLine("Something in the note section went wrong: " + line);
                            }
                        }
                    }
                    catch
                    {
                        Console.WriteLine("Loading a value failed: " + failedValue);
                    }
                }
            }
            catch { }
            return notes;
        }

        public void WriteDebug()
        {
            
            Console.WriteLine("--------");
            Console.WriteLine(name);
            Console.WriteLine(BPM);
            Console.WriteLine(lanes);
            Console.WriteLine(music);
            Console.WriteLine(background);
            Console.WriteLine(offset);
            Console.WriteLine(notesAmount);
            Console.WriteLine(sliderAmount);
        }

        void ReadLines()
        {
            if      (line.StartsWith(failedValue = "bpm="))             BPM         = int.Parse(line.Split('=')[1]);
            else if (line.Contains(failedValue = "music="))             music       = beatmapFile + @"\..\" + line.Split('=')[1];
            else if (line.Contains(failedValue = "lanes="))             lanes       = int.Parse(line.Split('=')[1]);
            else if (line.Contains(failedValue = "background="))        background  = beatmapFile + @"\..\" + line.Split('=')[1];
            else if (line.Contains(failedValue = "offset="))            offset      = int.Parse(line.Split('=')[1]);
            else if (line.Contains(failedValue = "name="))              name        = line.Split('=')[1];
            else if (line.Contains(failedValue = "beatmap"))        {   noteMode    = true;                                                          }
            else if (line.Contains(failedValue = "menutime="))          startTime   = int.Parse(line.Split('=')[1]);
            else if (line.Contains(failedValue = "difficulty="))        difficulty  = int.Parse(line.Split('=')[1]);
            else if (line.Contains(failedValue = "internal="))          internalName= line.Split('=')[1];
            else if (line.Contains(failedValue = "actualbpm="))         actualbpm   = int.Parse(line.Split('=')[1]);
        }
    }
}
