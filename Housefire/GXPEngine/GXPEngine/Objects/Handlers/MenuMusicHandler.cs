using GXPEngine.AddOns;
using GXPEngine.Core;
using GXPEngine.Core.Audio;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GXPEngine.AddOns.MouseHook;

namespace GXPEngine.Objects.Handlers
{
    public class MenuMusicHandler : GameObject
    {
        public bool _canBeInteractedWith = true;
        public bool canBeInteractedWith { get => _canBeInteractedWith && (visible && !disableVisibility); set { _canBeInteractedWith = value; } }

        public EasyDraw backgroundBar;

        public AudioObject music;
        string musicName = "";

        Sprite[] playerControls = new Sprite[3];
        BarObject audioBar;
        BarObject volumeBar;

        BeatmapButtonEasyDraw songNamePlate;

        bool findNew = true;

        public BeatmapSmall smallBeatmap;

        MouseHook mouseHook;

        public MenuMusicHandler()
        {

            backgroundBar = new EasyDraw(new Bitmap("songtitlebar.png"), false);
            AddChild(backgroundBar);

            audioBar = new BarObject(backgroundBar.width / 10 * 8, 10, Color.Magenta, false);
            audioBar.SetXY((backgroundBar.width - (backgroundBar.width / 10 * 8)) / 2, 47);
            audioBar.OnFillAmountChanged += OnAudioBarChange;

            songNamePlate = new BeatmapButtonEasyDraw(backgroundBar.width / 10 * 8, 30, false);
            songNamePlate.SetXY((backgroundBar.width - (backgroundBar.width / 10 * 8)) / 2, 12);
            songNamePlate.TextSize(15);
            songNamePlate.TextAlign(CenterMode.Min, CenterMode.Min);
            songNamePlate.SetColor(255, 255, 255);
            songNamePlate.collider = new BoxCollider(Vector2.Zero, new Vector2(backgroundBar.width / 10 * 8, 30), songNamePlate);
            //songNamePlate.Clear(Color.White);
            backgroundBar.AddChild(songNamePlate);

            volumeBar = new BarObject(250, 10, Color.White, false);
            volumeBar.SetXY(50, 70);
            volumeBar.OnFillAmountChanged += OnVolumeBarChange;

            backgroundBar.AddChild(volumeBar);
            backgroundBar.AddChild(audioBar);

            playerControls[0] = new Sprite("STOP.png", false, false);
            playerControls[1] = new Sprite("PLAY.png", false, false);
            playerControls[2] = new Sprite("PAUSE.png", false, false);

            for (int i = 0; i < playerControls.Length; i++)
            {
                backgroundBar.AddChild(playerControls[i]);
                playerControls[i].scale = new Vector2(0.25f, 0.25f);
                playerControls[i].SetXY(i * 50 + 320, 62);
                playerControls[i].alpha = 1;
                playerControls[i].collider = new BoxCollider(Vector2.Zero, new Vector2(100, 100), playerControls[i]);
                playerControls[i].collider.isTrigger = true;
            }

            mouseHook = new MouseHook();
            mouseHook.MouseWheel += new MouseHookCallback(MouseHook);
            mouseHook.Install();
        }

        bool disableVisibility = false;
        void MouseHook(MSLLHOOKSTRUCT mouseEvent)
        {
            if (Input.GetKey(Key.LEFT_ALT))
            {
                disableVisibility = !visible || disableVisibility;
                visible = true;
                
                if (mouseEvent.mouseData > 8000000)
                {
                    AudioHandler.volume -= 5;
                }
                else
                {
                    AudioHandler.volume += 5;
                }
            }
        }

        public override void Render(GLContext glContext)
        {
            if (disableVisibility)
            {
                songNamePlate.alpha = 0.5f;
                audioBar.bar.alpha = 0.5f;
                audioBar.barBackground.alpha = 0.5f;

                for (int i = 0; i < playerControls.Length; i++)
                {
                    playerControls[i].visible = false;
                }
            }

            if (disableVisibility && !Input.GetKey(Key.LEFT_ALT) && !canBeInteractedWith)
            {
                visible = false;
                disableVisibility = false;

                songNamePlate.alpha = 1;
                audioBar.bar.alpha = 1;
                audioBar.barBackground.alpha = 1;

                for (int i = 0; i < playerControls.Length; i++)
                {
                    playerControls[i].visible = true;
                }
            }

            base.Render(glContext);
        }

