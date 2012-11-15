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
        public ActionScreen actionScreen;
        public Global.Turn gameTurn = Global.Turn.EnemyTurn;
        public int turn = 1;
        public GameCursor gameCursor;
        public int[,] colisionMap;
        public int[,] objectMap;
        double counter = 0;
        public PathFinding pathFinder;
        public List<Point> path;
        public Object currSelect;
        private AI _aI;
        InfoScreen info;
        public Boolean isInScreen = false;
        public AttackStaminaChoice attStaChoice;
        public List<Global.DamageInfo> DamageList;
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
            attStaChoice = new AttackStaminaChoice(_scrManager, _graphics, this.Location, new Size(150, 100), this);
            isInScreen = true;
            DamageList = new List<Global.DamageInfo>();
            Initialize();
            _aI = new AI(EnemyList, PlayerList, colisionMap,objectMap,this);
        }

        public override void Initialize()
        {

            try
            {
                this._state = Global.ScreenState.GS_MAIN_GAME;
                gameCursor = new GameCursor(_graphics, this);
                camera = new DxCamera(new Point(150,150), this.Size, this.tileMap.TileMapSurface, _graphics.DDDevice);
                actionScreen = new ActionScreen(_scrManager, _graphics, Point.Empty, new Size(60, 95), this);
                EnemyList = new List<Object>();
                PlayerList = new List<Object>();
                int enemyIndex = 0;
                int playerIndex = 0;
                foreach (Object obj in objects)
                {
                    if (obj.Side == Global.Side.Enemy)
                    {
                        EnemyList.Add(obj);
                        EnemyList[enemyIndex].Index = enemyIndex;
                        enemyIndex++;
                    }
                    else
                    {
                        PlayerList.Add(obj);
                        PlayerList[playerIndex].Index = playerIndex;
                        playerIndex++;
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
                            PlayerList[i].Position = new Point(col * 32, row * 32);//32 = frameWidth 
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
                            EnemyList[i].Position = new Point(col * 32, row * 32);//32 = frameWidth 
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



                    //// Check gameCursor state;
                    gameCursor.Update(keyState);
                    
                    if (keyState[Key.Right] && !gameCursor.enable && !actionScreen.isInScreen)// && actionScreen.currSelect._stamina >0)                    
                    {   
                        Console.WriteLine("Enter Action Screen");
                        actionScreen.isInScreen = true;
                       // this.isInScreen = false;
                    }
                    if (keyState[Key.Z] && gameCursor.enable && actionScreen.choice == Global.ActionSreenChoice.NoAction)
                    //if (keyState[Key.Z] && this.isInScreen)
                    {
                        Console.WriteLine("Select Object");
                        IsSelect(gameCursor.tileMapPosition);                 

                    }

                    if (keyState[Key.X] && !gameCursor.enable && actionScreen.choice == Global.ActionSreenChoice.NoAction)
                    //if (keyState[Key.X] && this.isInScreen)
                    {
                        Console.WriteLine("UnSelect Object");
                        ComeBackGameScreen();
                    }
                    if (keyState[Key.X] && gameCursor.enable && actionScreen.choice != Global.ActionSreenChoice.NoAction && actionScreen.isInScreen)
                    {
                        Console.WriteLine("UnSelect Action");
                        ComeBackGameScreen();
                    }
                    //if (keyState[Key.X] && gameCursor.enable && actionScreen.choice == Global.ActionSreenChoice.NoAction && !actionScreen.isInScreen)
                    //{
                    //    Console.WriteLine("UnSelect Action");
                    //    actionScreen.SetPosition(actionScreen.currSelect.Position);
                    //    actionScreen.ResetSelectAction();
                    //}
                    if (keyState[Key.Space] && actionScreen.choice == Global.ActionSreenChoice.NoAction)
                    {
                        if (info != null)
                        {
                            if (info.isShow == true)
                            {
                                info.isShow = false;
                            }
                            else
                            {
                                Object obj = this.GetObjectAtPosition(gameCursor.bound, Global.Side.Enemy, true);
                                if (obj != null)
                                {
                                    GetObjectInfo(obj);
                                }
                               
                            }
                             
                        }
                        else
                        {
                            Object obj = this.GetObjectAtPosition(gameCursor.bound, Global.Side.Enemy, true);
                            if (obj != null)
                            {
                                GetObjectInfo(obj);
                            }
                            
                        }                                     
                      
                    }

                    attStaChoice.Update(deltaTime, keyState, mouseState);
                    actionScreen.Update(deltaTime, keyState, mouseState);
                   
                  
                    
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }

        /// <summary>
        /// Come Back State When No Choice Anything
        /// </summary>
        public void ComeBackGameScreen() 
        {
            actionScreen.isInScreen = false;
            actionScreen.isShow = false;
            gameCursor.enable = true;
            actionScreen.currSelect = null;
            this.currSelect = null;
            for (int i = 0; i < PlayerList.Count; i++)
            {
                if (PlayerList[i].isSelected)
                {
                    PlayerList[i].isSelected = false;
                    break;
                }
            }
            actionScreen.choice = Global.ActionSreenChoice.NoAction;
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
                       // Point p = new Point(gameCursor.gameScreenPosition.X + gameCursor.size.Width, gameCursor.gameScreenPosition.Y);
                        actionScreen.SetPosition(gameCursor.tileMapPosition);
                       // actionScreen.Location = p;
                        actionScreen.isShow = true;
                        actionScreen.currSelect = PlayerList[i];
                        this.currSelect = PlayerList[i];
                        gameCursor.enable = false;
                        //disabeCursor = true;
                    }
                }
            //}
            
        }

        /// <summary>
        /// Get the Object From Lists
        /// </summary>
        /// <param name="rect">GameCursor Rectangle</param>
        /// <param name="side"></param>
        /// <param name="all">Boolean if true will find in 2 Lists</param>
        /// <returns></returns>
        public Object GetObjectAtPosition(RectangleF rect, Global.Side side,Boolean all = false)
        {
            Object temp = null;
            if (all)
            {
                for (int i = 0; i < EnemyList.Count; i++)
                {

                    if (rect.Contains(EnemyList[i].positionCenter))
                    {
                        temp = EnemyList[i];
                        break;
                    }
                }
                for (int i = 0; i < PlayerList.Count; i++)
                {
                    if (rect.Contains(PlayerList[i].positionCenter))
                    {
                        temp = PlayerList[i];
                        break;
                    }
                }
            }
            else
            {
                switch (side)
                {
                    case Global.Side.Enemy:
                        for (int i = 0; i < EnemyList.Count; i++)
                        {

                            if (rect.Contains(EnemyList[i].positionCenter))
                            {
                                temp = EnemyList[i];
                                break;
                            }
                        }
                        break;
                    case Global.Side.Player:
                        for (int i = 0; i < PlayerList.Count; i++)
                        {
                            if (rect.Contains(PlayerList[i].positionCenter))
                            {
                                temp = PlayerList[i];
                                break;
                            }
                        }
                        break;
                }
            }
            return temp;
        }

        private void GetObjectInfo(Object obj)
        {
            Point p = new Point();
            info = new InfoScreen(_scrManager, _graphics, p, new Size(100, 100), obj, true);
            info.SetPosition(this.tileMap.tilemapSize, this.gameCursor.tileMapPosition, this.gameCursor.size);  

        }
        public override void Update(double deltaTime, KeyboardState keyState, MouseState mouseState) 
        {
            if (turn == 1)
            {
                for (int i = 0; i < PlayerList.Count; i++)
                {

                    PlayerList[i].Update(deltaTime, keyState, mouseState);
                    //if (PlayerList[i]._hp <= 0)
                    //{
                    //    PlayerList.RemoveAt(i);
                    //}


                }

                foreach (Object obj in this.EnemyList)
                {
                    obj.Update(deltaTime, keyState, mouseState);
                }
                    
                
            }
            counter += deltaTime;
            if (counter >= 80)
            {
                if (this.gameTurn == Global.Turn.PlayerTurn)
                {

                    for (int i = 0; i < PlayerList.Count; i++)
                    {

                        PlayerList[i].Update(deltaTime, keyState, mouseState);
                        //if (PlayerList[i]._hp <= 0)
                        //{
                        //    PlayerList.RemoveAt(i);
                        //}


                    }

                    //foreach (Object obj in this.EnemyList)
                    //{
                    //    obj.Update(deltaTime, keyState, mouseState);
                    //}
                    //    
                    //}
                    //    
                    //}


                    //if (!disabeCursor)
                    //{
                    
                    HandleKey(deltaTime,keyState,mouseState);
                    

                    counter = 0;
                }
                else
                {
                    
                    _aI.Update(deltaTime,_keyState,_mouseState);
                    //foreach (Object obj in this.objects)
                    //{
                    //    obj.Update(deltaTime, keyState, mouseState);                        
                    //}                    
                    HandleKey(deltaTime, keyState, mouseState);
                    if (_aI.IsFinishTurn)
                    {
                        ChangeTurn();
                        //_aI.IsFinishTurn = false;
                    }
                }

                for (int i = 0; i < DamageList.Count; i++)
                {
                    DamageList[i].timeline -= deltaTime;
                    DamageList[i].position.Y -= 2;
                    if (DamageList[i].timeline <= 0)
                    {
                        DamageList.RemoveAt(i);
                    }
                    
                }
                for (int i = 0; i < PlayerList.Count; i++)
                {
                    if (PlayerList[i]._hp <= 0)
                    {
                        Point endPoint = new Point(PlayerList[i].Position.X / 32, PlayerList[i].Position.Y / 32);
                        this.objectMap[endPoint.Y, endPoint.X] = 0;
                        PlayerList.RemoveAt(i);
                    }
                }
                for (int i = 0; i < EnemyList.Count; i++)
                {
                    if (EnemyList[i]._hp <= 0)
                    {
                        Point endPoint = new Point(EnemyList[i].Position.X / 32, EnemyList[i].Position.Y / 32);
                        this.objectMap[endPoint.Y, endPoint.X] = 0;
                        EnemyList.RemoveAt(i);
                    }
                }
                if (PlayerList.Count <= 0)
                {
                    GoToEndScreen(Global.Result.LOSE);
                }
                if (EnemyList.Count <= 0)
                {
                    GoToEndScreen(Global.Result.WIN);
                }
            }
            
        }
        public void SetGame(DxTileMap tileMap, List<Object> objects)
        {
            this.tileMap = tileMap;
            colisionMap = tileMap.CollisionMap;
            this.objects = objects;
            objectMap = new int[colisionMap.GetLength(0), colisionMap.GetLength(1)];
            pathFinder = new PathFinding(colisionMap);
            attStaChoice = new AttackStaminaChoice(_scrManager, _graphics, this.Location, new Size(150, 100), this);
            isInScreen = true;
            DamageList = new List<Global.DamageInfo>();
            Initialize();
            _aI = new AI(EnemyList, PlayerList, colisionMap, objectMap, this);
        }
        private void GoToEndScreen(Global.Result result)
        {
            _scrManager._state = Global.ScreenState.GS_ENDGAME;
            //Create GameScreen
            Boolean flag = false;
            foreach (DxScreen scr in _scrManager.Children)
            {
                if (scr._state == Global.ScreenState.GS_ENDGAME)
                {
                    flag = true;
                    break;
                }
            }
            if (!flag)
            {
                EndScreen end = new EndScreen(_scrManager, _graphics, this.Location, this.Size, result);
                _scrManager.Append(end);
                //_scrManager.UpdateIndex();
            }

            _scrManager.PlayScreen(Global.ScreenState.GS_ENDGAME);
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
                actionScreen.Draw();
                if (info != null)
                {
                    info.DrawTextObjectInfo(tileMap.TileMapSurface);                    
                }
                this.attStaChoice.Draw();
                foreach (Global.DamageInfo dinfo in this.DamageList)
                {
                    tileMap.TileMapSurface.DrawText(dinfo.position.X, dinfo.position.Y, dinfo.damage.ToString(), false);
                }
                camera.Draw(this.Surface);
                //actionScreen.Draw(this.Surface);
                
                base.Draw();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
            
            
        }
        public void ChangeTurn()
        {
            if (gameTurn == Global.Turn.EnemyTurn)
            {
                this.gameTurn = Global.Turn.PlayerTurn;
                actionScreen.isInScreen = false;
                gameCursor.enable = true;
                actionScreen.choice = Global.ActionSreenChoice.NoAction;
                foreach( Object obj in PlayerList)
                {
                    obj.State = Global.CharacterStatus.Idle;
                    obj.ResetStamina();
                }
                turn++;
            }
            else
            {
                this.gameTurn = Global.Turn.EnemyTurn;
                actionScreen.isInScreen = false;
                actionScreen.isShow = false;
                gameCursor.enable = true;
                actionScreen.currSelect = null;
                this.currSelect = null;
                for (int i = 0; i < PlayerList.Count; i++)
                {
                    PlayerList[i].State = Global.CharacterStatus.FinishTurn;
                    if (PlayerList[i].isSelected)
                    {
                        PlayerList[i].isSelected = false;
                        break;
                    }
                }
                if (_aI.IsFinishTurn)
                {
                    foreach (Object obj in EnemyList)
                    {
                        obj.State = Global.CharacterStatus.Idle;
                        obj.ResetStamina();

                    }
                    _aI.IsFinishTurn = false;
                }
            }
        }
    }
}
