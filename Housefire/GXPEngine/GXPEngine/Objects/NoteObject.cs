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

        public NoteObject() : base("Note.png", true, false)
        {
            scaleY = 0.4f;
        }

    }
}
