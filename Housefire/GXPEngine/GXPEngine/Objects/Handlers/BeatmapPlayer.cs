using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GXPEngine.Objects.Handlers
{
    public class BeatmapPlayer
    {

        Beatmap activeBeatmap;
        BeatmapHandler beatmapHandler;
        List<Note> activeNotes = new List<Note>();
        List<TimingPoint> timingPoints = new List<TimingPoint>();

        public int beatmapTimer = 0;

        List<LaneObject> laneObjects;

        public void SetupBeatmap(BeatmapHandler beatmapHandler, List<LaneObject> laneObjects)
        {
            this.laneObjects = laneObjects;
            this.beatmapHandler = beatmapHandler;
            activeBeatmap = BeatmapHandler.activeBeatmap;
            activeNotes = new List<Note>(activeBeatmap.notes);
            timingPoints = new List<TimingPoint>(activeBeatmap.points);
            beatmapTimer = 0;
        }


        public void Update()
        {
            if (!BeatmapHandler.isPlaying) return;
            beatmapTimer += Time.deltaTimeMiliseconds;
            HandleNoteSpawn();
        }

        void HandleNoteSpawn()
        {
            
            for(int i = activeNotes.Count - 1; i >= 0; i--)
            {
                if (beatmapTimer >= (activeNotes[i].hitTime - ((60000/BeatmapHandler.activeBeatmap.BPM) * 2)))
                //if (beatmapTimer >= (activeNotes[i].hitTime))
                {
                    foreach(LaneObject lane in laneObjects)
                    {
                        lane.SpawnNote(activeNotes[i]);
                    }
                    activeNotes.RemoveAt(i);
                }
            }
        }


        public void Stop()
        {
            beatmapTimer = 0;
        }

    }
}
