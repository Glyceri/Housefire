using GXPEngine.Core;
using GXPEngine.Objects.Handlers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GXPEngine.Objects.Scenes
{
    public class MenuScores : GameObject
    {
        bool _canBeInteractedWith = true;
        public bool canBeInteractedWith { get => _canBeInteractedWith && visible; set { _canBeInteractedWith = value; } }


        EasyDraw player1Panel;
        EasyDraw player2Panel;

        EasyDraw headerPlayer1;
        EasyDraw headerPlayer2;

        BeatmapButtonEasyDraw continueButton;

        public MenuScores()
        {
            using (Bitmap bitmap = new Bitmap("bigbar.png"))
            {
                player1Panel = new EasyDraw(500, 700, false);
                player1Panel.DrawSprite(bitmap, new Vector2(500 / (float)bitmap.Width, 700 / (float)bitmap.Height));
                player1Panel.SetXY(240, 150);
                player1Panel.collider = new BoxCollider(Vector2.Zero, new Vector2(player1Panel.width, player1Panel.height), player1Panel);
                AddChild(player1Panel);

                player2Panel = new EasyDraw(500, 700, false);
                player2Panel.DrawSprite(bitmap, new Vector2(-(500 / (float)bitmap.Width), 700 / (float)bitmap.Height), 2);
                player2Panel.SetXY(1180, 150);
                player2Panel.collider = new BoxCollider(Vector2.Zero, new Vector2(player2Panel.width, player2Panel.height), player2Panel);
                AddChild(player2Panel);
            }
            using (Bitmap bitmap = new Bitmap("colourfulthing.png"))
            {
                headerPlayer1 = new EasyDraw(450, 100, false);
                headerPlayer1.DrawSprite(bitmap, new Vector2(headerPlayer1.width / (float)bitmap.Width, headerPlayer1.height / (float)bitmap.Height));
                headerPlayer1.SetXY(-30, -30);
                player1Panel.AddChild(headerPlayer1);

                headerPlayer2 = new EasyDraw(450, 100, false);
                headerPlayer2.DrawSprite(bitmap, new Vector2((headerPlayer2.width / (float)bitmap.Width), headerPlayer2.height / (float)bitmap.Height));
                headerPlayer2.SetXY(-30, -30);
                player2Panel.AddChild(headerPlayer2);
            }

            ContinueButton();
        }


        void ContinueButton()
        {
            using (Bitmap bitmap = new Bitmap("songtitlebar.png"))
            {
                continueButton = new BeatmapButtonEasyDraw(500, 100, false);
                continueButton.Mirror(true, false);
                continueButton.DrawSprite(bitmap, new Vector2(-(continueButton.width / (float)bitmap.Width), continueButton.height / (float)bitmap.Height), continueButton.width / (float)bitmap.Width * 2, 0, false);
                continueButton.SetXY(1920 - continueButton.width, 1080 - continueButton.height);

                continueButton.Mirror(false, false);
                continueButton.TextAlign(CenterMode.Center, CenterMode.Center);
                continueButton.TextSize(30);
                continueButton.Text("Continue", 250, 50);
                continueButton.collider = new BoxCollider(Vector2.Zero, new Vector2(500, 100), continueButton);
                continueButton.collider.isTrigger = true;

                AddChild(continueButton);
            }
        }


        public void SetScores(BeatmapSmall beatmap, BeatScore scorePlayer1, BeatScore scorePlayer2)
        {
            MyGame.Instance.highscorehandler.WriteScore(beatmap.internalName, MyGame.Instance.player1.name, scorePlayer1.score, scorePlayer1.biggestCombo);
            MyGame.Instance.highscorehandler.WriteScore(beatmap.internalName, MyGame.Instance.player2.name, scorePlayer2.score, scorePlayer2.biggestCombo);


        }


        bool animation = false;
        public float delta = 0;
        float animationSpeed = 2;

        void Update()
        {
            if (canBeInteractedWith)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (continueButton.collider.HitTestPoint(Input.mouseX, Input.mouseY, false))
                    {
                        animation = true;
                        delta = 0;
                        canBeInteractedWith = false;
                    }
                }
            }

            if (animation)
            {
                delta += Time.deltaTime * animationSpeed;

                player1Panel.SetXY(240 - (delta * 2000), 150);
                player2Panel.SetXY(1180 + (delta * 2000), 150);
                MyGame.Instance.menuScreen.menuScreen.globalOffset = 1 - delta;
                MyGame.Instance.menuScreen.menuScreen.menuSongInfo.startButtonPanel.SetXY(510, 210 - ((1-delta) * 1000));
                MyGame.Instance.menuScreen.menuScreen.menuSongInfo.highscorePanel.SetXY(70 - ((1 - delta) * 1000), 325);
                MyGame.Instance.menuScreen.menuScreen.menuSongInfo.songInfoPanel.SetXY(510, 370 + ((1 - delta) * 1700));
                MyGame.Instance.livesMenu.delta = 1 - delta;
                if (delta >= 1)
                {
                    delta = 0;
                    animation = false;
                    canBeInteractedWith = false;
                    visible = false;

                    player1Panel.SetXY(240 , 150);
                    player2Panel.SetXY(1180 , 150);
                    MyGame.Instance.menuScreen.menuScreen.globalOffset = 0;
                    MyGame.Instance.OnPlayerSelectEnd();
                    MyGame.Instance.menuScreen.menuScreen.menuSongInfo.startButtonPanel.SetXY(510, 210);
                    MyGame.Instance.menuScreen.menuScreen.menuSongInfo.highscorePanel.SetXY(70, 325);
                    MyGame.Instance.menuScreen.menuScreen.menuSongInfo.songInfoPanel.SetXY(510, 370 );
                }
            }

            


        }
    }
}
