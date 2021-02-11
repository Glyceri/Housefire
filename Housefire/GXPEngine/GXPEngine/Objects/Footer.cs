using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GXPEngine.Objects
{
    public class Footer : GameObject
    {

        public Bitmap footerDead;
        public Bitmap footerAlive;

        public EasyDraw easyDraw;

        int myKey;

        public Footer(int key)
        {
            myKey = key;
            footerDead = new Bitmap("Note.png");
            footerAlive = new Bitmap("NoteHit.png");
            easyDraw = new EasyDraw(footerDead.Width, footerDead.Height, false);
            easyDraw.TextSize(20);
            easyDraw.TextAlign(CenterMode.Center, CenterMode.Center);

            float footerWidth = easyDraw.width;
            float footerHeight = easyDraw.height;

            easyDraw.SetOrigin(footerWidth / 2, footerHeight / 2);
            easyDraw.rotation = 90;
            easyDraw.scale = new Vector2(1, 2);
            DrawDead();
            AddChild(easyDraw);
        }

        void DrawAlive()
        {
            easyDraw.DrawSprite(footerAlive);
            DrawKey();
        }

        void DrawDead()
        {
            easyDraw.DrawSprite(footerDead);
            DrawKey();
        }

        void DrawKey()
        {
            easyDraw.Text(Key.Name(myKey), footerDead.Width / 2, footerDead.Height / 2);
        }

        float stayOnTime = 0;
        bool shouldCount = false;

        void Update()
        {
            if (Input.GetKeyDown(myKey))
            {
                DrawAlive();
            }

            if (Input.GetKey(myKey))
            {
                shouldCount = false;
                stayOnTime = 0;
            }

            if (Input.GetKeyUp(myKey))
            {
                
                shouldCount = true;
                stayOnTime = 0.05f;
            }

            if(stayOnTime >= 0 && shouldCount)
            {
                stayOnTime -= Time.deltaTime;
            }

            if(stayOnTime <= 0 && shouldCount)
            {
                shouldCount = false;
                DrawDead();
            }
        }
    }
}
