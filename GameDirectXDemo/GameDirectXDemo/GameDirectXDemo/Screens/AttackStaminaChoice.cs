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
    public class AttackStaminaChoice:DxScreen
    {
        DxImage bg;
        DxButton incr;
        DxButton decr;
        DxButton att;
        GameScreen parent;
        int stamina = 0;
        int MaxStamina;
        Object currObj;
        Object targetObj;
        Boolean isShow = false;
        Boolean isInScreen = false;
        public AttackStaminaChoice(ScreenManager scrManager, DxInitGraphics graphics, Point location, Size size, GameScreen gameScreen) :
            base(scrManager, graphics, location, size)
        {
            bg = new DxImage(GameResource.ActionScreen, Global.BitmapType.SOLID, 0, _graphics.DDDevice);
            incr = new DxButton(100, 30, GameResource.incr, _graphics.DDDevice, GameResource.incr.Width, GameResource.incr.Height/3);
            decr = new DxButton(40, 30, GameResource.desc, _graphics.DDDevice, GameResource.desc.Width, GameResource.desc.Height/3);
            att = new DxButton(55, 70, GameResource.attack, _graphics.DDDevice, GameResource.attack.Width, GameResource.attack.Height / 3);
            parent = gameScreen;


            att.OnMouseUp = delegate()
            {
                
            };

        }

        public void Show(Object obj,Object tart)
        {
            this.Location = obj.Position;
            this.MaxStamina = obj._stamina;
            currObj = obj;
            this.targetObj = tart;
            isShow = true;
            isInScreen = true;
        }
        public override void Update(double deltaTime, KeyboardState keyState, MouseState mouseState)
        {
            base.Update(deltaTime, keyState, mouseState);

            if (isInScreen)
            {
                HandleKey(keyState); 
            }
            
        }

        public void HandleKey(KeyboardState keyState)
        {
            if (keyState[Key.NumPadPlus])
            {
                stamina++;
            }
            if (keyState[Key.NumPadMinus])
            {
                stamina--;
            }
            if (keyState[Key.Z])
            {
                int dame = currObj.Attack(targetObj, stamina);
                Global.DamageInfo dinf = new Global.DamageInfo();
                dinf.damage = dame;
                dinf.position = targetObj.positionCenter;
                parent.DamageList.Add(dinf);
                isShow = false;
                isInScreen = false;
                parent.actionScreen.ResetSelectAction();
            }
            if (keyState[Key.X])
            {
                isShow = false;
                isInScreen = false;
                parent.actionScreen.ResetSelectAction();
            }
            
        }

        public override void Draw()
        {
            try
            {
                if (isShow)
                {

                    this.Surface.Draw(new Rectangle(Point.Empty, this.Size), bg._sourceSurface, DrawFlags.Wait);
                    incr.DrawFast(this.Surface);
                    decr.DrawFast(this.Surface);
                    att.DrawFast(this.Surface);
                    this.Surface.DrawText(75, 40, stamina.ToString(), false);
                    this.Surface.DrawText(20, 15, "Max Stamina: " + MaxStamina.ToString(), false);
                    parent.tileMap.TileMapSurface.Draw(new Rectangle(this.Location, this.Size), this.Surface, DrawFlags.Wait);
                }
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
            //bg.DrawImage(this.Surface);
            
            //base.Draw();
        }
    }
}
