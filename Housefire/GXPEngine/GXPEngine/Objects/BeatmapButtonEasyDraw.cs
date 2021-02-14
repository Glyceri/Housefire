using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GXPEngine.Objects
{
    public class BeatmapButtonEasyDraw : EasyDraw
    {
        bool _animateOverTime = false;
        public bool animateOvertime { get => _animateOverTime; set { _animateOverTime = value; counter = 0; } }
        float toAlphaOverTime = 0;
        float startAlpha;
        float timeToSpend = 0;

        float counter = 0;

     

        public BeatmapButtonEasyDraw(int width, int height, bool addCollider = true) : base(width, height, addCollider)
        {
        }

        public BeatmapButtonEasyDraw(System.Drawing.Bitmap bitmap, bool addCollider = true) : base(bitmap, addCollider)
        {
        }

        public BeatmapButtonEasyDraw(string filename, bool addCollider = true) : base(filename, addCollider)
        {
        }

        public void AnimateAlphaOverTime(float startAlpha, float endAlpha, float timeToSpend)
        {
            animateOvertime = true;
            alpha = startAlpha;
            toAlphaOverTime = endAlpha - startAlpha;
            this.startAlpha = startAlpha;
            this.timeToSpend = timeToSpend;
        }

        public void BaseUpdate()
        {
            if (animateOvertime)
            {
                counter += Time.deltaTime;
                alpha = startAlpha + (toAlphaOverTime * (counter / timeToSpend));
                if (alpha < 0) alpha = 0;
                if (alpha > 1) alpha = 1;
                if(counter >= timeToSpend)
                {
                    animateOvertime = false;
                }
            }
        }
    }
}
