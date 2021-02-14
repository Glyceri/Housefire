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

        public Button(int width, int height) : base(width, height, false)
        {
            collider = new BoxCollider(Vector2.Zero, new Vector2(width, height), this);
            collider.isTrigger = true;
            
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
