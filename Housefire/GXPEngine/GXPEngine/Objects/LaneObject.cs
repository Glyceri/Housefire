using GXPEngine.AddOns;
using GXPEngine.Core;
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

        Sprite header;
        public EasyDraw headerText;

        public EasyDraw scoreBackdrop;
        public EasyDraw scoreText;

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
        public LaneObject(BeatmapHandler beatmapHandler, int laneCount, Player player, bool flip = false)
        {
            this.beatmapHandler = beatmapHandler;
            keyboardHook = new KeyboardHook();
            keyboardHook.Install();

            flipped = flip;
            if (laneCount <= 3) laneCount = 4;

            scale = new Vector2(0.5f, 1f);
            using (Bitmap bitmap = new Bitmap("round.png")) 
            {
                scoreBackdrop = new EasyDraw(bitmap.Width/4*3, bitmap.Height/4*3, false);
                scoreBackdrop.DrawSprite(bitmap, new Vector2(scoreBackdrop.width/(float)bitmap.Width, scoreBackdrop.height/(float)bitmap.Height));
                scoreText = new EasyDraw(scoreBackdrop.width, scoreBackdrop.height, false);
                scoreText.SetXY(0, -5);
                scoreText.collider = new BoxCollider(Vector2.Zero, new Vector2(scoreBackdrop.width, scoreBackdrop.height), scoreText);
                scoreBackdrop.AddChild(scoreText);
                MyGame.Instance.AddChild(scoreBackdrop);
            }

            //AddDebugOrigin();
            for (int i = 0; i < laneCount; i++)
            {
                AddLane(laneCount, i, flip);
                try
                {
                    AddFooter(laneCount, i, player.keybinds[laneCount -1][i], flip);
                }
                catch
                {
                    AddFooter(laneCount, i, VKeys.NONAME, flip);
                }
            }
            AddHeader(laneCount);
           

            beatmapScoring = new BeatmapScoring(beatmapHandler, this, player);
        }

        void AddDebugOrigin()
        {
            Sprite debugOrigin = new Sprite("DebugOrigin.png", true, false);
            debugOrigin.SetOrigin(debugOrigin.width / 2, debugOrigin.height / 2);
            AddChild(debugOrigin);
        }

        void AddHeader(int laneCount)
        {
            Sprite headerold = new Sprite("OldUI/Note.png", false, false);
            header = new Sprite("round.png", true, false);
            AddChild(header);
            header.SetOrigin(header.width / 2, header.height / 4 * 3);
            header.scale = new Vector2((headerold.width / (float)header.width) * laneCount, scale.x * 1.3f);

            headerText = new EasyDraw(header.width, header.height, false);
            headerText.collider = new BoxCollider(Vector2.Zero, new Vector2(header.width, header.height), headerText);
            headerText.Move(- (header.width / 2), - (header.height / 4 * 3) - 20);
            header.AddChild(headerText);
        }

        void AddLane(int laneCount, int i, bool flip)
        {
            
            if(i == 0)
            {
                lanes.Add(new Lane(beatmapHandler, this, "LaneLeftSide.png"));
                //lanes.Add(new Lane(beatmapHandler, this, "Lane.png"));
            }
            else if(i == laneCount - 1)
            {
                lanes.Add(new Lane(beatmapHandler, this, "LaneRightSide.png"));
                //lanes.Add(new Lane(beatmapHandler, this, "Lane2.png"));
            }
            else
            {
                //lanes.Add(new Lane(beatmapHandler, this, "Lane3.png"));
                lanes.Add(new Lane(beatmapHandler, this, "LaneMiddle.png"));
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
            footers[i].SetXY(i * (lanes[0].easyDraw.width ) - (lanes[0].easyDraw.width * (laneCount/(float)2)), (lanes[i].lane.Height * lanes[i].scaleY) );
            //if(!flip) footers[i].SetXY(((i * footerWidth) - ((footerWidth / 2) * laneCount) + (footerWidth / 2)) * lanes[i].scaleX, (lanes[i].lane.Height * lanes[i].scaleY) + (footerWidth / 2));
            //else footers[i].SetXY(((footerWidth / 2 * laneCount) - ((i + 1) * footerWidth) + footerWidth / 2) * lanes[i].scaleX,    (lanes[i].lane.Height * lanes[i].scaleY) + (footerWidth / 2));
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
                //footers[i].easyDraw.Mirror(rotation > 0 && rotation < 180, rotation > 0 && rotation < 180);
            }

            beatmapScoring.CheckForHits();
        }

        public void SpawnNote(Note note, string noteImg = "Note.png")
        {
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
