using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GXPEngine.Objects.Handlers
{
    public class BeatmapScoringNew
    {
        BeatmapHandler beatmapHandler;
        LaneObject laneObject;

        Player player;

        public BeatmapScoringNew(BeatmapHandler beatmapHandler, LaneObject laneObject, Player player)
        {
            this.beatmapHandler = beatmapHandler;
            this.laneObject = laneObject;
            this.player = player;
        }
    }
}
