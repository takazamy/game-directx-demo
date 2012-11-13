﻿using System;
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
                if (keystate[Key.Down])
                {
                    PointF p = new PointF(select.Position.X, select.Position.Y + 30);
                    if (p.Y+select.FrameHeight<= this.Size.Height)
                    {
                        select.Position = p;
                    }
                    
                }
                if (keystate[Key.Up])
                {
                    PointF p = new PointF(select.Position.X, select.Position.Y - 30);
                    if (p.Y >= 0)
                    {
                        select.Position = p;
                    }
                    
                }
                if (keystate[Key.Left] || keystate[Key.X])
                {
                    this.isInScreen = false;
                }
                if (keystate[Key.Z])
                {
                    //CheckButtonClick(select.Position);
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void CheckButtonClick(PointF pos)
        {
            try
            {
                RectangleF rect = new RectangleF(pos, new SizeF((float)select.FrameWidth, (float)select.FrameHeight));
                if (rect.Contains(move.Position))
                {
                    choice = Global.ActionSreenChoice.Move;
                    // move.
                }
                if (rect.Contains(attack.Position))
                {
                    choice = Global.ActionSreenChoice.Attack;
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
            }
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
                    parent.Surface.Draw(new Rectangle(this.Location, this.Size), this.Surface, DrawFlags.Wait);
                }
            }
            catch (Exception ex)
            {

            }
            //base.Draw();
        }
        
    }
}
