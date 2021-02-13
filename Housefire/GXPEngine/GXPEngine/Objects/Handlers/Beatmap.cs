using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;

namespace GXPEngine.Objects.Handlers
{
    public class Beatmap : IDisposable
    {
        public List<Note> notes = new List<Note>();
        //public List<TimingPoint> points = new List<TimingPoint>();
        public Sound music = null;
        public Bitmap background = null;
        public int lanes = -1;
        public int BPM = -1;
        public int offset = 0;
        public string name = "Song!";

        bool noteMode = false;


        string[] lines = new string[0];
        string musicName = "";
        string imageName = "";
        public string beatmapFile = "";

        public Beatmap(string beatmapFile, bool loadAll = false)
        {
            this.beatmapFile = beatmapFile;
            lines = File.ReadAllLines(beatmapFile);
            ReadBasicData();
            if (loadAll)
            {
                background = ReadBackground();
                music = ReadMusic();
                ReadNotes();
            }
        }

        public void WriteDebug()
        {
            return;
            
            Console.WriteLine("--------");
            Console.WriteLine(name);
            Console.WriteLine(BPM);
            Console.WriteLine(lanes);
            Console.WriteLine(musicName);
            Console.WriteLine(imageName);
            Console.WriteLine(offset);
        }


        public void ReadBasicData()
        {

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
                    
                    if (line.Contains(failedValue = "bpm=")) BPM = int.Parse(line.Split('=')[1]);
                    else if (line.Contains(failedValue = "music=")) musicName = beatmapFile + @"\..\" + line.Split('=')[1];
                    else if (line.Contains(failedValue = "lanes=")) lanes = int.Parse(line.Split('=')[1]);
                    else if (line.Contains(failedValue = "background=")) imageName = beatmapFile + @"\..\" + line.Split('=')[1];
                    else if (line.Contains(failedValue = "offset")) offset = int.Parse(line.Split('=')[1]);
                    else if (line.Contains(failedValue = "name")) name = line.Split('=')[1];

                }
                catch
                {
                    Console.WriteLine("Loading a value failed: " + failedValue);
                }
            }
        }

        public Bitmap ReadBackground()
        {
            try
            {
                return background = new Bitmap(imageName);
            }
            catch
            {
                return null;
            }
        }

        public Sound ReadMusic()
        {
            if (musicName == "") return null;
            try
            {
                return new Sound(musicName);
            }
            catch
            {
                Console.WriteLine("Couldn't find music");
                return null;
            }
        }

        public void ReadNotes()
        {
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

        public void UnloadImage()
        {
            background?.Dispose();
            background = null;
        }

        public void UnloadMusic()
        {
            music?.Dispose();
            music = null;
        }

        public void ClearNotes()
        {
            notes?.Clear();
        }

        public bool IsValid()
        {
            if (BPM <= 0) return false;
            if (!(notes?.Count > 0)) return false;
            if (music == null) return false;
            if (background == null) return false;
            if (lanes < 1) return false;
            return true;
        }

        public void WriteDebug(bool notesToo = false)
        {
            return;
            Console.WriteLine("BPM: " + BPM);
            Console.WriteLine("Lanes:" + lanes);
            Console.WriteLine("Background: " + background);
            Console.WriteLine("Song: " + music);
            if (notesToo)
            {
                for (int i = 0; i < notes.Count; i++)
                {
                    Console.Write("Note: " + i + ">");
                    Console.Write(notes[i].lane + ", ");
                    Console.Write(notes[i]?.hitTime.ToString().Replace(",", ".") + ", ");
                    Console.WriteLine(notes[i]?.length.ToString().Replace(",", ".") + ", ");
                }
            }
        }

        public Sound Dispose(bool keepMusicGoing)
        {
            UnloadImage();
            if (!keepMusicGoing) UnloadMusic();
            ClearNotes();
            lines = new string[0];
            return music;
        }

        public void Dispose()
        {
            Dispose(false);
        }
    }
}
