using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Microsoft.DirectX.DirectDraw;
using System.Windows.Forms;
using Microsoft.DirectX.DirectInput;

namespace GameDirectXDemo.Core
{
    public delegate void Action();
    public class DxButton:DxImage
    {
        public Global.ButtonState _state;

        private Rectangle _bounding;

        public Rectangle Bounding
        {
            get { return _bounding; }
            set { _bounding = value; }
        }

        private byte[] _buttons;

        private Action onMouseDown = null;
        public Action OnMouseDown
        {

            set { onMouseDown = value; }
        }

        private Action onDrag = null;

        public Action OnDrag
        {

            set { onDrag = value; }
        }

        private Action onMouseUp = null;
        public Action OnMouseUp
        {
            set { onMouseUp = value; }
        }


        

        public DxButton(int x, int y, Bitmap img, Microsoft.DirectX.DirectDraw.Device graphicsDevice, int frameWidth, int frameHeight)
            :base(img, Global.BitmapType.TRANSPARENT, Color.White.ToArgb(), new PointF((float)x, (float)y), frameWidth, frameHeight, graphicsDevice)
        {
                      
            _state = Global.ButtonState.BS_NORMAL;
            _bounding = new Rectangle(x, y, base.FrameWidth, base.FrameHeight);
        }

        //public DxButton(int x, int y, DxImage image)
        //    : base(sprite.ImagePath, sprite.GraphicDevice, sprite.Framewidth, sprite.Frameheight)
        //{
        //    this.x = x;
        //    this.y = y;
        //    _state = Global.ButtonState.BS_NORMAL;
        //    //_bounding = new Rectangle(x, y, base._framewidth, base._frameheight);
        //}

        public virtual void Update(double deltaTime, MouseState state)
        {
            _buttons = state.GetMouseButtons();
            //Left Button Click
            if (_buttons[0] != 0)
            {
                //Check if Collision
                if (_bounding.Contains(Cursor.Position))
                {
                    //MouseDown
                    if (_state == Global.ButtonState.BS_NORMAL)
                    {
                        _state = Global.ButtonState.BS_HOLD;
                        if (onMouseDown != null)
                        {
                            onMouseDown();
                        }
                    }
                    //Mouse move

                    else
                    {
                        if (_state == Global.ButtonState.BS_HOLD)
                        {
                            if (onDrag != null)
                            {
                                onDrag();
                            }
                        }
                    }
                }
            }
            else
            {
                if (_state == Global.ButtonState.BS_HOLD)
                {
                    _state = Global.ButtonState.BS_NORMAL;
                    if (onMouseUp != null)
                    {
                        onMouseUp();
                    }
                }
            }
        }

        public void DrawFast(Surface destSurface)
        {
            base.DrawImage((int)_state + 1, destSurface);
        }

    }
}
