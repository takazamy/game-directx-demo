using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
//using Newtonsoft.Json;
//using Newtonsoft.Json.Linq;
using Microsoft.DirectX.DirectDraw;

namespace GameDirectXDemo.Core
{
    class DxTileMap
    {
        //StreamReader _reader;
        //JObject _jsonObject = new JObject();
        private int _cellWidth;
        private int _cellHeight;
        private int _rows;
        private int _columns;
        private int[,] _tileMap;
        private int[,] _colisionMap;
        public int[,] ColisionMap
        {
            get { return _colisionMap; }            
        }
        private DxImage _textute;
        private DxInitGraphics _graphics;
        private Surface _tileMapSurface;
        public Surface TileMapSurface
        {
            get { return _tileMapSurface; }
        }

        //public DxTileMap(string jsonFilePath)
        //{            
        //    _reader = File.OpenText("Map/1.json");
        //    _jsonObject = (JObject)JToken.ReadFrom(new JsonTextReader(_reader));
        //}
        public DxTileMap(string textFilePath, string mapImagePath,int cellWidth, int cellHeight, DxInitGraphics graphics)
        {
            #region ReadTextFile
            StreamReader reader = File.OpenText(textFilePath);
            string data;
            string[] array;
            int line = 0;
            while (reader.EndOfStream == false)
            {
                data = reader.ReadLine(); line++;
                if (data == "[header]")
                {
                    data = reader.ReadLine(); line++;
                    _columns = Convert.ToInt32(data.Remove(0, 6));
                    data = reader.ReadLine(); line++;
                    _rows = Convert.ToInt32(data.Remove(0, 7));
                    _tileMap = new int[_rows, _columns];
                    _colisionMap = new int[_rows, _columns];
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
                                _colisionMap[i, j] = Convert.ToInt32(array[j]);
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
        public void DrawTileMap(Surface destSurface)
        {
            for(int i = 0; i < _tileMap.GetLength(0); i++)
            {
                for (int j = 0; j < _tileMap.GetLength(1); j++)
                {
                    _textute.DrawImage(j * _cellWidth, i * _cellHeight,_tileMap[i, j], destSurface);
                }
            }
        }
        public void DrawTileMap()
        {
            DrawTileMap(_tileMapSurface);
        }

    }
}
