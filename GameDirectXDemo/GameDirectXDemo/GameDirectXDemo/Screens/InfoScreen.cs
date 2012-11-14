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
        DxImage bg;
        public Boolean isShow = false;
        Object currObj;
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
        public InfoScreen(ScreenManager scrManager, DxInitGraphics graphics, Point location, Size size, Object obj, Boolean isShow) :
            base(scrManager, graphics, location, size)
        {
            //Hp-stamina-damage-shield-range
            String inf = obj._hp + "-";
            inf += obj._stamina + "-";
            inf += obj._damage + "-";
            inf += obj._shield + "-";
            inf += obj.RangeAttack + "-";
            value = inf.Split('-');
            this.isShow = isShow;
            currObj = obj;
            bg = new DxImage(GameResource.ActionScreen, Global.BitmapType.SOLID, 0, _graphics.DDDevice);
        }
        public void SetPosition(Size tileMapsize, Point CursorPosition, Size gameCursor)
        {
            Point p = new Point(CursorPosition.X + gameCursor.Width, CursorPosition.Y);
            if (p.X + this.Size.Width <= tileMapsize.Width)
            {
                if (p.Y + this.Size.Height <= tileMapsize.Height)
                {
                    this.Location = p;
                }
                else
                {
                    p.Y = tileMapsize.Height - this.Size.Height;
                    this.Location = p;
                }

            }
            else
            {
                p = new Point(CursorPosition.X - this.Size.Width, CursorPosition.Y);
                if (p.X >= 0)
                {
                    if (p.Y + this.Size.Height <= tileMapsize.Height)
                    {
                        this.Location = p;

                    }
                    else
                    {
                        p.Y = tileMapsize.Height - this.Size.Height;
                        this.Location = p;
                    }
                }
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

        public void DrawTextObjectInfo(Surface sf)
        {

            if (this.isShow)
            {
                sf.Draw(new Rectangle(this.Location, this.Size), bg._sourceSurface, DrawFlags.Wait);
                sf.DrawText(this.Location.X, this.Location.Y + 5, "HP: " + value[0] + "/" + currObj._fullHp, false);
                sf.DrawText(this.Location.X, this.Location.Y + 20, "Stamina: " + value[1] + "/" + currObj._fullSta, false);
                sf.DrawText(this.Location.X, this.Location.Y + 35, "Damage: " + value[2], false);
                sf.DrawText(this.Location.X, this.Location.Y + 50, "Shield: " + value[3], false);
                sf.DrawText(this.Location.X, this.Location.Y + 65, "Range: " + value[4], false);
            }
        }
        public override void Draw()
        {
            
        }
    }
}
