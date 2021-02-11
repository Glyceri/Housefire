using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GXPEngine.Objects.Handlers
{

    public class BeatScore
    {
        public static Dictionary<PrecisionLevel, int> scoreValues = new Dictionary<PrecisionLevel, int>() {
            {PrecisionLevel.UltraMiss, 0 }  ,
            {PrecisionLevel.Miss, 0 }       ,
            {PrecisionLevel.Just, 50 }      ,
            {PrecisionLevel.Okay, 100 }     ,
            {PrecisionLevel.Good, 200 }     ,
            {PrecisionLevel.Perfect, 200}   };

        public int combo = 0;
        public int score = 0;

        public void BreakCombo(EasyDraw easyDraw)
        {
            combo = 0;
            easyDraw.Clear(Color.Red);
            easyDraw.Text("MISS", 0, 0);
        }
        public void AddCombo()
        {
            combo++;
        }

        public void AddScore(PrecisionLevel precisionLevel, EasyDraw easyDraw)
        {
            float comboMulti = 1;
            if (combo >= 50)    comboMulti = 1.5f;
            if (combo >= 100)   comboMulti = 2f;
            if (combo >= 200)   comboMulti = 2.5f;

            int scoreValue = Mathf.Ceiling(scoreValues[precisionLevel] * comboMulti);
            score += scoreValue;

            easyDraw.Clear(Color.Red);
            easyDraw.Text(scoreValue.ToString() + " : " + score + "\n combo: " + combo.ToString() , 0, 0);

        }
    }
}
