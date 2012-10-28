using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameDirectXDemo.Core;
using System.Drawing;
using Microsoft.DirectX.DirectInput;
using GameDirectXDemo.Manager;

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
            Initialize();
        }

        public override void Initialize()
        {
            base.Initialize();
            _surface.ColorFill(Color.FromArgb(0, 255, 0, 255));
         //   bg = new DxImage("Resources/SplashScreen.png", _graphics.DDDevice);
        }

        public override void Update(double deltaTime, KeyboardState keyState, MouseState mouseState)
        {
            _ellapsedTime += deltaTime;
            if (SoundManager.Instance.isLoop)
            {
                if (SoundManager.Instance.CheckDuration(SoundManager.SoundType.SplashScreenMusic))
                {
                    SoundManager.Instance.Play(SoundManager.SoundType.SplashScreenMusic);
                }
            }
            if (IsDone)
            {
                //_scrManager._state = TestDirectX2.ScreenManager.GameState.GS_MENU;
                _scrManager.NextScreen();
            }

            HandleKeyboard(keyState);
            HandleMouse(mouseState);
            base.Update(deltaTime, keyState, mouseState);
        }

        public void HandleKeyboard(KeyboardState keyState)
        {
            if (keyState[Key.Escape])
            {
                //_scrManager._state = TestDirectX2.ScreenManager.GameState.GS_MENU;
                _scrManager.NextScreen();
            }
        }

        public void HandleMouse(MouseState mouseState)
        {
            if (mouseState.GetMouseButtons()[0] != 0)
            {
                SoundManager.Instance.Stop(SoundManager.SoundType.SplashScreenMusic);
                //_scrManager._state = TestDirectX2.ScreenManager.GameState.GS_MENU;
                _scrManager.NextScreen();
            }

        }
        public override void Draw()
        {
           // bg.DrawFast(0, 0, base.Surface, DrawFastFlags.Wait);
            SoundManager.Instance.Play(SoundManager.SoundType.SplashScreenMusic);
            SoundManager.Instance.isLoop = true;
            //   .DrawFast(_location.X, _location.Y, bg.XImage, DrawFastFlags.Wait);
            base.Draw();
        }
    }
}
