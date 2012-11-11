using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameDirectXDemo.Core;
using System.Drawing;
using Microsoft.DirectX.DirectInput;
using GameDirectXDemo.Manager;
using Microsoft.DirectX.DirectDraw;

namespace GameDirectXDemo.Screens
{
    class SplashScreen:DxScreen
    {
        private double _ellapsedTime = 0;
        private double _liveTime = 0;

        private DxImage bg;


        public bool IsDone
        {
            get { return _ellapsedTime >= _liveTime; }
        }



        public SplashScreen(ScreenManager scrManager, DxInitGraphics graphics, Point location, Size size, double liveTime) :
            base(scrManager, graphics, location, size)
        {

            this._liveTime = liveTime;
            this._state = Global.ScreenState.GS_SPLASH_SCREEN;
            Initialize();
        }

        public override void Initialize()
        {
            base.Initialize();
            _surface.ColorFill(Color.FromArgb(0, 255, 0, 255));
            bg = new DxImage(GameResource.SplashScreen,Global.BitmapType.SOLID,0, _graphics.DDDevice);
        }

        public override void Update(double deltaTime, KeyboardState keyState, MouseState mouseState)
        {
            _ellapsedTime += deltaTime;
            //if (SoundManager.Instance.isLoop)
            //{
            //    if (SoundManager.Instance.CheckDuration(SoundManager.SoundType.SplashScreenMusic))
            //    {
            //        SoundManager.Instance.Play(SoundManager.SoundType.SplashScreenMusic);
            //    }
            //}
            if (IsDone)
            {
                GoToMenuScreen();
            }

            HandleKeyboard(keyState);
            HandleMouse(mouseState);
            base.Update(deltaTime, keyState, mouseState);
        }
        private void GoToMenuScreen()
        {
            _scrManager._state = Global.ScreenState.GS_MENU;
            Boolean flag = false;
            foreach (DxScreen scr in _scrManager.Children)
            {
                if (scr._state == Global.ScreenState.GS_MENU)
                {
                    flag = true;
                    break;
                }
            }
            if (!flag)
            {
                MenuScreen menu = new MenuScreen(_scrManager, _graphics, Point.Empty,
                    new Size(_scrManager._renderTarget.Width, _scrManager._renderTarget.Height));
                _scrManager.Append(menu);
                //_scrManager.UpdateIndex();
            }

            _scrManager.PlayScreen(Global.ScreenState.GS_MENU);
        }
        public void HandleKeyboard(KeyboardState keyState)
        {
            if (keyState[Key.Escape])
            {
                GoToMenuScreen();
            }
        }

        public void HandleMouse(MouseState mouseState)
        {
            if (mouseState.GetMouseButtons()[0] != 0)
            {
               // SoundManager.Instance.Stop(SoundManager.SoundType.SplashScreenMusic);
                GoToMenuScreen();
            }

        }
        public override void Draw()
        {
            bg.DrawImageTo(new Rectangle(new Point(0,0),this._size), this.Surface);
           // SoundManager.Instance.Play(SoundManager.SoundType.SplashScreenMusic);
          //  SoundManager.Instance.isLoop = true;
            //   .DrawFast(_location.X, _location.Y, bg.XImage, DrawFastFlags.Wait);
            base.Draw();
        }
    }
}
