using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace GameDirectXDemo
{
    public class AI
    {
        private List<Object> _objectList;
        private List<Object> _targetList;
        PathFinding _pathFinder;
        List<Point> _path;
        int[,] _objectMap;
        double elaspedTime = 0;
        Object _currObject;
        int _objectIndex = 0;
        List<Point> _choosePath = new List<Point>();
        //int _distanceCanMove;
       public bool IsFinishTurn = false;
        public AI(List<Object> objectList,List<Object> targetList,int[,] colisionMap,int[,] objectMap)
        {
            _objectList = objectList;
            _targetList = targetList;
            _pathFinder = new PathFinding(colisionMap);
            _objectMap = objectMap;
            
        }
        public void Move(double deltaTime)
        {
            elaspedTime += deltaTime;
            if(elaspedTime >= 500)
            {
                _currObject = _objectList[_objectIndex];
                if (_currObject.State == Global.CharacterStatus.Idle)
                {
                    Point objectMapPos = new Point((int)_currObject.Position.X / 32, (int)_currObject.Position.Y / 32);
                    Point targetMapPos = new Point((int)_targetList[0].Position.X / 32, (int)_targetList[0].Position.Y / 32);
                   // _distanceCanMove = _pathFinder.Heuristic(objectMapPos,targetMapPos);
                    _path = _pathFinder.FindPath(objectMapPos,targetMapPos);
                    ChoosePosition(_path);
                                      
                }
                _currObject.Move(_choosePath);
                
                if (_currObject.State == Global.CharacterStatus.FinishTurn)
                {
                    _objectIndex++;
                    _choosePath = new List<Point>();
                    if (_objectIndex >= _objectList.Count )
                    {
                        IsFinishTurn = true;
                        _objectIndex = 0;
                        //foreach (Object obj in _objectList)
                        //{
                        //    obj.State = Global.CharacterStatus.Idle;
                        //}
                    }
                }
            }
        }
        public void ChoosePosition(List<Point> path)
        {
            Point destPos;
            int choosePathIndex = _currObject._stamina -1;
            while (_choosePath.Count == 0)
            {
                destPos = path[choosePathIndex];
                if (_objectMap[destPos.Y / 32, destPos.X / 32] == 0)
                {
                    Point startPoint = new Point((int)_currObject.Position.X / 32, (int)_currObject.Position.Y / 32);
                    Point endPoint = new Point(destPos.X / 32, destPos.Y / 32);
                    _choosePath = _pathFinder.FindPath(startPoint, endPoint);
                    _objectMap[startPoint.Y, startPoint.X] = 0;
                    _objectMap[endPoint.Y, endPoint.X] = 2;
                }
                else
                {
                    choosePathIndex--;
                }
            }
        }
    }
}
