using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Microsoft.DirectX.DirectDraw;
using GameDirectXDemo.Core;
using GameDirectXDemo.Manager;

namespace GameDirectXDemo.Screens
{
    class HowToPlayScreen:DxScreen
    {
        Rectangle _destRect;
        public HowToPlayScreen(ScreenManager scrManager, DxInitGraphics graphics, Point location, Size size)
            : base(scrManager, graphics, location, size)
        {
            Initialize();
            
        }

        public override void Initialize()
        {
            base.Initialize();
            this._state = Global.ScreenState.GS_HELP;
            _destRect = new Rectangle(_location, _size);
        }
        public void Draw(Surface destSurface)
        {
            this._surface.ColorFill(Color.Yellow);
            this._surface.DrawText(0, 0, "How to play", false);
            destSurface.Draw(_destRect, _surface, DrawFlags.Wait);
        }
    }
}
