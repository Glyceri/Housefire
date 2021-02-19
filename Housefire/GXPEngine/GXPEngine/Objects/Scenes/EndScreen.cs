using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GXPEngine.Objects.Handlers
{
    public class EndScreen : GameObject
    {
        bool _canBeInteractedWith;
        public bool canBeInteractedWith { get => _canBeInteractedWith && visible; set { _canBeInteractedWith = value; visible = value; if (!value) timer = 0; if (value) DrawTextBox(); } }


        public EasyDraw backdrop;
        public EasyDraw textBox;

        float timer = 0;

        public EndScreen()
        {
            backdrop = new EasyDraw(1920, 1080, false);
            textBox = new EasyDraw(1920, 1080);
            backdrop.Clear(Color.Black);
            AddChild(backdrop);
            backdrop.AddChild(textBox);
        }


        public void DrawTextBox()
        {
            

            textBox.Clear(Color.Transparent);
            textBox.TextAlign(CenterMode.Center, CenterMode.Center);
            textBox.TextSize(50);
            textBox.Text(MyGame.Instance.player1.health > MyGame.Instance.player2.health ? MyGame.Instance.player1.name + " Wins!" : MyGame.Instance.player1.health < MyGame.Instance.player2.health ? MyGame.Instance.player2.name + " Wins" : "Draw", textBox.width / 2, textBox.height / 10);
            textBox.Text("____________________________________________________________________________________________________", textBox.width / 2, textBox.height / 20 * 3f);
            textBox.Text("Press Spacebar", textBox.width / 2, textBox.height / 10 * 9);
            textBox.Text("____________________________________________________________________________________________________", textBox.width / 2, textBox.height / 20 * 16);

            textBox.TextSize(35);
            textBox.Text("Project lead", textBox.width/2, textBox.height/20 * 4.4f);
            textBox.Text("_ _ _ _ _ _ _ _ _ _ _", textBox.width / 2, textBox.height / 20 * 4.7f);
            textBox.TextSize(27);
            textBox.Text("Deniz Jan Fisekc", textBox.width / 2, textBox.height / 20 * 5.7f);
            textBox.TextSize(35);
            textBox.Text("_________________________", textBox.width / 2, textBox.height / 20 * 6f);
            textBox.Text("Artists", textBox.width / 2, textBox.height / 20 * 7f);
            textBox.Text("_ _ _ _ _ _ _ _ _ _ _", textBox.width / 2, textBox.height / 20 * 7.15f);
            textBox.TextSize(27);
            textBox.Text("Deniz Jan Fisekc (3D)", textBox.width / 2, textBox.height / 20 * 8.1f);
            textBox.Text("Dilyana Parvanova", textBox.width / 2, textBox.height / 20 * 8.8f);
            textBox.Text("Ariana Ghindar", textBox.width / 2, textBox.height / 20 * 9.5f);
            textBox.TextSize(35);
            textBox.Text("_________________________", textBox.width / 2, textBox.height / 20 * 9.85f);
            textBox.Text("Designers", textBox.width / 2, textBox.height / 20 * 10.85f);
            textBox.Text("_ _ _ _ _ _ _ _ _ _ _", textBox.width / 2, textBox.height / 20 * 11.15f);
            textBox.TextSize(27);
            textBox.Text("David Kleykamp", textBox.width / 2, textBox.height / 20 * 12.1f);
            textBox.Text("Di-Qi Sun", textBox.width / 2, textBox.height / 20 * 12.8f);
            textBox.TextSize(35);
            textBox.Text("_________________________", textBox.width / 2, textBox.height / 20 * 13.15f);
            textBox.Text("Engineer", textBox.width / 2, textBox.height / 20 * 14.15f);
            textBox.Text("_ _ _ _ _ _ _ _ _ _ _", textBox.width / 2, textBox.height / 20 * 14.55f);
            textBox.TextSize(27);
            textBox.Text("Amber Kortier", textBox.width / 2, textBox.height / 20 * 15.5f);
        }

        void Update()
        {
            if (!canBeInteractedWith) return;
            timer += Time.deltaTime;
            if(timer <= 1)
            {
                backdrop.SetXY(0, -1080 + (1080 * timer));
            }

            if (Input.GetKeyDown(Key.SPACE))
            {
                MyGame.Instance.OnStartup();
            }
        }

    }
}
