using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GXPEngine.Objects
{
    public class NoteObject : Sprite
    {
        public int personalCounter = 0;
        public int length;
    

        public NoteObject(int length, string customNote = "Note.png") : base(length > 0 ? "NoteLong.png" : customNote, true, false)
        {
            this.length = length;
            scaleY = 0.3f;
            
            if (length > 0)
            {
                height = -(int)(Lane.laneSize * ((float)length / 1000));
            }
        }
    }
}
