using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.DirectX.DirectDraw;

namespace GameDirectXDemo.Core
{
     public class DxAnimation
    {
        private DxImage _imageObject;
        private double _frameTime; //milisecond;
        private double _elapsedMilisec;
        private Global.AnimationType _aniType;
        private int _currentFrame = 0;
        private bool _isPlaying = true;
        private int _firstFrame;
        private int _lastFrame;
        private int _tmpFirstFrame;
        private int _tmpLastFrame;
        private Global.ObjectDirection _lastDirection;

        public DxAnimation(DxImage imageObject, double frameTime, Global.AnimationType aniType)
        {
            _imageObject = imageObject;
            _frameTime = frameTime;
            _aniType = aniType;
        }
        public DxAnimation(DxImage imageObject, double frameTime, int firstFrame, int lastFarme, Global.AnimationType aniType)
        {
            _imageObject = imageObject;
            _frameTime = frameTime;
            _firstFrame = firstFrame;
            _lastFrame = lastFarme;
            _aniType = aniType;
        }
        public void Update(double elapsedMilisec)
        {
            if (_isPlaying)
            {
                _elapsedMilisec += elapsedMilisec;
                if (_elapsedMilisec >= _frameTime)
                {
                    _elapsedMilisec = 0;
                    _currentFrame++;
                    if (_currentFrame > _imageObject.TotalFrame - 1)
                    {
                        if (_aniType == Global.AnimationType.CONTINUOS)
                        {
                            _currentFrame = 0;
                        }
                        else
                        {
                            _currentFrame = 0;
                            _isPlaying = false;
                        }
                    }
                }
            }
        }
        public void Update(double elapsedMilisec, Global.ObjectDirection direction)
        {
            #region Get direction
            if (direction != _lastDirection)
            {
                switch (direction)
                {
                    case Global.ObjectDirection.DOWN:
                        {
                            _tmpFirstFrame = _firstFrame;
                            _tmpLastFrame = _lastFrame;
                            _currentFrame = _tmpFirstFrame;
                            _lastDirection = direction;
                            break;
                        }
                    case Global.ObjectDirection.LEFT:
                        {
                            _tmpFirstFrame = _firstFrame + _imageObject.Columns;
                            _tmpLastFrame = _lastFrame + _imageObject.Columns;
                            _currentFrame = _tmpFirstFrame;
                            _lastDirection = direction;
                            break;
                        }
                    case Global.ObjectDirection.RIGHT:
                        {
                            _tmpFirstFrame = _firstFrame + _imageObject.Columns * 2;
                            _tmpLastFrame = _lastFrame + _imageObject.Columns * 2;
                            _currentFrame = _tmpFirstFrame;
                            _lastDirection = direction;
                            break;
                        }
                    case Global.ObjectDirection.UP:
                        {
                            _tmpFirstFrame = _firstFrame + _imageObject.Columns * 3;
                            _tmpLastFrame = _lastFrame + _imageObject.Columns * 3;
                            _currentFrame = _tmpFirstFrame;
                            _lastDirection = direction;
                            break;
                        }
                }
            }
            #endregion
            if (_isPlaying)
            {
                _elapsedMilisec += elapsedMilisec;
                if (_elapsedMilisec >= _frameTime)
                {
                    _elapsedMilisec = 0;
                    _currentFrame++;
                    if (_currentFrame > _tmpLastFrame)
                    {
                        if (_aniType == Global.AnimationType.CONTINUOS)
                        {
                            _currentFrame = _tmpFirstFrame;
                        }
                        else
                        {
                            _currentFrame = _tmpFirstFrame;
                            _isPlaying = false;
                        }
                    }
                }
            }
        }
        public void ResetPlay()
        {
            _isPlaying = true;
        }
        public void Draw(Surface destSurface)
        {
            if (_isPlaying)
            {
                _imageObject.DrawImage(_currentFrame, destSurface);
            }
        }
    }
}
