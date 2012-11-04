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
            _scrManager = new ScreenManager();
            Initialized();

            _scrManager.PlayScreen(0);
        }

        public void Initialized()
        {


            _scrManager.Append(new SplashScreen(_scrManager, _graphics, Point.Empty,
              new Size(800, 600),
              5000));
            _scrManager.Append(new MenuScreen(_scrManager, _graphics, Point.Empty,
                 new Size(800, 600)));

            //_scrManager.Append(new LevelScreen(_scrManager, _graphics, Point.Empty, new Size(800, 600)));
            //_scrManager.Append(new InstructionScreen(_scrManager, _graphics, Point.Empty, new Size(800, 600)));
            //_scrManager.Append(new CreditScreen(_scrManager, _graphics, new Point(200, 300), new Size(400, 300)));
            //string s = "Assets/level01.xml";
            //_scrManager.Append(new MainGameScreen(_scrManager, _graphics, Point.Empty, new Size(800, 600), new Core.DxInitImage("Assets/map1.jpg", _graphics.GraphicsDevice), s));
            //_scrManager.Append(new EndGameScreen(_scrManager, _graphics, Point.Empty, new Size(800, 600)));

        }

        public void Update(double deltaTime, KeyboardState keyState, MouseState mouseState)
        {
            _scrManager.Update(deltaTime, keyState, mouseState);
        }

        public void Draw(DxInitGraphics graphics)
        {
            _scrManager.Draw(graphics);
        }
    }
}
