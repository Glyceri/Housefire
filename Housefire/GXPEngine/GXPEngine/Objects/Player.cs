using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GXPEngine.AddOns.KeyboardHook;

namespace GXPEngine.Objects
{
    public class Player
    {
        public string name;
        public List<VKeys[]> keybinds = new List<VKeys[]>();
        public int playerNum = 0;

        public int health = 3;

        public Player(int playerNum)
        {
            this.playerNum = playerNum;
            if(playerNum == 1)
            {
                name = "Player 1";
                keybinds = new List<VKeys[]>() { new VKeys[1] { VKeys.SPACE }, new VKeys[2] { VKeys.KEY_S, VKeys.KEY_F }, new VKeys[3] { VKeys.KEY_S, VKeys.SPACE, VKeys.KEY_F }, new VKeys[4] { VKeys.KEY_A, VKeys.KEY_S, VKeys.KEY_F, VKeys.KEY_G }, new VKeys[5] { VKeys.KEY_D, VKeys.KEY_F, VKeys.SPACE, VKeys.KEY_J, VKeys.KEY_K } };
            }

            if (playerNum == 2)
            {
                name = "Player 2";
                keybinds = new List<VKeys[]>() { new VKeys[1] { VKeys.RMENU }, new VKeys[2] { VKeys.KEY_J, VKeys.KEY_L }, new VKeys[3] { VKeys.KEY_J, VKeys.RMENU, VKeys.KEY_L }, new VKeys[4] { VKeys.KEY_H, VKeys.KEY_J, VKeys.KEY_L, VKeys.OEM_1 }, new VKeys[5] { VKeys.KEY_H, VKeys.KEY_J, VKeys.RMENU, VKeys.KEY_L, VKeys.OEM_1 } };
            }
        }


    }
}