        float songnameCounter = 0;
        void Update()
        {

            SliderValues();
            MusicControls();
            ScrollingText();
            RestartMusic();
            MusicSettings();
        }

        void SliderValues()
        {
            if (music != null)
                if (music.audioFile != null)
                    audioBar.fillAmount = (float)(music.audioFile.CurrentTime.TotalMilliseconds / music.audioFile.TotalTime.TotalMilliseconds);
            volumeBar.fillAmount = AudioHandler.volume / (float)100;
        }


        void MusicControls()
        {
            if (!canBeInteractedWith) return;
            if (Input.GetMouseButtonDown(0))
            {
                for (int i = 0; i < playerControls.Length; i++)
                {
                    if (playerControls[i].collider.HitTestPoint(Input.mouseX, Input.mouseY, false))
                    {
                        if (i == 0) { music?.Dispose(); findNew = false; }
                        if (i == 2) music?.Pause();
                        if (i == 1) if (music?.disposed ?? true) { LoadMusic(musicName); music?.Play(); } else { music?.Continue(); findNew = true; }
                    }
                }
            }
        }

        void ScrollingText()
        {
            if(music?.audioPlayer?.PlaybackState != NAudio.Wave.PlaybackState.Paused) songnameCounter += Time.deltaTime * 0.25f;
            if (songnameCounter >= 2f)
            {
                songnameCounter = 0;

            }
            songNamePlate.Clear(Color.Transparent);
            if (songnameCounter > 0)
            {
                songNamePlate.Text(smallBeatmap.name, ((1 - songnameCounter) * songNamePlate.width), 3);

            }
        }

        void RestartMusic()
        {
            if (findNew && (music?.disposed ?? true) && !(MyGame.Instance.beatmapHandler?.isPlaying ?? true))
            {
                LoadMusic(musicName);
                music?.Play();
            }
        }

        void MusicSettings()
        {
            if (music?.disposed ?? true)
            {
                songnameCounter = 1;
                audioBar.fillAmount = 0;
                audioBar.canInterract = false;
            }
            else
            {
                audioBar.canInterract = true;
            }
        }

        public void LoadMusic(string fileName)
        {

            try
            {
                musicName = fileName;
                music?.Dispose();
                music = AudioHandler.GetAudioObject(AudioHandler.RegisterAudio(musicName));
                findNew = true;
            }
            catch
            {
                Console.WriteLine("Audio File Could not be found");
            }
        }

        void OnAudioBarChange(float amount)
        {
            if (!canBeInteractedWith) return;
            if (music != null)
                if (music.audioFile != null)
                    music.audioFile.CurrentTime = new TimeSpan(0, 0, 0, 0, (int)(amount * music.audioFile.TotalTime.TotalMilliseconds));
        }

        void OnVolumeBarChange(float amount)
        {
            if (!visible) return;
            AudioHandler.volume = (int)(100 * amount);
        }

        public void ConvenientPlay(string fileName, int startTime = 0)
        {
            Dispose();
            try
            {
                LoadMusic(fileName);
                Play(startTime);
            }
            catch
            {
                Console.WriteLine("Audio File Could not be found");
            }
        }

        public void Play(int starttime = 0)
        {
            try
            {
                if (music != null)
                {
                    music?.Play();
                    music.audioFile.CurrentTime = new TimeSpan(0, 0, 0, 0, starttime);
                }
            }
            catch { }
        }

        public void Stop()
        {
            music?.Dispose();
            music = null;
        }

        public void Dispose()
        {
            music?.Dispose();
            music = null;
        }

        protected override void OnDestroy()
        {
            Dispose();
            audioBar.OnFillAmountChanged -= OnAudioBarChange;
            volumeBar.OnFillAmountChanged -= OnVolumeBarChange;
            mouseHook.MouseWheel -= new MouseHookCallback(MouseHook);
            mouseHook.Uninstall();
            base.OnDestroy();
        }
    }
}
