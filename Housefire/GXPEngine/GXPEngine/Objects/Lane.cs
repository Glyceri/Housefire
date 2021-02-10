using GXPEngine.Objects.Handlers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GXPEngine.Objects
{
    public class Lane : GameObject
    {
        EasyDraw easyDraw;
        public Bitmap lane;
        public float laneOffset = 0;

        public Lane(bool flipped = false)
        {
            lane = new Bitmap("Lane.png");
            scale = new Vector2(1, 2f);
            easyDraw = new EasyDraw(lane.Width, lane.Height, false);
            AddChild(easyDraw);
        }

        public Lane(string customLane, bool flipped = false)
        {
            lane = new Bitmap(customLane);
            scale = new Vector2(1, 2f);
            easyDraw = new EasyDraw(lane.Width, lane.Height, false);
            AddChild(easyDraw);
        }


        public void SpawnNode()
        {
            NoteObject obj = new NoteObject();
            AddChild(obj);
            obj.SetXY(0, 0);
        }

        int counter = 0;
        double BPM_calc;

        Stopwatch stopwatch = new Stopwatch();

        void Update()
        {
            if (!BeatmapHandler.isPlaying) return;
            BPM_calc = (((double)60000 / (double)BeatmapHandler.activeBeatmap.BPM) * 4);
            counter += Time.deltaTimeMiliseconds; //* (((double)BeatmapHandler.activeBeatmap?.BPM / (double)60) / (double)4);

            easyDraw.Clear(Color.Transparent);
            easyDraw.DrawSprite(lane, 0, (double)counter / BPM_calc, true);

            if(counter >= BPM_calc)
            {
                counter -= (int)BPM_calc;
                //counter = 0;
            }

            foreach(GameObject child in GetChildren(false))
            {
                if (child is NoteObject) {
                    (child as NoteObject).personalCounter += Time.deltaTimeMiliseconds;
                    child.SetXY(child.x, (lane.Height * scale.x) * (float)((double)(child as NoteObject).personalCounter / BPM_calc));

                    if (Vector2.Distance(child.position, position) > (lane.Height * scale.x)+30)
                    {
                        child.LateDestroy();
                    }
                }
            }
        }

        protected override void OnDestroy()
        {
            lane.Dispose();
            lane = null;
            easyDraw = null;
        }
    }
}
