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
    public class ActionScreen:DxScreen
    {
        public ActionScreen(ScreenManager scrManager, DxInitGraphics graphics, Point location, Size size) :
            base(scrManager, graphics, location, size)
        {

        }

        public override void  Initialize()
        {
 	         base.Initialize();
        }

        public override void Update(double deltaTime,KeyboardState keyState, MouseState mouseState)
        {
            base.Update(deltaTime, keyState, mouseState);
        }

        public override void Draw()
        {
            base.Draw();
        }
    }
}
