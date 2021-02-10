using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GXPEngine.Objects
{
    public class HitPlate : Sprite
    {
        EasyDraw easyDraw;

        public HitPlate(string keyName) : base("Note.png", true, false)
        {
            easyDraw = new EasyDraw(width, height, false);
            AddChild(easyDraw);
            easyDraw.TextAlign(CenterMode.Center, CenterMode.Center);
            easyDraw.TextSize(12);
            easyDraw.Text(keyName, width / 2, height / 2);
            easyDraw.SetXY(Vector2.Zero);
        }

    }
}
