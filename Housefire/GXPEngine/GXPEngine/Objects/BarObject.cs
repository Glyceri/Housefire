using GXPEngine.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GXPEngine.Objects
{
    public class BarObject : GameObject
    {
        float _fillAmount = 0;
        public float fillAmount { get => _fillAmount; set { value = Mathf.Clamp(value, 0, 1);  _fillAmount = value; } }
        public bool canInterract = true;

        public BeatmapButtonEasyDraw barBackground { get; private set; }
        public BeatmapButtonEasyDraw bar { get; private set; }

        int barWidth;

        public delegate void FillAmountChanged(float amount);
        public event FillAmountChanged OnFillAmountChanged = null;

        public BarObject(int width, int height, Color color, bool addCollider = true)
        {
            barBackground = new BeatmapButtonEasyDraw(width, height, addCollider);
            barBackground.collider = new BoxCollider(Vector2.Zero, new Vector2(width, height), barBackground);
            barBackground.collider.isTrigger = true;
            using (Bitmap bitmap = new Bitmap("notsellectedsong.png"))
            {
                barBackground.DrawSprite(bitmap, new Vector2(width / (float)bitmap.Width, height / (float)bitmap.Height), 0, 0, false);
            }
            AddChild(barBackground);
            bar = new BeatmapButtonEasyDraw(width-4, height-6, addCollider);
            bar.Clear(color);
            barBackground.AddChild(bar);
            bar.x += 2;
            bar.y += 3;
            barWidth = width-4;
        }

        void Update()
        {
            fillAmount = Mathf.Clamp(fillAmount, 0, 1);
            barBackground.BaseUpdate();
            bar.BaseUpdate();
            bar.width = (int)(fillAmount * barWidth);

            if (canInterract)
            {
                if (Input.GetMouseButton(0))
                {
                    if(barBackground.collider.HitTestPoint(Input.mouseX, Input.mouseY, false))
                    {
                        fillAmount = ((Input.mouseX - bar.TransformPoint(0,0).x)/ (float)barBackground.width);
                        OnFillAmountChanged?.Invoke(fillAmount);
                    }
                }
            }
        }
    }
}
