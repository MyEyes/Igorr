﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using IGORR.Protocol;
using IGORR.Protocol.Messages;
using IGORR.Server.Logic.Body;

namespace IGORR.Server.Logic
{
    public class Player : GameObject
    {
        Vector2 _speed;
        Vector2 _moveVector;
        bool _moveSet = false;
        bool hasStoppedMoving = false;
        Vector2 _lastSpeed = Vector2.Zero;
        public const float gravity = 60*2.5f;
        bool _onGround;
        protected int _maxhp = 50;
        protected int _hp = 50;
        protected float _invincibleTime = 3;

        protected int _groupID = 0;

        public bool ShadowsOn { get; set; }

        const float baseSpeed = 60;

        int wasMoving = 0;

        int changed = 0;

        /*
        int Level=1;
        int Exp=0;
        int LastExp = 0;
        int ExpToNextLevel=70;
        int DmgBonus=5;
        int DefBonus=3;
         */

        protected bool _invincible = false;
        string _charFile = "";
        //List<BodyPart> _bodyParts;
        //BodyPart[] _attackSlots = new BodyPart[3];
        //BodyPart _completeBody;
        Body.Body _body;
        Inventory _inventory;
        bool jump = false;
        bool flying = false;
        bool Left = false;
        bool wallCollision;
        Vector4 _collisionOffset;
        float attackCooldown = 0f;
        protected float attackerTimeout = 0f;
        int attackerID;

        //protected BaseBody _baseBody = new BaseBody();

        const int updateAtLeastAllXFrames = 60;
        int updateCountdown = updateAtLeastAllXFrames;

        public Vector2 _lastPosition;
        public Vector2 _lastlastSpeed;

        float stunTimeout;
        protected bool stunned;
        protected bool airstun = false;

        public Player(IMap map, Rectangle spawnPos, int id)
            : base(map,spawnPos, id)
        {
            _groupID = 1;
            _speed = Vector2.Zero;
            _onGround = false;
            //_bodyParts = new List<BodyPart>();
            //_bodyParts.Add(_baseBody);
            _collisionOffset = new Vector4((((16 - spawnPos.Width) % 16 + 16) % 16) + 0.6f, 0, (((16 - spawnPos.Height) % 16 + 16) % 16) + 0.99975f, 0.0015f);
            flying = true;
            //_completeBody = new BodyPart();
            _body = new Body.Body(this);
            //CalculateTotalBonus();
            ShadowsOn = true;
            _objectType = 0;
            _body = new Body.Body(this);
            _inventory = new Inventory(this);
        }

        public Player(IMap map, string CharFile, Rectangle spawnPos, int id):base(map,spawnPos,id)
        {
            _groupID = 1;
            _charFile = CharFile;
            StreamReader reader = new StreamReader("Content\\chars\\" + CharFile + ".chr");
            string fileContent = reader.ReadToEnd();
            reader.Close();
            string[] lines = fileContent.Split(new string[] { "\n", Environment.NewLine }, StringSplitOptions.None);
            int.TryParse(lines[0], out _rect.Width);
            int.TryParse(lines[1], out _rect.Height);
            _position.X -= _rect.Width - 16;
            _position.Y -= _rect.Height - 16;
            int.TryParse(lines[5], out _hp);
            _maxhp = _hp;
            ShadowsOn = true;
            _speed = Vector2.Zero;
            _onGround = false;
            //_bodyParts = new List<BodyPart>();
            //_bodyParts.Add(_baseBody);
            _collisionOffset = new Vector4((((16 - _rect.Width) % 16 + 16) % 16) + 0.6f, 0, (((16 - _rect.Height) % 16 + 16) % 16) + 0.99975f, 0.0015f);
            flying = true;
            //_completeBody = new BodyPart();
            //CalculateTotalBonus();
            _objectType = 0;

            _body = new Body.Body(this);
            _inventory = new Inventory(this);
        }

