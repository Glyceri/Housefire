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

        public EasyDraw comboBackdrop;
        public EasyDraw comboText;

        public EasyDraw sideThing;

        public EasyDraw scoreBackdrop;
        public EasyDraw scoreText;

        public List<Lane> lanes = new List<Lane>();
        List<Footer> footers = new List<Footer>();

        public bool flipped = false;

        BeatmapHandler beatmapHandler;

        public KeyboardHook keyboardHook;
        public BeatmapScoring beatmapScoring;

        public EasyDraw hitText;


        EasyDraw livesPanelText;

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

           

            using (Bitmap bitmap = new Bitmap("extrathings/sidething.png"))
            {
                sideThing = new EasyDraw(bitmap.Width, bitmap.Height, false);
                sideThing.scale *= new Vector2(0.85f, 0.85f);
               

                if (!flip)
                {
                    sideThing.SetXY(-540 * 2, -60);
                    //sideThing.SetXY(-460 * 2, 160);
                    sideThing.DrawSprite(bitmap, new Vector2(sideThing.width / (float)bitmap.Width, sideThing.height / (float)bitmap.Height));
                    
                }
                else
                {
                    sideThing.DrawSprite(bitmap, new Vector2(-(sideThing.width / (float)bitmap.Width), sideThing.height / (float)bitmap.Height), 2);
                    sideThing.SetXY(880, -60);
                }

                AddChild(sideThing);
            }

            using (Bitmap bitmap = new Bitmap("extrathings/combobaracctivated.png"))
            {
                livesPanelText = new EasyDraw(bitmap.Width / 4 * 3, bitmap.Height / 4 * 3, false);
                livesPanelText.collider = new BoxCollider(Vector2.Zero, new Vector2(bitmap.Width / 4 * 3, bitmap.Height / 4 * 3), livesPanelText);
                livesPanelText.Clear(Color.Transparent);
                using (Bitmap bit = new Bitmap("ui/Healthbar.png")) {
                    if (!flip)
                    {
                        livesPanelText.SetXY(-470 * 2, -100);
                        livesPanelText.DrawSprite(bit, new Vector2(livesPanelText.width / (float)bit.Width, livesPanelText.height / (float)bit.Height));

                        if (MyGame.Instance.player1.health >= 1) using (Bitmap hp1 = new Bitmap("UI/Health/1hp.png"))
                        livesPanelText.DrawSprite(hp1, new Vector2(livesPanelText.width / (float)hp1.Width, livesPanelText.height / (float)hp1.Height));

                        if (MyGame.Instance.player1.health >= 2) using (Bitmap hp1 = new Bitmap("UI/Health/2Hp.png"))
                        livesPanelText.DrawSprite(hp1, new Vector2((livesPanelText.width / (float)hp1.Width), livesPanelText.height / (float)hp1.Height));

                        if (MyGame.Instance.player1.health >= 3) using (Bitmap hp1 = new Bitmap("UI/Health/Full Hp.png"))
                        livesPanelText.DrawSprite(hp1, new Vector2((livesPanelText.width / (float)hp1.Width), livesPanelText.height / (float)hp1.Height));
                    }
                    else
                    {
                        livesPanelText.DrawSprite(bit, new Vector2(-(livesPanelText.width / (float)bit.Width), livesPanelText.height / (float)bit.Height), 2);
                        livesPanelText.SetXY(510, -100);

                        if (MyGame.Instance.player2.health >= 1) using (Bitmap hp1 = new Bitmap("UI/Health/1hp.png"))
                        livesPanelText.DrawSprite(hp1, new Vector2(-(livesPanelText.width / (float)hp1.Width), livesPanelText.height / (float)hp1.Height), 2);

                        if (MyGame.Instance.player2.health >= 2) using (Bitmap hp1 = new Bitmap("UI/Health/2Hp.png"))
                        livesPanelText.DrawSprite(hp1, new Vector2(-(livesPanelText.width / (float)hp1.Width), livesPanelText.height / (float)hp1.Height), 2);

                        if (MyGame.Instance.player2.health >= 3) using (Bitmap hp1 = new Bitmap("UI/Health/Full Hp.png"))
                        livesPanelText.DrawSprite(hp1, new Vector2(-(livesPanelText.width / (float)hp1.Width), livesPanelText.height / (float)hp1.Height), 2);
                    }

                    AddChild(livesPanelText);
                }


                scoreBackdrop = new EasyDraw(bitmap.Width/4*3, bitmap.Height/4*3, false);
                scoreBackdrop.DrawSprite(bitmap, new Vector2(scoreBackdrop.width/(float)bitmap.Width, scoreBackdrop.height/(float)bitmap.Height));
                scoreText = new EasyDraw(scoreBackdrop.width, scoreBackdrop.height, false);
                scoreText.SetXY(0, -5);
                scoreText.collider = new BoxCollider(Vector2.Zero, new Vector2(scoreBackdrop.width, scoreBackdrop.height), scoreText);
                scoreBackdrop.AddChild(scoreText);

                comboBackdrop = new EasyDraw(scoreBackdrop.width, scoreBackdrop.height, false);
                comboBackdrop.DrawSprite(bitmap, new Vector2(comboBackdrop.width / (float)bitmap.Width, comboBackdrop.height / (float)bitmap.Height));
                comboText = new EasyDraw(comboBackdrop.width, comboBackdrop.height, false);
                //comboText.SetXY(0, -5);
                comboText.collider = new BoxCollider(Vector2.Zero, new Vector2(comboBackdrop.width, comboBackdrop.height), comboText);
                comboBackdrop.AddChild(comboText);

                if (!flip)
                {
                    scoreBackdrop.SetXY(-460 *2, 50);
                    comboBackdrop.SetXY(-470 * 2, 190);
                }
                else
                {
                    scoreBackdrop.SetXY(480, 50);
                    comboBackdrop.SetXY(500, 190);
                }

                AddChild(scoreBackdrop);
                AddChild(comboBackdrop);

               
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
            AddHeader(laneCount, player);
           

            beatmapScoring = new BeatmapScoring(beatmapHandler, this, player);


            hitText = new EasyDraw(300, 100);
            hitText.Clear(Color.Transparent);
            hitText.SetXY(-(hitText.width / (float)2) - 40, 640);
            hitText.TextSize(30);
            hitText.TextAlign(CenterMode.Center, CenterMode.Center);
            AddChild(hitText);
        }

        float clearTextcounter = 0;
        bool clearText = false;
        public void SetText(string text, Color color)
        {
            hitText.Clear(Color.Transparent);
            hitText.SetColor(color);
            hitText.Text(text, hitText.width / (float)2, hitText.height / (float)2);
            clearTextcounter = 0.33f;
            clearText = true;
        }

        void AddDebugOrigin()
        {
            Sprite debugOrigin = new Sprite("DebugOrigin.png", true, false);
            debugOrigin.SetOrigin(debugOrigin.width / 2, debugOrigin.height / 2);
            AddChild(debugOrigin);
        }

        void AddHeader(int laneCount, Player player)
        {
            Sprite headerold = new Sprite("OldUI/Note.png", false, false);
            header = new Sprite("extrathings/combotbarbasic.png", true, false);
            AddChild(header);

            header.SetOrigin(header.width / 2, header.height);
            header.scale = new Vector2((headerold.width / (float)header.width) * laneCount, scale.x * 1.3f);

            headerText = new EasyDraw(header.width, header.height, false);
            headerText.collider = new BoxCollider(Vector2.Zero, new Vector2(header.width, header.height), headerText);
            headerText.Move(- (header.width / 2), - (header.height *1.25f) );
            headerText.TextSize(50);
            headerText.TextAlign(CenterMode.Center, CenterMode.Center);
            headerText.Text(player.name, headerText.width / 2, headerText.height / 2);
            header.AddChild(headerText);
        }

        void AddLane(int laneCount, int i, bool flip)
        {
            if (!flip)
            {
                if (i == 0)
                {
                    lanes.Add(new Lane(beatmapHandler, this, "LaneLeftSide.png"));
                    //lanes.Add(new Lane(beatmapHandler, this, "Lane.png"));
                }
                else if (i == laneCount - 1)
                {
                    lanes.Add(new Lane(beatmapHandler, this, "LaneRightSide.png"));
                    //lanes.Add(new Lane(beatmapHandler, this, "Lane2.png"));
                }
                else
                {
                    //lanes.Add(new Lane(beatmapHandler, this, "Lane3.png"));
                    lanes.Add(new Lane(beatmapHandler, this, "LaneMiddle.png"));
                }
            }
            else
            {
                if (i == 0)
                {
                    lanes.Add(new Lane(beatmapHandler, this, "LaneRightSide.png"));
                    //lanes.Add(new Lane(beatmapHandler, this, "Lane.png"));
                }
                else if (i == laneCount - 1)
                {
                    
                    lanes.Add(new Lane(beatmapHandler, this, "LaneLeftSide.png"));
                    //lanes.Add(new Lane(beatmapHandler, this, "Lane2.png"));
                }
                else
                {
                    //lanes.Add(new Lane(beatmapHandler, this, "Lane3.png"));
                    lanes.Add(new Lane(beatmapHandler, this, "LaneMiddle.png"));
                }
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
            if(clearTextcounter >= 0 && clearText)
            {
                clearTextcounter -= Time.deltaTime;
                hitText.scale = new Vector2(1 + clearTextcounter, 1 + clearTextcounter);
                //hitText.SetXY(-(hitText.width / (float)2), 640);
            }
            if(clearTextcounter <=0 && clearText)
            {
                clearText = false;
                clearTextcounter = 0;
                hitText.Clear(Color.Transparent);
                hitText.scale = Vector2.One;
                //hitText.SetXY(-(hitText.width / (float)2), 640);
            }
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
