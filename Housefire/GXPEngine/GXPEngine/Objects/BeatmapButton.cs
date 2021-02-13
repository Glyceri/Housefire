using GXPEngine.Core;
using GXPEngine.Objects.Handlers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GXPEngine.Objects
{
    public class BeatmapButton : EasyDraw
    {
        public Beatmap beatmap;

        int objectWidth;
        int objectHeight;

        bool drewImage = false;

        BeatmapButtonHandler buttonHandler;

        public BeatmapButton(int width, int height, BeatmapButtonHandler buttonHandler) : base(width, height, false)
        {
            this.buttonHandler = buttonHandler;
            objectWidth = width;
            objectHeight = height;

            collider = new BoxCollider(Vector2.Zero, new Vector2(200, 100), this);
            collider.isTrigger = true;

           
        }

        public void SetBeatmap(Beatmap beatmap)
        {
            Cleanup();
            this.beatmap = beatmap;
            DrawText();
        }

        void Update()
        {
            if (beatmap?.background != null && !drewImage)
            {
                drewImage = true;
                DrawSprite(beatmap?.background, new Vector2(objectWidth / (float)beatmap?.background.Width, objectHeight / (float)beatmap?.background.Height), 0, 0, false);
                DrawText();
                SetCustomVertices(0,0, 1.2f, 0, 1, 1, 0, 1);
                useCustomVertices = true;
            }

            if (Input.GetMouseButtonDown(0))
            {
                if(collider.HitTestPoint(Input.mouseX, Input.mouseY, false))
                {
                    MyGame.Instance.oldSong?.Dispose();
                    MyGame.Instance.oldSong = RequestMusic();
                    MyGame.Instance.oldSong?.Play();
                    buttonHandler.RegisterPlay(ref beatmap);
                }
            }
        }

        void DrawText()
        {
            TextAlign(CenterMode.Min, CenterMode.Min);
            TextSize(10);
            SetColor(255, 255, 255);
            Text(beatmap.name, 0, 0);
        }

        
        public void RequestImage()
        {
            beatmap?.ReadBackground();
        }

        public Sound RequestMusic()
        {
            return beatmap?.ReadMusic();
        }

        protected override void OnDestroy()
        {
            beatmap?.Dispose();
            beatmap = null;
            buttonHandler = null;
        }

        public void Cleanup()
        {
            
            beatmap?.Dispose();
            beatmap = null;
            
            drewImage = false;
            Clear(Color.Black);
        }
    }
}
