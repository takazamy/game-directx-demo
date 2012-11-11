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
        Global.Turn gameTurn = Global.Turn.EnemyTurn;
        DxImage gameCursor;
        int[,] colisionMap;
        int[,] objectMap;
        double counter = 0;
        PathFinding pathFinder;
        List<Point> path;

        public GameScreen(ScreenManager scrManager, DxInitGraphics graphics, Point location, Size size,List<Object> objects,DxTileMap tileMap) :
            base(scrManager, graphics, location, size)
        {
            this.tileMap = tileMap;
            colisionMap = tileMap.CollisionMap;
            this.objects = objects;
            objectMap = new int[colisionMap.GetLength(0), colisionMap.GetLength(1)];
            pathFinder = new PathFinding(colisionMap);
            Initialize();

        }

        public override void Initialize()
        {
            try
            {
                this._state = Global.ScreenState.GS_MAIN_GAME;
                gameCursor = new DxImage(GameResource.GameScreenCursor, Global.BitmapType.TRANSPARENT, Color.White.ToArgb(), _graphics.DDDevice);
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }

        private void CreateGame()
        {
            RandomPostion();
            path = pathFinder.FindPath(new Point((int)EnemyList[0].Position.X/32,(int)EnemyList[0].Position.Y/32),
                new Point((int)PlayerList[0].Position.X/32,(int)PlayerList[0].Position.Y/32));
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

                        if (colisionMap[row, col] == 0 && objectMap[row,col] == 0)
                        {
                            PlayerList[i].Position = new PointF((float)col * 32, (float)row * 32);//32 = frameWidth 
                            objectMap[row, col] = 1;
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

                        if (colisionMap[row, col] == 0 && objectMap[row,col] == 0)
                        {
                            EnemyList[i].Position = new PointF((float)col * 32, (float)row * 32);//32 = frameWidth 
                            objectMap[row, col] = 2;
                            break;
                        }
                    }

                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
           
        }
        private void HandleKey(KeyboardState keyState)
        {
            try
            {
                if (keyState != null)
                {
                    // on escape -> exit

                    if (keyState[Key.Right])
                    {
                        PointF p = new PointF(gameCursor.Position.X+32,gameCursor.Position.Y);
                        if (p.X + gameCursor.FrameWidth*2 > this.Size.Width)
                        {
                            camera.Update(keyState);
                        }
                        else
                        {
                            gameCursor.Position = p;
                        }
                        
                       
                    }
                    if (keyState[Key.Left])
                    {
                        PointF p = new PointF(gameCursor.Position.X - 32, gameCursor.Position.Y);
                        if (p.X < 0)
                        {
                            camera.Update(keyState);
                        }
                        else
                        {
                            gameCursor.Position = p;
                        }
                       
                        
                    }
                    if (keyState[Key.Down])
                    {
                        PointF p = new PointF(gameCursor.Position.X, gameCursor.Position.Y+32);
                        if (p.Y + gameCursor.FrameHeight * 2 > this.Size.Height)
                        {
                            camera.Update(keyState);
                        }
                        else
                        {
                            gameCursor.Position = p;
                        }
                        
                    }
                    if (keyState[Key.Up])
                    {
                        PointF p = new PointF(gameCursor.Position.X, gameCursor.Position.Y - 32);
                        if (p.Y < 0)
                        {
                            camera.Update(keyState);
                        }
                        else
                        {
                            gameCursor.Position = p;
                        }
                       
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        public override void Update(double deltaTime, KeyboardState keyState, MouseState mouseState) 
        {
            
            counter += deltaTime;
            if (counter >= 100)
            {
                foreach (Object obj in this.objects)
                {
                    //if (obj.Side == Global.Side.Player)
                    //{
                    obj.Update(deltaTime, keyState, mouseState);
                    //}
                }
                if (gameTurn == Global.Turn.PlayerTurn)
                {
                    
                    PlayerList[0].Move(path);
                }
                else if(gameTurn == Global.Turn.EnemyTurn)
                {
                    foreach(Object obj in this.EnemyList)
                    {
                        obj.Update(deltaTime, keyState, mouseState);
                    }
                    EnemyList[0].Move(path);
                }



                HandleKey(keyState);
                counter = 0;
            }
            
            
            //camera.Update(keyState);
            
        }

        public override void Draw() 
        {
           //
            try
            {
               
                this.tileMap.DrawTileMap();
                foreach (Object obj in this.PlayerList)
                {
                    //if (obj.Side == Global.Side.Player)
                    //{
                    obj.Draw(tileMap.TileMapSurface);
                    //}
                    
                }
                foreach (Object obj in this.EnemyList)
                {
                    //if (obj.Side == Global.Side.Player)
                    //{
                    obj.Draw(tileMap.TileMapSurface);
                    //}
                    
                }
               
                
                camera.Draw(this.Surface);
                //actionScreen.Draw(this.Surface);
                actionScreen.Draw();
                gameCursor.DrawImage(this.Surface);
                base.Draw();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
            
            
        }
    }
}
