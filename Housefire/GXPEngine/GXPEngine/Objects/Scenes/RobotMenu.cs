using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GXPEngine.Objects.Scenes
{
    public class RobotMenu : GameObject
    {
        public AnimationSprite robotOneIdle;
        public AnimationSprite robotTwoIdle;

        //public AnimationSprite robotOneAttack;
       //public AnimationSprite robotTwoAttack;

        //public AnimationSprite robotOneDamage;
        //public AnimationSprite robotTwoDamage;

        //public AnimationSprite robotOneLoss;
        //public AnimationSprite robotTwoLoss;



        public Pivot robotOneHolder;
        public Pivot robotTwoHolder;


        public RobotMenu()
        {
            robotOneHolder = new Pivot();
            robotTwoHolder = new Pivot();

            
            robotOneIdle = new AnimationSprite("SpriteSheets/Robot1/Radiohead_Idle.png", 3, 2, 6, false, false);
            robotOneHolder.AddChild(robotOneIdle);

            //robotOneAttack = new AnimationSprite("SpriteSheets/Robot1/Radiohead_Attack.png", 4, 3, 11, false, false);
            //robotOneHolder.AddChild(robotOneAttack);
            //robotOneAttack.scale = new Vector2(1.1f, 1.1f);


            //robotOneDamage = new AnimationSprite("SpriteSheets/Robot1/Radiohead_Damage.png", 5, 3, 13, false, false);
            //robotOneHolder.AddChild(robotOneDamage);


            robotTwoIdle = new AnimationSprite("SpriteSheets/Robot2/idle_spritesheet.png", 2, 2, 4, false, false);
            robotTwoHolder.AddChild(robotTwoIdle);

            //robotTwoAttack = new AnimationSprite("SpriteSheets/Robot2/shooting_spritesheet.png", 8, 3, 19, false, false);
            //robotTwoHolder.AddChild(robotTwoAttack);

            //robotTwoDamage = new AnimationSprite("SpriteSheets/Robot2/damage_spritesheet.png", 3, 2, 6, false, false);
            //robotTwoHolder.AddChild(robotTwoDamage);

            robotOneHolder.scale = new Vector2(0.375f, 0.375f);
            robotOneHolder.scale /= 1.75f;
            robotTwoHolder.scale /= 1.75f;
            robotOneHolder.SetXY(10-2000, 550);
            robotTwoHolder.SetXY(1520 - 2000, 550);
            AddChild(robotOneHolder);
            AddChild(robotTwoHolder);

            robotOneIdle.SetCycle(0, 6, 10, true);
        }

        float idleOneCounter;
        float attackOneCounter;
        float damageOneCounter;

        float idleTwoCounter;
        float attackTwoCounter;
        float damageTwoCounter;


        public float delta = 0;


        void Update()
        {
            robotOneHolder.SetXY(10 - (1 - delta) * 2000, 550);
            robotTwoHolder.SetXY(1520 + (1 - delta) * 2000, 550);

            idleOneCounter += Time.deltaTime;
            attackOneCounter += Time.deltaTime;
            damageOneCounter += Time.deltaTime;

            idleTwoCounter += Time.deltaTime;
            attackTwoCounter += Time.deltaTime;
            damageTwoCounter += Time.deltaTime;

            if (idleOneCounter >= 0.2f)
            {
                idleOneCounter = 0;
                robotOneIdle.NextFrame();
            }

            if(attackOneCounter >= 0.05f)
            {
                //robotOneAttack.NextFrame();
                attackOneCounter = 0;
            }

            if(damageOneCounter >= 0.08)
            {
                //robotOneDamage.NextFrame();
                damageOneCounter = 0;
            }

            if(idleTwoCounter >= 0.2f)
            {
                robotTwoIdle.NextFrame();
                idleTwoCounter = 0;
            }

            if(attackTwoCounter >= 0.08f)
            {
                //robotTwoAttack.NextFrame();
                attackTwoCounter = 0;
            }

            if(damageTwoCounter >= 0.1)
            {
                //robotTwoDamage.NextFrame();
                damageTwoCounter = 0;
            }
        }
    }
}
