using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Microsoft.DirectX.DirectDraw;
using Microsoft.DirectX.DirectInput;

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
        private Microsoft.DirectX.DirectDraw.Device _graphicsDevice;
        private Rectangle _sourceRect,_destRect;
        private int _cameraSpeed = 8;
        

        public DxCamera(Point position,Size surfaceSize, Surface sourceSurface, Microsoft.DirectX.DirectDraw.Device graphicsDevice)
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
            _destRect = new Rectangle(new Point(0, 0), _size);
        }
        public void Draw(Surface destSurface)
        {
            _renderSurface.Draw(_sourceSurface, _sourceRect, DrawFlags.Wait);
            destSurface.Draw(_destRect,_renderSurface, DrawFlags.Wait);
        }
        public void Update(KeyboardState state)
        {
            if (state[Key.Right])
            {
                if (_sourceRect.X < _sourceSurface.SurfaceDescription.Width - _size.Width)
                {
                    _sourceRect.X += _cameraSpeed;
                        if (_sourceRect.X > _sourceSurface.SurfaceDescription.Width - _size.Width)
                        {
                            _sourceRect.X = _sourceSurface.SurfaceDescription.Width - _size.Width;
                        }
                }
            }
            else if(state[Key.Left])
            {
                if (_sourceRect.X > 0)
                {
                    _sourceRect.X -= _cameraSpeed;
                    if (_sourceRect.X < 0)
                    {
                        _sourceRect.X = 0;
                    }                    
                }
            }
            else if (state[Key.Up])
            {
                if (_sourceRect.Y > 0)
                {
                    _sourceRect.Y -= _cameraSpeed;
                    if (_sourceRect.Y < 0)
                    {
                        _sourceRect.Y = 0;
                    }  
                }
            }
            else if (state[Key.Down])
            {
                if (_sourceRect.Y < _sourceSurface.SurfaceDescription.Height - _size.Height)
                {
                    _sourceRect.Y += _cameraSpeed;
                    if (_sourceRect.Y > _sourceSurface.SurfaceDescription.Height - _size.Height)
                    {
                        _sourceRect.Y = _sourceSurface.SurfaceDescription.Height - _size.Height;
                    }
                }
            }
        }
    }
}
