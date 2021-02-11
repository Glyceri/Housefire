using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GXPEngine.Objects.Handlers
{
    public class BeatmapPlayerOld
    {
        /*
        BeatmapHandler beatmapHandler;
        List<Note> activeNotes = new List<Note>();
        List<TimingPoint> timingPoints = new List<TimingPoint>();

        

        List<LaneObject> laneObjects = new List<LaneObject>();
        Stopwatch stopwatch = new Stopwatch();

        public int beatmapTimer = 0;
        public int oneSecondCounter = 0;
        public int BPM_calc = 0;

        public void SetupBeatmap(BeatmapHandler beatmapHandler, List<LaneObject> laneObjects)
        {
            CleanupOld();
            this.laneObjects = laneObjects;
            this.beatmapHandler = beatmapHandler;
            activeNotes = new List<Note>(BeatmapHandler.activeBeatmap.notes);
            timingPoints = new List<TimingPoint>(BeatmapHandler.activeBeatmap.points);
            beatmapTimer = 0;
            stopwatch.Restart();
        }

        public void Update()
        {
            Console.WriteLine("One sec: " + oneSecondCounter);
            Console.WriteLine("BeatmapTimer: " + beatmapTimer);
            Console.WriteLine("BPM_CALC: " + BPM_calc);

            BPM_calc = (int)(((double)60000 / (double)BeatmapHandler.activeBeatmap.BPM) * 4);
            oneSecondCounter += Time.deltaTimeMiliseconds;
            beatmapTimer += Time.deltaTimeMiliseconds;

            foreach(LaneObject lane in laneObjects)
            {
                lane.LaneObjectUpdate();
            }

            HandleNoteSpawn();
           

            if (oneSecondCounter >= (int)BPM_calc)
            {
                oneSecondCounter -= (int)BPM_calc;
            }
        }

        void HandleNoteSpawn()
        {
            
            for(int i = activeNotes.Count - 1; i >= 0; i--)
            {
                //if (beatmapTimer >= (activeNotes[i].hitTime - ((60000/BeatmapHandler.activeBeatmap.BPM) * 2)))
                if (beatmapTimer >= activeNotes[i].hitTime)
                {
                    Console.WriteLine("Called at: " + stopwatch.ElapsedMilliseconds);
                    foreach (LaneObject lane in laneObjects)
                    {
                        
                        lane.SpawnNote(activeNotes[i]);
                    }
                    activeNotes.RemoveAt(i);
                }
            }
        }


        public void CleanupOld()
        {
            beatmapHandler = null;
            for(int i = activeNotes.Count-1; i >= 0; i--)
            {
                activeNotes.RemoveAt(i);
            }
            activeNotes.Clear();
            for (int i = timingPoints.Count - 1; i >= 0; i--)
            {
                timingPoints.RemoveAt(i);
            }
            timingPoints.Clear();
            for(int i = 0; i < laneObjects.Count; i++)
            {
                laneObjects[i]?.LateDestroy();
            }
            laneObjects.Clear();
        }

        public void Stop()
        {
            beatmapTimer = 0;
        }
        */
    }
}
