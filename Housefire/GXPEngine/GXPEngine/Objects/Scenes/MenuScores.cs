﻿using GXPEngine.Core;
using GXPEngine.Core.Audio;
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


        // EasyDraw player1Panel;
        // EasyDraw player2Panel;

        // EasyDraw headerPlayer1;
        // EasyDraw headerPlayer2;

        BeatmapButtonEasyDraw continueButton;


        public MenuScoresRobot robotMenu;

        public EasyDraw winBackPanel;
        public EasyDraw winText;

        public MenuScores()
        {

            AddChild(robotMenu = new MenuScoresRobot());

            using (Bitmap bitmap = new Bitmap("extrathings/combobaracctivated.png"))
            {
                winBackPanel = new EasyDraw(800, 200, false);
                winBackPanel.DrawSprite(bitmap, new Vector2(winBackPanel.width / (float)bitmap.Width, winBackPanel.height / (float)bitmap.Height));
                winBackPanel.SetXY((1920 - winBackPanel.width) / 2, 50);
                AddChild(winBackPanel);
                winText = new EasyDraw(winBackPanel.width, winBackPanel.height, false);
                winBackPanel.AddChild(winText);

            }
            /*
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
            }*/



            ContinueButton();
        }

        public void SetWinText(string text)
        {

            winText.Clear(Color.Transparent);
            winText.TextAlign(CenterMode.Center, CenterMode.Center);
            winText.TextSize(40);
            winText.Text(text, winText.width / 2, winText.height / 2);

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
            if (scorePlayer1.score > scorePlayer2.score)
            {
                SetWinText(MyGame.Instance.player1.name + " WINS");
                playerOneWin = true;
            }
            else if (scorePlayer1.score < scorePlayer2.score)
            {
                SetWinText(MyGame.Instance.player2.name + " WINS");
                playerTwoWin = true;
            }
            else
            {
                SetWinText("DRAW");
                playerDraw = true;
            }

        }

        public void PlayPlayerOneWinAnimation()
        {
            playerWinTimer += Time.deltaTime;
            if (playerWinTimer <= 1.2f)
            {
                robotMenu.robotOneAttack.visible = true;
                robotMenu.robotTwoAttack.visible = false;

                robotMenu.robotOneDamage.visible = false;
                robotMenu.robotTwoDamage.visible = true;

                robotMenu.robotOneLoss.visible = false;
                robotMenu.robotTwoLoss.visible = false;

                robotMenu.robotOneWin.visible = false;
                robotMenu.robotTwoWin.visible = false;

            }
            else
            {
                robotMenu.robotOneAttack.visible = false;
                robotMenu.robotTwoDamage.visible = false;

                robotMenu.robotTwoLoss.visible = true;
                robotMenu.robotOneWin.visible = true;
            }
        }

        public void PlayPlayerTwoWinAnimation()
        {
            playerWinTimer += Time.deltaTime;
            if (playerWinTimer <= 1.2f)
            {
                robotMenu.robotOneAttack.visible = false;
                robotMenu.robotTwoAttack.visible = true;

                robotMenu.robotOneDamage.visible = true;
                robotMenu.robotTwoDamage.visible = false;

                robotMenu.robotOneLoss.visible = false;
                robotMenu.robotTwoLoss.visible = false;

                robotMenu.robotOneWin.visible = false;
                robotMenu.robotTwoWin.visible = false;

            }
            else
            {
                robotMenu.robotTwoAttack.visible = false;
                robotMenu.robotOneDamage.visible = false;

                robotMenu.robotOneLoss.visible = true;
                robotMenu.robotTwoWin.visible = true;
            }
        }

        public void PlayDrawAnimation()
        {
            playerWinTimer += Time.deltaTime;
            if (playerWinTimer <= 1.2f)
            {
                robotMenu.robotOneAttack.visible = false;
                robotMenu.robotTwoAttack.visible = false;

                robotMenu.robotOneDamage.visible = true;
                robotMenu.robotTwoDamage.visible = true;

                robotMenu.robotOneLoss.visible = false;
                robotMenu.robotTwoLoss.visible = false;

                robotMenu.robotOneWin.visible = false;
                robotMenu.robotTwoWin.visible = false;

            }
            else
            {
                robotMenu.robotTwoDamage.visible = false;
                robotMenu.robotOneDamage.visible = false;

                robotMenu.robotOneLoss.visible = true;
                robotMenu.robotTwoLoss.visible = true;
            }
        }


        float playerWinTimer = 0;

        bool playerOneWin = false;
        bool playerTwoWin = false;
        bool playerDraw = false;



        bool animation = false;
        public float delta = 0;
        float animationSpeed = 2;

        void Update()
        {

            if (playerOneWin)
            {
                PlayPlayerOneWinAnimation();
            }
            if (playerTwoWin)
            {
                PlayPlayerTwoWinAnimation();
            }
            if (playerDraw)
            {
                PlayDrawAnimation();
            }

            if (canBeInteractedWith)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (continueButton.collider.HitTestPoint(Input.mouseX, Input.mouseY, false))
                    {
                        animation = true;
                        delta = 0;
                        canBeInteractedWith = false;
                        new Sound("soft-hitnormal.wav")?.Play(false, 0, AudioHandler.volume / (float)100);
                    }
                }
            }

            if (animation)
            {
                delta += Time.deltaTime * animationSpeed;

                MyGame.Instance.menuScreen.menuScreen.globalOffset = 1 - delta;
                MyGame.Instance.menuScreen.menuScreen.menuSongInfo.startButtonPanel.SetXY(510, 210 - ((1 - delta) * 1000));
                MyGame.Instance.menuScreen.menuScreen.menuSongInfo.highscorePanel.SetXY(70 - ((1 - delta) * 1000), 325);
                MyGame.Instance.menuScreen.menuScreen.menuSongInfo.songInfoPanel.SetXY(510, 370 + ((1 - delta) * 1700));
                MyGame.Instance.livesMenu.delta = 1 - delta;

                robotMenu.robotOneHolder.SetXY(230 - (delta) * 1000, 260);
                robotMenu.robotTwoHolder.SetXY(1260 + (delta) * 1000, 260);
                winBackPanel.SetXY((1920 - winBackPanel.width) / 2, 50 - (delta * 1000));

                if (delta >= 1)
                {
                    playerOneWin = false;
                    playerTwoWin = false;
                    playerDraw = false;
                    playerWinTimer = 0;
                    winText.Clear(Color.Transparent);
                    

                    delta = 0;
                    animation = false;
                    canBeInteractedWith = false;
                    visible = false;

                    robotMenu.Reset();

                    MyGame.Instance.menuScreen.menuScreen.globalOffset = 0;
                    MyGame.Instance.OnPlayerSelectEnd();
                    MyGame.Instance.menuScreen.menuScreen.menuSongInfo.startButtonPanel.SetXY(510, 210);
                    MyGame.Instance.menuScreen.menuScreen.menuSongInfo.highscorePanel.SetXY(70, 325);
                    MyGame.Instance.menuScreen.menuScreen.menuSongInfo.songInfoPanel.SetXY(510, 370);
                    
                    winBackPanel.SetXY((1920 - winBackPanel.width) / 2, 50);
                }
            }
        }
    }
}
