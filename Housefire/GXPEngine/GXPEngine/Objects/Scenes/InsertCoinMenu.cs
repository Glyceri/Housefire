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

        public InsertCoinMenu() : base(1920,1080, false)
        {
            using (Bitmap bitmap = new Bitmap("notsellectedsong.png"))
            {
                DrawSprite(bitmap, new Vector2(1920 / (float)bitmap.Width, 1080 / (float)bitmap.Height), 0, 0, false);
                button = new Button(1920, 100);
               
                button.DrawSprite(bitmap, new Vector2(button.width / (float)bitmap.Width, button.height / (float)bitmap.Height), 0, 0, false);
                button.SetXY(0, (1080 / (float)2) - (bitmap.Height / (float)2));
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
            }
        }

    }
}
