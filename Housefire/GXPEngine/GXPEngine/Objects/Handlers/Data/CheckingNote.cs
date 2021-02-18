using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GXPEngine.Objects.Handlers.Data
{
    public struct CheckingNote
    {

        public int noteTime;
        public bool shouldRewardPoints;
        public string noteHitsound;
        public CheckingNote(int note, bool shouldRewardPoints, string hitsound)
        {
            noteTime = note;
            this.shouldRewardPoints = shouldRewardPoints;
            noteHitsound = hitsound;
        }
    }
}
