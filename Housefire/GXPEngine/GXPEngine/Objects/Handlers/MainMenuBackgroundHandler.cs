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

        public BeatmapButtonEasyDraw background;

        

        public MainMenuBackgroundHandler() 
        {
            background = new BeatmapButtonEasyDraw(1920, 1080, false);
            AddChild(background);
            background.scale = new Vector2(1.007f, 1.007f);

            
        }


        public void SetBackground(Bitmap bitmap)
        {
            background.Clear(Color.Transparent);
            background.DrawSprite(bitmap, new Vector2(1920/(float)bitmap.Width, 1080/ (float)bitmap.Height), 0,0, false);
            background.AnimateAlphaOverTime(0, 1f, 1f);
        }


        public void BaseUpdate()
        {
            background.BaseUpdate();
            if(canBeInteractedWith)
            background.SetXY(-((1920 / (float)1000) * 7 * (Input.mouseX / (float)1920)), - ((1080 / (float)1000) * 7 * (Input.mouseY / (float)1080)));
        }
    }
}
