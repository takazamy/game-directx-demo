using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameDirectXDemo.Core;
using GameDirectXDemo.Manager;
using System.Drawing;

namespace GameDirectXDemo.Screens
{
    class EndScreen:DxScreen
    {
        public EndScreen(ScreenManager scrManager, DxInitGraphics graphics, Point location, Size size, GameScreen gameScreen) :
            base(scrManager, graphics, location, size)
        { 

        }
    }
}
