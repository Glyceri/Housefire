using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GXPEngine.Objects.Handlers
{
    public class BeatmapHandler
    {
        List<int[]> keysPerLanecount = new List<int[]>() { new int[1] { Key.SPACE }, new int[2] { Key.F, Key.J }, new int[3] { Key.F, Key.SPACE, Key.J }, new int[4] { Key.D, Key.F, Key.J, Key.K }, new int[5] { Key.D, Key.F, Key.SPACE, Key.J, Key.K }, new int[6] { Key.S, Key.D, Key.F, Key.J, Key.K, Key.L }, new int[7] { Key.S, Key.D, Key.F, Key.SPACE, Key.J, Key.K, Key.L } };

        Sprite background;

        public List<LaneObject> lanes = new List<LaneObject>();
        public Beatmap activeBeatmap = null;

        public double BPM_calc = 0;
        public int oneSecondTimer = 0;
        public int infiniteBeatmapTimer = -1000;

        BeatmapPlayer beatmapPlayer;

        public bool isPlaying { get; private set; } = false;

        public BeatmapHandler(string beatmapName)
        {
            SpawnBeatmap(new Beatmap(beatmapName));
        }

        public BeatmapHandler(Beatmap beatmap)
        {
            SpawnBeatmap(beatmap);
        }

        void SpawnBeatmap(Beatmap beatmap)
        {
            if (beatmap == null || !(beatmap?.IsValid() ?? false)) return;
            activeBeatmap = beatmap;
            SpawnBackground();
            SpawnLanes();
            SpawnBeatmapPlayer();
        }

        void SpawnBackground()
        {
            background = new Sprite(activeBeatmap.background, false);
            background.scale = new Vector2(1 / ((float)background.width / MyGame.Instance.width), 1 / ((float)background.height / MyGame.Instance.height));
            MyGame.Instance.AddChild(background);
        }

        void SpawnLanes()
        {
            lanes.Add(null);
            lanes.Add(null);
            MyGame.Instance.AddChild(lanes[0] = new LaneObject(this, activeBeatmap.lanes, keysPerLanecount[activeBeatmap.lanes - 1], false));
            lanes[0].SetXY(500, 100);
            lanes[0].rotation = 0;

            MyGame.Instance.AddChild(lanes[1] = new LaneObject(this, activeBeatmap.lanes, keysPerLanecount[activeBeatmap.lanes - 1], false));
            lanes[1].SetXY(1200, 100);
            lanes[1].rotation = 0;
        }

        void SpawnBeatmapPlayer()
        {
            beatmapPlayer = new BeatmapPlayer(this);
            beatmapPlayer.notes = new List<Note>(activeBeatmap.notes);
            beatmapPlayer.points = new List<TimingPoint>(activeBeatmap.points);
        }

        public void Play()
        {
            if (activeBeatmap == null || isPlaying || !(activeBeatmap?.IsValid() ?? false)) return;
            isPlaying = true;

        }

        bool musicIsPlaying = false;

        public void Update()
        {
            if (!isPlaying) return;

            Timers();
            Updates();
            if (infiniteBeatmapTimer >= 0 && !musicIsPlaying)
            {
                musicIsPlaying = true;
                activeBeatmap.music?.Play();
            }
        }


        void Timers()
        {
            BPM_calc = (double)60000 / activeBeatmap.BPM * 4;
            oneSecondTimer += Time.deltaTimeMiliseconds;
            infiniteBeatmapTimer += Time.deltaTimeMiliseconds;

            if (oneSecondTimer >= BPM_calc)
            {
                oneSecondTimer -= (int)BPM_calc;
            }
        }

        void Updates()
        {
            beatmapPlayer.Tick();
            foreach (LaneObject lane in lanes) lane.Tick();

        }


        public Sound Stop(bool keepSongplaying = false)
        {
            
            beatmapPlayer?.Stop();
            background?.LateDestroy();
            background = null;
            foreach (LaneObject lane in lanes) lane?.LateDestroy();
            lanes = new List<LaneObject>();
            return activeBeatmap?.Dispose(keepSongplaying);

        }

    }
}
