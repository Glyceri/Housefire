using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GXPEngine.Objects.Scenes
{
    public class MenuScoresRobot : GameObject
    {
        public AnimationSprite robotOneAttack;
        public AnimationSprite robotTwoAttack;

        public AnimationSprite robotOneDamage;
        public AnimationSprite robotTwoDamage;

        public AnimationSprite robotOneLoss;
        public AnimationSprite robotTwoLoss;

        public AnimationSprite robotOneWin;
        public AnimationSprite robotTwoWin;

        public Pivot robotOneHolder;
        public Pivot robotTwoHolder;
        public MenuScoresRobot()
        {
            robotOneHolder = new Pivot();
            robotTwoHolder = new Pivot();

            robotOneAttack = new AnimationSprite("SpriteSheets/Robot1/Radiohead_Attack.png", 4, 3, 11, false, false);
            robotOneHolder.AddChild(robotOneAttack);
            robotOneAttack.scale = new Vector2(1.1f, 1.1f);

            robotOneLoss = new AnimationSprite("SpriteSheets/Robot1/Radiohead_Loss_.png", 4, 1, 4, false, false);
            robotOneHolder.AddChild(robotOneLoss);

            robotOneWin = new AnimationSprite("SpriteSheets/Robot1/Radiohead_Win.png", 4, 2, 8, false, false);
            robotOneHolder.AddChild(robotOneWin);



            robotOneDamage = new AnimationSprite("SpriteSheets/Robot1/Radiohead_Damage.png", 5, 3, 13, false, false);
            robotOneHolder.AddChild(robotOneDamage);

            robotTwoAttack = new AnimationSprite("SpriteSheets/Robot2/shooting_spritesheet.png", 8, 3, 19, false, false);
            robotTwoHolder.AddChild(robotTwoAttack);

            robotTwoDamage = new AnimationSprite("SpriteSheets/Robot2/damage_spritesheet.png", 3, 2, 6, false, false);
            robotTwoHolder.AddChild(robotTwoDamage);

            robotTwoLoss = new AnimationSprite("SpriteSheets/Robot2/fail_spritesheet.png", 3, 2, 5, false, false);
            robotTwoHolder.AddChild(robotTwoLoss);

            robotTwoWin = new AnimationSprite("SpriteSheets/Robot2/dance_spritesheet.png", 4, 2, 8, false, false);
            robotTwoHolder.AddChild(robotTwoWin);



            robotOneHolder.scale = new Vector2(0.375f, 0.375f);
            robotOneHolder.scale /= 1.45f;
            robotTwoHolder.scale /= 1.45f;
            robotOneHolder.SetXY(230, 260);
            robotTwoHolder.SetXY(1260, 260);
            AddChild(robotOneHolder);
            AddChild(robotTwoHolder);

            Reset();
        }


        public void Reset()
        {
            robotOneAttack.visible = false;
            robotTwoAttack.visible = false;

            robotOneDamage.visible = false;
            robotTwoDamage.visible = false;

            robotOneLoss.visible = false;
            robotTwoLoss.visible = false;

            robotOneWin.visible = false;
            robotTwoWin.visible = false;
        }


        float attackOneCounter;
        float damageOneCounter;

        float attackTwoCounter;
        float damageTwoCounter;

        float lossOneCounter;
        float lossTwoCounter;

        float winOneCounter;
        float winTwoCounter;

        void Update()
        {
            attackOneCounter += Time.deltaTime;
            damageOneCounter += Time.deltaTime;

            attackTwoCounter += Time.deltaTime;
            damageTwoCounter += Time.deltaTime;

            lossOneCounter += Time.deltaTime;
            lossTwoCounter += Time.deltaTime;

            winOneCounter += Time.deltaTime;
            winTwoCounter += Time.deltaTime;

            if (attackOneCounter >= 0.05f)
            {
                robotOneAttack.NextFrame();
                attackOneCounter = 0;
            }

            if (damageOneCounter >= 0.08)
            {
                robotOneDamage.NextFrame();
                damageOneCounter = 0;
            }
            
            if (attackTwoCounter >= 0.08f)
            {
                robotTwoAttack.NextFrame();
                attackTwoCounter = 0;
            }

            if (damageTwoCounter >= 0.1)
            {
                robotTwoDamage.NextFrame();
                damageTwoCounter = 0;
            }

            if(lossOneCounter >= 0.15f)
            {
                robotOneLoss.NextFrame();
                lossOneCounter = 0;
            }

            if (lossTwoCounter >= 0.15f)
            {
                robotTwoLoss.NextFrame();
                lossTwoCounter = 0;
            }

            if (winOneCounter >= 0.15f)
            {
                robotOneWin.NextFrame();
                winOneCounter = 0;
            }

            if (winTwoCounter >= 0.15f)
            {
                robotTwoWin.NextFrame();
                winTwoCounter = 0;
            }
        }
    }
}
