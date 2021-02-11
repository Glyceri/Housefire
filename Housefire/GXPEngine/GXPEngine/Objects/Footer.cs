using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GXPEngine.AddOns.KeyboardHook;

namespace GXPEngine.Objects
{
    public class Footer : GameObject
    {

        public Bitmap footerDead;
        public Bitmap footerAlive;

        public EasyDraw easyDraw;

        VKeys myKey;

        LaneObject laneObject;

        public Footer(VKeys key, LaneObject laneObject)
        {
            myKey = key;

            this.laneObject = laneObject;
            laneObject.keyboardHook.KeyDown += new KeyboardHookCallback(OnKeyDown);
            laneObject.keyboardHook.KeyUp += new KeyboardHookCallback(OnKeyUp);

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
            try
            {
                easyDraw.Text(myKey.ToString().Split('_')[1], footerDead.Width / 2, footerDead.Height / 2);
            }
            catch
            {
                easyDraw.Text(myKey.ToString(), footerDead.Width / 2, footerDead.Height / 2);
            }
        }

        protected override void OnDestroy()
        {

        }

        public void OnKeyUp(VKeys key)
        {
            if (key == myKey)
            {
                shouldCount = true;
                stayOnTime = 0.04f;
            }
        }
        public void OnKeyDown(VKeys key)
        {
            if(key == myKey)
            {
                DrawAlive();
                shouldCount = false;
                stayOnTime = 0;
            }
        }



        float stayOnTime = 0;
        bool shouldCount = false;

        void Update()
        {

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
