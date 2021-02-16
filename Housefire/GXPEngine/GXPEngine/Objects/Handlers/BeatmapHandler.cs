using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GXPEngine.AddOns.KeyboardHook;

namespace GXPEngine.Objects.Handlers
{
    public class BeatmapHandler
    {
        public static int beatOffset = 0;

        public static int approachRate = 1000;


        //List<VKeys[]> keysPerLanecount = new List<VKeys[]>() { new VKeys[1] { VKeys.SPACE }, new VKeys[2] { VKeys.KEY_F, VKeys.KEY_J }, new VKeys[3] { VKeys.KEY_F, VKeys.SPACE, VKeys.KEY_J }, new VKeys[4] { VKeys.KEY_D, VKeys.KEY_F, VKeys.KEY_J, VKeys.KEY_K }, new VKeys[5] { VKeys.KEY_D, VKeys.KEY_F, VKeys.SPACE, VKeys.KEY_J, VKeys.KEY_K }, new VKeys[6] { VKeys.KEY_S, VKeys.KEY_D, VKeys.KEY_F, VKeys.KEY_J, VKeys.KEY_K, VKeys.KEY_L }, new VKeys[7] { VKeys.KEY_S, VKeys.KEY_D, VKeys.KEY_F, VKeys.SPACE, VKeys.KEY_J, VKeys.KEY_K, VKeys.KEY_L } };

        Sprite background;

        public List<LaneObject> lanes = new List<LaneObject>();
        public BeatmapSmall activeBeatmap = null;

        public double BPM_calc = 0;
        public int oneSecondTimer = 0;
        public int infiniteBeatmapTimer = -1000;

        public float delta = 0;


        public BeatmapPlayer beatmapPlayer { get; private set; }

        public bool isPlaying { get; private set; } = false;

        public BeatmapHandler(string beatmapName)
        {
            SpawnBeatmap(new BeatmapSmall(beatmapName));
        }

        public BeatmapHandler(BeatmapSmall beatmap)
        {
            SpawnBeatmap(beatmap);
        }

        void SpawnBeatmap(BeatmapSmall beatmap)
        {
            if (beatmap == null) return;
            beatmap.ReadBasicData();
            activeBeatmap = beatmap;
            //SpawnBackground();
            SpawnLanes();
            SpawnBeatmapPlayer();
            
            MyGame.Instance.SetChildIndex(MyGame.Instance.musicHandler, MyGame.Instance.GetChildren().Count);
        }

        void SpawnBackground()
        {
            try
            {
                background = new Sprite(activeBeatmap.background, false, false);
                background.scale = new Vector2(1 / ((float)background.width / MyGame.Instance.width), 1 / ((float)background.height / MyGame.Instance.height));
                MyGame.Instance.AddChild(background);
            }
            catch
            {
                try
                {
                    background = new Sprite("backgroundfinal.jpg", false, false);
                    background.scale = new Vector2(1 / ((float)background.width / MyGame.Instance.width), 1 / ((float)background.height / MyGame.Instance.height));
                    MyGame.Instance.AddChild(background);
                }
                catch
                {

                }
            }
        }

        void SetupAudio()
        {
            try
            {
                MyGame.Instance.musicHandler?.LoadMusic(activeBeatmap.music);
                //MyGame.Instance.musicHandler?.music.Pause();
                SetupAudio();
            }
            catch
            {

            }
        }

        void SpawnLanes()
        {
            lanes.Add(null);
            lanes.Add(null);
            MyGame.Instance.AddChild(lanes[0] = new LaneObject(this, activeBeatmap.lanes, MyGame.Instance.player1, false));
            lanes[0].SetXY(-1000, 100);
            lanes[0].rotation = 0;
            lanes[0].scoreBackdrop.SetXY(20, 200);

            MyGame.Instance.AddChild(lanes[1] = new LaneObject(this, activeBeatmap.lanes, MyGame.Instance.player2, false));
            lanes[1].SetXY(-1000, 100);
            lanes[1].rotation = 0;
            lanes[1].scoreBackdrop.SetXY((1920 - (lanes[1].scoreBackdrop.width ) - 20), 200);
        }

        void SpawnBeatmapPlayer()
        {
            beatmapPlayer = new BeatmapPlayer(this);
            beatmapPlayer.notes = activeBeatmap.ReadNotes();
        }

        public void Play()
        {
            if (activeBeatmap == null || isPlaying) return;
            isPlaying = true;
            infiniteBeatmapTimer = 0;
            
        }

        bool musicIsPlaying = false;

        public void Update()
        {
            
            if (lanes.Count >= 2)
            {
                lanes[0].SetXY(-1500 + (2000*delta), 170);
                lanes[1].SetXY(3420 - (2000 * delta), 170);

                lanes[1].scoreBackdrop.SetXY((1920 - (lanes[1].scoreBackdrop.width) - 20 + 2000 - (2000 * delta)), 200);

                lanes[0].scoreBackdrop.SetXY((-1980 +  (2000 * delta)), 200);
            }

            if (!isPlaying) return;

            Timers();
            Updates();
            if (infiniteBeatmapTimer >= 0 && !musicIsPlaying)
            {
                musicIsPlaying = true;
                MyGame.Instance.musicHandler?.Play();
            }
        }


        void Timers()
        {
            BPM_calc = ((double)60000 / activeBeatmap.BPM) * 4;
            //BPM_calc = (((double)60000 / 250) * 4);
            oneSecondTimer += Time.deltaTimeMiliseconds;
            infiniteBeatmapTimer += Time.deltaTimeMiliseconds;

            if (oneSecondTimer >= BPM_calc)
            {
                oneSecondTimer -= (int)BPM_calc;
            }
        }

        void Updates()
        {
            beatmapPlayer?.Tick();
            foreach (LaneObject lane in lanes) lane.Tick();
        }


        public void Stop(bool keepSongplaying = false)
        {
            if (lanes.Count > 0)
            {
                if (lanes[0].beatmapScoring.beatScore.score > lanes[1].beatmapScoring.beatScore.score)
                {
                    MyGame.Instance.player2.health--;
                    Console.WriteLine("Player 2 lost health");
                }
                else if (lanes[0].beatmapScoring.beatScore.score < lanes[1].beatmapScoring.beatScore.score)
                {
                    MyGame.Instance.player1.health--;
                    Console.WriteLine("Player 1 lost health");
                }
                else
                {
                    MyGame.Instance.player1.health--;
                    MyGame.Instance.player2.health--;
                    Console.WriteLine("Player 1 & 2 lost health");
                }

                MyGame.Instance.livesMenu.SetLives();
                MyGame.roundsRemaining--;
                MyGame.Instance.livesMenu.SetRoundsRemaining();
            }

            beatmapPlayer?.Stop();
            beatmapPlayer = null;
            background?.LateDestroy();
            background = null;
            isPlaying = false;
            foreach (LaneObject lane in lanes) lane?.LateDestroy();
            lanes = new List<LaneObject>();
        }

    }
}
