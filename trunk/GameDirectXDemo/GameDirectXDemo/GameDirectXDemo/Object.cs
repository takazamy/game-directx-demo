using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameDirectXDemo.Core;
using System.Drawing;
//using GameDirectXDemo.Global;
using Microsoft.DirectX.DirectInput;
using Microsoft.DirectX.DirectDraw;
namespace GameDirectXDemo
{
    public class Object
    {

        public delegate void Action();
        public int _hp = 0;
        public int _stamina = 0;
        public int _damage = 0;
        public int _shield = 0;
        protected Rectangle bound;
        public Boolean _canAttackNear;
        public Boolean _canAttackFar;
        protected Boolean _canControl;
        protected DxImage _objectImg;
        protected DxAnimation _ani;
        protected DxInitGraphics _graphics;
        protected float _moveSpeed = 5;

        protected PointF _position;
        public PointF Position
        {
            get { return _position; }
            set { _position = value; }
        }
        protected Global.ObjectDirection _lastDirection;
        protected Global.ObjectDirection _currentDirection;
        public Global.ObjectDirection CurrentDirection
        {
            get { return _currentDirection; }
        }
        protected Global.ObjectType _objType;

        public Global.ObjectType ObjectType
        {
            get { return _objType; }
            set 
            {
                _objType = value;
                switch(_objType)
                {
                    case Global.ObjectType.Defender:
                        _canAttackFar = false;
                        _canAttackNear = true;
                        _ani = new DxAnimation(this._objectImg, 300, 9, 11, Global.AnimationType.CONTINUOS);
                       //khởi tạo image cho object
                        
                        break;
                    case Global.ObjectType.Ranger:
                        _canAttackFar = true;
                        _canAttackNear = false;
                        _ani = new DxAnimation(this._objectImg, 300, 6, 8, Global.AnimationType.CONTINUOS);
                        break;
                    case Global.ObjectType.Assault:
                        _canAttackFar = true;
                        _canAttackNear = true;
                        _ani = new DxAnimation(this._objectImg, 300, 0, 2, Global.AnimationType.CONTINUOS);
                        break;
                }                
            }
        }

        protected Global.Side _side;

        public Global.Side Side
        {
            get { return _side; }
            set 
            {
                _side = value;

                if (_side == Global.Side.Enemy)
                    _canControl = false;
                else
                    _canControl = true;
            }
        }
        

        private Action onMouseDown = null;
        public Action OnMouseDown
        {
            set { onMouseDown = value; }
        }


        public Object(String info, DxInitGraphics graphics)
        {
            //Type,Side,hp,stamina,damage,shield
            this._graphics = graphics;
            Initialize();
            String[] val = info.Split('-');
            ObjectType = (Global.ObjectType)Enum.Parse(typeof(Global.ObjectType), val[0]);
            Side = (Global.Side)Enum.Parse(typeof(Global.Side), val[1]);
            _hp = int.Parse(val[2]);
            _stamina = int.Parse(val[3]);
            _damage = int.Parse(val[4]);
            _shield = int.Parse(val[5]);
            
            
        }


        public void Initialize()
        {
            _objectImg = new DxImage(GameResource.robot, Global.BitmapType.TRANSPARENT, Color.White.ToArgb(),PointF.Empty,32,32, _graphics.DDDevice);
            //if (this._side == Global.Side.Player)
            //{

            //    this.onMouseDown = delegate()
            //    {

            //    };
            //}
        }

        public virtual void Move() 
        {
            switch (_currentDirection)
            {
                case Global.ObjectDirection.DOWN:
                    {
                        this._position.Y += _moveSpeed;
                        break;
                    }
                case Global.ObjectDirection.LEFT:
                    {
                        this._position.X -= _moveSpeed;
                        break;
                    }
                case Global.ObjectDirection.RIGHT:
                    {
                        this._position.X += _moveSpeed;
                        break;
                    }
                case Global.ObjectDirection.UP:
                    {
                        this._position.Y -= _moveSpeed;
                        break;
                    }
            }
        }


        public virtual void Attack() { }

        public virtual void Die() { }

        public void Update(double deltaTime,KeyboardState keyState, MouseState mouseState)
        {
            this._objectImg.Position = this.Position;
            _ani.Update(deltaTime, this._currentDirection);
        }

        public void Draw(Surface desSurf)
        {
            _ani.Draw(desSurf);
        }



    }
}
