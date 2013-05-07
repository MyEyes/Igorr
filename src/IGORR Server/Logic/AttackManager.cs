using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using IGORR.Protocol;
using IGORR.Protocol.Messages;

namespace IGORR.Server.Logic
{

    

    public class AttackManager:IAttackManager
    {
        List<Attack> _attacks;
        List<Attack> _addAttack;
        IGORR.Server.Server _server;

        public AttackManager(IGORR.Server.Server server)
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
                    _attacks[x].Hit();
                    damage += _attacks[x].Damage;
                    player.GetDamage(_attacks[x].Damage, _attacks[x].parentID);
                    if (_attacks[x].Knockback != Vector2.Zero)
                        player.Knockback(_attacks[x].Knockback);
                    if (_attacks[x].HitOnce)
                    {
                        Remove(x);
                        x--;
                    }
                }
            }
            return damage;
        }

        public void Remove(int index)
        {
            if (_attacks.Count > index)
            {
                Protocol.Messages.DeSpawnMessage dsm = (Protocol.Messages.DeSpawnMessage) Protocol.ProtocolHelper.NewMessage(MessageTypes.DeSpawn);
                dsm.id = _attacks[index].ID;
                dsm.Encode();
                _server.SendAllMapReliable(_attacks[index].map, dsm, true);

                _attacks.RemoveAt(index);
            }
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

        public void Spawn(IMap map, int damage, Rectangle rect, Vector2 mov, float lifeTime,int parentID, int id)
        {
            Attack attack = new Attack(map, damage, rect, mov, lifeTime, parentID, 0, id,map.ObjectManager.getID());
            Spawn(attack);
        }

        public void Spawn(Attack attack)
        {
            _server.SetChannel(3);
            SpawnAttackMessage sam = (SpawnAttackMessage)ProtocolHelper.NewMessage(MessageTypes.SpawnAttack);
            sam.id = attack.ID;
            sam.attackID = attack.AttackID;
            sam.position = attack.Rect;
            sam.move = attack.Movement;
            sam.Encode();
            _server.SendAllMapReliable(attack.map, sam, true);
            attack.SetAttackManager(this);
            _addAttack.Add(attack);
        }

        public void Update(IMap map, float ms)
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
