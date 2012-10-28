using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameDirectXDemo
{
    public class Global
    {
        public enum BitmapType{ SOLID = 0,TRANSPARENT = 1}        
       
        public enum CursorState{Normal,Highlight}

        public enum ObjectType{Tanker,Ranger,Defender}

        public enum Side{Enemy,Player}

        public enum AnimationType { CONTINUOS = 0, SINGLESEQUENCE = 1 }
    }
}
