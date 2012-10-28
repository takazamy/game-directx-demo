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
        public enum GameState
        {
            GS_SPLASH_SCREEN = 0,
            GS_MENU = 1,
            GS_LEVEL = 2,
            GS_HELP = 3,
            GS_CREDIT = 4,
            GS_MAIN_GAME = 5,
            GS_ENDGAME = 6,
            GS_EXIT = 7
        }

        public GameState _state = GameState.GS_SPLASH_SCREEN;

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