        public virtual void Update(IMap map, float seconds)
        {
            if (changed > 0)
                changed--;
            _map = map;
            _lastlastSpeed = _lastSpeed;
            _lastSpeed = Speed;
            if (_moveSet)
            {
                Move(_moveVector.X, _moveVector.Y, true);
            }

            if (!wallCollision) _speed.Y += gravity * seconds;
            if (_speed.X < 0) Left = true;
            else if (_speed.X > 0) Left = false;
            //Making sure we don't glitch through walls
            //_lastlastSpeed = _lastSpeed;
            //_lastPosition = _position;

            _body.Update(seconds * 1000f);


            wallCollision = false;
            TryMove(_speed * seconds);

            if (wallCollision)
                _speed.Y = 0;

            if (!map.MapBoundaries.Contains((int)MidPosition.X, (int)MidPosition.Y))
            {  
                GetDamage(10);
            }

            _map.DoPlayerEvents(this);

            _invincibleTime -= seconds;
            _speed.X = 0;
            attackCooldown -= seconds;

            if (attackerTimeout > -1)
            {
                attackerTimeout -= seconds;
                if (attackerTimeout < 0)
                    attackerID = -1;
            }
            else if (attackerTimeout < -1)
            {
                attackerTimeout = 0;
                if (_hp < _maxhp)
                {
                    //this.GetDamage(-1);
                }
            }
            if (_onGround || wallCollision)
                airstun = false;
            if (stunTimeout <= 0 && !airstun)
                stunned = false;
            stunTimeout -= seconds;

            //Make sure that hasStoppedMoving is only set for one frame
            //when this guy stops moving
            if (wasMoving > 0)
            {
                hasStoppedMoving = false;
                wasMoving--;
            }
            else if (wasMoving == 0)
            {
                hasStoppedMoving = true;
                _moveVector = Vector2.Zero;
                wasMoving--;
            }
            else
                hasStoppedMoving = false;
        }

        public void SetAnimation(bool force, int extraID)
        {
            SetAnimationMessage ani = (SetAnimationMessage)map.ObjectManager.Server.ProtocolHelper.NewMessage(MessageTypes.SetAnimation);
            ani.force = force;
            ani.animationNumber = extraID;
            ani.objectID = _id;
            ani.Encode();
            _map.ObjectManager.Server.SendAllMapReliable(_map, ani, true);
        }

        public void Knockback(float x, float y)
        {
            Knockback(new Vector2(x, y));
        }

        public void Knockback(Vector2 move)
        {
            _speed = move;
            stunTimeout = 0.3f;
            stunned = true;
            airstun = true;
            _onGround = false;
            changed = 2;
            IGORR.Protocol.Messages.KnockbackMessage kbm = (IGORR.Protocol.Messages.KnockbackMessage) map.ObjectManager.Server.ProtocolHelper.NewMessage(IGORR.Protocol.MessageTypes.Knockback);
            kbm.id = this.ID;
            kbm.Move = move;
            kbm.Encode();
            _map.ObjectManager.Server.SendClient(this, kbm);
        }

        public void SetMove(Vector3 move)
        {
            _moveVector = new Vector2(move.X,move.Y);
            _moveSet = true;
            _speed.Y = move.Z;
        }

        void TryMove(Vector2 movement)
        {
            Position += Vector2.UnitX * movement.X;
            if (_map.Collides(this))
            {
                //Position -= Vector2.UnitX * movement.X;
                if (movement.X > 0)
                {
                    _position.X = (float)(16 * Math.Floor(_position.X / 16.0)) + _collisionOffset.X;
                    Position = _position;
                }
                else
                {
                    _position.X = (float)(16 * Math.Ceiling(_position.X / 16.0) + _collisionOffset.Y);
                    Position = _position;
                }
                wallCollision = true;
                _speed.X = 0;
            }

            _onGround = false;
            Position += Vector2.UnitY * movement.Y;
            if (_map.Collides(this))
            {
                if (movement.Y > 0)
                {
                    _onGround = true;
                    _position.Y = (float)(16 * Math.Floor(_position.Y / 16.0)) + _collisionOffset.Z;
                    Position = _position;
                }
                else if(movement.Y<0)
                {
                    _position.Y = (float)(16 * Math.Ceiling(_position.Y / 16.0)) + _collisionOffset.W;
                    Position = _position;
                    //Position -= Vector2.UnitY * movement.Y;
                }
                _speed.Y = 0;
            }
            if (_map.Collides(this))
                Console.WriteLine("Collision not resolved!\n ObjectID: " + ID.ToString()+" "+this.ToString());
            if (!jump && !_onGround)
                flying = true;
        }

