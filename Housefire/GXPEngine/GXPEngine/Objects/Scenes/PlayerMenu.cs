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

    enum MenuState
    {
        PlayerSelect,
        Accepting,
        NameChange,
        KeybindChange
    }

    public class PlayerMenu : GameObject
    {
        bool _canBeInteractedWith = false;
        public bool canBeInteractedWith { get => _canBeInteractedWith && visible; set { _canBeInteractedWith = value; } }

        BeatmapButtonEasyDraw onePlayerButton;
        AnimationSprite animationSpritePlayerOne;
        AnimationSprite animationSpritePlayerTwo;
        BeatmapButtonEasyDraw twoPlayersButton;

        BeatmapButtonEasyDraw textPlayerOneBackDrop;
        BeatmapButtonEasyDraw textPlayerTwoBackDrop;
        BeatmapButtonEasyDraw textplayeroneEasyDraw;
        BeatmapButtonEasyDraw textplayerTwoEasyDraw;

        BeatmapButtonEasyDraw continueButton;

        BeatmapButtonEasyDraw background;

        public delegate void OnAnimationEnd();
        public event OnAnimationEnd onAnimationEnd = null;

        float counter = 0;

        public PlayerMenu()
        {
            using (Bitmap bitmap = new Bitmap("backgroundpanel.png"))
            {
                background = new BeatmapButtonEasyDraw(1920, 1080, false);
                background.DrawSprite(bitmap, new Vector2(1920/ (float)bitmap.Width, 1080/(float)bitmap.Height), 0, 0, false);
                background.alpha = 1;
                AddChild(background);
            }

            using (Bitmap bitmap = new Bitmap("bigbar.png"))
            {

                OnePlayerBar(bitmap);

                animationSpritePlayerOne = new AnimationSprite("SpriteSheets/Robot1/Radiohead_Idle.png", 3, 2, -1, false, false);
                animationSpritePlayerOne.scale = new Vector2(0.2f, 0.2f);
                animationSpritePlayerOne.SetXY((onePlayerButton.width - (animationSpritePlayerOne.width / (float)2)) / (float)4, 0);

                TextElementPlayerOne();

                TwoPlayerBar(bitmap);

                animationSpritePlayerTwo = new AnimationSprite("SpriteSheets/Robot2/idle_spritesheet.png", 2, 2, -1, false, false);
                animationSpritePlayerTwo.scale = new Vector2(0.55f, 0.55f);
                animationSpritePlayerTwo.SetXY((onePlayerButton.width - (animationSpritePlayerTwo.width / (float)2)) / (float)4, 0);

                twoPlayersButton.AddChild(animationSpritePlayerTwo);

                TextElementPlayerTwo();

                onePlayerButton.collider = new BoxCollider(Vector2.Zero, new Vector2(600, 800), onePlayerButton);
                onePlayerButton.collider.isTrigger = true;
                twoPlayersButton.collider = new BoxCollider(Vector2.Zero, new Vector2(600, 800), twoPlayersButton);
                twoPlayersButton.collider.isTrigger = true;
            }

            ContinueButton();
        }

        void OnePlayerBar(Bitmap bitmap)
        {
            onePlayerButton = new BeatmapButtonEasyDraw(600, 800, false);
            onePlayerButton.DrawSprite(bitmap, new Vector2(onePlayerButton.width / (float)bitmap.Width, onePlayerButton.height / (float)bitmap.Height), 0, 0, false);
            onePlayerButton.alpha = 1f;
            onePlayerButton.TextAlign(CenterMode.Center, CenterMode.Center);
            onePlayerButton.TextSize(30);
            onePlayerButton.Text("Controls: \n D F J K", 300, 500);

            onePlayerButton.SetXY(160, 140);
            AddChild(onePlayerButton);
        }

        void TwoPlayerBar(Bitmap bitmap)
        {
            onePlayerButton.AddChild(animationSpritePlayerOne);

            twoPlayersButton = new BeatmapButtonEasyDraw(600, 800, false);
            twoPlayersButton.DrawSprite(bitmap, new Vector2(-(twoPlayersButton.width / (float)bitmap.Width), twoPlayersButton.height / (float)bitmap.Height), (twoPlayersButton.width / (float)bitmap.Width) * 2, 0, false);
            twoPlayersButton.alpha = 1f;
            twoPlayersButton.TextAlign(CenterMode.Center, CenterMode.Center);
            twoPlayersButton.TextSize(30);
            twoPlayersButton.Text("            Controls: \n LEFT UP DOWN RIGHT", 300, 500);

            twoPlayersButton.SetXY(1920 - 160 - 600, 140);
            AddChild(twoPlayersButton);


        }

        void TextElementPlayerOne()
        {
            using (Bitmap bitmap = new Bitmap("notsellectedsong.png"))
            {
                textPlayerOneBackDrop = new BeatmapButtonEasyDraw(onePlayerButton.width - 100, 100, false);
                textPlayerOneBackDrop.DrawSprite(bitmap, new Vector2(textPlayerOneBackDrop.width / (float)bitmap.Width, textPlayerOneBackDrop.height / (float)bitmap.Height));
                textPlayerOneBackDrop.SetXY(((onePlayerButton.width - textPlayerOneBackDrop.width) / (float)2), 650);
                textPlayerOneBackDrop.collider = new BoxCollider(Vector2.Zero, new Vector2(textPlayerOneBackDrop.width, textPlayerOneBackDrop.height), textPlayerOneBackDrop);
                textPlayerOneBackDrop.collider.isTrigger = true;

                textplayeroneEasyDraw = new BeatmapButtonEasyDraw(onePlayerButton.width - 100, 100, false);
                textplayeroneEasyDraw.TextAlign(CenterMode.Center, CenterMode.Center);
                textplayeroneEasyDraw.TextSize(30);
                textplayeroneEasyDraw.Text("Player 1", textplayeroneEasyDraw.width / 2, textplayeroneEasyDraw.height / 2);

                textPlayerOneBackDrop.AddChild(textplayeroneEasyDraw);
                onePlayerButton.AddChild(textPlayerOneBackDrop);
            }
        }

        void TextElementPlayerTwo()
        {
            using (Bitmap bitmap = new Bitmap("notsellectedsong.png"))
            {
                textPlayerTwoBackDrop = new BeatmapButtonEasyDraw(twoPlayersButton.width - 100, 100, false);
                textPlayerTwoBackDrop.DrawSprite(bitmap, new Vector2(textPlayerTwoBackDrop.width / (float)bitmap.Width, textPlayerTwoBackDrop.height / (float)bitmap.Height));
                textPlayerTwoBackDrop.SetXY(((onePlayerButton.width - textPlayerTwoBackDrop.width) / (float)2), 650);
                textPlayerTwoBackDrop.collider = new BoxCollider(Vector2.Zero, new Vector2(textPlayerTwoBackDrop.width, textPlayerTwoBackDrop.height), textPlayerTwoBackDrop);
                textPlayerTwoBackDrop.collider.isTrigger = true;

                textplayerTwoEasyDraw = new BeatmapButtonEasyDraw(twoPlayersButton.width - 100, 100, false);
                textplayerTwoEasyDraw.TextAlign(CenterMode.Center, CenterMode.Center);
                textplayerTwoEasyDraw.TextSize(30);
                textplayerTwoEasyDraw.Text("Player 2", textplayerTwoEasyDraw.width / 2, textplayerTwoEasyDraw.height / 2);

                textPlayerTwoBackDrop.AddChild(textplayerTwoEasyDraw);
                twoPlayersButton.AddChild(textPlayerTwoBackDrop);
            }
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
                //continueButton.Mirror(true, false);

                AddChild(continueButton);
            }
        }

        void Deselect()
        {
            inEditMode = false;
            playerEdited = 0;
            textplayeroneEasyDraw.Clear(Color.Transparent);
            textplayerTwoEasyDraw.Clear(Color.Transparent);

            textplayeroneEasyDraw.Text(MyGame.Instance.player1.name, textplayeroneEasyDraw.width / 2, textplayeroneEasyDraw.height / 2);

            textplayerTwoEasyDraw.Text(MyGame.Instance.player2.name, textplayerTwoEasyDraw.width / 2, textplayerTwoEasyDraw.height / 2);
        }


        public bool inEndingAnimation = false;
        float animationTimer = 0;
        float animationSpeed = 2;

        string spawnText = "";
        public bool inEditMode = false;
        int playerEdited = 0;

        void Update()
        {
            if (canBeInteractedWith)
            {
                CheckEditMode();
                if (Input.GetMouseButtonDown(0))
                {
                    if (continueButton.collider.HitTestPoint(Input.mouseX, Input.mouseY, false))
                    {
                        inEndingAnimation = true;
                        canBeInteractedWith = false;
                    }
                }
            }
            else if(!canBeInteractedWith && inEditMode)
            {
                Deselect();
            }
            EditMode();

            counter += Time.deltaTime;
            if (counter >= 0.1f)
            {
                counter = 0;
                animationSpritePlayerOne?.NextFrame();
                animationSpritePlayerTwo?.NextFrame();
            }

           

            if (inEndingAnimation)
            {
                animationTimer += (Time.deltaTime * animationSpeed);
                onePlayerButton.SetXY(160 - (2000 * animationTimer), 140);
                twoPlayersButton.SetXY(1920 - 160 - 600 + (2000 * animationTimer), 140);
                continueButton.SetXY(1920 - continueButton.width, 1080 - continueButton.height + (500 * animationTimer));
                background.alpha = 1 - animationTimer;
                if (animationTimer >= 1)
                {
                    animationTimer = 0;
                    onePlayerButton.SetXY(160, 140);
                    twoPlayersButton.SetXY(1920 - 160 - 600, 140);
                    continueButton.SetXY(1920 - continueButton.width, 1080 - continueButton.height);
                    inEndingAnimation = false;
                    background.alpha = 1;
                    onAnimationEnd?.Invoke();
                }
            }
        }

        void EditMode()
        {
            if (inEditMode)
            {
                EditSpawnText();
                if (playerEdited == 1)
                {
                    textplayeroneEasyDraw.Clear(Color.Transparent);
                    textplayeroneEasyDraw.Text(spawnText + GetTicker(), textplayeroneEasyDraw.width / 2, textplayeroneEasyDraw.height / 2);
                    if (Input.GetKeyDown(Key.ENTER))
                    {
                        MyGame.Instance.player1.name = spawnText;
                        Deselect();
                    }
                }
                else
                {
                    textplayerTwoEasyDraw.Clear(Color.Transparent);
                    textplayerTwoEasyDraw.Text(spawnText + GetTicker(), textplayerTwoEasyDraw.width / 2, textplayerTwoEasyDraw.height / 2);
                    if (Input.GetKeyDown(Key.ENTER))
                    {
                        MyGame.Instance.player2.name = spawnText;
                        Deselect();
                    }
                }
            }
        }

        void CheckEditMode()
        {
            if (Input.GetMouseButtonDown(0))
            {

                bool hit = false;
                if (textPlayerOneBackDrop.collider.HitTestPoint(Input.mouseX, Input.mouseY, false))
                {
                    hit = true;
                    if (playerEdited != 1)
                    {
                        playerEdited = 1;
                        spawnText = MyGame.Instance.player1.name;
                        inEditMode = true;
                        textplayerTwoEasyDraw.Clear(Color.Transparent);
                        textplayerTwoEasyDraw.Text(MyGame.Instance.player2.name, textplayerTwoEasyDraw.width / 2, textplayerTwoEasyDraw.height / 2);
                    }

                }
                if (textPlayerTwoBackDrop.collider.HitTestPoint(Input.mouseX, Input.mouseY, false))
                {
                    hit = true;
                    if (playerEdited != 2)
                    {
                        playerEdited = 2;
                        spawnText = MyGame.Instance.player2.name;
                        inEditMode = true;
                        textplayeroneEasyDraw.Clear(Color.Transparent);
                        textplayeroneEasyDraw.Text(MyGame.Instance.player1.name, textplayeroneEasyDraw.width / 2, textplayeroneEasyDraw.height / 2);
                    }
                }
                if (!hit)
                {
                    Deselect();
                }
            }
        }


        float tickerCounter = 1;
        int tickerPoint = 0;
        string GetTicker()
        {
            tickerCounter += Time.deltaTime;
            if (tickerCounter >= 0.3f)
            {
                tickerPoint++;
                if (tickerPoint >= 2)
                {
                    tickerPoint = 0;
                }
                tickerCounter = 0;
            }
            if (tickerPoint == 0)
            {
                return " ";
            }
            else
            {
                return "/";
            }
        }

        void EditSpawnText()
        {
            spawnText = TypeToStringHandler.GetOutputString(spawnText, TypeToStringHandler.AllowedTypes.Alphabet | TypeToStringHandler.AllowedTypes.Numbers);
            if (spawnText.Length > 10)
            {
                spawnText = spawnText.Substring(0, 10);
            }
        }
    }
}
