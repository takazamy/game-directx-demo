using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace GameDirectXDemo
{
    public class PathFinding
    {
        private class SearchNode
        {
            public Point Position { get; set; }
            public bool WalkAble { get; set; }
            public SearchNode[] Neighbors { get; set; }
            public SearchNode Parent { get; set; }
            public bool InOpenList;
            public bool InClosedList;
            public float DistanceToGoal;
            public float DistanceTraveled;
        }
       

        private SearchNode[,] _searchNodes;
        private int _columns, _rows;
        private int[,] _map;
        private List<SearchNode> _openList = new List<SearchNode>();
        private List<SearchNode> _closedList = new List<SearchNode>();


        public PathFinding(int[,] collisionMap)
        {
            _map = collisionMap;
            _rows = _map.GetLength(0);
            _columns = _map.GetLength(1);
            _searchNodes = new SearchNode[_rows, _columns];
            InitSearchNode(_map);
        }
        private void InitSearchNode(int[,] map)
        {
            _searchNodes = new SearchNode[_rows, _columns];
            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0; j < _columns; j++)
                {
                    SearchNode node = new SearchNode();
                    node.Position = new Point(i, j);
                    node.WalkAble = map[i, j] == 0; // Equal to if(map[i,j] == 0) return true or false 
                    if (node.WalkAble == true)
                    {
                        node.Neighbors = new SearchNode[4];
                        _searchNodes[i, j] = node;
                    }
                }
            }

            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0; j < _columns; j++)
                {
                    SearchNode node = _searchNodes[i, j];
                    if (node == null || node.WalkAble == false)
                    {
                        continue;
                    }

                    Point[] neighbors = new Point[]
                    {
                        new Point (i ,j-1),
                        new Point (i ,j+1),
                        new Point (i-1 ,j),
                        new Point (i+1 ,j),
                    };
                    for (int n = 0; n < neighbors.Length; n++)
                    {
                        Point position = neighbors[n];
                        if (position.X < 0 || position.X > _rows - 1 ||
                            position.Y < 0 || position.Y > _columns - 1)
                        {
                            continue;
                        }
                        SearchNode neighbor = _searchNodes[position.X, position.Y];
                        if (neighbor == null || neighbor.WalkAble == false)
                        {
                            continue;
                        }
                        node.Neighbors[n] = neighbor;
    

                    }
                }

            }


        }
        private float Heuristic(Point point1, Point point2)
        {
            return Math.Abs(point1.X - point2.X) + Math.Abs(point1.Y - point2.Y);
        }
        public void ResetSearchNode()
        {
            _openList.Clear();
            _closedList.Clear();
            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0; j < _columns; j++)
                {
                    SearchNode node = _searchNodes[i, j];
                    if (node == null)
                    {
                        continue;
                    }
                    node.InOpenList = false;
                    node.InClosedList = false;
                    node.DistanceToGoal = float.MaxValue;
                    node.DistanceTraveled = float.MaxValue;
                }
            }
        }
        private SearchNode FindBestNode()
        {
            SearchNode currentTile = _openList[0];
            float smallestDistanceToGoal = float.MaxValue;
            for (int i = 0; i < _openList.Count; i++)
            {
                if (_openList[i].DistanceToGoal < smallestDistanceToGoal)
                {
                    currentTile = _openList[i];
                    smallestDistanceToGoal = _openList[i].DistanceToGoal;
                }
            }
            return currentTile;
        }
        private List<Point> FindFinalPath(SearchNode startNode, SearchNode endNode)
        {
            _closedList.Add(endNode);
            SearchNode parentTile = endNode.Parent;
            while (parentTile != startNode)
            {
                _closedList.Add(parentTile);
                parentTile = parentTile.Parent;
            }
            List<Point> finalPath = new List<Point>();
            for (int i = _closedList.Count - 1; i >= 0; i--)
            {
                finalPath.Add(new Point(_closedList[i].Position.X * 32, _closedList[i].Position.Y * 32));
            }
            return finalPath;
        }
        public List<Point> FindPath(Point startPoint, Point endPoint)
        {
            if (startPoint == endPoint)
            {
                return new List<Point>();
            }
            ResetSearchNode();
            SearchNode startNode = _searchNodes[startPoint.X, startPoint.Y];
            SearchNode endNode = _searchNodes[endPoint.X, endPoint.Y];
            startNode.InOpenList = true;
            startNode.DistanceToGoal = Heuristic(startPoint, endPoint);
            startNode.DistanceTraveled = 0;
            _openList.Add(startNode);
            while (_openList.Count > 0)
            {
                SearchNode currentNode = FindBestNode();
                if (currentNode == null)
                {
                    break;
                }
                if (currentNode == endNode)
                {
                    return FindFinalPath(startNode, endNode);
                }
                for (int i = 0; i < currentNode.Neighbors.Length; i++)
                {
                    SearchNode neighbor = currentNode.Neighbors[i];
                    if (neighbor == null || neighbor.WalkAble == false)
                    {
                        continue;
                    }
                     float distanceTraveled = currentNode.DistanceTraveled + 1;
                     float heuristic = Heuristic(neighbor.Position, endPoint);
                     if (neighbor.InOpenList == false && neighbor.InClosedList == false)
                     {
                         neighbor.DistanceTraveled = distanceTraveled;
                         neighbor.DistanceToGoal = distanceTraveled + heuristic;
                         neighbor.Parent = currentNode;
                         neighbor.InOpenList = true;
                         _openList.Add(neighbor);
                     }
                     else if(neighbor.InOpenList || neighbor.InClosedList)
                     {
                         if (neighbor.DistanceTraveled > distanceTraveled)
                         {
                             neighbor.DistanceTraveled = distanceTraveled;
                             neighbor.DistanceToGoal = distanceTraveled + heuristic;
                             neighbor.Parent = currentNode;
                         }
                     }
                }
                _openList.Remove(currentNode);
                currentNode.InClosedList = true;
            }
            return new List<Point>();
        }
    }
}
