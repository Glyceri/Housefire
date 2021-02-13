using GXPEngine.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GXPEngine.Objects
{
    public class Button : EasyDraw
    {

        public Button(Bitmap image) : base(200, 100, false)
        {
            collider = new BoxCollider(Vector2.Zero, new Vector2(200, 100), this);
            collider.isTrigger = true;
            if (image == null) return;
            DrawSprite(image, new Vector2(200 / (float)image.Width, 100 / (float)image.Height), 0, 0, false);
            
            
        }


        public bool Pressed()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if(collider.HitTestPoint(Input.mouseX, Input.mouseY, false))
                {
                    return true;
                }
            }
            return false;
        }

    }
}
