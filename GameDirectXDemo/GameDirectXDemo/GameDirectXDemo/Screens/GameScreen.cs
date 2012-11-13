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
        public DxTileMap tileMap;
        public DxCamera camera;
        List<Object> objects;
        List<Object> PlayerList;
        List<Object> EnemyList;
        ActionScreen actionScreen;
        Global.Turn gameTurn = Global.Turn.PlayerTurn;
       
        GameCursor gameCursor;
        int[,] colisionMap;
        int[,] objectMap;
        double counter = 0;
        PathFinding pathFinder;
        List<Point> path;
        Object currSelect;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="scrManager"></param>
        /// <param name="graphics"></param>
        /// <param name="location"></param>
        /// <param name="size"></param>
        /// <param name="objects"></param>
        /// <param name="tileMap"></param>
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
                gameCursor = new GameCursor(_graphics, this);
                camera = new DxCamera(Point.Empty, this.Size, this.tileMap.TileMapSurface, _graphics.DDDevice);
                actionScreen = new ActionScreen(_scrManager, _graphics, Point.Empty, new Size(60, 95), this);
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

        #region Create Game
        /// <summary>
        /// Create the elements for game
        /// </summary>
        private void CreateGame()
        {
            RandomPostion();
            path = pathFinder.FindPath(new Point((int)EnemyList[0].Position.X / 32, (int)EnemyList[0].Position.Y / 32),
                new Point((int)PlayerList[0].Position.X / 32, (int)PlayerList[0].Position.Y / 32));
        }
        /// <summary>
        /// Function Create Position for all Object in game
        /// </summary>
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

        #endregion
        /// <summary>
        /// Manage Interaction Through Keyboard
        /// </summary>
        /// <param name="keyState">Array of keys'value </param>
        private void HandleKey(double deltaTime,KeyboardState keyState,MouseState mouseState)
        {
            try
            {
                if (Global.CheckKeyDown(keyState))
                {
                    // Check gameCursor state;
                    gameCursor.Update(keyState);
                    if (!actionScreen.isInScreen && actionScreen.isShow == false)
                    {
                        if (keyState[Key.Z])
                        {
                            //if (!disabeCursor)
                            //{
                            IsSelect(gameCursor.tileMapPosition);
                            //}

                        }
                        if (keyState[Key.X])
                        {
                            //IsSelect(gameCursor.tileMapPosition);
                            actionScreen.isShow = false;
                            gameCursor.enable = true;
                            for (int i = 0; i < PlayerList.Count; i++)
                            {
                                if (PlayerList[i].isSelected)
                                {
                                    PlayerList[i].isSelected = false;
                                    break;
                                }
                            }
                        }
                        
                          
                    }else if (!actionScreen.isInScreen)
                    {
                        if (keyState[Key.Right])
                        {
                            actionScreen.isInScreen = true;
                        }
                        if (keyState[Key.Z])
                        {
                            //if (!disabeCursor)
                            //{
                            IsSelect(gameCursor.tileMapPosition);
                            //}

                        }
                        if (keyState[Key.X])
                        {
                            //IsSelect(gameCursor.tileMapPosition);
                            actionScreen.isShow = false;
                            gameCursor.enable = true;
                            for (int i = 0; i < PlayerList.Count; i++)
                            {
                                if (PlayerList[i].isSelected)
                                {
                                    PlayerList[i].isSelected = false;
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        actionScreen.Update(deltaTime, keyState, mouseState); 
                    }
                   
                    
                }
            }
            catch (Exception ex)
            {

            }
        }
        /// <summary>
        /// Check the selected of Objects
        /// </summary>
        /// <param name="position">the game cursor position</param>
        private void IsSelect(PointF position)
        {
            bool haveTrue = false;
            //if (gameCursor.enable)
            //{
                for (int i = 0; i < PlayerList.Count; i++)
                {
                    if (!PlayerList[i].IsSelected(position))
                    {
                        if (!haveTrue)
                        {
                            actionScreen.isShow = false;
                            gameCursor.enable = true;
                        }

                    }
                    else
                    {
                        haveTrue = true;
                        //Console.WriteLine("true:" + i);
                        Point p = new Point((int)gameCursor.gameScreenPosition.X + gameCursor.size.Width, (int)gameCursor.gameScreenPosition.Y);
                        actionScreen.Location = p;
                        actionScreen.isShow = true;
                        actionScreen.currSelect = PlayerList[i];
                        this.currSelect = PlayerList[i];
                        gameCursor.enable = false;
                        //disabeCursor = true;
                    }
                }
            //}
            
        }

        public override void Update(double deltaTime, KeyboardState keyState, MouseState mouseState) 
        {
            
            counter += deltaTime;
            if (counter >= 100)
            {
                if (this.gameTurn == Global.Turn.PlayerTurn)
                {
                    foreach (Object obj in this.PlayerList)
                    {
                        obj.Update(deltaTime, keyState, mouseState);
                    }
                    //if (gameTurn == Global.Turn.PlayerTurn)
                    //{

                        //PlayerList[0].Move(path);
                    //}
                    //else if (gameTurn == Global.Turn.EnemyTurn)
                    //{
                        foreach (Object obj in this.EnemyList)
                        {
                            obj.Update(deltaTime, keyState, mouseState);
                        }
                    //    
                    //}


                    //if (!disabeCursor)
                    //{
                    HandleKey(deltaTime,keyState,mouseState);
                    //}
                    //else
                    // {
                    //actionScreen.Update(deltaTime, keyState, mouseState);
                    // }

                    counter = 0;
                }
                else
                {
                    foreach (Object obj in this.EnemyList)
                    {
                        //if (obj.Side == Global.Side.Player)
                        //{
                        //obj.Update(deltaTime, keyState, mouseState);
                        //}
                        //EnemyList[0].Move(path);
                    }
                }
                
            }
            
            
            
            
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
                gameCursor.Draw(tileMap.TileMapSurface);
                
                camera.Draw(this.Surface);
                //actionScreen.Draw(this.Surface);
                actionScreen.Draw();
                base.Draw();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
            
            
        }
    }
}
