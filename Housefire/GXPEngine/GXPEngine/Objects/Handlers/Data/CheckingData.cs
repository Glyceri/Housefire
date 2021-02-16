using GXPEngine.Objects.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GXPEngine.Objects.Handlers.Data
{
    public class CheckingData
    {
        public bool noteHit;
        public PrecisionLevel precisionLevel;

        public CheckingData(int noteTime, int hitTime, BeatmapHandler beatmapHandler)
        {
            //hitTime -= (int)(beatmapHandler.BPM_calc / 4) - (int)(beatmapHandler.activeBeatmap.offset/4);
            
            //hitTime -= (int)(1000 - beatmapHandler.BPM_calc);
            noteTime -= (int)(1000 - beatmapHandler.BPM_calc);

            noteTime += BeatmapHandler.beatOffset;
            precisionLevel = PrecisionLevel.UltraMiss;
            noteHit = HasHit(noteTime, hitTime);
        }

        bool HasHit(int noteTime, int hitTime)
        {
            if (noteTime + HitWindow.miss > hitTime && noteTime - HitWindow.miss < hitTime)
            {
                precisionLevel = PrecisionLevel.Miss;
                if (noteTime + HitWindow.just > hitTime && noteTime - HitWindow.just < hitTime)
                {
                    precisionLevel = PrecisionLevel.Just;
                    if (noteTime + HitWindow.okay > hitTime && noteTime - HitWindow.okay < hitTime)
                    {
                        precisionLevel = PrecisionLevel.Okay;
                        if (noteTime + HitWindow.good > hitTime && noteTime - HitWindow.good < hitTime)
                        {
                            precisionLevel = PrecisionLevel.Good;
                            if (noteTime + HitWindow.perfect > hitTime && noteTime - HitWindow.perfect < hitTime)
                            {
                                precisionLevel = PrecisionLevel.Perfect;
                            }
                            return true;
                        }
                        return true;
                    }
                    return true;
                }
                return false;
            }
            return false;
        }
    }
}
