using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameDirectXDemo.Core;
using GameDirectXDemo.Manager;
using System.Drawing;
using Microsoft.DirectX.DirectInput;

namespace GameDirectXDemo.Screens
{
    public class GameScreen:DxScreen
    {
        DxTileMap tileMap;
        Object[] objects;
        public GameScreen(ScreenManager scrManager, DxInitGraphics graphics, Point location, Size size) :
            base(scrManager, graphics, location, size)
        {
            Initialize();

        }

        public override void Initialize()
        {
            
            
        }

        public void CreateGame()
        {
           // objects = new Object(
        }
        public void Update(double deltaTime, KeyboardState keyState, MouseState mouseState) 
        { 
            
        }

        public void Draw() 
        {
            base.Draw();
        }
    }
}
