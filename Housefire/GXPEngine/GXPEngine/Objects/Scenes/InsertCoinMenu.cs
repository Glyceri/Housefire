using GXPEngine.Core.Audio;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GXPEngine.Objects.Scenes
{
    public class InsertCoinMenu : BeatmapButtonEasyDraw
    {
        bool _canBeInteractedWith = true;
        public bool canBeInteractedWith { get => _canBeInteractedWith && visible; set { _canBeInteractedWith = value; } }

        public Button button;

        public bool doAnimation = false;

        float animationSpeed = 5;
        float animationDelta = 0;

        public delegate void AnimationEnd();
        public event AnimationEnd onAnimationEnd = null;

        //I mean I should fix this to have the right dimensions and all now that I changed the menu background thing but :eyes: do I care
        public InsertCoinMenu() : base(1920,1080, false)
        {
            using (Bitmap bitmap = new Bitmap("backgroundpanel.png"))
            {
                bitmap.MakeTransparent(Color.FromArgb(127, 127, 127));
                DrawSprite(bitmap, new Vector2(1920 / (float)bitmap.Width, 1080 / (float)bitmap.Height), 0, 0, false);
            }
            using(Bitmap roboRhythm = new Bitmap("Robo_Rhythm.png"))
            //using(Bitmap roboRhythm = new Bitmap("thumbnail.png"))
            {
                DrawSprite(roboRhythm, new Vector2(1400 / (float)roboRhythm.Width, 1000 / (float)roboRhythm.Height), 0.25f,0, false);
            }

            using(Bitmap bitmap = new Bitmap("insertcoin2.png")) { 
                button = new Button(1920, 100);
               
                button.DrawSprite(bitmap, new Vector2(button.width / (float)bitmap.Width, button.height / (float)bitmap.Height), 0, 0, false);
                button.SetXY(0, (1080 / (float)2) - (button.height / (float)2));
                button.SetColor(255, 255, 255);
                button.TextAlign(CenterMode.Center, CenterMode.Center);
                button.TextSize(40);
                button.Text("Insert Coin", width / (float)2, bitmap.Height/(float)4);
                AddChild(button);
            }
        }

        public void Reset()
        {
            animationDelta = 0;
            DoAnimation();
            doAnimation = false;
        }


        void Update()
        {
            BaseUpdate();
            CheckPressed();
            DoAnimation();
        }


        void DoAnimation()
        {
            if (!doAnimation) return;
            animationDelta += Time.deltaTime * animationSpeed;
            button.SetXY(0, (((height + (button.height)) / 2f) * animationDelta) +(height/2f) - (button.height/2f));
            alpha = Mathf.Clamp(1 - animationDelta, 0, 1);
            if(animationDelta >= 1)
            {
                onAnimationEnd?.Invoke();
                Reset();
            }
        }

        void CheckPressed()
        {
            if (!canBeInteractedWith) return;
            if (button.Pressed())
            {
                doAnimation = true;
                new Sound("Impact_-_Robot_Punch.wav")?.Play(false, 0, AudioHandler.volume / (float)100);
            }
        }

    }
}
