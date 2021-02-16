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

            footerDead = new Bitmap("BottomBarBack.png");
            footerAlive = new Bitmap("BottomBarBackDead.png");
            easyDraw = new EasyDraw(100,50, false);
            easyDraw.TextSize(10);
            easyDraw.TextAlign(CenterMode.Center, CenterMode.Center);

            float footerWidth = easyDraw.width;
            float footerHeight = easyDraw.height;

            //easyDraw.SetOrigin(footerWidth / 3, footerHeight / 3);
            //easyDraw.rotation = 90;
            easyDraw.scale = new Vector2(laneObject.lanes[0].scaleX * 3, 2) ;
            //easyDraw.scale = Vector2.One;
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
                if (myKey.ToString().Contains("_"))
                {
                    easyDraw.Text(myKey.ToString().Split('_')[1], 14, 8);
                }
                else
                {
                    easyDraw.Text(myKey.ToString(), 14, 8);
                }
            }
            catch
            {
                easyDraw.Text(myKey.ToString(), 14, 8);
            }
        }

        protected override void OnDestroy()
        {
            footerAlive?.Dispose();
            footerAlive = null;
            footerDead?.Dispose();
            footerDead = null;
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
