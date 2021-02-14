using GXPEngine.Objects.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using GXPEngine.Core;
using GXPEngine.AddOns;
using static GXPEngine.AddOns.MouseHook;
using GXPEngine.Core.Audio;
using System.Drawing;

namespace GXPEngine.Objects.Scenes
{
    public class MenuScreen : GameObject
    {
        bool _canBeInteractedWith = true;
        public bool canBeInteractedWith { get => _canBeInteractedWith && visible; set { _canBeInteractedWith = value; backgroundHandler.canBeInteractedWith = value; } }

        List<BeatmapButton> beatmapButtons = new List<BeatmapButton>();
        List<BeatmapSmall> smallBeatmaps = new List<BeatmapSmall>();

        Vector2 elementSize = new Vector2(500, 140);
        Vector2 elementSpacing = new Vector2(1920, 30);

        float scrollAmount = 0;
        int screenHeight = 1080;

        int amountAllowedActive = 0;

        int underflow = 0;
        int lastUnderflow = 0;
        int amountOverflow = 1;

        MouseHook mouseHook;

        int selectedBeatmap = -1;

       
        MainMenuBackgroundHandler backgroundHandler;

        MainMenuScreen mainMenu;

        public MenuScreen(MainMenuScreen mainMenuScreen)
        {
            mainMenu = mainMenuScreen;
            screenHeight = game.height;

            backgroundHandler = new MainMenuBackgroundHandler();
            AddChild(backgroundHandler);
            
            MakeList();

            amountAllowedActive = Mathf.Ceiling((screenHeight - elementSpacing.y) / (elementSize.y + elementSpacing.y)) + amountOverflow;
            amountAllowedActive = (smallBeatmaps.Count < amountAllowedActive ? smallBeatmaps.Count : amountAllowedActive);

            for (int i = 0; i < amountAllowedActive; i++)
            {
                beatmapButtons.Add(new BeatmapButton(elementSize));
                beatmapButtons[i].SetXY(elementSpacing.x, (elementSpacing.y * i) + (elementSize.y * i) + elementSpacing.y);
                AddChild(beatmapButtons[i]);
            }

            for (int i = 0; i < amountAllowedActive; i++)
            {
                beatmapButtons[i].SetBeatmapSmall(smallBeatmaps[i]);
            }

            mouseHook = new MouseHook();
            mouseHook.MouseWheel += new MouseHookCallback(MouseHook);
            mouseHook.Install();

            

            try
            {
                selectedBeatmap = new Random().Next(smallBeatmaps.Count - 1);
                scrollAmount = (float)(((smallBeatmaps.Count * (elementSize.y + elementSpacing.y)) - screenHeight + elementSpacing.y) / (float)smallBeatmaps.Count) * (int)Mathf.Clamp(selectedBeatmap - 3, 0, 10000);
                underflow = (int)Mathf.Clamp(selectedBeatmap - (amountAllowedActive - amountOverflow), 0, 10000);
                SetSelectedSong(selectedBeatmap);

                SetBackground();
            }
            catch { }
        }

        void MouseHook(MSLLHOOKSTRUCT mouseEvent)
        {
            if (!canBeInteractedWith) return;
            if (!Input.GetKey(Key.LEFT_ALT))
            {
                if (smallBeatmaps.Count - 1 >= amountAllowedActive - amountOverflow)
                {
                    if (mouseEvent.mouseData > 8000000)
                    {
                        scrollAmount += 80;
                    }
                    else
                    {
                        scrollAmount -= 80;
                    }
                }
            }
        }

        void MakeList()
        {
            CleanupOld();
            PopulateList();
        }

        void Update()
        {
            if (selectedBeatmap >= 0)
            {
                MyGame.Instance.musicHandler.smallBeatmap = smallBeatmaps[selectedBeatmap];
            }
            backgroundHandler.BaseUpdate();
            if (canBeInteractedWith) 
            if (smallBeatmaps.Count - 1 >= amountAllowedActive - amountOverflow)
            {
                Scroll();
                Underflow();
            }
            ImageLoader();
            ImagePositions();
            if(canBeInteractedWith)
            BeatmapClick();
        }



        protected override void OnDestroy()
        {
            mouseHook.MouseWheel -= new MouseHookCallback(MouseHook);
            mouseHook.Uninstall();
            smallBeatmaps?.Clear();
            for (int i = 0; i < smallBeatmaps.Count; i++)
            {
                beatmapButtons[i]?.LateDestroy();
            }
            beatmapButtons?.Clear();
            
        }

        void SetBackground()
        {
            try
            {
                using (Bitmap bitmap = new Bitmap(smallBeatmaps[selectedBeatmap].background))
                {
                    backgroundHandler.SetBackground(bitmap);
                }
            }
            catch { }
        }

