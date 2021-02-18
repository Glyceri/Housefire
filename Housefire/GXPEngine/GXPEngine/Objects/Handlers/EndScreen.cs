using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GXPEngine.Objects.Handlers
{
    public class EndScreen : GameObject
    {
        bool _canBeInteractedWith;
        public bool canBeInteractedWith { get => _canBeInteractedWith && visible; set { canBeInteractedWith = value; } }


        public EasyDraw backdrop;

        public EndScreen()
        {
            backdrop = new EasyDraw(1920, 1080, false);
            backdrop.Clear(Color.Black);
            AddChild(backdrop);
        }


        void Update()
        {

        }

    }
}
