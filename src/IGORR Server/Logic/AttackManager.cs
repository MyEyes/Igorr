using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using IGORR.Protocol;
using IGORR.Protocol.Messages;

namespace IGORR_Server.Logic
{

    public class Attack : GameObject
    {
        protected AttackManager _manager;
        public int Damage;
        public float lifeTime;
        public int parentID = -1;
        public int groupID = 0;
        public bool HitOnce = false;
        public bool Penetrates = false;
        public Vector2 Knockback = Vector2.Zero;

        public Attack(Map map, int damage, Rectangle rect, Vector2 mov, float lifeTime,int parentID,int groupID, int id)
            : base(map,rect, id)
        {
            Damage = damage;
            _movement = mov;
            this.groupID = groupID;
            this.parentID = parentID;
            this.lifeTime = lifeTime;
            _objectType = 'z' - 'a';
            _name = "Attack";
        }

        public void SetAttackManager(AttackManager am)
        {
            _manager = am;
        }

        public virtual void Update(float ms)
        {
            Move(_movement * ms / 1000f);
            lifeTime -= ms;
        }
    }

    public class AttackManager
    {
        List<Attack> _attacks;
        List<Attack> _addAttack;
        Server _server;

        public AttackManager(Server server)
        {
            _attacks = new List<Attack>();
            _addAttack = new List<Attack>();
            _server = server;
        }

        public int CheckPlayer(Player player)
        {
            int damage = 0;

            for (int x = 0; x < _attacks.Count; x++)
            {
                if (player.Collides(_attacks[x]) && player.ID != _attacks[x].parentID &&(_attacks[x].groupID==0 || _attacks[x].groupID!= player.GroupID) && !player.Invincible)
                {
                    damage += _attacks[x].Damage;
                    player.GetDamage(_attacks[x].Damage, _attacks[x].parentID);
                    if (_attacks[x].Knockback != Vector2.Zero)
                        player.Knockback(_attacks[x].Knockback);
                    if (_attacks[x].HitOnce)
                    {
                        _attacks.RemoveAt(x);
                        x--;
                    }
                }
            }
            return damage;
        }

        public int CheckObject(GameObject obj)
        {
            int damage = 0;

            for (int x = 0; x < _attacks.Count; x++)
            {
                if (obj.Collides(_attacks[x]))
                {
                    damage += _attacks[x].Damage;
                    if (_attacks[x].HitOnce)
                    {
                        _attacks.RemoveAt(x);
                        x--;
                    }
                }
            }
            return damage;
        }

        public void Spawn(Map map, int damage, Rectangle rect, Vector2 mov, float lifeTime,int parentID, int id)
        {
            Attack attack = new Attack(map, damage, rect, mov, lifeTime, parentID,0, id);
            Spawn(attack);
        }

        public void Spawn(Attack attack)
        {
            _server.SetChannel(3);
            SpawnAttackMessage sam = (SpawnAttackMessage)Protocol.NewMessage(MessageTypes.SpawnAttack);
            sam.id = attack.ID;
            sam.position = attack.Position;
            sam.move = attack.Movement;
            sam.Encode();
            _server.SendAllMapReliable(attack.map,sam,true);
            attack.SetAttackManager(this);
            _addAttack.Add(attack);
        }

        public void Update(Map map, float ms)
        {
            _attacks.AddRange(_addAttack);
            _addAttack.Clear();
            for (int x = 0; x < _attacks.Count; x++)
            {
                _attacks[x].Update(ms);
                if ((!_attacks[x].Penetrates && map.Collides(_attacks[x])) || _attacks[x].lifeTime < 0)
                {
                    _attacks.RemoveAt(x);
                    x--;
                }
            }
        }
    }
}
