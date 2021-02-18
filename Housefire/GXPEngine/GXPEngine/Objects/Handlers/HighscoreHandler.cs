using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GXPEngine.Objects.Handlers.Data;
using System.IO;

namespace GXPEngine.Objects.Handlers
{
    public class HighscoreHandler
    {
        public Dictionary<string, List<Highscore>> highscores = new Dictionary<string, List<Highscore>>();


        public void ReadHighscores()
        {
            CleanOld();
            try
            {
                foreach(string file in Directory.GetFiles("Highscores"))
                {
                    if (file.EndsWith(".high"))
                    {
                        
                        string fileName = file.Replace(@"Highscores\", "").Replace(".high", "");
                        highscores.Add(fileName, new List<Highscore>());
                        try
                        {
                            foreach(string line in File.ReadAllLines(file))
                            {
                                try
                                {
                                    highscores[fileName].Add(new Highscore(line.Split(':')[0], int.Parse(line.Split(':')[1]), int.Parse(line.Split(':')[2])));
                                }
                                catch
                                {
                                   // Console.WriteLine("Converting line went wrong");
                                }
                            }
                        }
                        catch
                        {
                            Console.WriteLine("File not found");
                        }
                    }
                }
            }
            catch
            {
                Console.WriteLine("Folder not found");
            }

            foreach(string key in highscores.Keys)
            {
                highscores[key].Sort((a, b) => a.score > b.score ? a.score : b.score);
            }
        }

        public void WriteScore(string internalName, string name, int score, int combo)
        {
            if (score == 0 || combo == 0) return;
            if (!Directory.Exists("Highscores")) Directory.CreateDirectory("Highscores");
            if (!File.Exists("Highscores/" + internalName + ".high")) File.Create("Highscores/" + internalName + ".high").Close();
            string fileText = File.ReadAllText("Highscores/" + internalName + ".high");
            fileText += '\n' + name + ":" + score.ToString() + ":" + combo.ToString();
            File.WriteAllText("Highscores/" + internalName + ".high", fileText);
            ReadHighscores();
        }

        public List<Highscore> GetHighscores(string internalName)
        {
            try
            {
                return (from score in highscores[internalName] orderby score.score descending select score).ToList();
            }
            catch
            {
                return new List<Highscore>(0);
            }
        }

        void CleanOld()
        {
            highscores.Clear();
        }

    }
}
