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
        #region Properties
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
        public Surface _sourceSurface;
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
        public int Rows
        {
            get { return _rows; }
        }
        protected int _columns;
        public int Columns
        {
            get { return _columns; }
        }
        public int TotalFrame
        {
            get { return _rows * _columns; }
        }
        protected Global.BitmapType _objectType;
        protected ColorKey _tempKey;

        protected int _colorKey;
        protected Rectangle _sourceRec;

        protected Rectangle[] _recArray;
        #endregion

        #region Constructor
        // Use for Image
        /// <summary>
        /// Construtor for Image with Parameter Image String Path
        /// </summary>
        /// <param name="imagePath"></param>
        /// <param name="objectType"></param>
        /// <param name="colorkey"></param>
        /// <param name="position"></param>
        /// <param name="graphicsDevice"></param>
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
        /// <summary>
        /// Constructor for Sprite with parameter Image String Path
        /// </summary>
        /// <param name="imagePath"></param>
        /// <param name="objectType"></param>
        /// <param name="colorkey"></param>
        /// <param name="position"></param>
        /// <param name="frameWidth"></param>
        /// <param name="frameHeight"></param>
        /// <param name="graphicsDevice"></param>
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
        /// <summary>
        /// Constructor for Sprite with parameter Source Bitmap
        /// </summary>
        /// <param name="sourceBitmap"></param>
        /// <param name="objectType"></param>
        /// <param name="colorkey"></param>
        /// <param name="position"></param>
        /// <param name="frameWidth"></param>
        /// <param name="frameHeight"></param>
        /// <param name="graphicsDevice"></param>
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

       

        //For Image
        /// <summary>
        /// Constructor for Image
        /// </summary>
        /// <param name="SourceBitmap"></param>
        /// <param name="ObjectType"></param>
        /// <param name="cKey"></param>
        /// <param name="TargetDevice"></param>
        public DxImage(Bitmap SourceBitmap, Global.BitmapType ObjectType, int cKey, Device TargetDevice)
        {
            // Save references to the source bitmap,
            // to the bitmap type and the device
            SourceImage = SourceBitmap;
            _objectType = ObjectType;
            _graphicsDevice = TargetDevice;            

            // Create ColorKey object
            _tempKey = new ColorKey();

            // Save Colorkey value
             _colorKey = cKey;

            // Create surface
             CreateSurface();
           // initializeSurface(cKey);
        }

        /// <summary>
        /// Constructor use for Image with sourceBitMap, and require first position for Source Image
        /// </summary>
        /// <param name="SourceBitmap"></param>
        /// <param name="ObjectType"></param>
        /// <param name="cKey"></param>
        /// <param name="position"></param>
        /// <param name="TargetDevice"></param>
        public DxImage(Bitmap SourceBitmap, Global.BitmapType ObjectType, int cKey,Point position, Device TargetDevice)
        {
            // Save references to the source bitmap,
            // to the bitmap type and the device
            SourceImage = SourceBitmap;
            _objectType = ObjectType;
            _graphicsDevice = TargetDevice;


            //Position
            _position = position;

            // Create ColorKey object
            _tempKey = new ColorKey();

            // Save Colorkey value
            _colorKey = cKey;

            // Create surface
            CreateSurface();
            // initializeSurface(cKey);
        }

#endregion

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
                case Global.BitmapType.OVERLAY:
                    _tempKey.ColorSpaceHighValue = _colorKey;
                    _tempKey.ColorSpaceLowValue = _colorKey;
                    _sourceSurface.SetColorKey(ColorKeyFlags.SourceOverlay, _tempKey);
                    break;


            }
        }

        #region DrawImage for Image
        //Use for Image
        /// <summary>
        /// Draw Image 
        /// </summary>
        /// <param name="destSurface"></param>
        public void DrawImage(Surface destSurface)
        {
            if (this._objectType == Global.BitmapType.TRANSPARENT || this._objectType == Global.BitmapType.OVERLAY)
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

        #endregion

        #region DrawImage for Sprite
        //Use for Sprite
        /// <summary>
        /// Draw Image used as Sprite
        /// </summary>
        /// <param name="frameIndex"></param>
        /// <param name="destSurface"></param>
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

        #endregion

        #region DrawImage for TileMap
        //Use for tileMap
        /// <summary>
        /// Draw Image for TileMap
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="frameIndex"></param>
        /// <param name="destSurface"></param>
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
        #endregion
    }
}
