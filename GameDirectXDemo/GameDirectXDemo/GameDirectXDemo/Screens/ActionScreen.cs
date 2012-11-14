using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameDirectXDemo.Core;
using GameDirectXDemo.Manager;
using System.Drawing;
using Microsoft.DirectX.DirectInput;
using Microsoft.DirectX.DirectDraw;

namespace GameDirectXDemo.Screens
{
    public class ActionScreen:DxScreen
    {
        DxButton move;
        DxButton attack;
        DxButton endTurn;
        GameScreen parent;
        DxImage bg;
        DxImage select;
        public Boolean isShow = false;
        public Boolean isInScreen = false;
        public Object currSelect;
        public Global.ActionSreenChoice choice;
        

        public ActionScreen(ScreenManager scrManager, DxInitGraphics graphics, Point location, Size size, GameScreen gameScreen) :
            base(scrManager, graphics, location, size)
        {
            parent = gameScreen;
            Initialize();
        }

        public override void  Initialize()
        {
 	         base.Initialize();
             bg = new DxImage(GameResource.ActionScreen, Global.BitmapType.SOLID, 0, _graphics.DDDevice);
             select = new DxImage(GameResource.ActionScreenCursor, Global.BitmapType.TRANSPARENT, Color.White.ToArgb(),new Point(0,0), _graphics.DDDevice);
             choice = Global.ActionSreenChoice.NoAction;
           //  this.Surface.SurfaceDescription.SurfaceCaps.Alpha = true;
           //  this.Surface.SurfaceDescription.SurfaceCaps.Overlay = true;
             move = new DxButton(5, 5, GameResource.move, _graphics.DDDevice, GameResource.move.Width, GameResource.move.Height/3);
             attack = new DxButton(5, 35, GameResource.attack, _graphics.DDDevice, GameResource.attack.Width, GameResource.attack.Height / 3);
             endTurn = new DxButton(5, 65, GameResource.endturn, _graphics.DDDevice, GameResource.endturn.Width, GameResource.endturn.Height / 3);
        }

        public override void Update(double deltaTime,KeyboardState keyState, MouseState mouseState)
        {
            //select.Position = move.Position;
            if (isInScreen)
            {
                HanldeKey(keyState);
            }
            
            base.Update(deltaTime, keyState, mouseState);
        }

