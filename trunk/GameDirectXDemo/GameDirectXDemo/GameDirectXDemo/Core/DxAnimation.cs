using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.DirectX.DirectDraw;

namespace GameDirectXDemo.Core
{
   public enum AnimationType
    {
        CONTINUOS = 0,
        SINGLESEQUENCE = 1
    }
    public class DxAnimation
    {
        private DxImage _imageObject;
        private double _frameTime; //milisecond;
        private double _elapsedMilisec;
        private AnimationType _aniType;
        private int _currentFrame = 0;
        private bool _isPlaying = true;

        public DxAnimation(DxImage imageObject, double frameTime, AnimationType aniType)
        {
            _imageObject = imageObject;
            _frameTime = frameTime;
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
                        if (_aniType == AnimationType.CONTINUOS)
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
