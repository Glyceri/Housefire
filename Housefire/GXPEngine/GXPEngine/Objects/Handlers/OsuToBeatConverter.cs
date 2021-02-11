using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace GXPEngine.Objects.Handlers
{
    public class OsuToBeatConverter
    {
        public static void ConvertFile(int lanes)
        {
            string[] osuLines = File.ReadAllLines("toconvert.txt");
            List<string> beatLines = new List<string>();
            foreach(string line in osuLines)
            {
                try
                {
                    string newLine = "";
                    string[] csl = line.Split(',');
                    float result = Mathf.Floor(int.Parse(csl[0]) * lanes / 512);
                    newLine += Mathf.Clamp(result, 0, lanes - 1).ToString() + ",";
                    newLine += csl[2] + ",";
                    if (!line.Contains('|'))
                    {
                        if (csl[5].Split(':').Length == 5)  newLine += "-1;";
                        else                                newLine += csl[5].Split(':')[0] + ";";
                    }
                    else
                    {
                        Console.WriteLine("This line is not compatible... sorry");
                    }
                    beatLines.Add(newLine);
                }
                catch 
                {
                    Console.WriteLine("Conversion failed");
                }
            }
            File.WriteAllLines("convertedFile.txt", beatLines.ToArray());
        }


    }
}
