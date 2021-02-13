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
        public int offset = 0;
        public string name = "Song!";
        public int notesAmount = 0;
        public int sliderAmount = 0;

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
            if (line.Contains(failedValue = "bpm=")) BPM = int.Parse(line.Split('=')[1]);
            else if (line.Contains(failedValue = "music=")) music = beatmapFile + @"\..\" + line.Split('=')[1];
            else if (line.Contains(failedValue = "lanes=")) lanes = int.Parse(line.Split('=')[1]);
            else if (line.Contains(failedValue = "background=")) background = beatmapFile + @"\..\" + line.Split('=')[1];
            else if (line.Contains(failedValue = "offset=")) offset = int.Parse(line.Split('=')[1]);
            else if (line.Contains(failedValue = "name=")) name = line.Split('=')[1];
            else if (line.Contains(failedValue = "beatmap")) { noteMode = true; }
        }
    }
}
