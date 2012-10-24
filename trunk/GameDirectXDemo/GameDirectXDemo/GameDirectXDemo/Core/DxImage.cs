using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Microsoft.DirectX.DirectDraw;

namespace GameDirectXDemo.Core
{
    public enum BitmapType
    {
        SOLID = 0,
        TRANSPARENT = 1
    }
    public class DxImage
    {
        protected string _imagePath;
        protected Image _sourceImage;
        protected Surface _sourceSurface;
        protected Device _graphicsDevice;
        public Device GraphicsDevice
        {
            get
            {
                return _graphicsDevice;
            }
            set
            {
                _graphicsDevice = value;
            }
        }
        protected PointF _position;
        public PointF Position
        {
            get
            {
                return _position;
            }
            set
            {
                _position = value;
            }
        }
        protected PointF _lastPosition;
        public PointF LastPosition
        {
            get
            {
                return _lastPosition;
            }
            set
            {
                _lastPosition = value;
            }
        }
        protected int _frameWidth;
        public int FrameWidth
        {
            get { return _frameWidth; }
            set { _frameWidth = value; }
        }        
        protected int _frameHeight;
        public int FrameHeight
        {
            get { return _frameHeight; }
            set { _frameHeight = value; }
        }
        protected int _rows;
        protected int _columns;
        public int TotalFrame
        {
            get { return _rows * _columns; }
        }
        protected BitmapType _objectType;
        protected ColorKey _tempKey;
        protected int _colorKey;
        protected Rectangle _sourceRec;

        // Use for Image
        public DxImage(string imagePath,BitmapType objectType,int colorkey,PointF position, Device graphicsDevice)
        {
            _sourceImage = Image.FromFile(imagePath);
            _graphicsDevice = graphicsDevice;
            _objectType = objectType;
            _colorKey = colorkey;
            _tempKey = new ColorKey();
            _position = position;
            CreateSurface();
        }
        //Use for sprite
        public DxImage(string imagePath, BitmapType objectType, int colorkey, PointF position, int frameWidth, int frameHeight, Device graphicsDevice)
        {
            _sourceImage = Image.FromFile(imagePath);
            _graphicsDevice = graphicsDevice;
            _objectType = objectType;
            _colorKey = colorkey;
            _tempKey = new ColorKey();
            _position = position;
            _frameWidth = frameWidth;
            _frameHeight = frameHeight;
            _columns = _sourceImage.Width / _frameWidth;
            _rows = _sourceImage.Height / _frameHeight;
            CreateSurface();
        }

        private void CreateSurface()
        {            
            SurfaceDescription desc = new SurfaceDescription();
            desc.SurfaceCaps.OffScreenPlain = true;
            desc.Width = _sourceImage.Width;
            desc.Height = _sourceImage.Height;
            _sourceSurface = new Surface(new Bitmap(_sourceImage), desc, _graphicsDevice);
            switch (_objectType)
            {
                case BitmapType.TRANSPARENT:
                    _tempKey.ColorSpaceHighValue = _colorKey;
                    _tempKey.ColorSpaceLowValue = _colorKey;
                    _sourceSurface.SetColorKey(ColorKeyFlags.SourceDraw, _tempKey);
                    break;

            }
        }
        //Use for Image
        public void DrawImage(Surface destSurface)
        {
            if (this._objectType == BitmapType.TRANSPARENT)
            {
                destSurface.DrawFast((int)_position.X, (int)_position.Y, _sourceSurface, DrawFastFlags.SourceColorKey | DrawFastFlags.Wait);
            }
            else
            {
                destSurface.DrawFast((int)_position.X, (int)_position.Y, _sourceSurface, DrawFastFlags.NoColorKey | DrawFastFlags.Wait);
            }
        }
        //Use for Sprite
        public void DrawImage(int frameIndex, Surface destSurface)
        {
            int column = frameIndex % _columns;
            int row = frameIndex / _columns;
            _sourceRec = new Rectangle(column * _frameWidth, row * _frameHeight, _frameWidth, _frameHeight);
            if (this._objectType == BitmapType.TRANSPARENT)
            {
                destSurface.DrawFast((int)_position.X, (int)_position.Y, _sourceSurface,_sourceRec, DrawFastFlags.SourceColorKey | DrawFastFlags.Wait);
            }
            else
            {
                destSurface.DrawFast((int)_position.X, (int)_position.Y, _sourceSurface,_sourceRec, DrawFastFlags.NoColorKey | DrawFastFlags.Wait);
            }
        }
    }
}
