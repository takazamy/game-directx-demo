using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using Generic.TwoD.API;
using Microsoft.DirectX.DirectInput;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.InteropServices;

namespace GameDirectXDemo.Core
{
    

    class DxMouse
    {
        protected Device mice = null;
        protected DxInitGraphics graphics;
        protected DxImage MouseCur;
       // protected DxAnimationSF MouseCur1;
        GameLogic game;
        Control form1;
        public MouseState mouseState;
        Rectangle recMouse;
        byte[] buttonPressed;
        public int mousex, mousey;

        #region Using User32 to SetCurPos
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetCursorPos(int X, int Y);
        #endregion

        public bool mLeftDown;
        public bool mRightDown;
        public bool mUpPress;
        bool cursorOutOfGame = false;

        enum StatusMouse
        {
            notarget = 0,
            targetChar = 1,
        }
        public DxMouse(DxInitGraphics graphics, Control form1, GameLogic game)
        {
            this.graphics = graphics;
            this.game = game;
            this.form1 = form1;
            this.form1.MouseEnter += new EventHandler(form1_MouseEnter);
            this.form1.MouseLeave += new EventHandler(form1_MouseLeave);
            mice = new Device(SystemGuid.Mouse);
            mice.SetDataFormat(DeviceDataFormat.Mouse);
            mice.SetCooperativeLevel(form1,
                                         CooperativeLevelFlags.Foreground |
                                         CooperativeLevelFlags.NonExclusive |
                                         CooperativeLevelFlags.NoWindowsKey);
          //  MouseCur = new DxImage(new Bitmap(Ani.MouseCur0), BitmapType.TRANSPARENT, 0xFFFFFF, this.graphics.DDDevice);
            Rectangle[] listRec = new Rectangle[2];
            listRec[0] = new Rectangle(0, 0, 32, 32);
            listRec[1] = new Rectangle(32, 0, 32, 32);
           // MouseCur1 = new DxAnimationSF(this.graphics, MouseCur, listRec, AniType.Continuous, 24);
            mousex = form1.Width / 2;
            mousey = form1.Height / 2;
            SetCursorPos(mousex, mousey);
            recMouse = new Rectangle(mousex, mousey, 2, 2);
        }

        void form1_MouseLeave(object sender, EventArgs e)
        {
            this.cursorOutOfGame = true;
        }

        void form1_MouseEnter(object sender, EventArgs e)
        {
            cursorOutOfGame = false;
            SetCursorPos(mousex, mousey);
        }
        public void DrawMouse(double dLoopDuration)
        {
            //MouseCur1.Play(this.mousex, this.mousey, 1, 2, dLoopDuration);

        }
        public void UpdateMousePos(int x, int y)
        {
            Rectangle rect = new Rectangle(this.form1.Location, this.form1.Size);
            if (rect.Contains(mousex, mousey))
            //if (mousex >= 0 && mousex <= form1.Width && mousey >= 0 && mousey <= form1.Height)
            {
                this.mousex += x;
                this.mousey += y;
                SetCursorPos(mousex, mousey);
                if (mousex < 0)
                {
                    mousex = 0;
                    //SetCursorPos(mousex, mousey);
                }
                if (mousey < 0)
                {
                    mousey = 0;
                    //SetCursorPos(mousex, mousey);
                }
                if (mousex > form1.Width - 32)
                {
                    mousex = form1.Width - 32;
                    //SetCursorPos(mousex, mousey);
                }
                if (mousey > form1.Height - 32)
                {
                    mousey = form1.Height - 32;
                    //SetCursorPos(mousex, mousey);
                }
            }
            else
            {
                cursorOutOfGame = true;
                Console.WriteLine("mouse out");
            }
        }

        public void mLeftClick()
        {

        }

        public void mRightClick()
        {

        }
        public void mLeftClickDown()
        {

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

        public void getMouseState()
        {
            if (!cursorOutOfGame)
            {
                mUpPress = false;
                mouseState = GetMouseState();
                UpdateMousePos(mouseState.X, mouseState.Y);
                buttonPressed = mouseState.GetMouseButtons();
                if (buttonPressed[0] != 0)
                {
                    mLeftDown = true;
                }
                else
                {
                    if (mLeftDown == true)
                    {
                        mUpPress = true;
                    }
                    mLeftDown = false;
                }
                if (buttonPressed[1] != 0)
                {
                    mRightDown = true;
                }
                else
                {
                    mRightDown = false;
                }
            }
        }
        public void restoreSurface()
        {
            mousex = form1.Width / 2;
            mousey = form1.Height / 2;
            SetCursorPos(mousex, mousey);
           // this.MouseCur.RestoreSurface();
        }
    }
}
