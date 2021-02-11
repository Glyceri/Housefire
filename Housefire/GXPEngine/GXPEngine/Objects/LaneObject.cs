using GXPEngine.AddOns;
using GXPEngine.Objects.Handlers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GXPEngine.AddOns.KeyboardHook;

namespace GXPEngine.Objects
{
    public class LaneObject : GameObject
    {
        Sprite debugOrigin;

        Sprite header;
        public List<Lane> lanes = new List<Lane>();
        List<Footer> footers = new List<Footer>();

        public bool flipped = false;

        BeatmapHandler beatmapHandler;

        public KeyboardHook keyboardHook;
        public BeatmapScoring beatmapScoring;


        /// <summary>
        /// Spawn a lane object that will handle inputs and spawning of notes and all that jazz.
        /// </summary>
        /// <param name="laneCount">Amount of lanes in this object</param>
        /// <param name="rotation">rotation of this object</param>
        /// <param name="bpm">beats per minute of this song</param>
        public LaneObject(BeatmapHandler beatmapHandler, int laneCount, VKeys[] keyRegisters, bool flip = false)
        {
            this.beatmapHandler = beatmapHandler;
            keyboardHook = new KeyboardHook();
            
            keyboardHook.Install();

            flipped = flip;
            if (laneCount <= 0) laneCount = 1;

            scale = new Vector2(0.5f, 1f);
            AddDebugOrigin();
            for (int i = 0; i < laneCount; i++)
            {
                AddLane(laneCount, i, flip);
                try
                {
                    AddFooter(laneCount, i, keyRegisters[i], flip);
                }
                catch
                {
                    AddFooter(laneCount, i, VKeys.NONAME, flip);
                }
            }
            AddHeader(laneCount);
            AddChild(debugOrigin);

            beatmapScoring = new BeatmapScoring(beatmapHandler, this, keyRegisters);
        }

        void AddDebugOrigin()
        {
            debugOrigin = new Sprite("DebugOrigin.png", true, false);
            debugOrigin.SetOrigin(debugOrigin.width / 2, debugOrigin.height / 2);
        }

        void AddHeader(int laneCount)
        {
            header = new Sprite("Note.png", true, false);
            AddChild(header);
            header.SetOrigin(header.width / 2, header.height);
            header.scale = new Vector2(laneCount, scale.x * 1.3f);
        }

        void AddLane(int laneCount, int i, bool flip)
        {
            
            if(i == 0)
            {
                lanes.Add(new Lane(beatmapHandler, this, "Lane2.png"));
            }else if(i == 2)
            {
                lanes.Add(new Lane(beatmapHandler, this, "Lane3.png"));
            }else
            {
                lanes.Add(new Lane(beatmapHandler, this));
            }
            AddChild(lanes[i]);
            int laneWidth = lanes[i].lane.Width;
            if (!flip) lanes[i].SetXY((i * laneWidth) - ((laneWidth / 2) * laneCount), 0);
            else lanes[i].SetXY(((laneWidth / 2) * laneCount) - ((i + 1) * laneWidth), 0);
        }

        void AddFooter(int laneCount, int i, VKeys key, bool flip)
        {
            footers.Add(new Footer(key, this));
            
            float footerWidth = footers[i].footerDead.Width;
            if(!flip) footers[i].SetXY(((i * footerWidth) - ((footerWidth / 2) * laneCount) + (footerWidth / 2)) * lanes[i].scaleX, (lanes[i].lane.Height * lanes[i].scaleY) + (footerWidth / 2));
            else footers[i].SetXY(((footerWidth / 2 * laneCount) - ((i + 1) * footerWidth) + footerWidth / 2) * lanes[i].scaleX,    (lanes[i].lane.Height * lanes[i].scaleY) + (footerWidth / 2));
            AddChild(footers[i]); 
        }

        public void Tick()
        {
            foreach(Lane lane in lanes)
            {
                lane.LaneUpdate();
            }

            for(int i = 0; i < footers.Count; i++)
            {
                footers[i].easyDraw.Mirror(rotation > 0 && rotation < 180, rotation > 0 && rotation < 180);
            }

            beatmapScoring.CheckForHits();
        }

        public void SpawnNote(Note note, string noteImg = "Note.png")
        {
            //Console.WriteLine("Spawning note: " + note.lane + " ,Time: " + note.hitTime + " ,Length: " + note.length);
            if (flipped)
            {
                lanes[lanes.Count - note.lane - 1].SpawnNode(note.length, noteImg);
            }
            else
            {
                lanes[note.lane].SpawnNode(note.length, noteImg);
            }
        }


        protected override void OnDestroy()
        {
            keyboardHook?.Uninstall();
            lanes.Clear();
            footers.Clear();
        }
    }
}
