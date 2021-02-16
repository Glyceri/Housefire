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
                keybinds = new List<VKeys[]>() { new VKeys[1] { VKeys.SPACE }, new VKeys[2] { VKeys.KEY_F, VKeys.KEY_J }, new VKeys[3] { VKeys.KEY_F, VKeys.SPACE, VKeys.KEY_J }, new VKeys[4] { VKeys.KEY_D, VKeys.KEY_F, VKeys.KEY_J, VKeys.KEY_K }, new VKeys[5] { VKeys.KEY_D, VKeys.KEY_F, VKeys.SPACE, VKeys.KEY_J, VKeys.KEY_K }, new VKeys[6] { VKeys.KEY_S, VKeys.KEY_D, VKeys.KEY_F, VKeys.KEY_J, VKeys.KEY_K, VKeys.KEY_L }, new VKeys[7] { VKeys.KEY_S, VKeys.KEY_D, VKeys.KEY_F, VKeys.SPACE, VKeys.KEY_J, VKeys.KEY_K, VKeys.KEY_L } };
            }

            if (playerNum == 2)
            {
                name = "Player 2";
                keybinds = new List<VKeys[]>() { new VKeys[1] { VKeys.UP }, new VKeys[2] { VKeys.LEFT, VKeys.RIGHT }, new VKeys[3] { VKeys.LEFT, VKeys.UP, VKeys.RIGHT }, new VKeys[4] { VKeys.LEFT, VKeys.UP, VKeys.DOWN, VKeys.RIGHT }, new VKeys[5] { VKeys.LEFT, VKeys.UP, VKeys.RCONTROL, VKeys.DOWN, VKeys.RIGHT }, new VKeys[6] { VKeys.RCONTROL, VKeys.LEFT, VKeys.UP, VKeys.DOWN, VKeys.RIGHT, VKeys.NUMPAD0 }, new VKeys[7] { VKeys.RCONTROL, VKeys.LEFT, VKeys.UP, VKeys.RSHIFT, VKeys.DOWN, VKeys.RIGHT, VKeys.NUMPAD0 } };
            }
        }


    }
}
