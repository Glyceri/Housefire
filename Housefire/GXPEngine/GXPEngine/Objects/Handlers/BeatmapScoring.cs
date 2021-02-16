using GXPEngine.AddOns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GXPEngine.AddOns.KeyboardHook;
using GXPEngine.Objects.Handlers.Data;
using GXPEngine.Objects.Enums;

namespace GXPEngine.Objects.Handlers
{
    public enum PrecisionLevel
    {
        UltraMiss,
        Miss,
        Just,
        Okay,
        Good,
        Perfect
    }

    public class BeatmapScoring : IDisposable
    {
        public BeatScore beatScore = new BeatScore();

        BeatmapHandler beatmapHandler;
        LaneObject laneObject;

        List<CheckingNote>[] downNotes;
        List<CheckingNote>[] upNotes;

        List<VKeys> validKeys;
        List<bool> hasGoneUp;

        //EasyDraw easyDraw = new EasyDraw(500, 400, false);

        Player player;

        public BeatmapScoring(BeatmapHandler beatmapHandler, LaneObject laneObject, Player player)
        {
            this.player = player;
            //laneObject.AddChild(easyDraw);
            //easyDraw.TextSize(60);
            //easyDraw.SetColor(200, 200, 200);
           // easyDraw.TextAlign(CenterMode.Min, CenterMode.Min);
            //easyDraw.SetXY(((int)Mathf.Floor(laneObject.lanes.Count / 2)) * 100, 0);

            SetupThis(beatmapHandler, laneObject, player.keybinds[laneObject.lanes.Count -1]);
            SetupGoneUp(player.keybinds[laneObject.lanes.Count - 1]);
            SetupKeyboardHook(laneObject);
            SetupNotes();
        }

        void SetupThis(BeatmapHandler beatmapHandler, LaneObject laneObject, VKeys[] validKeys)
        {
            this.beatmapHandler = beatmapHandler;
            this.laneObject = laneObject;
            this.validKeys = validKeys.ToList();
        }

        void SetupGoneUp(VKeys[] validKeys)
        {
            hasGoneUp = new List<bool>();
            for (int i = 0; i < validKeys.Length; i++)
            {
                hasGoneUp.Add(true);
            }
        }

        void SetupKeyboardHook(LaneObject laneObject)
        {
            laneObject.keyboardHook.KeyDown += new KeyboardHookCallback(KeyboardKeyDown);
            laneObject.keyboardHook.KeyUp += new KeyboardHookCallback(KeyboardKeyUp);
        }

        void SetupNotes()
        {
            List<Note> tempNotes = beatmapHandler.activeBeatmap.ReadNotes();
            downNotes = new List<CheckingNote>[validKeys.Count];
            upNotes = new List<CheckingNote>[validKeys.Count];

            for (int valids = 0; valids < validKeys.Count; valids++)
            {
                downNotes[valids] = new List<CheckingNote>();
                upNotes[valids] = new List<CheckingNote>();
                for (int i = 0; i < tempNotes.Count; i++)
                {
                    if (tempNotes[i].lane == valids)
                    {
                        downNotes[valids].Add(new CheckingNote(tempNotes[i].hitTime, true));
                        upNotes[valids].Add(new CheckingNote(tempNotes[i].hitTime, false));
                        if (tempNotes[i].length > 0)
                        {
                            upNotes[valids].Add(new CheckingNote(tempNotes[i].hitTime + tempNotes[i].length, true));
                        }
                    }
                }
            }
        }


        public void CheckForHits()
        {
            CheckForMissDown();
            CheckForMissUp();
        }

        void CheckForMissDown()
        {
            for (int i = 0; i < downNotes.Length; i++)
            {
                if (downNotes[i].Count > 0)
                {
            
                    if(beatmapHandler.infiniteBeatmapTimer > downNotes[i][0].noteTime - (1000 - beatmapHandler.BPM_calc) + HitWindow.miss)
                    {
                       
                        Miss(PrecisionLevel.Miss, i, downNotes[i][0].shouldRewardPoints, true);
                    }
                }
            }
        }

        void CheckForMissUp()
        {
            for (int i = 0; i < upNotes.Length; i++)
            {
                if (upNotes[i].Count > 0)
                {
                   
                    if (beatmapHandler.infiniteBeatmapTimer > upNotes[i][0].noteTime - (1000 - beatmapHandler.BPM_calc) + (HitWindow.miss * 2))
                    {
                   
                        Miss(PrecisionLevel.Miss, i, upNotes[i][0].shouldRewardPoints, false);
                       
                    }
                }
            }
        }

        void Miss(PrecisionLevel precisionLevel, int lane, bool shouldRewardPoints, bool down)
        {
            if (precisionLevel == PrecisionLevel.Miss)
            {
                if (down) DeleteDownNote(lane);
                else DeleteUpNote(lane);
            }
            if (shouldRewardPoints)
            {
                BreakCombo();
            }
        }

        public void KeyboardKeyDown(VKeys key)
        {

            if (!validKeys.Contains(key)) return;
            int lane = validKeys.IndexOf(key);
            if (downNotes[lane].Count <= 0 || !hasGoneUp[lane]) return;
            
            hasGoneUp[lane] = false;
            CheckingNote checkingNote = downNotes[lane][0];
            CheckingData checkingData = NoteChecking(checkingNote);


            if (checkingData.noteHit)
            {
                
                if (checkingNote.shouldRewardPoints)
                {
                    AddCombo();
                    GivePoints(checkingData.precisionLevel, laneObject.scoreText);
                }
                DeleteDownNote(lane);
            }
            else
            {
                Miss(checkingData.precisionLevel, lane, checkingNote.shouldRewardPoints, true);
            }
        }

       

        public void KeyboardKeyUp(VKeys key)
        {
            if (!validKeys.Contains(key)) return;
            int lane = validKeys.IndexOf(key);
            if (upNotes[lane].Count <= 0 || hasGoneUp[lane]) return;
            hasGoneUp[lane] = true;

            CheckingNote checkingNote = upNotes[lane][0];
            CheckingData checkingData = NoteChecking(checkingNote);

            if (checkingData.noteHit)
            {
                DeleteUpNote(lane);
                if (checkingNote.shouldRewardPoints)
                {
                    AddCombo();
                    GivePoints(checkingData.precisionLevel, laneObject.scoreText);
                }
            }
            else
            {
                Miss(checkingData.precisionLevel, lane, checkingNote.shouldRewardPoints, false);
            }
        }

        void AddCombo()
        {
            beatScore.AddCombo();
        }

        void BreakCombo()
        {
            beatScore.BreakCombo(laneObject.headerText);
        }

        void GivePoints(PrecisionLevel precisionLevel, EasyDraw easydraw)
        {
            beatScore.AddScore(precisionLevel, laneObject.scoreText, laneObject.headerText);
        }

        void DeleteDownNote(int lane)
        {
            
            downNotes[lane].RemoveAt(0);
        }

        void DeleteUpNote(int lane)
        {
            
            upNotes[lane].RemoveAt(0);
        }

        CheckingData NoteChecking(CheckingNote note)
        {
            return new CheckingData(note.noteTime, beatmapHandler.infiniteBeatmapTimer, beatmapHandler);
        }

        public void Dispose()
        {
            laneObject.keyboardHook.KeyDown -= new KeyboardHookCallback(KeyboardKeyDown);
            laneObject.keyboardHook.KeyUp -= new KeyboardHookCallback(KeyboardKeyUp);

            downNotes = null;
            upNotes = null;
        }
    }
}
