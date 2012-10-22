using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameDirectXDemo
{
    public class Object
    {
        public enum Type
        {
            Tanker,
            Ranger,
            Defender
        }

        public enum Side
        {
            Enemy,
            Player
        }
        protected int _hp = 0;
        protected int _stamina = 0;
        protected int _damage = 0;
        protected int _shield = 0;
        protected int _shieldRestorePoint = 0;
        protected Type _type;

        public Object()
        {
        }

        public virtual void Move() { }

        public virtual void Attack() { }

        public virtual void Die() { }

        protected  virtual void Update()
        {
        
        }

        public virtual void Draw()
        { }



    }
}
