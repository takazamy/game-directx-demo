using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.DirectX.DirectInput;
using GameDirectXDemo.Core;
using System.Windows.Forms;
using GameDirectXDemo.Screens;
using System.Drawing;

namespace GameDirectXDemo.Manager
{
    public class GameManager
    {
        private DxInitGraphics _graphics;
        private Control _renderTarget;

        //public static Camera _camera;
        public Control RenderTarget
        {
            get { return _renderTarget; }
            set { _renderTarget = value; }
        }

        public DxInitGraphics Graphics
        {
            get { return _graphics; }
            set { _graphics = value; }
        }

        public ScreenManager _scrManager;

        public GameManager(Control parent, DxInitGraphics graphics)
        {
            _renderTarget = parent;
            _graphics = graphics;
            _scrManager = new ScreenManager(_renderTarget);
            Initialized();

            _scrManager.PlayScreen(0);
        }

        public void Initialized()
        {
            

            _scrManager.Append(new SplashScreen(_scrManager, _graphics, Point.Empty,
              new Size(_renderTarget.Width, _renderTarget.Height),
              5000));
            
            _scrManager.CurrentScreen = _scrManager.Children[0];
        }

        public void Update(double deltaTime, KeyboardState keyState, MouseState mouseState)
        {
            _scrManager.Update(deltaTime, keyState, mouseState);
        }

        public void Draw(DxInitGraphics graphics)
        {
            try
            {
                _scrManager.Draw(graphics);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }
    }
}
