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
        List<Object> PlayerList;
        List<Object> EnemyList;
        ActionScreen actionScreen;
        Global.Turn gameTurn = Global.Turn.PlayerTurn;
        int[,] colisionMap;
        public GameScreen(ScreenManager scrManager, DxInitGraphics graphics, Point location, Size size,List<Object> objects,DxTileMap tileMap) :
            base(scrManager, graphics, location, size)
        {
            this.tileMap = tileMap;
            colisionMap = tileMap.CollisionMap;
            this.objects = objects;

            Initialize();

        }

        public override void Initialize()
        {
            this._state = Global.ScreenState.GS_MAIN_GAME;
            camera = new DxCamera(Point.Empty, this.Size, this.tileMap.TileMapSurface, _graphics.DDDevice);
            actionScreen = new ActionScreen(_scrManager, _graphics, Point.Empty, new Size(60, 95), this.Surface);
            EnemyList = new List<Object>();
            PlayerList = new List<Object>();
            foreach (Object obj in objects)
            {
                if (obj.Side == Global.Side.Enemy)
                {
                    EnemyList.Add(obj);
                }
                else
                {
                    PlayerList.Add(obj);
                }
            }
            CreateGame();
        }

        private void CreateGame()
        {
            RandomPostion();
        }
        
        private void RandomPostion()
        {
            // chọn khu vực để tạo vị trí random cho quân
            try
            {
                int row, col;
                Random rand = new Random();
                int temp = 0;
                for (int i = 0; i < PlayerList.Count; i++)
                {
                    while (true)
                    {
                        temp = rand.Next(0, 4);
                        row = temp;
                        temp = rand.Next(0, 10);
                        col = temp;

                        if (colisionMap[row, col] == 0)
                        {
                            PlayerList[i].Position = new PointF((float)col * 32, (float)row * 32);//32 = frameWidth 
                            colisionMap[row, col] = -1;
                            break;
                        }
                    }

                }
                temp = 0;
                for (int i = 0; i < EnemyList.Count; i++)
                {
                    while (true)
                    {
                        temp = rand.Next(18, 22);
                        row = temp;
                        temp = rand.Next(25, 34);
                        col = temp;

                        if (colisionMap[row, col] == 0)
                        {
                            EnemyList[i].Position = new PointF((float)col * 32, (float)row * 32);//32 = frameWidth 
                            colisionMap[row, col] = -2;
                            break;
                        }
                    }

                }
            }
            catch(Exception ex)
            {
                
            }
           
        }

        public override void Update(double deltaTime, KeyboardState keyState, MouseState mouseState) 
        {
            if (gameTurn == Global.Turn.PlayerTurn)
            {
                foreach (Object obj in this.objects)
                {
                    //if (obj.Side == Global.Side.Player)
                    //{
                        obj.Update(deltaTime, keyState, mouseState);
                    //}
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
