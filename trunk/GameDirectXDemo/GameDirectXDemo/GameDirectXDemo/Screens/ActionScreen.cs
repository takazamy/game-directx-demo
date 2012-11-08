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
    public class ActionScreen:DxScreen
    {
        DxButton move;
        DxButton attack;
        DxButton endTurn;
        Surface parent;
        DxImage bg;
        public Boolean isShow = false;
        public ActionScreen(ScreenManager scrManager, DxInitGraphics graphics, Point location, Size size,Surface gameScreenSurf) :
            base(scrManager, graphics, location, size)
        {
            parent = gameScreenSurf;
            Initialize();
        }

        public override void  Initialize()
        {
 	         base.Initialize();
             bg = new DxImage(GameResource.ActionScreen, Global.BitmapType.SOLID, 0, _graphics.DDDevice);
             move = new DxButton(5, 5, GameResource.move, _graphics.DDDevice, GameResource.move.Width, GameResource.move.Height/3);
        }

        public override void Update(double deltaTime,KeyboardState keyState, MouseState mouseState)
        {
            base.Update(deltaTime, keyState, mouseState);
        }
        public override void Draw()
        {
            if (isShow)
            {
                bg.DrawImage(this.Surface);
                move.DrawFast(this.Surface);
                parent.Draw(new Rectangle(this.Location, this.Size), this.Surface, DrawFlags.Wait);
            }           
            //base.Draw();
        }
        
    }
}
