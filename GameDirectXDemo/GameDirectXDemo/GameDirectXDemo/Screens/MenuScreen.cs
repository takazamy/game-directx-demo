﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameDirectXDemo.Core;
using GameDirectXDemo.Manager;
using Microsoft.DirectX.DirectDraw;
using Microsoft.DirectX.DirectInput;
using System.Drawing;

namespace GameDirectXDemo.Screens
{
    public class MenuScreen:DxScreen
    {
        private DxImage bg;
       // private DxImage credit;
        private CreditScreen _credit;
        private HowToPlayScreen _htp;
        private DxImage title;
        private DxButton _playBtn = null;
        private DxButton _exitBtn = null;
        private DxButton _creditBtn = null;
        private DxButton _howtoplayBtn = null;
        private int _onCredit = -1;
        private int _onHTP = -1;

        public MenuScreen(ScreenManager scrManager, DxInitGraphics graphics, Point location, Size size) :
            base(scrManager, graphics, location, size)
        {
            Initialize();

        }

        public override void Initialize()
        {
            base.Initialize();

            this._state = Global.ScreenState.GS_MENU;
            _playBtn = new DxButton(600, 300, GameResource.NewGame, _graphics.DDDevice, 150, 50);
            _creditBtn = new DxButton(600, 350,GameResource.cRedit, _graphics.DDDevice, 150, 50);
            _exitBtn = new DxButton(600, 450,GameResource.EXIT, _graphics.DDDevice, 150, 50);
            _howtoplayBtn = new DxButton(600, 400, GameResource.HowToPlay, _graphics.DDDevice, 150, 50);
            
            bg = new DxImage(GameResource.MenuScreen,Global.BitmapType.SOLID,0, _graphics.DDDevice);
            title = new DxImage(GameResource.tittle, Global.BitmapType.TRANSPARENT, 0xffffff, _graphics.DDDevice);
            PointF position = new PointF(100, 20);
            title.Position = position;
            //credit = new DxImage("Assets/CreditSreen.png", _graphics.DDDevice);
            //howtoplay = new DxImage("Assets/howtoplayScreen.png", _graphics.DDDevice); 
            _credit = new CreditScreen(_scrManager,_graphics,new Point(100,100),new System.Drawing.Size(200,300));
            _htp = new HowToPlayScreen(_scrManager, _graphics, new Point(100, 100), new System.Drawing.Size(200, 300));
            _playBtn.OnMouseUp = delegate()
            {
                //SoundManager.Instance.Stop(SoundManager.SoundType.MenuScreenMusic);
                GoToLevelScreen();
                //  Console.WriteLine("Mouse up");
            };

            _exitBtn.OnMouseUp = delegate()
            {
                if(_exitBtn.Bounding.Contains(GameLogic.mouse.mousex,GameLogic.mouse.mousey))
                {
                    GameLogic.gameState = Global.GameStates.Exit;
                }

            };

            _creditBtn.OnMouseUp = delegate()
            {
                _onCredit *= -1;
                _onHTP = -1;
            };

            _howtoplayBtn.OnMouseUp = delegate()
            {
                _onHTP *= -1;
                _onCredit = -1;
            };
        }
        private void GoToLevelScreen()
        {
            _scrManager._state = Global.ScreenState.GS_LEVEL;
            Boolean flag = false;
            LevelScreen level = null;
            foreach (DxScreen scr in _scrManager.Children)
            {
                if (scr._state == Global.ScreenState.GS_LEVEL)
                {
                    flag = true;
                    level = (LevelScreen)scr;
                    break;
                }
            }
            if (!flag)
            {
                level = new LevelScreen(_scrManager, _graphics, Point.Empty, new Size(_scrManager._renderTarget.Width, _scrManager._renderTarget.Height));
                _scrManager.Append(level);
                //_scrManager.UpdateIndex();
            }
            
            
            _scrManager.PlayScreen(Global.ScreenState.GS_LEVEL);
        }
        public override void Update(double deltaTime, KeyboardState keyState, MouseState mouseState)
        {
           
            //if (SoundManager.Instance.isLoop)
            //{
            //    if (SoundManager.Instance.CheckDuration(SoundManager.SoundType.MenuScreenMusic))
            //    {
            //        SoundManager.Instance.Replay(SoundManager.SoundType.MenuScreenMusic);
            //    }
            //}
            base.Update(deltaTime, keyState, mouseState);
            _playBtn.Update(deltaTime, mouseState);
            _exitBtn.Update(deltaTime, mouseState);
            _creditBtn.Update(deltaTime, mouseState);
            _howtoplayBtn.Update(deltaTime, mouseState);
        }

        public override void Draw( )
        {
            //SoundManager.Instance.Play(SoundManager.SoundType.MenuScreenMusic);
            //SoundManager.Instance.isLoop = true;
            bg.DrawImageTo(new Rectangle(0,0,this._size.Width,this._size.Height), base.Surface);
            title.DrawImage(this.Surface);
            _playBtn.DrawFast(this.Surface);
            _exitBtn.DrawFast(this.Surface);
            _creditBtn.DrawFast(this.Surface);
            _howtoplayBtn.DrawFast(this.Surface);
            if (_onCredit > 0)
            {
                //credit.DrawFast(200, 300, base.Surface, DrawFastFlags.Wait);
                _credit.Draw(this._surface);
            }
            if (_onHTP > 0)
            {
                //howtoplay.DrawFast(200, 300, base.Surface, DrawFastFlags.Wait);
                _htp.Draw(this._surface);
            }
            //   .DrawFast(_location.X, _location.Y, bg.XImage, DrawFastFlags.Wait);
            base.Draw();
        }
       
    }
}
