using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameDirectXDemo.Core;
using GameDirectXDemo.Manager;
using System.Drawing;
using Microsoft.DirectX.DirectInput;

namespace GameDirectXDemo.Screens
{
    class EndTurnBox:DxScreen
    {
        DxImage bg;
        DxButton yes;
        DxButton no;
        Boolean isShow = false;
        Boolean isInScreen = false;
        public EndTurnBox(ScreenManager scrManager, DxInitGraphics graphics, Point location, Size size, GameScreen gameScreen) :
            base(scrManager, graphics, location, size)
        {
            Initialize();
        }

        public override void Initialize()
        {
            base.Initialize();
            bg = new DxImage(GameResource.ActionScreen, Global.BitmapType.SOLID, 0, _graphics.DDDevice);

        }

        public override void Update(double deltaTime, KeyboardState keyState, MouseState mouseState)
        {
            base.Update(deltaTime, keyState, mouseState);
        }

        public override void Draw()
        {
            base.Draw();
        }
    }
}
