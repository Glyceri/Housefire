using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GXPEngine.Objects.Handlers
{
    public class MainMenuBackgroundHandler : GameObject
    {
        public bool canBeInteractedWith = true;

        BeatmapButtonEasyDraw easyDraw;

        

        public MainMenuBackgroundHandler() 
        {
            easyDraw = new BeatmapButtonEasyDraw(1920, 1080, false);
            AddChild(easyDraw);
            easyDraw.scale = new Vector2(1.007f, 1.007f);

            
        }


        public void SetBackground(Bitmap bitmap)
        {
            easyDraw.Clear(Color.Transparent);
            easyDraw.DrawSprite(bitmap, new Vector2(1920/(float)bitmap.Width, 1080/ (float)bitmap.Height), 0,0, false);
            easyDraw.AnimateAlphaOverTime(0, 1f, 1f);
        }


        public void BaseUpdate()
        {
            easyDraw.BaseUpdate();
            if(canBeInteractedWith)
            easyDraw.SetXY(-((1920 / (float)1000) * 7 * (Input.mouseX / (float)1920)), - ((1080 / (float)1000) * 7 * (Input.mouseY / (float)1080)));
        }
    }
}
