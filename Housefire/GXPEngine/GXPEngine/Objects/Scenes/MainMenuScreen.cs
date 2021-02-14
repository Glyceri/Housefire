using GXPEngine.Objects.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GXPEngine.Objects.Scenes
{
    public class MainMenuScreen : GameObject
    {
        bool _canBeInteractedWith;
        public bool canBeInteractedWith { get => _canBeInteractedWith && visible; set { _canBeInteractedWith = value; menuScreen.canBeInteractedWith = value; } }

        public MenuScreen menuScreen;
        

        public MainMenuScreen()
        {
           
            menuScreen = new MenuScreen(this);
            AddChild(menuScreen);    
        }


        public void SetSelectedSong(BeatmapButton button)
        {
            MyGame.Instance.musicHandler?.ConvenientPlay(button?.beatmapSmall.music, button.beatmapSmall.startTime);
        }
    }
}