        public bool CanAttack(int attackID)
        {
            bool can = false;

            if (attackCooldown < 0)
                if (attackID>=0 && attackID<_body.Attacks.Length && _body.Attacks[attackID] != null)
                    can = true;
             
            if (stunned)
                can = false;
            return can;
        }

        public void Attack(float coolDown)
        {
            attackCooldown = coolDown;
        }

        public void Move(float xDiff, float yDiff, bool forced=false)
        {
            if (stunned)
                return;
            changed = (changed == 2 || (!forced && (_moveVector.X != xDiff || _moveVector.Y != yDiff)) ? 2 : 0);
            _moveVector = new Vector2(xDiff, yDiff);
            _speed.X += baseSpeed * xDiff;
            _body.Move(xDiff, yDiff);
            _moveSet = forced;
            wasMoving = 2;
        }

        public void GetExp(int xp, Vector2 startPos)
        {
            //_baseBody.Exp += xp;
            IGORR.Protocol.Messages.ExpMessage xpm = (IGORR.Protocol.Messages.ExpMessage)map.ObjectManager.Server.ProtocolHelper.NewMessage(IGORR.Protocol.MessageTypes.ExpMessage);
            xpm.exp = xp;
            xpm.startPos = startPos;
            xpm.Encode();
            _map.ObjectManager.Server.SendClient(this, xpm);
            /*
            if (_baseBody.Exp > _baseBody.ExpToNextLevel)
            {
                LevelUp();
            }
             */
        }
        
        private void LevelUp()
        {
            /*
            _baseBody.Level++;
            int add = _baseBody.ExpToNextLevel - _baseBody.LastExp;
            _baseBody.LastExp = _baseBody.ExpToNextLevel;
            _baseBody.ExpToNextLevel += (add + add / 3);
            if (_baseBody.Level % 2 == 0)
            {
                _baseBody.AttBonus += _baseBody.Level / 7 + 1;
                IGORR.Protocol.Messages.PlayerInfoMessage pim = (IGORR.Protocol.Messages.PlayerInfoMessage)IGORR.Protocol.ProtocolHelper.NewMessage(IGORR.Protocol.MessageTypes.PlayerInfo);
                pim.playerID = this.ID;
                pim.Text = "Level UP!\n" + "Att+: " + (_baseBody.Level / 7 + 1).ToString();
                pim.Encode();
                _map.ObjectManager.Server.SendAllMap(_map, pim, true);
            }
            else
            {
                _baseBody.DefBonus += _baseBody.Level / 13 + 1;

                IGORR.Protocol.Messages.PlayerInfoMessage pim = (IGORR.Protocol.Messages.PlayerInfoMessage)IGORR.Protocol.ProtocolHelper.NewMessage(IGORR.Protocol.MessageTypes.PlayerInfo);
                pim.playerID = this.ID;
                pim.Text = "Level UP!\n" + "Def+: " + (_baseBody.Level / 13 + 1).ToString();
                pim.Encode();
                _map.ObjectManager.Server.SendAllMap(_map, pim, true);
            }
             */
        }

        public void GetDamage(int damage)
        {
            if (_invincibleTime < 0 && !_invincible)
            {
                if (damage > 0)
                {
                    //int dmgReduction = _random.Next(_baseBody.DefBonus / 2, _baseBody.DefBonus);
                    //damage = damage - dmgReduction;
                    if (damage < 0)
                        damage = 1;
                }
                _hp -= damage;
                _hp = _maxhp > _hp ? _hp : _maxhp;
                _invincibleTime = 0.6f;

                IGORR.Protocol.Messages.DamageMessage dm = (IGORR.Protocol.Messages.DamageMessage)map.ObjectManager.Server.ProtocolHelper.NewMessage(IGORR.Protocol.MessageTypes.Damage);
                dm.posX = this.MidPosition.X;
                dm.posY = this.MidPosition.Y;
                dm.playerID = this.ID;
                dm.damage = damage;
                dm.Encode();
                _map.ObjectManager.Server.SendAllMap(_map, dm, true);
            }
        }

