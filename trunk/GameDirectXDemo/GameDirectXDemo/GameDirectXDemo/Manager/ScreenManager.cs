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
            set 
            {
                _currentScreen = value;
                _currentIndex = _currentScreen._index;
            }
        }

        private int _currentIndex = -1;

        public ScreenManager()
        {
            _children = new List<DxScreen>();
        }

        public void Append(DxScreen screen)
        {
            screen._index = this._children.Count - 1;
            _children.Add(screen);            
        }

        public void PlayScreen(int index)
        {
            _currentScreen = _children[index];
            _currentIndex = index;
        }
        public void PlayScreen(Global.ScreenState state)
        {
            try
            {
                foreach (DxScreen scr in this._children)
                {
                    if (scr._state == state)
                    {
                        _currentScreen = scr;
                        _currentIndex = scr._index;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }
        public void NextScreen()
        {
            _currentScreen = _children[++_currentIndex];           
        }

        public void PrevScreen()
        {
            _currentScreen = _children[--_currentIndex];
        }

        public void UpdateIndex()
        {
            for (int i = 0; i < this._children.Count; i++)
            {
                _children[i]._index = i;
            }
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
