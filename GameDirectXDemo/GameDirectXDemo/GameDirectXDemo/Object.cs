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
        #region Properties
        int pathIndex = 0;
        public delegate void Action();
        public int _fullHp;
        public int _hp = 0;
        public int _fullSta;
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
        protected float _moveSpeed = 32;
        public Boolean isSelected = false;
        DxImage selectImage;
        public Point positionCenter;
        public float RangeAttack { get; set; }
        float rate;
        public float range;
        public List<Point> path = null;

        protected Point _position;
        public Point Position
        {
            get { return _position; }
            set { _position = value; }
        }
        protected Global.ObjectDirection _lastDirection;
        protected Global.ObjectDirection _currentDirection = Global.ObjectDirection.DOWN;
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
                        _ani = new DxAnimation(this._objectImg, 100, 9, 11, Global.AnimationType.CONTINUOS);
                        RangeAttack = 1;
                       
                       //khởi tạo image cho object
                        
                        break;
                    case Global.ObjectType.Ranger:
                        _canAttackFar = true;
                        _canAttackNear = false;
                        _ani = new DxAnimation(this._objectImg, 100, 6, 8, Global.AnimationType.CONTINUOS);
                        RangeAttack = 20;
                        
                        break;
                    case Global.ObjectType.Assault:
                        _canAttackFar = true;
                        _canAttackNear = true;
                        _ani = new DxAnimation(this._objectImg, 100, 0, 2, Global.AnimationType.CONTINUOS);
                        RangeAttack = 3;
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

        protected Global.CharacterStatus _state = Global.CharacterStatus.Idle;

        public Global.CharacterStatus State
        {
            get { return _state; }
            set { _state = value; }
        }
        #endregion

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
            _fullHp = _hp;
            _fullSta = _stamina;
            path = new List<Point>();
          
        }   

        public void Initialize()
        {
            _objectImg = new DxImage(GameResource.robot, Global.BitmapType.TRANSPARENT, Color.White.ToArgb(),PointF.Empty,32,32, _graphics.DDDevice);
            selectImage = new DxImage(GameResource.selectImage, Global.BitmapType.TRANSPARENT, Color.White.ToArgb(), _graphics.DDDevice);
            positionCenter = this.Position;
            //if (this._side == Global.Side.Player)
            //{

            //    this.onMouseDown = delegate()
            //    {

            //    };
            //}
        }


        public virtual void Move(List<Point> path)
        {
            if (path.Count > 0)
            {
                Console.WriteLine("Object Move");
                if (pathIndex <= 0)
                {
                    _state = Global.CharacterStatus.Move;
                }
                if (_state == Global.CharacterStatus.Move)
                {
                    #region get direction
                    if (this.Position != path[pathIndex])
                    {

                        if (this._position.X < path[pathIndex].X)
                        {
                            _currentDirection = Global.ObjectDirection.RIGHT;
                        }
                        else if (this._position.X > path[pathIndex].X)
                        {
                            _currentDirection = Global.ObjectDirection.LEFT;
                        }
                        else if (this._position.Y < path[pathIndex].Y)
                        {
                            _currentDirection = Global.ObjectDirection.DOWN;
                        }
                        else if (this._position.Y > path[pathIndex].Y)
                        {
                            _currentDirection = Global.ObjectDirection.UP;
                        }
                    }
                    #endregion
                    #region move
                    switch (_currentDirection)
                    {
                        case Global.ObjectDirection.DOWN:
                            {
                                this._position.Y += (int)_moveSpeed;
                                if (this._position.Y > path[pathIndex].Y)
                                {
                                    this._position.Y = path[pathIndex].Y;
                                }
                                break;
                            }
                        case Global.ObjectDirection.LEFT:
                            {
                                this._position.X -= (int)_moveSpeed;
                                if (this._position.X < path[pathIndex].X)
                                {
                                    this._position.X = path[pathIndex].X;
                                }
                                break;
                            }
                        case Global.ObjectDirection.RIGHT:
                            {
                                this._position.X += (int)_moveSpeed;
                                if (this._position.X > path[pathIndex].X)
                                {
                                    this._position.X = path[pathIndex].X;
                                }
                                break;
                            }
                        case Global.ObjectDirection.UP:
                            {
                                this._position.Y -= (int)_moveSpeed;
                                if (this._position.Y < path[pathIndex].Y)
                                {
                                    this._position.Y = path[pathIndex].Y;
                                }
                                break;
                            }
                    }
                    //Console.WriteLine("Direction {0}", _currentDirection);
                    #endregion
                    
                    Point objectPos = new Point((int)this._position.X / 32, (int)this._position.Y / 32);
                    Point pathPos = new Point((int)path[pathIndex].X / 32, (int)path[pathIndex].Y / 32);
                    if (objectPos == pathPos)
                    {
                        try
                        {
                            pathIndex++;
                            Console.WriteLine("pathcount {0}", path.Count);
                            Console.WriteLine("pathindex {0}", pathIndex);
                            //Console.WriteLine("objectPos {0}", objectPos);
                            //Console.WriteLine("pathPos {0}", pathPos);
                            if (pathIndex >= path.Count)
                            {

                                _state = Global.CharacterStatus.FinishTurn;
                                this.path = new List<Point>();
                                pathIndex = 0;
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.StackTrace);
                        }
                }
              
            }
            }
        }


        public virtual int Attack(Object targetobj,int stamina) 
        {
            Console.WriteLine("Attack The Enemy");
            Random rand = new Random();
            _state = Global.CharacterStatus.Attack;
            this._stamina -= stamina;
            if(this.ObjectType == Global.ObjectType.Defender)
            {
                rate = 1;
            }
            if (this.ObjectType == Global.ObjectType.Ranger)
	        {
		        rate = range* (float)0.1;
	        }
            if (this.ObjectType == Global.ObjectType.Assault)
	        {
		        rate = 1 - range * (float)0.1;
	        }
            float dam = stamina *this._damage *rate - targetobj._shield;
           
            targetobj._hp -= (int)dam;

            return (int)dam;

        }

        public virtual void Die() { }

        public Boolean IsSelected(PointF selectPosition)
        {
            if (isSelected)
            {
                this.isSelected = false;
                return false;
            }
            else
            {
               if (selectPosition == this.Position)
                {
                    this.isSelected = true;
                    return true;
                }
                else
                {
                    this.isSelected = false;
                    return false;
                }
            }

        }
        public void DeSelected()
        {
            this.isSelected = false;
        }
        public void ResetStamina()
        {
            this._stamina = _fullSta;
        }
        public void Update(double deltaTime,KeyboardState keyState, MouseState mouseState)
        {
            this.Move(path);
            this._objectImg.Position = this.Position;

            this.selectImage.Position = this.Position;
           

            if (_state != Global.CharacterStatus.FinishTurn)
            {
                _ani.Update(deltaTime, this._currentDirection);
            }
            positionCenter.X = this.Position.X + this._objectImg.FrameWidth / 2;
            positionCenter.Y = this.Position.Y + this._objectImg.FrameHeight / 2;

        }

        public void Draw(Surface desSurf)
        {            
            if (this.isSelected && this.Side == Global.Side.Player)
            {
                selectImage.DrawImage(desSurf);
            }

            _ani.Draw(desSurf);
        }




        
    }
}
