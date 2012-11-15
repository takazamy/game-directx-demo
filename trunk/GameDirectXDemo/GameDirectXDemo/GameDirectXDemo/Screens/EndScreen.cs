using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameDirectXDemo.Core;
using GameDirectXDemo.Manager;
using System.Drawing;
using Microsoft.DirectX.DirectDraw;
using Microsoft.DirectX.DirectInput;

namespace GameDirectXDemo.Screens
{
    class EndScreen:DxScreen
    {
        DxImage win;
        DxImage lose;
        DxImage curr;
        public EndScreen(ScreenManager scrManager, DxInitGraphics graphics, Point location, Size size, Global.Result result) :
            base(scrManager, graphics, location, size)
        {
            win = new DxImage(GameResource.WinScreen, Global.BitmapType.SOLID, 0, _graphics.DDDevice);
            lose = new DxImage(GameResource.LoseScreen, Global.BitmapType.SOLID, 0, _graphics.DDDevice);
            if (result == Global.Result.WIN)
            {
                curr = win;
            }
            else
            {
                curr = lose;
            }
        }


        public override void Update(double deltaTime, KeyboardState keyState, MouseState mouseState)
        {
            base.Update(deltaTime, keyState, mouseState);
            if (keyState[Key.Escape])
            {
                _scrManager._state = Global.ScreenState.GS_MENU;
                Boolean flag = false;
                foreach (DxScreen scr in _scrManager.Children)
                {
                    if (scr._state == Global.ScreenState.GS_MENU)
                    {
                        flag = true;
                        break;
                    }
                }
                if (!flag)
                {
                    MenuScreen menu = new MenuScreen(_scrManager, _graphics, Point.Empty,
                        new Size(_scrManager._renderTarget.Width, _scrManager._renderTarget.Height));
                    _scrManager.Append(menu);
                    //_scrManager.UpdateIndex();
                }

                _scrManager.PlayScreen(Global.ScreenState.GS_MENU);
            }
        }

        public override void Draw()
        {
            this.Surface.Draw(new Rectangle(Point.Empty,this.Size),curr._sourceSurface, DrawFlags.Wait);
            base.Draw();
        } 
    }
}
