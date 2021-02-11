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
        public CheckingNote(int note, bool shouldRewardPoints)
        {
            noteTime = note;
            this.shouldRewardPoints = shouldRewardPoints;
        }
    }
}
