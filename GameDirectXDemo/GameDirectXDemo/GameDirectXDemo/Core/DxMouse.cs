using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using Generic.TwoD.API;
using Microsoft.DirectX.DirectInput;
using System.Windows.Forms;

namespace GameDirectXDemo.Core
{
    enum CursorState
    {
        Normal,
        Highlight
    }

    class DxMouse
    {
        int x, y;
        DxImageObject imageObject;
        DxInitGraphics graphics;

        protected Device mice = null;
        protected byte[] ButtonPressed;

        CursorState currentState = CursorState.Normal;
        public CursorState CurrentState
        {
            get
            {
                return currentState;
            }
            set
            {
                currentState = value;
            }
        }

        public int X
        {
            get
            {
                return x;
            }
        }

        public int Y
        {
            get
            {
                return y;
            }
        }

        public DxMouse(Control gameForm, DxInitGraphics graphics)
        {
            this.graphics = graphics;
            // Create a new Device with the keyboard guid
            mice = new Device(SystemGuid.Mouse);

            // Set data format to keyboard data
            mice.SetDataFormat(DeviceDataFormat.Mouse);

            // Set the cooperative level to foreground non-exclusive
            // and deactivate windows key
            mice.SetCooperativeLevel(gameForm,
                                         CooperativeLevelFlags.Foreground |
                                         CooperativeLevelFlags.NonExclusive |
                                         CooperativeLevelFlags.NoWindowsKey);

            imageObject = new DxImageObject(GameResource.Cursor, BitmapType.TRANSPARENT, 0xFFFFFF, graphics.DDDevice);

        }

        public void Update()
        {
           
            // This will save the current Mouse state
            MouseState state;

            // Get the Mouse state
            state = GetMouseState();
            this.x += state.X;
            this.y += state.Y;
        }

        public void Draw()
        {

            imageObject.DrawBitmap(graphics.RenderSurface, this.X, this.Y);
        }

        public MouseState GetMouseState()
        {
            // This will hold the current Mouse state
            MouseState state;

            do
            {
                // Try to get the current state
                try
                {
                    state = this.mice.CurrentMouseState;

                    // if fetching the state is successful -> exit loop
                    break;
                }
                catch (InputException)
                {
                    // let the application handle Windows messages
                    Application.DoEvents();

                    // Try to get reacquire the mice 
                    // and don't care about exceptions
                    try
                    {
                        mice.Acquire();
                    }
                    catch (InputLostException)
                    {
                        continue;
                    }
                    catch (OtherApplicationHasPriorityException)
                    {
                        continue;
                    }
                }
            }
            while (true); // Do this until it's successful

            // return the retrieved keyboard state
            return state;
        }
    }
}
