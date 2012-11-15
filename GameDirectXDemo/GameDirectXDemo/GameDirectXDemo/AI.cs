using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using GameDirectXDemo.Screens;
using Microsoft.DirectX.DirectInput;

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
        GameScreen _parent;
        //int _distanceCanMove;
       public bool IsFinishTurn = false;
        public AI(List<Object> objectList,List<Object> targetList,int[,] colisionMap,int[,] objectMap, GameScreen parent)
        {
            _objectList = objectList;
            _targetList = targetList;
            _pathFinder = new PathFinding(colisionMap);
            _objectMap = objectMap;
            _parent = parent;
            
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
                Point tempDestPos;
                int tempPosIndex;
                List<Point> tempPath= new List<Point>();
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
                            //tempPath = _pathFinder.FindPath(startPoint, endPoint);
                            //tempPosIndex = 





                            _currObject._stamina -= _choosePath.Count;
                            _objectMap[startPoint.Y, startPoint.X] = 0;
                            _objectMap[endPoint.Y, endPoint.X] = 2;
                            Console.WriteLine("choose Path {0}", _choosePath.Count);
                        }
                        else
                        {
                            if (choosePathIndex > 0)
                            {
                                choosePathIndex--;
                            }
                            else
                            {
                                _currObject.State = Global.CharacterStatus.FinishTurn;
                                choosePathIndex = 0;
                                break;
                            }
                        }
                    }
                }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }
        public void Attack()
        {
           
        }
        public void Update(double deltaTime, KeyboardState keyState, MouseState mouseState)
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
                        Console.WriteLine("Enemy {0} move", _currObject.Index);
                        this.Move(_currObject, _currTarget);
                        //if (_currObject._stamina > 0 && _currObject.State == Global.CharacterStatus.FinishTurn)
                        //{
                        //    //_currObject.State = Global.CharacterStatus.Attack;
                        //    Console.WriteLine("enemy {0} Can Attack", _currObject.Index);
                        //}
                    }
                    else
                    {
                        Console.WriteLine("in range");
                        if (_currObject._stamina > 0)
                        {
                            int damage;
                            Console.WriteLine("Enemy {0} attack", _currObject.Index);
                            _currObject.range = _pathFinder.Heuristic(new Point(_currObject.Position.X / 32, _currObject.Position.Y / 32), new Point(_currTarget.Position.X / 32, _currTarget.Position.Y / 32));
                            damage = _currObject.Attack(_currTarget, _currObject._stamina);
                            Global.DamageInfo dinf = new Global.DamageInfo();
                            dinf.damage = damage;
                            dinf.position = _currTarget.positionCenter;
                            _parent.DamageList.Add(dinf);
                            _currObject.State = Global.CharacterStatus.FinishTurn;
                        }
                        else
                        {
                            _currObject.State = Global.CharacterStatus.FinishTurn;
                        }
                    }
                    if (_currObject.State == Global.CharacterStatus.FinishTurn)
                    {
                        try
                        {
                            Console.WriteLine(_objectIndex);
                            Console.WriteLine(_choosePath.Count);
                            _currObject.Update(deltaTime, keyState, mouseState);
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
                    _currObject.Update(deltaTime, keyState, mouseState);
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
