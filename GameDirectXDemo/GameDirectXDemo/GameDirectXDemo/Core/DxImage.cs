using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Microsoft.DirectX.DirectDraw;
//using GameDirectXDemo.Global;

namespace GameDirectXDemo.Core
{
    
    public class DxImage
    {
        protected string _imagePath;
        private Image _sourceImage;

        protected Image SourceImage
        {
            get { return _sourceImage; }
            set
            { 
                _sourceImage = value;
                _frameWidth = _sourceImage.Width;
                _frameHeight = _sourceImage.Height;
            }
        }
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
        protected Global.BitmapType _objectType;
        protected ColorKey _tempKey;

        protected int _colorKey;
        protected Rectangle _sourceRec;

        protected Rectangle[] _recArray;

        // Use for Image
        public DxImage(string imagePath,Global.BitmapType objectType,int colorkey,PointF position, Device graphicsDevice)
        {
            SourceImage = Image.FromFile(imagePath);
            _graphicsDevice = graphicsDevice;
            _objectType = objectType;
            _colorKey = colorkey;
            _tempKey = new ColorKey();
            _position = position;
            CreateSurface();
        }
        //Use for sprite
        public DxImage(string imagePath, Global.BitmapType objectType, int colorkey, PointF position, int frameWidth, int frameHeight, Device graphicsDevice)
        {
            SourceImage = Image.FromFile(imagePath);
            _graphicsDevice = graphicsDevice;
            _objectType = objectType;
            _colorKey = colorkey;
            _tempKey = new ColorKey();
            _position = position;
            _frameWidth = frameWidth;
            _frameHeight = frameHeight;
            _columns = _sourceImage.Width / _frameWidth;
            _rows = _sourceImage.Height / _frameHeight;

            int column;
            int row;
            _recArray = new Rectangle[TotalFrame];
            for(int i = 0; i < _recArray.Length; i++)
            {
                column = i % _columns;
                row = i / _columns;
                _recArray[i] = new Rectangle(column * _frameWidth, row * _frameHeight, _frameWidth, _frameHeight);
            }
            CreateSurface();
        }
        //use for sprite
        public DxImage(Bitmap sourceBitmap, Global.BitmapType objectType, int colorkey, PointF position, int frameWidth, int frameHeight, Device graphicsDevice)
        {
            _sourceImage = sourceBitmap;
            _graphicsDevice = graphicsDevice;
            _objectType = objectType;
            _colorKey = colorkey;
            _tempKey = new ColorKey();
            _position = position;
            _frameWidth = frameWidth;
            _frameHeight = frameHeight;
            _columns = _sourceImage.Width / _frameWidth;
            _rows = _sourceImage.Height / _frameHeight;


            int column;
            int row;
            _recArray = new Rectangle[TotalFrame];
            for (int i = 0; i < _recArray.Length; i++)
            {
                column = i % _columns;
                row = i / _columns;
                _recArray[i] = new Rectangle(column * _frameWidth, row * _frameHeight, _frameWidth, _frameHeight);
            }
            CreateSurface();
        }

        /*public DxImage(Bitmap image, Global.BitmapType objectType, int colorkey, PointF position, int frameWidth, int frameHeight, Device graphicsDevice)
        {
            SourceImage = image;
            _graphicsDevice = graphicsDevice;
            _objectType = objectType;
            _colorKey = colorkey;
            _tempKey = new ColorKey();
            _position = position;
            _frameWidth = frameWidth;
            _frameHeight = frameHeight;
            _columns = _sourceImage.Width / _frameWidth;
            _rows = _sourceImage.Height / _frameHeight;

            int column;
            int row;
            _recArray = new Rectangle[TotalFrame];
            for (int i = 0; i < _recArray.Length; i++)
            {
                column = i % _columns;
                row = i / _columns;
                _recArray[i] = new Rectangle(column * _frameWidth, row * _frameHeight, _frameWidth, _frameHeight);
            }
            CreateSurface();
        }*/

