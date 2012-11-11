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
    class CreditScreen:DxScreen
    {
        //DxButton _exitBtn = null;
        Rectangle _destRect;
        public CreditScreen(ScreenManager scrManager, DxInitGraphics graphics, Point location, Size size) :
            base(scrManager, graphics, location, size)
        {
            Initialize();
            
        }
        public override void Initialize()
        {
            base.Initialize(); 
           
            this._state = Global.ScreenState.GS_CREDIT;
            _destRect = new Rectangle(_location, _size);

        }
        public void Draw(Surface destSurface)
        {
            this._surface.ColorFill(Color.White);
            this._surface.DrawText(0, 0, "Desinger", false);
            destSurface.Draw(_destRect,_surface, DrawFlags.Wait);
        }
    }
}
