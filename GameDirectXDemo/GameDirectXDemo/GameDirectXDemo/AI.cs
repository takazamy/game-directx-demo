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
        Object _currTarget;
        int _objectIndex = 0;
        int minDistance = int.MaxValue;
        int choosePathIndex;
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
        public void Move(Object obj, Object target)
        {
            Object currObj, currTarget;
            currObj = obj;
            currTarget = target;
            try
            {
                if (currObj.State == Global.CharacterStatus.Idle)
                {
                    Point objectMapPos = new Point((int)currObj.Position.X / 32, (int)currObj.Position.Y / 32);
                    Point targetMapPos = new Point((int)currTarget.Position.X / 32, (int)currTarget.Position.Y / 32);
                    // _distanceCanMove = _pathFinder.Heuristic(objectMapPos,targetMapPos);
                    _path = _pathFinder.FindPath(objectMapPos, targetMapPos);
                    ChoosePosition(_path);

                }
                _currObject.Move(_choosePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }
        public void ChoosePosition(List<Point> path)
        {
            try
            {
                Point destPos;
                //if (path.Count > _currObject._stamina)
                //{
                    Console.WriteLine("path count {0}", path.Count);
                    if (path.Count >= _currObject._stamina)
                    {
                        choosePathIndex = _currObject._stamina - 1;
                    }
                    else
                    {
                        choosePathIndex = path.Count - 1;
                    }
                    while (_choosePath.Count == 0)
                    {
                        destPos = path[choosePathIndex];
                        if (_objectMap[destPos.Y / 32, destPos.X / 32] == 0)
                        {
                            Point startPoint = new Point((int)_currObject.Position.X / 32, (int)_currObject.Position.Y / 32);
                            Point endPoint = new Point(destPos.X / 32, destPos.Y / 32);
                            _choosePath = _pathFinder.FindPath(startPoint, endPoint);
                            _currObject._stamina -= _choosePath.Count;
                            _objectMap[startPoint.Y, startPoint.X] = 0;
                            _objectMap[endPoint.Y, endPoint.X] = 2;
                            Console.WriteLine("choose Path {0}", _choosePath.Count);
                        }
                        else
                        {
                            if (choosePathIndex >= 0)
                            {
                                choosePathIndex--;
                            }
                            else
                            {
                                _currObject.State = Global.CharacterStatus.FinishTurn;
                            }
                        }
                    }
                }
                //else
                //{
                //    for (int i = 0; i < path.Count - 1; i++)
                //    {
                //       _choosePath.Add(path[i]);                        
                //    }
                    
                //}
            
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }
        public void Attack()
        {
           
        }
        public void Update(double deltaTime)
        {
            try
            {
                elaspedTime += deltaTime;
                
                if (elaspedTime >= 500)
                {
                    _currObject = _objectList[_objectIndex];
                    _currTarget = this.ChooseTarget(_currObject, _targetList);
                    if (_currObject.RangeAttack < minDistance)
                    {
                        this.Move(_currObject, _currTarget);
                    }
                    else
                    {
                        _currObject.Attack(_currTarget,_currObject._stamina);
                    }
                    if (_currObject.State == Global.CharacterStatus.FinishTurn)
                    {
                        try
                        {
                            Console.WriteLine(_objectIndex);
                            Console.WriteLine(_choosePath.Count);
                            _objectIndex++;
                            elaspedTime = 0;
                            _choosePath = new List<Point>();
                            if (_objectIndex >= _objectList.Count)
                            {
                                IsFinishTurn = true;
                                _objectIndex = 0;
                            }
                            
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.StackTrace);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }
        private Object ChooseTarget(Object obj,List<Object> targetList)
        {
           
            Object currTarget = null;
            Object currentObj;
            try
            {
                currentObj = obj;
                Point starPoint, endPoint;
                minDistance = int.MaxValue;
                int distance;
                starPoint = new Point(currentObj.Position.X / 32, currentObj.Position.Y / 32);
                foreach (Object target in targetList)
                {
                    endPoint = new Point(target.Position.X / 32, target.Position.Y / 32);
                    distance = (int)_pathFinder.Heuristic(starPoint, endPoint);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        currTarget = target;
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
            return currTarget;
        }
    }
}