        private void HanldeKey(KeyboardState keystate)
        {
            try
            {
                if (keystate[Key.Down] && this.choice == Global.ActionSreenChoice.NoAction )
                {
                    Console.WriteLine("Action Screen Move Down");
                    PointF p = new PointF(select.Position.X, select.Position.Y + 30);
                    if (p.Y+select.FrameHeight<= this.Size.Height)
                    {
                        select.Position = p;
                    }
                    
                }
                if (keystate[Key.Up] && this.choice == Global.ActionSreenChoice.NoAction)
                {
                    Console.WriteLine("Action Screen Move Up");
                    PointF p = new PointF(select.Position.X, select.Position.Y - 30);
                    if (p.Y >= 0)
                    {
                        select.Position = p;
                    }
                    
                }
                if (keystate[Key.Left] && this.choice == Global.ActionSreenChoice.NoAction)
                {
                    Console.WriteLine("Action Screen UnInScreen");
                    this.isInScreen = false;
                }
                if (keystate[Key.Z] && this.choice == Global.ActionSreenChoice.NoAction)
                {
                    Console.WriteLine("Action Screen Choice Action");
                    CheckButtonClick(select.Position);
                    return;
                }

                if (keystate[Key.Z] && this.choice == Global.ActionSreenChoice.Move)
                {
                    Point startPoint = new Point(currSelect.Position.X/32,currSelect.Position.Y/32);
                    Point endPoint = new Point(parent.gameCursor.tileMapPosition.X/32,parent.gameCursor.tileMapPosition.Y/32);
                    if (parent.colisionMap[endPoint.Y,endPoint.X] == 0 && parent.objectMap[endPoint.Y,endPoint.X] == 0)
                    {
                        int cost = (int)parent.pathFinder.Heuristic(startPoint,endPoint);
                        if (cost <= currSelect._stamina)
                        {
                            currSelect._stamina -= cost;
                            currSelect.path = parent.pathFinder.FindPath(startPoint, endPoint);
                            parent.objectMap[startPoint.Y, startPoint.X] = 0;
                            parent.objectMap[endPoint.Y, endPoint.X] = 1;
                            this.choice = Global.ActionSreenChoice.NoAction;
                            this.isInScreen = false;
                        }
                        else
                        {
                            ResetSelectAction();
                        }
                     
                    }
                    
                    //currSelect.path = parent.path;
                    
                }
                if (keystate[Key.Z] && this.choice == Global.ActionSreenChoice.Attack)
                {
                    Object targetobj = parent.GetObjectAtPosition(parent.gameCursor.bound, Global.Side.Enemy);
                    if (CheckCanAttack(currSelect, targetobj))
                    {
                        if (currSelect._stamina > 0)
                        {
                            currSelect.Attack(targetobj);
                        }


                    }
                    else
                    {
                        ResetSelectAction();
                    }                   
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }
        private void ResetSelectAction()
        {
            this.choice = Global.ActionSreenChoice.NoAction;
            this.isInScreen = true;
            this.isShow = true;
            parent.gameCursor.enable = false;
        }

        private void CheckButtonClick(PointF pos)
        {
            try
            {
                RectangleF rect = new RectangleF(pos, new SizeF((float)select.FrameWidth, (float)select.FrameHeight));
                if (rect.Contains(move.Position))
                {
                    choice = Global.ActionSreenChoice.Move;
                    this.isShow = false;
                    this.parent.gameCursor.enable = true;
                    // move.
                }
                if (rect.Contains(attack.Position))
                {
                    choice = Global.ActionSreenChoice.Attack;
                    this.isShow = false;
                    this.parent.gameCursor.enable = true;
                }
                if (rect.Contains(endTurn.Position))
                {
                    choice = Global.ActionSreenChoice.EndTurn;
                    //Show EndTurnBox
                    //this.parent 
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }

        private Boolean CheckCanAttack(Object currObj, Object targetObj)
        {
            Point start = new Point(currObj.Position.X/32,currObj.Position.Y/32);
            Point end = new Point(targetObj.Position.X/32,targetObj.Position.Y/32);
            float range = parent.pathFinder.Heuristic(start,end);
            if (range > currObj.RangeAttack)
                return false;
            else
                return true;
            
        }

        public override void Draw()
        {
            try
            {
                if (isShow)
                {
                    
                    //bg.DrawImageTo(new Rectangle(this.Location, this.Size), this.Surface);
                    
                    bg.DrawImage(this.Surface);
                    if (isInScreen)
                    {
                        select.DrawImage(this.Surface);
                    }
                    
                    move.DrawFast(this.Surface);
                    attack.DrawFast(this.Surface);
                    endTurn.DrawFast(this.Surface);
                    parent.tileMap.TileMapSurface.Draw(new Rectangle(this.Location, this.Size), this.Surface, DrawFlags.Wait);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
            //base.Draw();
        }

        internal void SetPosition(Point point)
        {

            Point p = new Point(point.X + parent.gameCursor.size.Width,point.Y);
            if (point.X + this.Size.Width <= parent.tileMap.tilemapSize.Width)
            {
                if (p.Y+ this.Size.Height <= parent.tileMap.tilemapSize.Height)
                {
                    this.Location = p;
                }
                else 
                {
                    p.Y = parent.tileMap.tilemapSize.Height- this.Size.Height ;
                    this.Location = p;
                }
               
            }else 
	        {
                p = new Point(point.X - this.Size.Width, point.Y);
                if (p.X >= 0)
                {
                    if (p.Y + this.Size.Height <= parent.tileMap.tilemapSize.Height)
                    {
                        this.Location = p;

                    }
                    else
                    {
                        p.Y = parent.tileMap.tilemapSize.Height - this.Size.Height;
                        this.Location = p;
                    }
                }
	        }
            
        }
    }
}
