using GXPEngine.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GXPEngine.Objects.Scenes
{
    public class LivesMenu : GameObject
    {

        EasyDraw livesPanel;
        EasyDraw livesPanelText;

        EasyDraw roundsPanel;
        EasyDraw roundsPanelText;

        EasyDraw namePlate1;
        EasyDraw namePlate2;

        public LivesMenu()
        {
            using (Bitmap bitmap = new Bitmap("round2.png"))
            {
                livesPanel = new EasyDraw(500, 200, false);
                livesPanel.DrawSprite(bitmap, new Vector2(500 / (float)bitmap.Width, 200 / (float)bitmap.Height));
                livesPanel.collider = new BoxCollider(Vector2.Zero, new Vector2(500, 200), livesPanel);
                livesPanel.SetXY(600, -40);
                AddChild(livesPanel);

                livesPanelText = new EasyDraw(500, 200, false);
                livesPanelText.collider = new BoxCollider(Vector2.Zero, new Vector2(500, 200), livesPanelText);
                livesPanelText.Clear(Color.Transparent);
                livesPanelText.SetXY(0, 50);
                livesPanel.AddChild(livesPanelText);

                roundsPanel = new EasyDraw(400, 170, false);
                roundsPanel.DrawSprite(bitmap, new Vector2(roundsPanel.width / (float)bitmap.Width, roundsPanel.height / (float)bitmap.Height));
                roundsPanel.collider = new BoxCollider(Vector2.Zero, new Vector2(roundsPanel.width, roundsPanel.height), roundsPanel);
                roundsPanel.SetXY(45, 70);
                AddChild(roundsPanel);

                roundsPanelText = new EasyDraw(400, 170, false);
                roundsPanelText.collider = new BoxCollider(Vector2.Zero, new Vector2(roundsPanelText.width, roundsPanelText.height), roundsPanel);
                roundsPanelText.Clear(Color.Transparent);
                roundsPanel.AddChild(roundsPanelText);

                namePlate1 = new EasyDraw(250, 100, false);
                namePlate1.DrawSprite(bitmap, new Vector2(250 / (float)bitmap.Width, 100 / (float)bitmap.Height));
                namePlate1.SetXY(0, 140);
                livesPanel.AddChild(namePlate1);

                namePlate2 = new EasyDraw(250, 100, false);
                namePlate2.DrawSprite(bitmap, new Vector2(250 / (float)bitmap.Width, 100 / (float)bitmap.Height));
                namePlate2.SetXY(250, 140);
                livesPanel.AddChild(namePlate2);
            }
        }


        public float delta = 0;

        void Update()
        {
            livesPanel.SetXY(600, -40 - (1000 * delta));
            roundsPanel.SetXY(45 - (1000 * delta), 70);
        }


        public void SetLives()
        {

            //livesPanelText.Clear(Color.Transparent);
            namePlate1.Clear(Color.Transparent);
            namePlate2.Clear(Color.Transparent);
            livesPanelText.Clear(Color.Transparent);
            using (Bitmap bitmap = new Bitmap("round2.png"))
            {
                namePlate1.DrawSprite(bitmap, new Vector2(250 / (float)bitmap.Width, 100 / (float)bitmap.Height));
                namePlate2.DrawSprite(bitmap, new Vector2(250 / (float)bitmap.Width, 100 / (float)bitmap.Height));
            }
            using (Bitmap bitmap = new Bitmap("UI/Healthbar.png"))
            {
                livesPanelText.DrawSprite(bitmap, new Vector2(240 / (float)bitmap.Width, 150 / (float)bitmap.Height), 0.2f, -0.1f);
                livesPanelText.DrawSprite(bitmap, new Vector2(-(240 / (float)bitmap.Width), 150 / (float)bitmap.Height), 2f, -0.1f);
            }
            using (Bitmap bitmap = new Bitmap("UI/Health/1hp.png"))
            {
                if (MyGame.Instance.player1.health == 1) livesPanelText.DrawSprite(bitmap, new Vector2(240 / (float)bitmap.Width, 150 / (float)bitmap.Height), 0.2f, -0.1f);
                if (MyGame.Instance.player2.health == 1) livesPanelText.DrawSprite(bitmap, new Vector2(-(240 / (float)bitmap.Width), 150 / (float)bitmap.Height), 2f, -0.1f);
            }
            using (Bitmap bitmap = new Bitmap("UI/Health/2Hp.png"))
            {
                if (MyGame.Instance.player1.health == 2) livesPanelText.DrawSprite(bitmap, new Vector2(240 / (float)bitmap.Width, 150 / (float)bitmap.Height), 0.2f, -0.1f);
                if (MyGame.Instance.player2.health == 2) livesPanelText.DrawSprite(bitmap, new Vector2(-(240 / (float)bitmap.Width), 150 / (float)bitmap.Height), 2f, -0.1f);
            }
            using (Bitmap bitmap = new Bitmap("UI/Health/Full Hp.png"))
            {
                if (MyGame.Instance.player1.health == 3) livesPanelText.DrawSprite(bitmap, new Vector2(240 / (float)bitmap.Width, 150 / (float)bitmap.Height), 0.2f, -0.1f);
                if (MyGame.Instance.player2.health == 3) livesPanelText.DrawSprite(bitmap, new Vector2(-(240 / (float)bitmap.Width), 150 / (float)bitmap.Height), 2f, -0.1f);
            }

            namePlate1.TextSize(20);
            namePlate1.TextAlign(CenterMode.Center, CenterMode.Center);
            namePlate2.TextSize(20);
            namePlate2.TextAlign(CenterMode.Center, CenterMode.Center);
            namePlate1.Text(MyGame.Instance.player1.name, namePlate1.width / (float)2, namePlate1.height/(float)2);
            namePlate2.Text(MyGame.Instance.player2.name, namePlate2.width / (float)2 , namePlate2.height / (float)2);
        }

        public void SetRoundsRemaining()
        {
            roundsPanelText.Clear(Color.Transparent);
            roundsPanelText.TextSize(22);
            roundsPanelText.TextAlign(CenterMode.Center, CenterMode.Center);
            roundsPanelText.Text(MyGame.roundsRemaining != 1 ? MyGame.roundsRemaining.ToString() + " Rounds Remaining" : "1 Round Remaining", roundsPanelText.width / 2, roundsPanelText.height / 2);
        }
    }
}
