using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GXPEngine.Objects.Scenes
{
    public class MenuOverlay : BeatmapButtonEasyDraw
    {

        public MenuOverlay() : base(1920, 1080, false)
        {
            using (Bitmap bitmap = new Bitmap("backgroundpanel.png"))
            {
                DrawSprite(bitmap, new Vector2(1920 / (float)bitmap.Width, 1080 / (float)bitmap.Height), 0, 0, false);
            }
        }
    }
}
