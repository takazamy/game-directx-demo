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
        ActionScreen actionScreen;
        Global.Turn gameTurn = Global.Turn.PlayerTurn;
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
            actionScreen = new ActionScreen(_scrManager, _graphics, Point.Empty, new Size(60, 95), this.Surface);
            CreateGame();
        }

        private void CreateGame()
        {
           
        }

        private void RandomPostion()
        {
            // chọn khu vực để tạo vị trí random cho quân
           
        }

        public override void Update(double deltaTime, KeyboardState keyState, MouseState mouseState) 
        {
            if (gameTurn == Global.Turn.PlayerTurn)
            {
                foreach (Object obj in this.objects)
                {
                    if (obj.Side == Global.Side.Player)
                    {
                        obj.Update(deltaTime, keyState, mouseState);
                    }
                }
            }
            camera.Update(keyState);
            
        }

        public override void Draw() 
        {
           //
            try
            {
                foreach (Object obj in this.objects)
                {
                    //if (obj.Side == Global.Side.Player)
                    //{
                    obj.Draw(tileMap.TileMapSurface);
                    //}
                }
                camera.Draw(this.Surface);
                //actionScreen.Draw(this.Surface);
                actionScreen.Draw();

                base.Draw();
            }
            catch (Exception ex)
            { 
                
            }
            
            
        }
    }
}
