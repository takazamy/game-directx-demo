using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.DirectX.DirectDraw;

namespace GameDirectXDemo.Core
{
    public class DxTileMap
    {
        StreamReader _reader;
        JObject _jsonObject = new JObject();
        private int _cellWidth;
        private int _cellHeight;
        private int _rows;
        private int _columns;
        private int[,] _tileMap;
        private int[,] _collisionMap;
        public int[,] CollisionMap
        {
            get { return _collisionMap; }            
        }
        private DxImage _textute;
        private DxInitGraphics _graphics;
        private Surface _tileMapSurface;
        public Surface TileMapSurface
        {
            get { return _tileMapSurface; }
        }
        private List<Layer> _layers = new List<Layer>();
        private class Layer
        {
            public string _name;
            public int _height, _width;
            public bool _isVisible;
            public int[,] _data;
            public Layer(int height, int width, string name, bool isVisible, int[,] data)
            {
                _height = height;
                _width = width;
                _name = name;
                _isVisible = isVisible;
                _data = data;
            }
        }

        //Json
        public DxTileMap(string jsonFilePath, string mapImagePath, DxInitGraphics graphics)
        {
            #region ReadJsonFile
            _reader = File.OpenText(jsonFilePath);
            _jsonObject = JObject.Parse(_reader.ReadToEnd());
            Layer templayer;
            _rows = (int)_jsonObject["height"];
            _columns = (int)_jsonObject["width"];
            _cellWidth = (int)_jsonObject["tilewidth"];
            _cellHeight = (int)_jsonObject["tileheight"];
            _collisionMap = new int[_rows, _columns];
            foreach (var array in _jsonObject["layers"])
            {
                int height = (int)array["height"];
                int width = (int)array["width"];
                string name = (string)array["name"];
                bool isVisible = (bool)array["visible"];
                int[,]data = new int[height,width];
                int i = 0;
                foreach (var value in array["data"])
                {

                    if (name != "collision")
                    {
                        data[i / width, i % width] = (int)value - 1;
                    }
                    else
                    {
                        data[i / width, i % width] = (int)value;
                    }
                    i++;
                }
                if (name == "collision")
                {
                    _collisionMap = data;
                }
                templayer = new Layer(height, width, name, isVisible, data);
                _layers.Add(templayer);
                //Console.WriteLine(array["height"]);
            }
            
            #endregion
            _graphics = graphics;
            _textute = new DxImage(mapImagePath, Global.BitmapType.SOLID, 0, new PointF(0, 0), _cellWidth, _cellHeight, _graphics.DDDevice);
            CrearTileMapSurface();
           
        }


        public DxTileMap(string jsonFilePath, Bitmap mapImage, DxInitGraphics graphics)
        {
            #region ReadJsonFile
            _reader = File.OpenText(jsonFilePath);
            _jsonObject = JObject.Parse(_reader.ReadToEnd());
            Layer templayer;
            _rows = (int)_jsonObject["height"];
            _columns = (int)_jsonObject["width"];
            _cellWidth = (int)_jsonObject["tilewidth"];
            _cellHeight = (int)_jsonObject["tileheight"];
            _collisionMap = new int[_rows, _columns];
            foreach (var array in _jsonObject["layers"])
            {
                int height = (int)array["height"];
                int width = (int)array["width"];
                string name = (string)array["name"];
                bool isVisible = (bool)array["visible"];
                int[,] data = new int[height, width];
                int i = 0;
                foreach (var value in array["data"])
                {

                    if (name != "collision")
                    {
                        data[i / width, i % width] = (int)value - 1;
                    }
                    else
                    {
                        data[i / width, i % width] = (int)value;
                    }
                    i++;
                }
                if (name == "collision")
                {
                    _collisionMap = data;
                }
                templayer = new Layer(height, width, name, isVisible, data);
                _layers.Add(templayer);
                //Console.WriteLine(array["height"]);
            }

            #endregion
            _graphics = graphics;
            _textute = new DxImage(mapImage, Global.BitmapType.SOLID, 0, new PointF(0, 0), _cellWidth, _cellHeight, _graphics.DDDevice);
            CrearTileMapSurface();

        }
        //Text txt
        public DxTileMap(string textFilePath, string mapImagePath,int cellWidth, int cellHeight, DxInitGraphics graphics)
        {
            #region ReadTextFile
            StreamReader reader = File.OpenText(textFilePath);
            string data;
            string[] array;
            int line = 0;
            while (reader.EndOfStream == false)
            {
                data = reader.ReadLine(); 
                line++;
                if (data == "[header]")
                {
                    data = reader.ReadLine(); 
                    line++;
                    _columns = Convert.ToInt32(data.Remove(0, 6));
                    data = reader.ReadLine(); 
                    line++;
                    _rows = Convert.ToInt32(data.Remove(0, 7));
                    _tileMap = new int[_rows, _columns];
                    _collisionMap = new int[_rows, _columns];
                }
                else if (data == "data=")
                {
                    for (int i = 0; i < _rows; i++)
                    {
                        data = reader.ReadLine(); line++;
                        array = data.Split(',');
                        for (int j = 0; j < _columns; j++)
                        {
                            if (line < _rows + 8)
                            {
                                _tileMap[i, j] = Convert.ToInt32(array[j]) - 1;
                            }
                            else
                            {
                                _collisionMap[i, j] = Convert.ToInt32(array[j]);
                            }
                        }
                    }
                }
            }
            reader.Close();
            #endregion
            _graphics = graphics;
            _cellWidth = cellWidth;
            _cellHeight = cellHeight;
            _textute = new DxImage(mapImagePath, Global.BitmapType.SOLID, 0, new PointF(0, 0),_cellWidth,_cellHeight, _graphics.DDDevice);
            CrearTileMapSurface();
        }
        private void CrearTileMapSurface()
        {
            SurfaceDescription desc = new SurfaceDescription();
            desc.SurfaceCaps.OffScreenPlain = true;
            desc.Width = _columns * _cellWidth;
            desc.Height = _rows * _cellHeight;
            _tileMapSurface = new Surface(desc, _graphics.DDDevice);
            DrawTileMap();
        }
        //use for read text file
        //public void DrawTileMap(Surface destSurface)
        //{
        //    for(int i = 0; i < _tileMap.GetLength(0); i++)
        //    {
        //        for (int j = 0; j < _tileMap.GetLength(1); j++)
        //        {
        //            _textute.DrawImage(j * _cellWidth, i * _cellHeight,_tileMap[i, j], destSurface);
        //        }
        //    }
        //}
        public void DrawTileMap()
        {
            DrawTileMap(_tileMapSurface);
        }
        public void DrawTileMap(Surface destSurface)
        {
            foreach (Layer l in _layers)
            {
                if (l._isVisible == true)
                {
                    for(int i = 0; i < l._data.GetLength(0); i++)
                    {
                        for (int j = 0; j < l._data.GetLength(1); j++)
                        {
                            _textute.DrawImage(j * _cellWidth, i * _cellHeight, l._data[i, j], destSurface);
                        }
                    }
                }
            }
        }       
    }
}
