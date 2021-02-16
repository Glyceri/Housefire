using GXPEngine.Objects.Handlers;
using System;
using System.Drawing;
using System.Linq;

namespace GXPEngine.Objects
{
    public class Lane : GameObject
    {
        public EasyDraw easyDraw;
        EasyDraw backdrop;
        public Bitmap lane;
        public Bitmap laneBackground;
        public float laneOffset = 0;

        public static float laneSize;

        BeatmapHandler beatmapHandler;
        LaneObject laneObject;
        public Lane(BeatmapHandler beatmapHandler, LaneObject laneObject, bool flipped = false)
        {
            this.beatmapHandler = beatmapHandler;
            this.laneObject = laneObject;
            lane = new Bitmap("Lane.png");
            laneBackground = new Bitmap("LaneBackground.png");
            scale = new Vector2(1, 2f);
            easyDraw = new EasyDraw(lane.Width, lane.Height, false);
            backdrop = new EasyDraw(lane.Width, lane.Height, false);
            backdrop.DrawSprite(laneBackground, 0, 0, true);
            backdrop.DrawSprite(lane, 0, 0, true);
            AddChild(backdrop);
            backdrop.AddChild(easyDraw);
        }

        public Lane(BeatmapHandler beatmapHandler, LaneObject laneObject, string customLane, bool flipped = false)
        {
            this.beatmapHandler = beatmapHandler;
            this.laneObject = laneObject;
            lane = new Bitmap(customLane);
            laneBackground = new Bitmap("LaneBackground.png");
            scale = new Vector2(1, 2f);
            easyDraw = new EasyDraw(lane.Width, lane.Height, false);
            backdrop = new EasyDraw(lane.Width, lane.Height, false);
            backdrop.DrawSprite(laneBackground, 0, 0, true);
            backdrop.DrawSprite(lane, 0, 0, true);
            AddChild(backdrop);
            backdrop.AddChild(easyDraw);
        }


        public void SpawnNode(int length, string noteImg = "Note.png")
        {
            //laneSize = lane.Height * scale.x * (1 / (1000 / (float)BeatmapHandler.approachRate));
            laneSize = lane.Height * scale.x * (1000/(float)beatmapHandler.BPM_calc);
            NoteObject obj = new NoteObject(length, noteImg);
            AddChild(obj);
            obj.SetXY(0, 0);
            obj.personalCounter = 0;
        }




        public void LaneUpdate()
        {
            double yCalcualtion = beatmapHandler.BPM_calc == 0 ? 0 : beatmapHandler.oneSecondTimer / beatmapHandler.BPM_calc;
            easyDraw.Clear(Color.Transparent);

            Vector2 oldScale = scale;
            foreach (NoteObject child in GetChildren(false).Where(a => a is NoteObject))
            {
                child.visible = false;

                if ((BeatmapHandler.approachRate / (float)1000) >= 1)
                {
                    child.personalCounter += (int)(Time.deltaTimeMiliseconds);
                }
                else { 
                    child.personalCounter += (int)(Time.deltaTimeMiliseconds * (BeatmapHandler.approachRate / (float)1000));
                }

                child.SetXY(child.x, (lane.Height * scale.x ) * (float)(child.personalCounter / beatmapHandler.BPM_calc));

 
                //* (1 / (1000 / (float)BeatmapHandler.approachRate))
                float calcedOffset = ((float)((child.personalCounter * (BeatmapHandler.approachRate / (float)1000)) / beatmapHandler.BPM_calc ) * 8);
                easyDraw.DrawSprite(child.texture.bitmap, child.scale, 0, calcedOffset, false);
                
                
                scale = Vector2.One;
                if (Vector2.Distance(TransformPoint(child.position), laneObject.TransformPoint(position)) > (lane.Height * scale.x) + child.height)
                {
                    child.LateDestroy();
                }
                if (MyGame.drawCollision)
                {
                    Gizmos.SetColor(200, 200, 200, 255);
                    Gizmos.DrawRectangle(laneObject.TransformPoint(position), Vector2.Sprite / 4);
                    Gizmos.DrawRectangle(TransformPoint(new Vector2(child.position.x , child.position.y * (1 / parent.scale.x))), Vector2.Sprite / 4);
                }
                scale = oldScale;
            }
        }

        protected override void OnDestroy()
        {
            lane?.Dispose();
            laneBackground?.Dispose();
            laneBackground = null;
            lane = null;
            easyDraw = null;
        }
    }
}
