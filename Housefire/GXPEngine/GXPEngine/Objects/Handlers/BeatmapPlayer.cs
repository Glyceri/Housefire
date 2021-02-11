using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GXPEngine.Objects.Handlers
{
    public class BeatmapPlayer
    {
        BeatmapHandler beatmapHandler;

        public List<Note> notes;
        public List<TimingPoint> points;

        public List<Note> notesInQueue = new List<Note>();

       

        public BeatmapPlayer(BeatmapHandler beatmapHandler)
        {
            this.beatmapHandler = beatmapHandler;
        }


        public void Tick()
        {
            HandleNodeSpawn();
        }

        void HandleNodeSpawn()
        {

            for (int i = notes.Count - 1; i >= 0; i--)
            {
                if (beatmapHandler.infiniteBeatmapTimer + ((1000 / beatmapHandler.BPM_calc) * 1000) + beatmapHandler.activeBeatmap.offset >= notes[i].hitTime)
                {
                    foreach (LaneObject lane in beatmapHandler.lanes)
                    {
                        if (notes[i].length > 0)
                        {
                            lane.SpawnNote(notes[i]);
                            lane.SpawnNote(new Note(notes[i].lane, notes[i].hitTime, -1), "NoteOpenTop.png");
                            notesInQueue.Add(new Note(notes[i].lane, notes[i].hitTime + notes[i].length, -1));
                        }
                        else
                        {
                            lane.SpawnNote(notes[i]);
                        }
                    }
                    notes.RemoveAt(i);
                }
            }

            for (int i = notesInQueue.Count - 1; i >= 0; i--)
            {
                if (beatmapHandler.infiniteBeatmapTimer + ((1000 / beatmapHandler.BPM_calc) * 1000) + beatmapHandler.activeBeatmap.offset >= notesInQueue[i].hitTime)
                {
                    foreach (LaneObject lane in beatmapHandler.lanes)
                    {
                        lane.SpawnNote(notesInQueue[i], "NoteOpenBot.png");
                    }
                    notesInQueue.RemoveAt(i);
                }
            }

            if(notes.Count <= 0 && notesInQueue.Count <= 0)
            {
                endingTimer += Time.deltaTime;
                if(endingTimer >= 2f)
                {
                    MyGame.Instance.oldSong = beatmapHandler.Stop(true);
                }
            }
        }

        float endingTimer = 0;

        public void Stop()
        {
            notes?.Clear();
            points?.Clear();
        }
    }
}