        public void GetDamage(int damage, int attacker)
        {
            if (_invincible)
                return;
            attackerID = attacker;
            GetDamage(damage);
            _invincibleTime = 0;
            attackerTimeout = 2;
        }
        /*
        public void CalculateTotalBonus()
        {
            _completeBody.Clear();
            //_completeBody.jumpBonus = 100f;
            //_completeBody.speedBonus = 60f;
            for (int x = 0; x < _bodyParts.Count; x++)
                _completeBody.Add(_bodyParts[x]);
        }
         */

        public bool GivePart(BodyPart part)
        {/*
            for (int x = 0; x < _bodyParts.Count; x++)
                if (_bodyParts[x].GetID() == part.GetID())
                    return false;
            if (part.hasAttack)
            {
                for (int x = 0; x < _attackSlots.Length; x++)
                    if (_attackSlots[x] == null)
                    {
                        EquipAttack(x, part);
                        break;
                    }
            }
            _bodyParts.Add(part);
            CalculateTotalBonus();
            return true;
          */
            if (!_body.TryEquip(-1, part))
                _inventory.Add(part);
            return true;
        }

        public void GivePart(int id)
        {
            IPartContainer ip = _map.ObjectManager.Server.GetObject(id) as IPartContainer;
            if (ip != null)
            {
                this.GivePart(ip.Part);
                PickupMessage pum = (PickupMessage)map.ObjectManager.Server.ProtocolHelper.NewMessage(MessageTypes.Pickup);
                pum.PlayerID = this.ID;
                pum.id = ip.Part.GetID();
                pum.Encode();
                _map.ObjectManager.Server.SendAllMapReliable(map, pum, true);
            }
        }

        public void RemovePart(BodyPart part)
        {
            //_bodyParts.Remove(part);
            //CalculateTotalBonus();
            _body.Unequip(part);
        }

        public void Jump()
        {
            if (!stunned)
            {
                _body.Jump(1);
                changed = 2;
            }
            /*
            if (_onGround)
            {
                _speed.Y = -_completeBody.jumpBonus;
                _onGround = false;
                jump = false;
                flying = true;
            }
            else if (_completeBody.airJumpCount < _completeBody.airJumpMax)
            {
                _completeBody.airJumpCount++;
                _speed.Y -= _completeBody.airJumpStrength;
                _speed.Y = _speed.Y > -_completeBody.jumpBonus ? _speed.Y : -_completeBody.jumpBonus;
            }
             */
        }

        public bool HasPart(BodyPart part)
        {
            if(_inventory.Contains(part))
                return true;
            if (_body.HasEquipped(part))
                return true;
            return false;
        }

        public void EquipAttack(int slot, Body.AttackPart part)
        {
            _body.TryEquip(slot, part);
        }

