using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.DirectX.AudioVideoPlayback;

namespace GameDirectXDemo.Manager
{
    public class SoundManager
    {
        // music = new Audio("intro.wav",true);
        private static SoundManager _instance = new SoundManager();

        public Boolean isLoop = false;
        #region Const

        public struct SoundType
        {
            public const string Click = "Click";
            public const string SplashScreenMusic = "SplashScreenMusic";
            public const string MenuScreenMusic = "MenuScreenMusic";
            public const string LevelScreenMusic = "LevelScreenMusic";
            public const string Map1Music = "Map1Music";
            public const string AtkSound = "AtkSound";
        }

        #endregion

        #region Audio

        Audio _click = new Audio("Assets/Sound/Click.mp3");
        Audio _splashScreenMusic = new Audio("Assets/Sound/SplashScreenSound.mp3");
        Audio _menuScreenMusic = new Audio("Assets/Sound/MenuSound.mp3");
        Audio _levelScreenMusic = new Audio("Assets/Sound/LevelScreenMusic.mp3");
        Audio _map1Music = new Audio("Assets/Sound/map1Music.mp3");
        Audio _atkSound = new Audio("Assets/Sound/AttackSound.mp3");


        #endregion

        Dictionary<string, Audio> _library;


        private SoundManager()
        {
            LoadAllSounds();

        }

        public static SoundManager Instance
        {
            get { return _instance; }
        }

        public Boolean CheckDuration(string key)
        {
            return (_library[key].CurrentPosition == _library[key].Duration);
        }

        public void Replay(string key)
        {
            _library[key].CurrentPosition = 0;
        }

        public void LoadAllSounds()
        {
            // Load sounds
            _library = new Dictionary<string, Audio>();

            _library.Add(SoundType.SplashScreenMusic, _splashScreenMusic);
            _library.Add(SoundType.Click, _click);
            _library.Add(SoundType.MenuScreenMusic, _menuScreenMusic);
            _library.Add(SoundType.LevelScreenMusic, _levelScreenMusic);
            _library.Add(SoundType.Map1Music, _map1Music);
            _library.Add(SoundType.AtkSound, _atkSound);

        }

        public void Play(string key)
        {
            _library[key].Play();

        }
        public void Stop(string key)
        {
            _library[key].Stop();
        }
    }
}
