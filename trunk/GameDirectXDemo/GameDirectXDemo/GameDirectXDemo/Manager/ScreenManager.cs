using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameDirectXDemo.Core;
using Microsoft.DirectX.DirectInput;

namespace GameDirectXDemo.Manager
{
    public class ScreenManager
    {


        public Global.ScreenState _state = Global.ScreenState.GS_SPLASH_SCREEN;

        List<DxScreen> _children;

        internal List<DxScreen> Children
        {
            get { return _children; }

        }

        public DxScreen this[int index]
        {
            get
            {
                return _children[index];
            }
            set
            {
                _children[index] = value;
            }
        }

        private DxScreen _currentScreen = null;

        internal DxScreen CurrentScreen
        {
            get { return _currentScreen; }
            set { _currentScreen = value; }
        }

        private int _currentIndex = -1;

        public ScreenManager()
        {
            _children = new List<DxScreen>();
        }

        public void Append(DxScreen screen)
        {
            _children.Add(screen);
        }

        public void PlayScreen(int index)
        {
            _currentScreen = _children[index];
            _currentIndex = index;
        }

        public void NextScreen()
        {
            _currentScreen = _children[++_currentIndex];
        }

        public void PrevScreen()
        {
            _currentScreen = _children[--_currentIndex];
        }

        public void Update(double deltaTime, KeyboardState keyState, MouseState mouseState)
        {
            _currentScreen.Update(deltaTime, keyState, mouseState);
        }

        public void Draw(DxInitGraphics graphics)
        {
            _currentScreen.Draw();
        }
    }
}