        public Attack GetAttack(int id,Vector2 dir, int info)
        {
            Logic.Attack att = null;
            if (id < 0 || id > _body.Attacks.Length || _body.Attacks[id]==null)
                return null;
            int DmgBonus = 0;// _random.Next(_baseBody.AttBonus / 2, _baseBody.AttBonus + 1);
            att = _body.Attacks[id].GetAttack(this, dir, DmgBonus, info);
            if (att == null)
                return null;
            return att;
            /*
            switch (id)
            {
                case 1:
                    Rectangle startRect = this.Rect;
                    startRect.Height -= 4;
                    startRect.Y += 3;
                    startRect.X += this.LookLeft ? -8 : 8;
                    att = new Attack(_map, 15+DmgBonus, startRect, new Vector2(this.LastSpeed.X+(this.LookLeft ? -200 : 200), 0), 100, this.ID, this.GroupID, 1);
                    att.HitOnce = true;
                    this.Attack(0.3f);
                    break;
                case 2:
                    startRect = this.Rect;
                    startRect.Height -= 4;
                    startRect.Y += 3;
                    startRect.X += this.LookLeft ? -8 : 8;
                    att = new Attack(_map, 1+DmgBonus/5, startRect, new Vector2(this.LastSpeed.X+(this.LookLeft ? -200 : 200), 0), 200, this.ID, this.GroupID, 2);
                    att.HitOnce = false;
                    att.Penetrates = true;
                    this.Attack(0.6f);
                    break;
                case 3:
                    startRect = this.Rect;
                    startRect.Height -= 4;
                    startRect.Y += 3;
                    startRect.X += this.LookLeft ? -8 : 8;
                    att = new Grenade(_map, 0, startRect, new Vector2(this.LastSpeed.X + (this.LookLeft ? -60 : 60), this.LastSpeed.Y-70), 2500, this.ID, this.GroupID, 3);
                    att.HitOnce = false;
                    att.Penetrates = true;
                    this.Attack(5f);
                    break;
            }
            return att;
             * */
        }

        public virtual void Die()
        {

        }

        public void AttachAnimation(string animation)
        {
            AttachAnimationMessage aam = (AttachAnimationMessage)_map.ObjectManager.Server.ProtocolHelper.NewMessage(MessageTypes.AttachAnimation);
            aam.playerID = this._id;
            aam.animationFile = animation;
            aam.Encode();
            _map.ObjectManager.Server.SendAllMap(_map, aam, false);
        }

        public void Stun(float stunTime)
        {
            StunMessage aam = (StunMessage)_map.ObjectManager.Server.ProtocolHelper.NewMessage(MessageTypes.Stun);
            aam.id = this._id;
            aam.stunTime = stunTime;
            aam.Encode();
            _map.ObjectManager.Server.SendAllMap(_map, aam, false);

        }

        public int HP
        {
            get { return _hp; }
            set { _hp = value; }
        }

        public int MaxHP
        {
            get { return _maxhp; }
        }

        public bool OnGround
        {
            get { return _onGround; }
        }

        public bool OnWall
        {
            get { return wallCollision; }
        }

        public bool Flying
        {
            get { return flying; }
        }

        public override string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                _info = _name;
            }
        }

        public bool Jumping
        {
            get { return jump; }
        }

        public Vector2 Speed
        {
            set { _speed = value; }
            get { return _speed; }
        }

        public Vector2 LastSpeed
        {
            get { return _lastSpeed; }
        }

        public bool Invincible
        {
            get { return _invincible || _invincibleTime>0; }
        }

        public bool LookLeft
        {
            get { return Left; }
        }

        public int Attacker
        {
            get { return attackerID; }
        }

        public string CharFile
        {
            get { return _charFile; }
        }

        public int GroupID
        {
            get { return _groupID; }
        }
        /*
        public List<BodyPart> Parts
        {
            get { return _bodyParts; }
        }
         */

        public Vector2 LastMovement
        {
            get { return _moveVector; }
        }
        /*
        public int TotalXP
        {
            get { return _baseBody.Exp; }
        }

        public int NextLevelXP
        {
            get { return _baseBody.ExpToNextLevel; }
        }

        public int LastLevelXP
        {
            get { return _baseBody.LastExp; }
        }
         */

        public bool UpdateCountdown
        {
            get
            {
                bool val = updateCountdown--<=0;
                if (val)
                    updateCountdown = updateAtLeastAllXFrames;
                return val;
            }
        }

        public void SetTeam(int id)
        {
            _groupID = id;
        }

        public Body.Body Body
        {
            get { return _body; }
            set { _body = value; }
        }

        public Inventory Inventory
        {
            get { return _inventory; }
            set { _inventory = value; }
        }

        public bool MovementChanged
        {
            get { return changed > 0 || hasStoppedMoving; }
        }

        public bool StoppedMoving
        {
            get { return hasStoppedMoving; }
        }
    }
}