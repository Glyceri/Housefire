using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace GXPEngine.Objects.Handlers
{
    public class Beatmap : IDisposable
    {
        public List<Note> notes = new List<Note>();
        public List<TimingPoint> points = new List<TimingPoint>();
        public Sound music;
        public int lanes = -1;
        public Bitmap background;
        public int BPM = -1;
        public int offset = 0;
        public string name = "Song!";

        bool noteMode = false;
        bool timingMode = false;

        public Beatmap(Beatmap beatmap)
        {
            notes = new List<Note>(beatmap.notes);
            points = new List<TimingPoint>(beatmap.points);
            music = new Sound(beatmap.music.fileName);
            lanes = beatmap.lanes;
            background = new Bitmap(beatmap.background);
            BPM = beatmap.BPM;
            offset = beatmap.offset;
            name = beatmap.name;

        }

        public Beatmap(string beatmapFile)
        {
            try
            {
                string[] lines = File.ReadAllLines(beatmapFile);
                string line = "";
                for(int i = 0; i < lines.Length; i++)
                {
                    string failedValue = "";
                    try
                    {
                        line = lines[i];
                        if(line.Contains("//")) line = line.Split('/')[0];
                        line = line.TrimEnd(' ');
                        if (!line.EndsWith(";")) continue;
                        line = line.Replace(";", "");


                        if (!noteMode && !timingMode)
                        {
                                 if (line.Contains(failedValue = "bpm="))           BPM = int.Parse(line.Split('=')[1]);
                            else if (line.Contains(failedValue = "music="))         music = new Sound(beatmapFile + @"\..\" + line.Split('=')[1]);
                            else if (line.Contains(failedValue = "lanes="))         lanes = int.Parse(line.Split('=')[1]);
                            else if (line.Contains(failedValue = "background="))    background = new Bitmap(beatmapFile + @"\..\" + line.Split('=')[1]);
                            else if (line.Contains(failedValue = "beatmap")) {      noteMode = true; timingMode = false; }
                            else if (line.Contains(failedValue = "timingPoints")) { timingMode = true; noteMode = false; }
                            else if (line.Contains(failedValue = "offset"))         offset = int.Parse(line.Split('=')[1]);
                            else if (line.Contains(failedValue = "name"))           name = line.Split('=')[1]; 
                        }
                        else if(noteMode)
                        {
                            try
                            {
                                if(line == "end")
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
                        }else if (timingMode)
                        {
                            try
                            {

                                if (line == "end")
                                {
                                    timingMode = false;
                                    continue;
                                }
                                string[] csl = line.Replace(";", "").Split(',');
                                TimingPoint tp = new TimingPoint();
                                tp.activationTime = int.Parse(csl[0]);
                                tp.beatLength = int.Parse(csl[1]);
                                points.Add(tp);
                            }
                            catch
                            {
                                Console.WriteLine("Something in the timingPoints section went wrong");
                            }
                        }

                    }
                    catch { Console.WriteLine("Loading a value failed: " + failedValue); }
                }
            }
            catch { Console.WriteLine("File not found: " + beatmapFile); }
            Console.WriteLine("Beatmap Read!");
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
            background?.Dispose();
            background = null;
            if (!keepMusicGoing)
            {
                music?.Dispose();
                music = null;
                return null;
            }
            notes?.Clear();
            lanes = 0;
            BPM = 0;
            return music;
        }
        
        public void Dispose()
        {
            Dispose(false);
        }
    }
}
