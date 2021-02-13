using GXPEngine.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GXPEngine.Objects.Handlers
{
    public class BeatmapButtonHandler : GameObject
    {
        List<Beatmap> beatmaps = new List<Beatmap>();
        List<BeatmapButton> beatmapButtons = new List<BeatmapButton>();

        List<BeatmapSmall> smallBeatmaps = new List<BeatmapSmall>();

        Vector2 elementSize = new Vector2(200, 100);
        Vector2 elementSpacing = new Vector2(0, 20);

        float scrollAmount = 0;
        float lastScrollAmount = 0;
        int screenHeight = 1080;

        int amountAllowedActive = 0;

        int underflow = 0;
        int lastUnderflow = 0;
        int amountOverflow = 1;

        public BeatmapButtonHandler()
        {
            screenHeight = game.height;
            MakeList();

            amountAllowedActive = Mathf.Ceiling((screenHeight - elementSpacing.y) / (elementSize.y + elementSpacing.y)) + amountOverflow;
            amountAllowedActive = (beatmaps.Count < amountAllowedActive ? beatmaps.Count : amountAllowedActive);
            
            for (int i = 0; i < amountAllowedActive; i++)
            {
                beatmapButtons.Add(new BeatmapButton((int)elementSize.x, (int)elementSize.y, this));
                beatmapButtons[i].SetXY(elementSpacing.x, (elementSpacing.y * i) + (elementSize.y * i) + elementSpacing.y);
                AddChild(beatmapButtons[i]);
            }

            for(int i = 0; i < amountAllowedActive; i++)
            {
                beatmapButtons[i].SetBeatmap(beatmaps[i]);
                beatmapButtons[i].RequestImage();
            }
        }

        Beatmap registeredBeatmap;
        public void RegisterPlay(ref Beatmap beatmap)
        {
            registeredBeatmap = beatmap;
        }

        void MakeList()
        {
            CleanupOld();
            PopulateList();
            for (int i = 0; i < beatmaps.Count; i++)
            {
                beatmaps[i].WriteDebug(false);
            }
        }

        void Update()
        {
            Scroll();
            Underflow();
            ImageLoader();
            ImagePositions();
            RegisteredBeatmap();

            for(int i = smallBeatmaps.Count -1; i > 0; i--)
            {
                BeatmapSmall small = smallBeatmaps[i];
                if (small.isLoaded)
                {
                    small.WriteDebug();
                    smallBeatmaps.Remove(small);
                }
            }
        }

       

        protected override void OnDestroy()
        {
            for (int i = 0; i < beatmaps.Count; i++)
            {
                beatmaps[i]?.Dispose();
            }
            beatmaps?.Clear();
            for (int i = 0; i < beatmaps.Count; i++)
            {
                beatmapButtons[i]?.LateDestroy();
            }
            beatmapButtons?.Clear();
        }

        void Scroll()
        {
            if (Input.GetKey(Key.DOWN))
            {
                scrollAmount += 500 * Time.deltaTime;
            }
            if (Input.GetKey(Key.UP))
            {
                scrollAmount -= 500 * Time.deltaTime;
            }

            scrollAmount = Mathf.Clamp(scrollAmount, 0, (beatmaps.Count * (elementSize.y + elementSpacing.y)) - screenHeight + elementSpacing.y);
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

                    but.SetBeatmap(beatmaps[underflow]);
                }
                if (underflow > lastUnderflow)
                {
                    BeatmapButton but = beatmapButtons[0];
                    beatmapButtons.RemoveAt(0);
                    beatmapButtons.Insert(beatmapButtons.Count - (amountOverflow), but);

                    but.SetBeatmap(beatmaps[(underflow - 1) + Mathf.Ceiling(screenHeight / (elementSize.y + elementSpacing.y))]);

                }
                lastUnderflow = underflow;
            }
        }

        float counter = 0;
        void ImageLoader()
        {
            if (lastScrollAmount != scrollAmount)
            {
                lastScrollAmount = scrollAmount;
                counter = 0;
            }
            else
            {
                counter += Time.deltaTime;
                if (counter > 0.25f)
                {
                    for (int i = 0; i < beatmapButtons.Count; i++)
                    {
                        if (beatmapButtons[i].beatmap?.background == null)
                        {
                            beatmapButtons[i].RequestImage();
                            counter = 0;
                            break;
                        }
                    }
                }
            }
        }
        void ImagePositions()
        {
            for (int i = 0; i < beatmapButtons.Count; i++)
            {
                float elSpace = (elementSpacing.y * (i + underflow)) + (elementSize.y * (i + underflow)) + elementSpacing.y;

                beatmapButtons[i].SetXY(elementSpacing.x, (elSpace) - scrollAmount);
            }
        }
        void RegisteredBeatmap()
        {
            if (registeredBeatmap != null)
            {

                if (Input.GetMouseButtonDown(0))
                {
                    BoxCollider box = new BoxCollider(new Vector2(220, 0), new Vector2(1920, 1080), this);
                    box.DrawSelf();

                    if (box.HitTestPoint(Input.mouseX, Input.mouseY))
                    {

                        MyGame.Instance.StartBeatMap(registeredBeatmap.beatmapFile);
                        LateDestroy();
                    }
                }
            }
        }

        void PopulateList()
        {
            Looptrough("Songs/");
        }
        void Looptrough(string path)
        {
            foreach (string file in Directory.GetFiles(path))
            {
                if (file.EndsWith(".beat"))
                {
                    //beatmaps.Add(new Beatmap(file));
                    smallBeatmaps.Add(new BeatmapSmall(file));
                }
            }
            foreach (string directory in Directory.GetDirectories(path))
            {
                Looptrough(directory);
            }
        }
        void CleanupOld()
        {
            beatmaps.Clear();
        }
    }
}
