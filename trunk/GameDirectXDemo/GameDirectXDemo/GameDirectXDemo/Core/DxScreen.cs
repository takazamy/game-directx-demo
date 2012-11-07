using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Microsoft.DirectX.DirectDraw;
using Microsoft.DirectX.DirectInput;
using GameDirectXDemo.Screens;
using GameDirectXDemo.Manager;

namespace GameDirectXDemo.Core
{
    public class DxScreen
    {
        protected Point _location;
        public Point Location
        {
            get { return _location; }
            set { _location = value; }
        }

        protected Size _size;
        public Size Size
        {
            get { return _size; }
            set { _size = value; }
        }

        protected Surface _surface;
        public Surface Surface
        {
            get { return _surface; }
        }

        protected DxInitGraphics _graphics;
        public DxInitGraphics Graphics
        {
            get { return _graphics; }
            set { _graphics = value; }
        }

        protected KeyboardState _keyState;
        protected MouseState _mouseState;
        protected ScreenManager _scrManager;
        public Global.ScreenState _state;
        public int _index;

        public DxScreen(ScreenManager scrManager, DxInitGraphics graphics, Point location, Size size)
        {
            _scrManager = scrManager;
            _graphics = graphics;
            _location = location;
            _size = size;
            Restore();
        }

        public virtual void Restore()
        {
            SurfaceDescription desc = new SurfaceDescription();
            desc.SurfaceCaps.OffScreenPlain = true;

            desc.Width = _size.Width;
            desc.Height = _size.Height;

            _surface = new Surface(desc, _graphics.DDDevice);
        }

        public virtual void Initialize() { }

        public virtual void Update(double deltaTime, KeyboardState keyState, MouseState mouseState)
        {
            _keyState = keyState;
            _mouseState = mouseState;
        }

        public virtual void Draw()
        {
            _graphics.RenderSurface.DrawFast(_location.X, _location.Y, _surface, DrawFastFlags.Wait);
        }
    }
}
