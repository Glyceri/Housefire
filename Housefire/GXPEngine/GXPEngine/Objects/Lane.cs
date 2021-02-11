using GXPEngine.Objects.Handlers;
using System;
using System.Drawing;
using System.Linq;

namespace GXPEngine.Objects
{
    public class Lane : GameObject
    {
        EasyDraw easyDraw;
        public Bitmap lane;
        public float laneOffset = 0;

        public static float laneSize;

        BeatmapHandler beatmapHandler;
        LaneObject laneObject;
        public Lane(BeatmapHandler beatmapHandler, LaneObject laneObject, bool flipped = false)
        {
            this.beatmapHandler = beatmapHandler;
            this.laneObject = laneObject;
            lane = new Bitmap("Lane.png");
            scale = new Vector2(1, 2f);
            easyDraw = new EasyDraw(lane.Width, lane.Height, false);
            AddChild(easyDraw);
        }

        public Lane(BeatmapHandler beatmapHandler, LaneObject laneObject, string customLane, bool flipped = false)
        {
            this.beatmapHandler = beatmapHandler;
            this.laneObject = laneObject;
            lane = new Bitmap(customLane);
            scale = new Vector2(1, 2f);
            easyDraw = new EasyDraw(lane.Width, lane.Height, false);
            AddChild(easyDraw);
        }


        public void SpawnNode(int length, string noteImg = "Note.png")
        {
            laneSize = lane.Height * scale.x * (1000 / (float)beatmapHandler.BPM_calc);
            NoteObject obj = new NoteObject(length, noteImg);
            AddChild(obj);
            obj.SetXY(0, 0);
        }




        public void LaneUpdate()
        {
            double yCalcualtion = beatmapHandler.BPM_calc == 0 ? 0 : beatmapHandler.oneSecondTimer / beatmapHandler.BPM_calc;
            easyDraw.Clear(Color.Transparent);
            easyDraw.DrawSprite(lane, 0, yCalcualtion, true);


            Vector2 oldScale = scale;
            foreach (NoteObject child in GetChildren(false).Where(a => a is NoteObject))
            {
                child.visible = false;

                child.personalCounter += Time.deltaTimeMiliseconds;

                child.SetXY(child.x, lane.Height * scale.x * (float)(child.personalCounter / beatmapHandler.BPM_calc));

                easyDraw.DrawSprite(child.texture.bitmap, child.scale, 0, (float)(child.personalCounter / beatmapHandler.BPM_calc) * 8, false);
                
                
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
            lane = null;
            easyDraw = null;
        }
    }
}