        void Scroll()
        {
           
                if (Input.GetKey(Key.DOWN))
                {
                    scrollAmount += 800 * Time.deltaTime;
                }
                if (Input.GetKey(Key.UP))
                {
                    scrollAmount -= 800 * Time.deltaTime;
                }

                scrollAmount = Mathf.Clamp(scrollAmount, 0, (smallBeatmaps.Count * (elementSize.y + elementSpacing.y)) - screenHeight + elementSpacing.y);
            
        }
        void Underflow()
        {
            underflow = -Mathf.Ceiling((-scrollAmount) / (elementSize.y + elementSpacing.y));
            if (underflow < 0) underflow = 0;

            if (lastUnderflow != underflow)
            {
                if (lastUnderflow > underflow)
                {
                    BeatmapButton but = beatmapButtons[beatmapButtons.Count - 1];
                    beatmapButtons.RemoveAt(beatmapButtons.Count - 1);
                    beatmapButtons.Insert(0, but);

                    if (selectedBeatmap - underflow > 0)
                    {
                        but.DrawNotSelected();
                    }
                    if(underflow == selectedBeatmap)
                    {
                        but.DrawSelected();
                    }

                    but.SetBeatmapSmall(smallBeatmaps[underflow]);
                    lastUnderflow--;
                }
                if (underflow > lastUnderflow)
                {
                    BeatmapButton but = beatmapButtons[0];
                    beatmapButtons.RemoveAt(0);
                    beatmapButtons.Insert(beatmapButtons.Count - (amountOverflow), but);

                    int overflowToo = (underflow - 1) + Mathf.Ceiling(screenHeight / (elementSize.y + elementSpacing.y));
                    if (underflow - 1 == selectedBeatmap)
                    {
                        but.DrawNotSelected();
                    }
                    if (selectedBeatmap == underflow -1 + (amountAllowedActive - amountOverflow))
                    {
                        but.DrawSelected();
                    }
                    but.SetBeatmapSmall(smallBeatmaps[overflowToo]);
                    lastUnderflow++;
                }
                //lastUnderflow = underflow;
            }
        }

        void ImageLoader()
        {
            for (int i = 0; i < beatmapButtons.Count; i++)
            {
                if (!beatmapButtons[i].backgroundDrawn)
                {
                    beatmapButtons[i].SetImage();

                    break;
                }
            }
        }

        void ImagePositions()
        {
            for (int i = 0; i < beatmapButtons.Count; i++)
            {
                float elSpace = (elementSpacing.y * (i + underflow)) + (elementSize.y * (i + underflow)) + elementSpacing.y;
                Vector2 butAndEl = new Vector2(0, beatmapButtons[i].y + elementSize.y);
                float scaledDist = Vector2.Distance(butAndEl, new Vector2(0, (1080 / (float)2))) / 512;

                beatmapButtons[i].scale *= new Vector2(1.65f - (0.4f * (scaledDist)), 1f - (0.1f * scaledDist));
                beatmapButtons[i].SetXY(elementSpacing.x - beatmapButtons[i].width, (elSpace) - scrollAmount);

            }
        }
        void BeatmapClick()
        {
            if (Input.GetMouseButtonDown(0))
            {
                int hit = -1;
                for (int i = 0; i < beatmapButtons.Count; i++)
                {
                    if (beatmapButtons[i].collider.HitTestPoint(Input.mouseX, Input.mouseY, false))
                    {
                        if (selectedBeatmap != i + underflow) SetSelectedSong(i + underflow);
                        hit = i;
                    }
                }
                
                if(hit != -1)
                {
                    for (int i = 0; i < beatmapButtons.Count; i++)
                    {
                        if(i != hit)  beatmapButtons[i].DrawNotSelected();
                    }
                }
            }
        }

        public void SetSelectedSong(int selected)
        {
            selectedBeatmap = selected;
            try
            {
                beatmapButtons[selected - underflow].DrawSelected();
            }
            catch { }
            mainMenu.SetSelectedSong(beatmapButtons[selected - underflow]);
            SetBackground();
        }

        void PopulateList()
        {
            Looptrough("Songs/");
        }
        void Looptrough(string path)
        {
            try
            {
                foreach (string file in Directory.GetFiles(path))
                {
                    if (file.EndsWith(".beat"))
                    {
                        smallBeatmaps.Add(new BeatmapSmall(file));
                    }
                }
                foreach (string directory in Directory.GetDirectories(path))
                {
                    Looptrough(directory);
                }
            }
            catch { }
        }
        void CleanupOld()
        {
            smallBeatmaps?.Clear();
        }
    }
}
