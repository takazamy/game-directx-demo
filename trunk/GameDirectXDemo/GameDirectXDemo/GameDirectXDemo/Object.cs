using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameDirectXDemo.Core;
using System.Drawing;
//using GameDirectXDemo.Global;
using Microsoft.DirectX.DirectInput;
namespace GameDirectXDemo
{
    public class Object
    {
        protected int _hp = 0;
        protected int _stamina = 0;
        protected int _damage = 0;
        protected int _shield = 0;
        
        protected Boolean _canAttackNear;
        protected Boolean _canAttackFar;

        protected Global.ObjectType _objType;

        protected Global.ObjectType ObjectType
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
                       //khởi tạo image cho object
                        break;
                    case Global.ObjectType.Ranger:
                        _canAttackFar = true;
                        _canAttackNear = false;
                        break;
                    case Global.ObjectType.Assault:
                        _canAttackFar = true;
                        _canAttackNear = true;
                        break;
                }                
            }
        }

        protected Global.Side _side;

        protected Global.Side Side
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
        protected Boolean _canControl;
        protected DxImage _objectImg;
        public Object(String info)
        {
            //Type,Side,hp,stamina,damage,shield
            String[] val = info.Split('-');
            ObjectType = (Global.ObjectType)Enum.Parse(typeof(Global.ObjectType), val[0]);
            Side = (Global.Side)Enum.Parse(typeof(Global.Side), val[1]);
            _hp = int.Parse(val[2]);
            _stamina = int.Parse(val[3]);
            _damage = int.Parse(val[4]);
            _shield = int.Parse(val[5]);
           
        }

        public virtual void Move() { }

        public virtual void Attack() { }

        public virtual void Die() { }

        protected  virtual void Update(KeyboardState keyState, MouseState mouseState)
        {
            
        }

        public virtual void Draw()
        { 
            
        }



    }
}
