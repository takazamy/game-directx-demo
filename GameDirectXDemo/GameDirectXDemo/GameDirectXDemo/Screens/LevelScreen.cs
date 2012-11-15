using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameDirectXDemo.Core;
using GameDirectXDemo.Manager;
using System.Drawing;
using Microsoft.DirectX.DirectInput;
using System.Windows.Forms;
using Microsoft.DirectX.DirectDraw;

namespace GameDirectXDemo.Screens
{
    public class LevelScreen:DxScreen
    {
        DxImage unit1;
        DxImage unit2;
        DxImage unit3;
        //DxImage miniMap;
        InfoScreen info1;
        InfoScreen info2;
        InfoScreen info3;
        DxButton Fight;
        DxButton unit1Inc;
        DxButton unit1Des;
        DxButton unit2Inc;
        DxButton unit2Des;
        DxButton unit3Inc;
        DxButton unit3Des;
        int unit1Num;
        DxImage bg;
        public int Unit1Num
        {
            get { return unit1Num; }
            set 
            {
                unit1Num += value;
                if (unit1Num < 0)
                {
                    unit1Num = 0;
                    return;
                }
                if ((unit1Num + unit2Num + unit3Num) > 10 || (unit1Num + unit2Num + unit3Num) < 0)
                {
                    unit1Num -= value;
                }
               
            }
        }
        int unit2Num;

        public int Unit2Num
        {
            get { return unit2Num; }
            set
            { 
                unit2Num += value;
                if (unit2Num < 0)
                {
                    unit2Num = 0;
                    return;
                }
                if ((unit1Num + unit2Num + unit3Num) > 10 || (unit1Num + unit2Num + unit3Num) < 0)
                {
                    unit2Num -= value;
                }
            }
        }
        int unit3Num;

        public int Unit3Num
        {
            get { return unit3Num; }
            set
            { 
                unit3Num += value;
                if (unit3Num < 0)
                {
                    unit3Num = 0;
                    return;
                }
                if ((unit1Num + unit2Num + unit3Num) > 10 || (unit1Num + unit2Num + unit3Num) < 0)
                {
                    unit3Num -= value;
                }
            }
        }
        int mission ;
        string missionString = "";
        string mapPath = "";
        string errorString = "";
        List<Object> objects;
        DxTileMap mapGame;        
        public LevelScreen(ScreenManager scrManager, DxInitGraphics graphics, Point location, Size size) :
            base(scrManager, graphics, location, size)
        { 
             Initialize();

        }

