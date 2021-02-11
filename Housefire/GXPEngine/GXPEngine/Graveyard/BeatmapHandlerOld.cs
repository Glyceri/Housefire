using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GXPEngine.Objects.Handlers
{
    public class BeatmapHandlerOld : GameObject
    {
        /*
        public static BeatmapHandler Instance;
        public static Beatmap activeBeatmap { get; private set; }
        public static bool isPlaying { get; private set; } = false;

        public BeatmapPlayer beatmapPlayer = new BeatmapPlayer();

        Sprite background;

        LaneObject laneObjectLeft;
        LaneObject laneObjectRight;

        public void SpawnBeatmap(string beatmapName)
        {
            if (activeBeatmap != null || isPlaying) return;
            SpawnBeatmap(new Beatmap(beatmapName));
            activeBeatmap.WriteDebug(true);
            
        }

        public void SpawnBeatmap(Beatmap beatmap)
        {
            Instance = this;
            if (activeBeatmap != null || isPlaying) return;
            if (!(beatmap?.IsValid() ?? false)) return;
            CleanupOld();
            
            SpawnBackground(beatmap);
            SpawnLanes(beatmap);
            activeBeatmap = beatmap;

        }

        void CleanupOld()
        {
            isPlaying = false;
            activeBeatmap?.Dispose();
            activeBeatmap = null;

            background?.LateDestroy();
            laneObjectLeft?.LateDestroy();
            laneObjectRight?.LateDestroy();
            laneObjectLeft = null;
            laneObjectRight = null;
            background = null;
        }

        void SpawnBackground(Beatmap beatmap)
        {
            background = new Sprite(beatmap.background);
            background.scale = new Vector2(1 / ((float)background.width / game.width), 1 / ((float)background.height / game.height));
            AddChild(background);
        }

        void SpawnLanes(Beatmap beatmap)
        {

            AddChild(laneObjectLeft = new LaneObject(beatmap.lanes, new int[4] { Key.D, Key.F, Key.J, Key.K }));
            laneObjectLeft.SetXY(920, 100);
            laneObjectLeft.rotation = 0;

            AddChild(laneObjectRight = new LaneObject(beatmap.lanes, new int[4] { Key.J, Key.K, Key.L, Key.P }, true));
            //laneObjectRight.SetXY(1000, 900);
            laneObjectRight.rotation = -90;
        }

        public void Play()
        {
            
            if (activeBeatmap == null || isPlaying) return;
            isPlaying = true;
            beatmapPlayer.SetupBeatmap(this, new List<LaneObject>() { laneObjectLeft, laneObjectRight });
            beatmapPlayer.beatmapTimer = 0;
            beatmapPlayer.BPM_calc = 0;
            beatmapPlayer.oneSecondCounter = 0;
            activeBeatmap?.music.Play();
        }

        void Update()
        {
            if (isPlaying) { beatmapPlayer?.Update(); }
        }

        public void Stop()
        {
            CleanupOld();
            beatmapPlayer.Stop();
        }
        */
    }
}
