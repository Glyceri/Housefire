using GXPEngine.Objects.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace GXPEngine.Objects.Scenes
{
    public class MenuScreen : GameObject
    {
        List<Button> buttons = new List<Button>();
        List<Beatmap> beatmaps = new List<Beatmap>();

        public MenuScreen()
        {
            MakeList();
        }

        void MakeList()
        {
            CleanupOld();
            PopulateList();
            for (int i = 0; i < beatmaps.Count; i++)
            {
                buttons.Add(new Button(beatmaps[i].background));
                buttons[i].SetXY(0, i * 110);
                AddChild(buttons[i]);
            }
        }

        void PopulateList()
        {
            Looptrough("Songs/");
        }

        void Looptrough(string path)
        {
            foreach(string file in Directory.GetFiles(path))
            {
                if (file.EndsWith(".beat"))
                {
                    beatmaps.Add(new Beatmap(file));
                }
            }
            foreach(string directory in Directory.GetDirectories(path))
            {
                Looptrough(directory);
            }
        }

        void CleanupOld()
        {
            for (int i = 0; i < buttons.Count; i++)
            {
                buttons[i].LateDestroy();
            }
            buttons.Clear();
            beatmaps.Clear();
        }

        public void Update()
        {
            if (Input.GetKeyDown(Key.T))
            {
                MakeList();
            }

            for(int i = 0; i < buttons.Count; i++)
            {
                if (buttons[i].Pressed())
                {
                    MyGame.Instance.StartBeatMap(new Beatmap(beatmaps[i]));
                    break;
                }
            }
        }
    }
}
