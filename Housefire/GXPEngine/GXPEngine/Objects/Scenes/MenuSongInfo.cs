using GXPEngine.Core;
using GXPEngine.Core.Audio;
using GXPEngine.Objects.Handlers;
using GXPEngine.Objects.Handlers.Data;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GXPEngine.Objects.Scenes
{
    public class MenuSongInfo : GameObject
    {
        bool _canBeInteractedWith = true;
        public bool canBeInteractedWith { get => _canBeInteractedWith && visible; set { _canBeInteractedWith = value; } }

        public EasyDraw highscorePanel;
        EasyDraw highscorePanelScores;
        EasyDraw highscorePanelHeader;
        EasyDraw highscoreheaderText;

        public EasyDraw songInfoPanel;
        EasyDraw songInfoTextPanel;
        EasyDraw songInfoHeader;
        EasyDraw songInfoHeaderText;

        public EasyDraw startButtonPanel;
        EasyDraw startButtonHeader;
        EasyDraw startButtonText;

        MenuScreen mainMenu;

        public BeatmapSmall small;

        public MenuSongInfo(MenuScreen mainMenu)
        {
            this.mainMenu = mainMenu;
            using (Bitmap header = new Bitmap("colourfulthing.png"))
            {
                using (Bitmap longbar = new Bitmap("longbar.png"))
                {
                    highscorePanel = new EasyDraw(300, 700, false);
                    highscorePanel.DrawSprite(longbar, new Vector2(300 / (float)longbar.Width, 700 / (float)longbar.Height));
                    highscorePanel.SetXY(70, 325);
                    highscorePanel.collider = new BoxCollider(Vector2.Zero, new Vector2(highscorePanel.width, highscorePanel.height), highscorePanel);
                    AddChild(highscorePanel);

                    highscorePanelHeader = new EasyDraw(270, 100, false);
                    highscorePanelHeader.DrawSprite(header, new Vector2(-(270 / (float)header.Width), 100 / (float)header.Height), (270 / (float)header.Width) * 2);
                    highscorePanel.AddChild(highscorePanelHeader);
                    highscorePanelHeader.collider = new BoxCollider(Vector2.Zero, new Vector2(highscorePanelHeader.width, highscorePanelHeader.height), highscorePanelHeader);
                    highscorePanelHeader.SetXY(60, -30);

                    highscorePanelScores = new EasyDraw(270, 600, false);
                    highscorePanelScores.SetXY(30, 75);
                    highscorePanelScores.collider = new BoxCollider(Vector2.Zero, new Vector2(highscorePanelScores.width, highscorePanelScores.height), highscorePanelScores);
                    highscorePanel.AddChild(highscorePanelScores);

                    highscoreheaderText = new EasyDraw(270, 100, false);
                    highscoreheaderText.TextAlign(CenterMode.Min, CenterMode.Min);
                    highscoreheaderText.TextSize(30);
                    highscoreheaderText.Text("Highscores:", 25, 20);
                    highscorePanelHeader.AddChild(highscoreheaderText);
                }

                using (Bitmap bigbar = new Bitmap("bigbar.png"))
                {
                    songInfoPanel = new EasyDraw(500, 700, false);
                    songInfoPanel.DrawSprite(bigbar, new Vector2(songInfoPanel.width / (float)bigbar.Width, songInfoPanel.height / (float)songInfoPanel.height));
                    songInfoPanel.SetXY(510, 370);
                    songInfoPanel.collider = new BoxCollider(Vector2.Zero, new Vector2(songInfoPanel.width , songInfoPanel.height), songInfoPanel);
                    AddChild(songInfoPanel);

                    songInfoHeader = new EasyDraw(370, 100, false);
                    songInfoHeader.DrawSprite(header, new Vector2((songInfoHeader.width / (float)header.Width), songInfoHeader.height / (float)header.Height));
                    songInfoHeader.SetXY(-20, -20);
                    songInfoHeader.collider = new BoxCollider(Vector2.Zero, new Vector2(songInfoHeader.width, songInfoHeader.height), songInfoHeader);
                    songInfoPanel.AddChild(songInfoHeader);

                    songInfoHeaderText = new EasyDraw(325, 100, false);
                    songInfoHeaderText.TextAlign(CenterMode.Min, CenterMode.Min);
                    songInfoHeaderText.TextSize(20);
                    songInfoHeaderText.Text("songname:", 25, 25);
                    songInfoHeaderText.collider = new BoxCollider(Vector2.Zero, new Vector2(325,100), songInfoHeader);
                    songInfoHeader.AddChild(songInfoHeaderText);

                    songInfoTextPanel = new EasyDraw(450, 600, false);
                    songInfoTextPanel.SetXY(25, 75);
                    songInfoTextPanel.collider = new BoxCollider(Vector2.Zero, new Vector2(450, 600), songInfoTextPanel);
                    songInfoPanel.AddChild(songInfoTextPanel);
                }

                using (Bitmap songBar = new Bitmap("songtitlebar.png"))
                {
                    startButtonPanel = new EasyDraw(500, 100, false);
                    startButtonPanel.DrawSprite(songBar, new Vector2(500 / (float)songBar.Width, 100 / (float)songBar.Height));
                    startButtonPanel.SetXY(510, 210);
                    startButtonPanel.collider = new BoxCollider(Vector2.Zero, new Vector2(startButtonPanel.width, startButtonPanel.height), startButtonPanel);
                    startButtonPanel.collider.isTrigger = true;
                    AddChild(startButtonPanel);

                    startButtonHeader = new EasyDraw(370, 100, false);
                    startButtonHeader.DrawSprite(header, new Vector2((startButtonHeader.width / (float)header.Width), startButtonHeader.height / (float)header.Height));
                    startButtonHeader.SetXY(-20, -20);
                    startButtonHeader.collider = new BoxCollider(Vector2.Zero, new Vector2(startButtonHeader.width, startButtonHeader.height), startButtonHeader);
                    startButtonPanel.AddChild(startButtonHeader);

                    startButtonText = new EasyDraw(325, 100, false);
                    startButtonText.TextAlign(CenterMode.Min, CenterMode.Min);
                    startButtonText.TextSize(30);
                    startButtonText.Text("PLAY", 45, 20);
                    startButtonText.collider = new BoxCollider(Vector2.Zero, new Vector2(325, 100), startButtonText);
                    startButtonHeader.AddChild(startButtonText);
                }
            }
        }

      


        public void SetBeatmap(BeatmapSmall beatmap)
        {
            this.small = beatmap;
            songInfoHeaderText.Clear(Color.Transparent);
            songInfoHeaderText.Text(beatmap.name, 25, 25);

            songInfoTextPanel.Clear(Color.Transparent);
            songInfoTextPanel.TextSize(40);
            songInfoTextPanel.Text("Lanes: " + beatmap.lanes.ToString(), 10, 80);
            songInfoTextPanel.Text("BPM: " + (beatmap.actualbpm == -1 ? beatmap.BPM.ToString() : beatmap.actualbpm.ToString()), 10, 130);
            songInfoTextPanel.Text("Difficulty: " + beatmap.difficulty.ToString(), 10, 180);
            songInfoTextPanel.Text("Notes: " + beatmap.notesAmount.ToString(), 10, 230);
            songInfoTextPanel.Text("Slides: " + beatmap.sliderAmount.ToString(), 10, 280);

            SetHighscores(beatmap);
        }

        void SetHighscores(BeatmapSmall beatmap)
        {
            highscorePanelScores.Clear(Color.Transparent);
            List<Highscore> highscores = MyGame.Instance.highscorehandler.GetHighscores(beatmap.internalName);
            using (Bitmap bitmap = new Bitmap("notsellectedsong.png"))
            {
                for (int i = 0; i < highscores.Count; i++)
                {
                    if (i >= 7) break;
                    Highscore highscore = highscores[i];
                    highscorePanelScores.DrawSprite(bitmap, new Vector2(0.22f, 0.4f),  0f, 0.42f * i);
                    highscorePanelScores.TextAlign(CenterMode.Min, CenterMode.Min);
                    highscorePanelScores.TextSize(19);
                    highscorePanelScores.Text(highscore.name + ": " + '\n' + highscore.score + " | " + highscore.combo, 0.05f, 85*i);
                }
            }
        }

        bool inEndingAnimation = false;
        float timer = 0;
        float animationMultiplier = 0.4f;
        int countdown = 0;

        int startupVolume = 0;

        void Update()
        {
            if (inEndingAnimation)
            {
                countdown++;
                if (countdown >= 3)
                {
                    timer += (Time.deltaTime * animationMultiplier);

                    mainMenu.globalOffset = timer;
                    startButtonPanel.SetXY(510, 210 - (timer * 1000));
                    highscorePanel.SetXY(70 - (timer * 1000), 325);
                    songInfoPanel.SetXY(510, 370 + (timer * 1700));
                    //mainMenu.backgroundHandler.background.alpha = 1 - timer;
                    MyGame.Instance.musicHandler.visible = true;
                    MyGame.Instance.beatmapHandler.delta = timer;
                    AudioHandler.volume = (int)Mathf.Map((100 * (float)(1 - timer)), 0, 100, 0, startupVolume);
                    MyGame.Instance.musicHandler.SetXY(0, 0 - (MyGame.Instance.musicHandler.backgroundBar.height * timer));
                    MyGame.Instance.livesMenu.delta = timer;
                    

                    if (timer >= 1)
                    {
                        inEndingAnimation = false;
                        timer = 0;
                        MyGame.Instance.beatmapHandler?.Play();
                        MyGame.Instance.musicHandler.visible = false;
                        AudioHandler.volume = startupVolume;
                        MyGame.Instance.musicHandler.SetXY(0, 0);
                    }
                }
            }

            if (canBeInteractedWith)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if(startButtonPanel.collider.HitTestPoint(Input.mouseX, Input.mouseY, false))
                    {
                        inEndingAnimation = true;
                        canBeInteractedWith = false;
                        mainMenu.canBeInteractedWith = false;
                        MyGame.Instance.StartBeatMap(small.beatmapFile);
                        startupVolume = AudioHandler.volume;
                         timer = 0;
                        countdown = 0;
                        new Sound("soft-hitnormal.wav")?.Play(false, 0, AudioHandler.volume / (float)100);
                    }
                }
            }

        }
    }
}
