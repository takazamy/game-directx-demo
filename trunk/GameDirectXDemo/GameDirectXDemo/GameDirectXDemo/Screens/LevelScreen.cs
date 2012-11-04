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
    public class LevelScreen:DxScreen
    {
        public LevelScreen(ScreenManager scrManager, DxInitGraphics graphics, Point location, Size size) :
            base(scrManager, graphics, location, size)
        { 
             Initialize();

        }

        public override void Initialize()
        {
            //Tạo map ngẫu nhiên. 1-3
            string mapPath = "";
            switch (CreateRandomNumber(1, 3))
            {
                case 1:
                    mapPath = "" 
                    break;
                case 2:
                    break;
                case 3:
                    break;
            }
            
            //Tạo mission ngẫu nhiên
            //1: Tấn công
            //2: Phòng thủ
            switch (CreateRandomNumber(1, 2))
            {
                case 1:
                    break;
                case 2:
                    break;
                
            }

            //Tạo số lượng cho từng lính loại ngẫu nhiên cho enemy. Tối đa 10 lính
            for (int i = 0; i < 10; i++)
            {
                //Type-Side-hp-stamina-damage-shield
                //1: Tanker: 100-10-5-5
                //2: Ranger: 200-10-3-10
                //3: Defender: 50-5-10-5
                switch (CreateRandomNumber(1, 3))
                {
                    case 1:
                        break;
                    case 2:
                        break;
                    case 3:
                        break;
                }
            }
        }

        public int CreateRandomNumber(int min, int max) 
        {
            Random rand = new Random();
            int randNumber = rand.Next(min, max+1);
            return randNumber;
        }

        public void Update(double deltaTime, KeyboardState keyState, MouseState mouseState)
        {

        }

        public void Draw()
        {

        }

    }
}
