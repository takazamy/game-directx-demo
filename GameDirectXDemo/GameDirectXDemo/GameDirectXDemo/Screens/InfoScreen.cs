using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameDirectXDemo.Core;
using GameDirectXDemo.Manager;
using System.Drawing;
using Microsoft.DirectX.DirectInput;
using Microsoft.DirectX.DirectDraw;

namespace GameDirectXDemo.Screens
{
    public class InfoScreen:DxScreen
    {
        String[] value;
        String range;
        public InfoScreen(ScreenManager scrManager, DxInitGraphics graphics, Point location, Size size, String info) :
            base(scrManager, graphics, location, size)
        {
            //Type-hp-stamina-damage-shield
            value = info.Split('-');
            switch (value[0])
            {
                case "1":
                    range = "near,far";
                    break;
                case "2":
                    range = "far";
                    break;
                case "3":
                    range = "near";
                    break;
            }
        }

        public override void Update(double deltaTime, KeyboardState keyState, MouseState mouseState)
        {

        }

        public void DrawTextInfo(Surface sf)
        {
            sf.DrawText(this.Location.X , this.Location.Y + 5, "HP: " + value[1], false);
            sf.DrawText(this.Location.X , this.Location.Y + 20, "Stamina: " + value[2], false);
            sf.DrawText(this.Location.X , this.Location.Y + 35, "Damage: " + value[3], false);
            sf.DrawText(this.Location.X , this.Location.Y + 50, "Shield: " + value[4], false);
            sf.DrawText(this.Location.X , this.Location.Y + 65, "Range: " + range, false);
        }
        public override void Draw()
        {
            
        }
    }
}