        public override void Initialize()
        {
            try
            {
                this._state = Global.ScreenState.GS_LEVEL;

                CreateObjects();

                CreateRandomParas();

                CreateButtonsHandle();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }

        public int CreateRandomNumber(int min, int max) 
        {
            Random rand = new Random();
            int randNumber = rand.Next(min, max+1);
            return randNumber;
        }

        #region Create Random Parameters for a map
        private void CreateRandomParas()
        {
            //Tạo map ngẫu nhiên. 1-3
            mapPath = "Map/map1.json";
            switch (CreateRandomNumber(1, 3))
            {
                case 1:
                    mapPath = "Map/map1.json";
                    break;
                case 2:
                    break;
                case 3:
                    break;
            }
            mapGame = new DxTileMap(mapPath, GameResource.map, _graphics);
            //Tạo mission ngẫu nhiên
            //1: Tấn công
            //2: Phòng thủ
            mission = 1;
            missionString = "Kill them all";
            switch (CreateRandomNumber(1, 2))
            {
                case 1:
                    mission = 1;
                    missionString = "Kill them all";
                    break;
                case 2:
                    break;

            }

            //Tạo số lượng cho từng lính loại ngẫu nhiên cho enemy. Tối đa 10 lính
            for (int i = 0; i < 10; i++)
            {
                //Type-Side-hp-stamina-damage-shield
                //1: Assault: 150-20-2-5
                //2: Ranger: 50-5-10-5
                //3: Defender: 200-10-3-10
                Object temp = null;
                switch (CreateRandomNumber(1, 3))
                {
                    case 1:
                        temp = new Object("1-1-150-20-2-5", this._graphics);
                        break;
                    case 2:
                        temp = new Object("2-1-50-5-20-5", this._graphics);
                        break;
                    case 3:
                        temp = new Object("3-1-500-10-3-10", this._graphics);
                        break;
                }
                objects.Add(temp);
            }

        }
        #endregion

        #region Create HandleButtons
        public void CreateButtonsHandle()
        {
            Fight.OnMouseUp = delegate()
            {
                //SoundManager.Instance.Stop(SoundManager.SoundType.MenuScreenMusic);
                GoToGameScreen();

            };

            unit1Des.OnMouseUp = delegate()
            {
                //SoundManager.Instance.Stop(SoundManager.SoundType.MenuScreenMusic);
                // trong Set will increase with the value we assign for Properties
                Unit1Num = -1;
            };

            unit1Inc.OnMouseUp = delegate()
            {
                //SoundManager.Instance.Stop(SoundManager.SoundType.MenuScreenMusic);
                // trong Set will increase with the value we assign for Properties
                Unit1Num = 1;
            };

            unit2Des.OnMouseUp = delegate()
            {
                //SoundManager.Instance.Stop(SoundManager.SoundType.MenuScreenMusic);
                // trong Set will increase with the value we assign for Properties
                Unit2Num = -1;
            };

            unit2Inc.OnMouseUp = delegate()
            {
                //SoundManager.Instance.Stop(SoundManager.SoundType.MenuScreenMusic);
                // trong Set will increase with the value we assign for Properties
                Unit2Num = 1;
            };

            unit3Des.OnMouseUp = delegate()
            {
                //SoundManager.Instance.Stop(SoundManager.SoundType.MenuScreenMusic);
                // trong Set will increase with the value we assign for Properties
                Unit3Num = -1;
            };

            unit3Inc.OnMouseUp = delegate()
            {
                //SoundManager.Instance.Stop(SoundManager.SoundType.MenuScreenMusic);
                // trong Set will increase with the value we assign for Properties
                Unit3Num = 1;
            };
        }

        #endregion

        #region Create Button and Image for LevelScreen
        private void CreateObjects()
        {
            bg = new DxImage(GameResource.LevelBg, Global.BitmapType.SOLID, 0, _graphics.DDDevice);
            objects = new List<Object>();
            Fight = new DxButton(550, 500, GameResource.Fight_button, _graphics.DDDevice, GameResource.Fight_button.Width, GameResource.Fight_button.Height / 3);
            info1 = new InfoScreen(_scrManager, _graphics, new Point(2, 20), new Size(100, 150), "1-150-20-5-5");
            unit1 = new DxImage(GameResource.Assault, Global.BitmapType.SOLID, 0, new Point(104, 20), _graphics.DDDevice);

            info2 = new InfoScreen(_scrManager, _graphics, new Point(256, 20), new Size(100, 150), "2-50-5-20-5");
            unit2 = new DxImage(GameResource.Ranger, Global.BitmapType.SOLID, 0, new Point(358, 20), _graphics.DDDevice);

            info3 = new InfoScreen(_scrManager, _graphics, new Point(510, 20), new Size(100, 150), "3-500-10-3-10");
            unit3 = new DxImage(GameResource.Defender, Global.BitmapType.SOLID, 0, new Point(612, 20), _graphics.DDDevice);

            unit1Des = new DxButton(124, 175, GameResource.desc, _graphics.DDDevice, GameResource.desc.Width, GameResource.desc.Height / 3);
            unit1Inc = new DxButton(194, 175, GameResource.incr, _graphics.DDDevice, GameResource.incr.Width, GameResource.incr.Height / 3);

            unit2Des = new DxButton(378, 175, GameResource.desc, _graphics.DDDevice, GameResource.desc.Width, GameResource.desc.Height / 3);
            unit2Inc = new DxButton(448, 175, GameResource.incr, _graphics.DDDevice, GameResource.incr.Width, GameResource.incr.Height / 3);

            unit3Des = new DxButton(632, 175, GameResource.desc, _graphics.DDDevice, GameResource.desc.Width, GameResource.desc.Height / 3);
            unit3Inc = new DxButton(702, 175, GameResource.incr, _graphics.DDDevice, GameResource.incr.Width, GameResource.incr.Height / 3);
        }
        #endregion

        private void GoToGameScreen()
        {
            if ((unit1Num + unit2Num + unit3Num) == 10)
            {
                Object temp;
                for (int i = 0; i < unit1Num; i++)
                {
                    temp = null;
                    temp = new Object("1-2-150-20-5-5",this._graphics);
                    objects.Add(temp);
                }
                for (int i = 0; i < unit2Num; i++)
                {
                    temp = null;
                    temp = new Object("2-2-50-5-20-5", this._graphics);
                    objects.Add(temp);
                }
                for (int i = 0; i < unit3Num; i++)
                {
                    temp = null;
                    temp = new Object("3-2-500-10-3-10",this._graphics);
                    objects.Add(temp);
                }

                _scrManager._state = Global.ScreenState.GS_MAIN_GAME;
                //Create GameScreen
                Boolean flag = false;
                GameScreen game = null;
                foreach (DxScreen scr in _scrManager.Children)
                {
                    if (scr._state == Global.ScreenState.GS_MAIN_GAME)
                    {
                        flag = true;
                        game =(GameScreen)scr;
                        break;
                    }
                }
                if (!flag)
                {
                    game = new GameScreen(_scrManager, _graphics, this.Location, this.Size, this.objects, this.mapGame);
                    _scrManager.Append(game);
                    //_scrManager.UpdateIndex();
                }
                else
                {
                    game.SetGame(this.mapGame, this.objects);
                }
                
                _scrManager.PlayScreen(Global.ScreenState.GS_MAIN_GAME);
                //  Console.WriteLine("Mouse up");
            }
            else
            {
                errorString = " Not Enough Force !!!";
            }
        }

        public override void Update(double deltaTime, KeyboardState keyState, MouseState mouseState)
        {
            base.Update(deltaTime, keyState, mouseState);
            Fight.Update(deltaTime, mouseState);
            unit1Inc.Update(deltaTime,mouseState);
            unit1Des.Update(deltaTime, mouseState);
            unit2Inc.Update(deltaTime, mouseState);
            unit2Des.Update(deltaTime, mouseState);
            unit3Inc.Update(deltaTime, mouseState);
            unit3Des.Update(deltaTime, mouseState);
        }

        public override void Draw()
        {
            bg.DrawImageTo(new Rectangle(new Point(0,0),this.Size), base.Surface);

            this.Surface.DrawText((int)unit1.Position.X, 5, "Assault", false);
            this.Surface.DrawText((int)unit2.Position.X, 5, "Ranger", false);
            this.Surface.DrawText((int)unit3.Position.X, 5, "Defender", false);
            this.Surface.DrawText((int)unit1Des.Position.X +40, (int)unit1Inc.Position.Y, unit1Num.ToString(), false);
            this.Surface.DrawText((int)unit2Des.Position.X + 40, (int)unit2Inc.Position.Y, unit2Num.ToString(), false);
            this.Surface.DrawText((int)unit3Des.Position.X + 40, (int)unit3Inc.Position.Y, unit3Num.ToString(), false);
            this.Surface.DrawText(50, 250, "Map", false);
            this.mapGame.DrawTileMap();
            this.Surface.Draw(new Rectangle(100, 250, 200, 200), 
                mapGame.TileMapSurface, 
                new Rectangle(0, 0, mapGame.TileMapSurface.SurfaceDescription.Width, mapGame.TileMapSurface.SurfaceDescription.Height), 
                Microsoft.DirectX.DirectDraw.DrawFlags.Wait);

            this.Surface.DrawText(450, 250, "Mission:", false);
            this.Surface.DrawText(500, 300, this.missionString, false);
            this.Surface.DrawText((int)this.Fight.Position.X,(int) this.Fight.Position.Y - 50, this.errorString, false);

            info1.DrawTextInfo(this.Surface);
            info2.DrawTextInfo(this.Surface);
            info3.DrawTextInfo(this.Surface);

            unit1.DrawImageTo(new Rectangle((int)unit1.Position.X, (int)unit1.Position.Y, 150, 150), this.Surface);
            unit2.DrawImageTo(new Rectangle((int)unit2.Position.X, (int)unit2.Position.Y, 150, 150), this.Surface);
            unit3.DrawImageTo(new Rectangle((int)unit3.Position.X, (int)unit3.Position.Y, 150, 150), this.Surface);

            unit1Inc.DrawFast(this.Surface);
            unit1Des.DrawFast(this.Surface);
            unit2Inc.DrawFast(this.Surface);
            unit2Des.DrawFast(this.Surface);
            unit3Inc.DrawFast(this.Surface);
            unit3Des.DrawFast(this.Surface);

            Fight.DrawFast(this.Surface);
            base.Draw();
        }

    }
}
