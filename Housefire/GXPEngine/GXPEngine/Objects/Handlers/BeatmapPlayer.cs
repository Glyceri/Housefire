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

        public List<Note> notesInQueue = new List<Note>();

       

        public BeatmapPlayer(BeatmapHandler beatmapHandler)
        {
            this.beatmapHandler = beatmapHandler;
        }


        public void Tick()
        {
            HandleNodeSpawn();
        }

        public bool forceStop = false;

        void HandleNodeSpawn()
        {

            for (int i = notes.Count - 1; i >= 0; i--)
            {
                if(beatmapHandler.infiniteBeatmapTimer > notes[i].hitTime - (1500 - (BeatmapHandler.approachRate*0.5f))) 
                //if (beatmapHandler.infiniteBeatmapTimer + ((1000 / beatmapHandler.BPM_calc) * 1000) + beatmapHandler.activeBeatmap.offset >= notes[i].hitTime)
                //if (beatmapHandler.infiniteBeatmapTimer + ((1000 / (float)((60000/(float)250) * 4)) * 1000) + beatmapHandler.activeBeatmap.offset >= notes[i].hitTime)
                {
                    foreach (LaneObject lane in beatmapHandler.lanes)
                    {
                        if (notes[i].length > 0)
                        {
                            lane.SpawnNote(notes[i], "Note.png");
                            lane.SpawnNote(new Note(notes[i].lane, notes[i].hitTime, notes[i].hitSound, -1), "NoteOpenTop.png");
                            notesInQueue.Add(new Note(notes[i].lane, notes[i].hitTime + notes[i].length, notes[i].hitSound, -1));
                        }
                        else
                        {
                            lane.SpawnNote(notes[i], "Note.png");
                        }
                    }
                    notes.RemoveAt(i);
                }
            }

            for (int i = notesInQueue.Count - 1; i >= 0; i--)
            {
                //if (beatmapHandler.infiniteBeatmapTimer + ((1000 / beatmapHandler.BPM_calc) * 1000) + beatmapHandler.activeBeatmap.offset >= notesInQueue[i].hitTime)
                if (beatmapHandler.infiniteBeatmapTimer > notesInQueue[i].hitTime - (1500 - (BeatmapHandler.approachRate*0.5f)))
                {
                    foreach (LaneObject lane in beatmapHandler.lanes)
                    {
                        lane.SpawnNote(notesInQueue[i], "NoteOpenBot.png");
                    }
                    notesInQueue.RemoveAt(i);
                }
            }

            if((notes.Count <= 0 && notesInQueue.Count <= 0) || forceStop)
            {
              


                if(forceStop && endingTimer < 3)
                {
                    endingTimer = 3;
                }
                endingTimer += Time.deltaTime;
                if (endingTimer >= 3f)
                {
                    beatmapHandler.delta = 1 - (endingTimer - 3);
                    MyGame.Instance.musicHandler.visible = true;
                    MyGame.Instance.musicHandler.SetXY(0, 0 - (float)MyGame.Instance.musicHandler.backgroundBar.height * (1 - (endingTimer - 3)));
                    MyGame.Instance.menuScores.delta = 1 - (endingTimer - 3);
                    MyGame.Instance.menuScores.visible = true;
                    MyGame.Instance.menuScores.canBeInteractedWith = false;
                    
                }
                if (endingTimer >= 4f)
                {
                    MyGame.Instance.musicHandler.SetXY(0, 0);
                    MyGame.Instance.menuScores.SetScores(beatmapHandler.activeBeatmap, beatmapHandler.lanes[0].beatmapScoring.beatScore, beatmapHandler.lanes[1].beatmapScoring.beatScore);
                    forceStop = false;
                    MyGame.Instance.menuScores.visible = true;
                    MyGame.Instance.menuScores.delta = 0;
                    MyGame.Instance.menuScores.canBeInteractedWith = true;

                    beatmapHandler.Stop(true);
                }
            }
        }

        float endingTimer = 0;

        public void Stop()
        {
            notes?.Clear();
        }
    }
}
