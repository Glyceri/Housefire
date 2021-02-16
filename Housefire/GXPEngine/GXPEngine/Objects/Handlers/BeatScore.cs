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
        public int biggestCombo = 0;
        public int score = 0;

        public void BreakCombo(EasyDraw headerText)
        {
            combo = 0;
            DrawCombo(headerText);
        }

        public void AddCombo()
        {
            combo++;
            if(combo > biggestCombo)
            {
                biggestCombo = combo;
            }
        }

        void DrawCombo(EasyDraw headerText)
        {
            headerText.Clear(Color.Transparent);
            headerText.TextAlign(CenterMode.Center, CenterMode.Center);
            headerText.TextSize(30);
            headerText.Text("Combo: " + combo, headerText.width / 2, headerText.height / 2);
        }

        void DrawScore(EasyDraw scoreText)
        {
            scoreText.Clear(Color.Transparent);
            scoreText.TextAlign(CenterMode.Center, CenterMode.Center);
            scoreText.TextSize(30);
            scoreText.Text("Score: " + score.ToString(), scoreText.width/2, scoreText.height/2);
        }

        public void AddScore(PrecisionLevel precisionLevel, EasyDraw scoreText, EasyDraw headerText)
        {
            float comboMulti = 1;
            if (combo >= 50)    comboMulti = 1.5f;
            if (combo >= 100)   comboMulti = 2f;
            if (combo >= 200)   comboMulti = 2.5f;

            int scoreValue = Mathf.Ceiling(scoreValues[precisionLevel] * comboMulti);
            score += scoreValue;

            DrawCombo(headerText);
            DrawScore(scoreText);



        }
    }
}
