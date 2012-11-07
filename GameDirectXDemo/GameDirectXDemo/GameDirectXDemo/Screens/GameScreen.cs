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
        DxCamera camera;
        List<Object> objects;
        public GameScreen(ScreenManager scrManager, DxInitGraphics graphics, Point location, Size size,List<Object> objects,DxTileMap tileMap) :
            base(scrManager, graphics, location, size)
        {
            this.tileMap = tileMap;
            this.objects = objects;

            Initialize();

        }

        public override void Initialize()
        {
            this._state = Global.ScreenState.GS_MAIN_GAME;
            camera = new DxCamera(Point.Empty, this.Size, this.tileMap.TileMapSurface, _graphics.DDDevice);
            
        }

        public void CreateGame()
        {
           // objects = new Object(
        }
        public override void Update(double deltaTime, KeyboardState keyState, MouseState mouseState) 
        { 
            
        }

        public override void Draw() 
        {
            camera.Draw(this.Surface);
            base.Draw();
        }
    }
}
