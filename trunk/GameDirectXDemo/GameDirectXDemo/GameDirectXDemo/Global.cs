using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.DirectX.DirectInput;

namespace GameDirectXDemo
{
    public class Global
    {
        public enum BitmapType{ SOLID = 0,TRANSPARENT = 1, OVERLAY = 2}        
       
        public enum CursorState{Normal,Highlight}

        public enum ObjectType
        {
            Assault = 1,
            Ranger = 2,
            Defender = 3
        }

        public enum Side{Enemy = 1,Player = 2}

        public enum AnimationType { CONTINUOS = 0, SINGLESEQUENCE = 1 }

        public enum ScreenState
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


        public enum ButtonState
        {
            BS_NORMAL = 0,
            BS_HOLD = 1,
        }

        /// <summary>
        /// Holds the various game states.
        /// </summary>
        public enum GameStates
        {
            Run = 0,
            Exit = 1
        }
        public enum ObjectDirection
        {

            LEFT = 0,
            RIGHT = 1,
            UP = 2,
            DOWN = 3,
            
        }

        public enum Turn
        {
            EnemyTurn = 0,
            PlayerTurn = 1
        }

        public enum CharacterStatus
        {
            Idle = 0,
            Move = 1,
            Attack = 2,
            FinishTurn = 3
        }

        public static Boolean CheckKeyDown(KeyboardState keyState)
        {
            Boolean _keyDown = false;
            for (int i = 0; i < 256; i++)
            {
                if (keyState[(Key)i])
                {
                    _keyDown = true;
                }
            }
            return _keyDown;
        }

        public enum ActionSreenChoice
        {
            Move,
            Attack,
            EndTurn,
            NoAction
        }        
    }
}