        public DxImage(Bitmap SourceBitmap, Global.BitmapType ObjectType, int cKey, Device TargetDevice)
        {
            // Save references to the source bitmap,
            // to the bitmap type and the device
            SourceImage = SourceBitmap;
            _objectType = ObjectType;
            _graphicsDevice = TargetDevice;

            // Create a rectangle with the bitmaps dimensions
            //objectSizeRect = new Rectangle(0, 0, SourceBitmap.Width, SourceBitmap.Height);

            // Create a rectangle with the bitmaps position
           // rectPosition = new Rectangle(0, 0, SourceBitmap.Width, SourceBitmap.Height);

            // Save width and height seperately
           // iWidth = SourceBitmap.Width;
           // iHeight = SourceBitmap.Height;

            // Build surface description
           // CreateSurface();
           // initializeSurfaceDescription();

            // Create ColorKey object
            _tempKey = new ColorKey();

            // Save Colorkey value
             _colorKey = cKey;

            // Create surface
             CreateSurface();
           // initializeSurface(cKey);
        }

        /// <summary>
        /// Constructor that takes a resource string i.e. path to the image file.
        /// </summary>
        /// <param name="ObjectType">Desired bitmap type i.e SOLID or TRANSPARENT</param>
        /// <param name="TargetDevice">DirectDraw target device</param>
        /// <param name="cKey">Desired Colorkey for Image. Will be applied only if BitmapType is equal to TRANSPARENT.</param>
        /// <param name="AssemblyImageResource">Resource to images imbedded in the executables</param>
        public DxImage(Global.BitmapType ObjectType, Device TargetDevice, int cKey, string AssemblyImageResource)
        {
            // Create a new bitmap from the resource
            //sourceBitmap = new Bitmap(GetType().Module.Assembly.GetManifestResourceStream(AssemblyImageResource));

            // Save bitmap type and target device
          //  objectType = ObjectType;
          //  targetDevice = TargetDevice;

            // Create a rectangle with the bitmaps dimensions 
           // objectSizeRect = new Rectangle(0, 0, sourceBitmap.Width, sourceBitmap.Height);

            // Create a rectangle with the bitmaps position
           // rectPosition = new Rectangle(0, 0, sourceBitmap.Width, sourceBitmap.Height);

            // Save width and height seperately
          //  iWidth = sourceBitmap.Width;
           // iHeight = sourceBitmap.Height;

            // Build surface description
          //  initializeSurfaceDescription();

            // Create ColorKey object
           // tempKey = new ColorKey();

            // Save Colorkey value
           // colKey = cKey;

            // Create surface
           // initializeSurface(cKey);
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
                case Global.BitmapType.TRANSPARENT:
                    _tempKey.ColorSpaceHighValue = _colorKey;
                    _tempKey.ColorSpaceLowValue = _colorKey;
                    _sourceSurface.SetColorKey(ColorKeyFlags.SourceDraw, _tempKey);
                    break;

            }
        }
        //Use for Image
        public void DrawImage(Surface destSurface)
        {
            if (this._objectType == Global.BitmapType.TRANSPARENT)
            {
                destSurface.DrawFast((int)_position.X, (int)_position.Y, _sourceSurface, DrawFastFlags.SourceColorKey | DrawFastFlags.Wait);
            }
            else
            {
                destSurface.DrawFast((int)_position.X, (int)_position.Y, _sourceSurface, DrawFastFlags.NoColorKey | DrawFastFlags.Wait);
            }
        }
        public void DrawImageTo(Rectangle descRec,Surface destSurface)
        {
            destSurface.Draw(descRec, _sourceSurface, DrawFlags.Wait);
        }
        //Use for Sprite
        public void DrawImage(int frameIndex, Surface destSurface)
        {
            if (this._objectType == Global.BitmapType.TRANSPARENT)
            {
                destSurface.DrawFast((int)_position.X, (int)_position.Y, _sourceSurface,_recArray[frameIndex], DrawFastFlags.SourceColorKey | DrawFastFlags.Wait);
            }
            else
            {
                destSurface.DrawFast((int)_position.X, (int)_position.Y, _sourceSurface, _recArray[frameIndex], DrawFastFlags.NoColorKey | DrawFastFlags.Wait);
            }
        }

        //Use for tileMap
        public void DrawImage(int x,int y,int frameIndex, Surface destSurface)
        {
            if (this._objectType == Global.BitmapType.TRANSPARENT)
            {
                destSurface.DrawFast(x, y, _sourceSurface, _recArray[frameIndex], DrawFastFlags.SourceColorKey | DrawFastFlags.Wait);
            }
            else
            {
                destSurface.DrawFast(x, y, _sourceSurface, _recArray[frameIndex], DrawFastFlags.NoColorKey | DrawFastFlags.Wait);
            }
        }
    }
}
