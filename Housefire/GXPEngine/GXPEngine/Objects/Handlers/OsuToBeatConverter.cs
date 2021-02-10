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
                    newLine += csl[2];
                    newLine += ",-1;";
                    beatLines.Add(newLine);
                }
                catch 
                {

                }
            }
            File.WriteAllLines("convertedFile.txt", beatLines.ToArray());
        }


    }
}
