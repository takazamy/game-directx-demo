using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Microsoft.DirectX.DirectDraw;

namespace GameDirectXDemo.Core
{
    public class DxCamera
    {
        private Point _position;
        public Point Position
        {
            get { return _position; }
            set { _position = value; }
        }
        private Surface _renderSurface;
        public Surface RenderSurface
        {
            get { return _renderSurface; }
        }
        private Size _size;
        private Surface _sourceSurface;
        private Device _graphicsDevice;
        private Rectangle _sourceRect;

        public DxCamera(Point position,Size surfaceSize, Surface sourceSurface, Device graphicsDevice)
        {
            _position = position;
            _sourceSurface = sourceSurface;
            _graphicsDevice = graphicsDevice;
            _size = surfaceSize;
            CreateRenderSurface();
        }
        private void CreateRenderSurface()
        {
            SurfaceDescription desc = new SurfaceDescription();
            desc.SurfaceCaps.OffScreenPlain = true;
            desc.Width = _size.Width;
            desc.Height = _size.Height;
            _renderSurface = new Surface(desc, _graphicsDevice);
            _sourceRect = new Rectangle(_position, _size);
        }
        public void Draw(Surface destSurface)
        {
            _renderSurface.Draw(_sourceSurface, _sourceRect, DrawFlags.Wait);
            destSurface.Draw(_sourceRect,_renderSurface, DrawFlags.Wait);
        }
    }
}
