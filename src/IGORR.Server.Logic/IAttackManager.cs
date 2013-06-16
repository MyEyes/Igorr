using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using IGORR.Protocol;
using IGORR.Protocol.Messages;

namespace IGORR.Server.Logic
{

    public class Attack : GameObject
    {
        protected IAttackManager _manager;
        public int Damage;
        public float lifeTime;
        public int parentID = -1;
        public int groupID = 0;
        public bool HitOnce = false;
        public bool Penetrates = false;
        public Vector2 Knockback = Vector2.Zero;

        public Attack(IMap map, int damage, Rectangle rect, Vector2 mov, float lifeTime,int parentID,int groupID, int attackID, int id)
            : base(map,rect, id)
        {
            Damage = damage;
            _movement = mov;
            this.groupID = groupID;
            this.parentID = parentID;
            this.lifeTime = lifeTime;
            this.AttackID = attackID;
            _objectType = 'z' - 'a';
            _name = "Attack";
        }

        public void SetAttackManager(IAttackManager am)
        {
            _manager = am;
        }

        public override void Update(float ms)
        {
            Move(_movement * ms / 1000f);
            lifeTime -= ms;
        }

        public int AttackID
        {
            get;
            private set;
        }

        public virtual void Hit()
        {

        }
    }

    public interface IAttackManager
    {
        void Spawn(IMap map, int damage, Rectangle rect, Vector2 mov, float lifeTime, int parentID, int id);
        void Spawn(Attack attack);
        void Update(IMap map, float ms);
        int CheckObject(GameObject obj);
        int CheckPlayer(Player player);
    }

}
