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
    public class BeatmapButton : Sprite
    {
        public BeatmapSmall beatmapSmall;

        EasyDraw colourEasyDraw;
        BeatmapButtonEasyDraw backgroundImage;
        EasyDraw scorePanel;
        EasyDraw textPanel;
        EasyDraw scorePanelText;
        int backgroundImageOffset = 50;

        Vector2 oldScale = Vector2.One;
        Vector2 oldResolution;
        public new Vector2 scale { get => oldScale; set { width = Mathf.Ceiling(oldResolution.x * (oldScale * value).x); height = Mathf.Ceiling(oldResolution.y * (oldScale * value).y); actualScale = value; } }
        public Vector2 actualScale;

        public bool backgroundDrawn { get; private set; } = false;

        public BeatmapButton(Vector2 size) : base("notsellectedsong.png", false, false)
        {
            //float actualWidth = width;
            //float actualHeight = height;
            
            oldScale = scale;
            oldResolution = new Vector2(width, height);
            width = (int)size.x;
            height = (int)size.y;
            actualScale = scale;



            Bitmap colourThing = new Bitmap("colourfulthing.png");
            Bitmap scoreBitmap = new Bitmap("notsellectedsong.png");

            colourEasyDraw = new EasyDraw((int)(oldResolution.x/ 3),(int)oldResolution.y, false);
            colourEasyDraw.DrawSprite(colourThing, new Vector2(colourEasyDraw.width / (float)colourThing.Width, 1), 0,0,false);
            AddChild(colourEasyDraw);

            backgroundImage = new BeatmapButtonEasyDraw((colourEasyDraw.width - backgroundImageOffset), colourEasyDraw.height, false);
            backgroundImage.scale = new Vector2(0.9f, 0.9f);
            backgroundImage.SetXY(((colourEasyDraw.width - backgroundImageOffset) - backgroundImage.width) / 2, (colourEasyDraw.height - backgroundImage.height) / 2);
            backgroundImage.alpha = 0.2f;
            backgroundImage.Clear(Color.White);
            colourEasyDraw.AddChild(backgroundImage);

            textPanel = new EasyDraw((int)(oldResolution.x - (oldResolution.x / 3) - (oldResolution.x / 10)) + backgroundImageOffset, (int)oldResolution.y, false);
            colourEasyDraw.AddChild(textPanel);
            textPanel.SetXY(colourEasyDraw.width - backgroundImageOffset, 0);

            scorePanel = new EasyDraw((int)(oldResolution.x / 10), (int)oldResolution.y, false);
            colourEasyDraw.AddChild(scorePanel);
            scorePanel.SetXY(oldResolution.x - (oldResolution.x / 10), 0);
            scorePanel.DrawSprite(scoreBitmap, new Vector2(scorePanel.width/(float)scoreBitmap.Width, scorePanel.height/ (float)scoreBitmap.Height), 0,0, false);
            scorePanel.alpha = 1;

            scorePanelText = new EasyDraw(scorePanel.width, scorePanel.height, false);
            scorePanel.AddChild(scorePanelText);

            collider = new BoxCollider(Vector2.Zero, new Vector2(oldResolution.x, oldResolution.y), this);
            collider.isTrigger = true;

            colourThing.Dispose();
            scoreBitmap.Dispose();
            
            //REMOVE LATER
            textPanel.collider = new BoxCollider(Vector2.Zero, new Vector2((oldResolution.x - (oldResolution.x / 3) - (oldResolution.x / 10)) + backgroundImageOffset, oldResolution.y), textPanel);
            scorePanel.collider = new BoxCollider(Vector2.Zero, new Vector2((oldResolution.x / 10), oldResolution.y), scorePanel);
            colourEasyDraw.collider = new BoxCollider(Vector2.Zero, new Vector2((int)(oldResolution.x / 3), (int)oldResolution.y), colourEasyDraw);
            backgroundImage.collider = new BoxCollider(Vector2.Zero, new Vector2(colourEasyDraw.width - backgroundImageOffset, colourEasyDraw.height), backgroundImage);
            scorePanelText.collider = new BoxCollider(Vector2.Zero, new Vector2(scorePanelText.width, scorePanelText.height), scorePanelText);
            //--------


            oldResolution = new Vector2(width, height);

        }

        public void SetBeatmapSmall(BeatmapSmall small)
        {
            Dispose();
            beatmapSmall = small;
        }

        public void SetImage()
        {
            if (beatmapSmall == null) return;
            backgroundImage.visible = false;
            using (Bitmap bitmap = new Bitmap(beatmapSmall.background))
            {
                backgroundImage.DrawSprite(bitmap, new Vector2((float)(backgroundImage.width / (float)bitmap.Width) / backgroundImage.scale.x, ((float)backgroundImage.height / (float)bitmap.Height))/backgroundImage.scale.y, 0, 0, false);
            }
            backgroundImage.AnimateAlphaOverTime(0f, 0.7f, 1.8f);
            backgroundDrawn = true;
            backgroundImage.visible = true;
        }

        

        void Update()
        {
            backgroundImage.BaseUpdate();
            if (Input.GetMouseButtonDown(0))
            {
                if(collider.HitTestPoint(Input.mouseX, Input.mouseY, false))
                {
                    Console.WriteLine("HIT!");
                }
            }
        }

        /// <summary>
        /// Disposes old stuff, but keeps object active, so this object can be reused.
        /// </summary>
        public void Dispose()
        {
            backgroundImage.animateOvertime = false;
            backgroundImage.alpha = 0.2f;
            backgroundImage.Clear(Color.White);
            textPanel.Clear(Color.Transparent);
            scorePanelText.Clear(Color.Transparent);
            backgroundDrawn = false;
            beatmapSmall = null;
        }

        /// <summary>
        /// Disposes old stuff, and kills the object
        /// </summary>
        protected override void OnDestroy()
        {
            
        }
    }
}
