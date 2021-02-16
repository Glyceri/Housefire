using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GXPEngine.Objects.Handlers.Data
{
    public class Highscore
    {
        public string name;
        public int score;
        public int combo;

        public Highscore(string name, int score, int combo)
        {
            this.name = name;
            this.score = score;
            this.combo = combo;
        }

    }
}
