using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GXPEngine.Objects.Handlers
{
    public class TimingPoint
    {
        public int activationTime;
        public int beatLength;

        public TimingPoint()
        {

        }

        public TimingPoint(int activationTime, int beatLength)
        {
            this.activationTime = activationTime;
            this.beatLength = beatLength;
        }

    }
}
