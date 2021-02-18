using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GXPEngine.Objects.Handlers
{
    public class Note
    {

        public int hitTime;
        public int length;
        public int lane;

        public string hitSound = "";

        

        public Note() { }

        public Note(int lane, int hitTime, string hitSound, int length = 1)
        {
            this.hitTime = hitTime;
            this.length = length;
            this.lane = lane;
            this.hitSound = hitSound;
        }

    }
}
