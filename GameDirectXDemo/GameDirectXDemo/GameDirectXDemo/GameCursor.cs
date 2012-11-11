﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameDirectXDemo.Core;
using System.Drawing;
using Microsoft.DirectX.DirectInput;
using Microsoft.DirectX.DirectDraw;
using GameDirectXDemo.Screens;

namespace GameDirectXDemo
{
    class GameCursor
    {
        DxImage cursorImage;
        public PointF tileMapPosition;
        public PointF gameScreenPosition;
        DxInitGraphics _graphics;
        public Boolean enable = true;
        public Size size;
        GameScreen parent;
        Size tilemapSize;
        public GameCursor(DxInitGraphics graphics,GameScreen parent)
        {
            _graphics = graphics;
            cursorImage = new DxImage(GameResource.GameScreenCursor, Global.BitmapType.TRANSPARENT, Color.White.ToArgb(), _graphics.DDDevice);
            gameScreenPosition = new PointF(0, 0);
            tileMapPosition = new PointF(0, 0);
            this.parent = parent;
            tilemapSize = new Size(parent.tileMap.TileMapSurface.SurfaceDescription.Width, parent.tileMap.TileMapSurface.SurfaceDescription.Height);
            size = new Size(cursorImage.FrameWidth, cursorImage.FrameHeight);
        }

        public void Update(KeyboardState keystate)
        {
            if (enable)
            {
                HandleKey(keystate);
                cursorImage.Position = tileMapPosition;
            }
        }

        private void HandleKey(KeyboardState keystate)
        {
            if (keystate[Key.Right])
            {
                PointF p = new PointF(tileMapPosition.X + this.size.Width, tileMapPosition.Y);
                if (p.X + this.size.Width <= tilemapSize.Width)
                {
                    tileMapPosition = p;
                }
                p = new PointF(gameScreenPosition.X + this.size.Width, gameScreenPosition.Y);
                if (p.X + this.size.Width <= parent.Size.Width)
                {
                    gameScreenPosition = p;
                }
                else
                {
                    parent.camera.Update(keystate);
                }

                //tileMapPosition
            }
            if (keystate[Key.Left])
            {
                PointF p = new PointF(tileMapPosition.X - this.size.Width, tileMapPosition.Y);
                if (p.X >= 0)
                {
                    tileMapPosition = p;
                }
                p = new PointF(gameScreenPosition.X - this.size.Width, gameScreenPosition.Y);
                if (p.X >= 0)
                {
                    gameScreenPosition = p;
                }
                else
                {
                    parent.camera.Update(keystate);
                }
            }
            if (keystate[Key.Down])
            {

                PointF p = new PointF(tileMapPosition.X, tileMapPosition.Y + this.size.Height);
                if (p.Y + this.size.Height <= tilemapSize.Height)
                {
                    tileMapPosition = p;
                }
                p = new PointF(gameScreenPosition.X, gameScreenPosition.Y + this.size.Height);
                if(p.Y +this.size.Height <= parent.Size.Height)
                {
                    gameScreenPosition = p;
                }
                else
                {
                    parent.camera.Update(keystate);
                }
                
            }
            if (keystate[Key.Up])
            {
                PointF p = new PointF(tileMapPosition.X, tileMapPosition.Y - this.size.Height);
                if (p.Y >= 0)
                {
                    tileMapPosition = p;
                }
                p = new PointF(gameScreenPosition.X, gameScreenPosition.Y - this.size.Height);
                if (p.Y >= 0)
                {
                    gameScreenPosition = p;
                }
                else
                {
                    parent.camera.Update(keystate);
                }
            }
        }

        public void Draw(Surface descSurface)
        {
            try
            {
                if (enable)
                {
                    cursorImage.DrawImage(descSurface);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }
    }
}
