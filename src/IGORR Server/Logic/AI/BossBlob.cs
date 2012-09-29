using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace IGORR_Server.Logic.AI
{
    enum BossBlobPhase
    {
        JumpPhase,
        ShootPhase,
        SpawnPhase,
        EndPhase
    }
    class BossBlob : NPC
    {
        private Vector2 startPos;

        //Startup
        private float activityCountdown = 11f;
        private bool SetIntro = false;

        //Jump phase variables
        private float jumpDelay = 0;
        private int jumpCountdown = 2;
        private bool jumpLeft = true;
        private bool Landed = true;

        //ShootPhase
        private bool FixedDir = false;
        private float ShootCountdown;
        private float ShootDuration;
        private int ShotsCount = 0;

        private float HPmod = 2;

        //SpawnPhase
        private float spawnCountdown=1.2f;

        //EndPhase
        private float countdown = 10;
        private float spawnCountdown2 = 7;

        public bool defeated = false;
        public bool reset = false;
        bool hadTarget = false;

        private Player target;
        private BossBlobPhase _phase = BossBlobPhase.JumpPhase;
        /*
        public BossBlob(Rectangle spawnPos, int id)
            : base(spawnPos, id)
        {
            _groupID = 2;
            startPos = new Vector2(spawnPos.X, spawnPos.Y);
            GivePart(new BossBlobAttack1());
            GivePart(new BossBlobAttack2());
            GivePart(new BossBlobAttack3());
            GivePart(new BossBlobLegs());
            _name = "King Blob";
            _invincible = true;
            ShootDuration = 0.3f;
            ShootCountdown = 1.5f;
            IGORRProtocol.Messages.PlayMessage pm = (IGORRProtocol.Messages.PlayMessage)IGORRProtocol.Protocol.NewMessage(IGORRProtocol.MessageTypes.Play);
            pm.SongName = "BossIntro01";
            pm.Loop = false;
            pm.Encode();
            _map.ObjectManager.Server.SendAllMap(_map,pm);
        }
         */

        public BossBlob(Map map,string charfile, Rectangle spawnPos, int id)
            : base(map,charfile, spawnPos, id)
        {
            _XPBonus = 250;
            _groupID = 2;
            startPos = new Vector2(spawnPos.X, spawnPos.Y);
            GivePart(new BossBlobLegs());
            GivePart(new BossBlobAttack1());
            GivePart(new BossBlobAttack2());
            GivePart(new BossBlobAttack3());
            _name = "King Blob";
            _invincible = true;
            ShootDuration = 0.3f;
            ShootCountdown = 1.5f;
            IGORRProtocol.Messages.PlayMessage pm = (IGORRProtocol.Messages.PlayMessage)IGORRProtocol.Protocol.NewMessage(IGORRProtocol.MessageTypes.Play);
            pm.SongName = "BossIntro01";
            pm.Loop = false;
            pm.Queue = false;
            pm.Encode();
            _map.ObjectManager.Server.SendAllMap(_map, pm, true);
            pm = (IGORRProtocol.Messages.PlayMessage)IGORRProtocol.Protocol.NewMessage(IGORRProtocol.MessageTypes.Play);
            pm.SongName = "Boss01";
            pm.Loop = true;
            pm.Queue = true;
            pm.Encode();
            _map.ObjectManager.Server.SendAllMap(_map, pm, true);
            pm = (IGORRProtocol.Messages.PlayMessage)IGORRProtocol.Protocol.NewMessage(IGORRProtocol.MessageTypes.Play);
            pm.SongName = "Level01";
            pm.Loop = true;
            pm.Queue = true;
            pm.Encode();
            _map.ObjectManager.Server.SendAllMap(_map, pm, true);
            _hp = (int)(_hp * HPmod);
            _maxhp = (int)(_maxhp * HPmod);
            IGORRProtocol.Messages.SetPlayerStatusMessage hpm = (IGORRProtocol.Messages.SetPlayerStatusMessage) IGORRProtocol.Protocol.NewMessage(IGORRProtocol.MessageTypes.SetHP);
            hpm.playerID = id;
            hpm.maxHP = _maxhp;
            hpm.currentHP = _hp;
            hpm.Encode();
            _map.ObjectManager.Server.SendAllMap(_map, hpm, true);
        }

        public override void Update(Map map, float seconds)
        {
            if (activityCountdown > 0)
            {
                activityCountdown -= seconds;
                if (activityCountdown < 0)
                {
                    _invincible = false;
                    IGORRProtocol.Messages.SetAnimationMessage sam = (IGORRProtocol.Messages.SetAnimationMessage)IGORRProtocol.Protocol.NewMessage(IGORRProtocol.MessageTypes.SetAnimation);
                    sam.force = false;
                    sam.animationNumber = 0;
                    sam.objectID = ID;
                    sam.Encode();
                    _map.ObjectManager.Server.SendAllMap(_map, sam, true);
                    SetIntro = true;
                }

                if (!SetIntro)
                {
                    IGORRProtocol.Messages.SetAnimationMessage sam = (IGORRProtocol.Messages.SetAnimationMessage)IGORRProtocol.Protocol.NewMessage(IGORRProtocol.MessageTypes.SetAnimation);
                    sam.force = true;
                    sam.animationNumber = 1;
                    sam.objectID = ID;
                    sam.Encode();
                    _map.ObjectManager.Server.SendAllMap(_map, sam, true);
                    SetIntro = true;
                }
            }
            else
            {
                if (target == null || target.HP <= 0)
                {
                    AcquireTarget();
                    if (_hp < _maxhp && _phase != BossBlobPhase.EndPhase && target == null)
                    {
                        IGORRProtocol.Messages.DamageMessage dm = (IGORRProtocol.Messages.DamageMessage)IGORRProtocol.Protocol.NewMessage(IGORRProtocol.MessageTypes.Damage);
                        dm.posX=this._position.X;
                        dm.posY = this._position.Y;
                        dm.playerID = this.ID;
                        dm.damage = _hp - _maxhp;
                        dm.Encode();
                        _map.ObjectManager.Server.SendAllMap(_map, dm, true);
                        this.GetDamage(_hp - _maxhp);
                    }
                    if (hadTarget && target == null)
                    {
                        reset = true;
                        hadTarget = false;
                    }
                    if (target != null)
                    {
                        hadTarget = true;
                        reset = false;
                    }
                }

                if (HP < 20 && _phase != BossBlobPhase.EndPhase)
                {
                    _invincible = true;
                    defeated = true;
                    attackerTimeout = 15;
                    _phase = BossBlobPhase.EndPhase;
                    IGORRProtocol.Messages.SetAnimationMessage sam = (IGORRProtocol.Messages.SetAnimationMessage)IGORRProtocol.Protocol.NewMessage(IGORRProtocol.MessageTypes.SetAnimation);
                    sam.force = true;
                    sam.animationNumber = 0;
                    sam.objectID = ID;
                    sam.Encode();
                    _map.ObjectManager.Server.SendAllMap(_map, sam, true);
                    return;
                }

                if (target != null || _phase == BossBlobPhase.EndPhase)
                {
                    switch (_phase)
                    {
                        #region JumpPhase
                        case BossBlobPhase.JumpPhase:
                            if (OnGround)
                            {
                                if (!Landed)
                                {
                                    _map.ObjectManager.SpawnAttack(ID, 1);
                                    _map.ObjectManager.SpawnAttack(ID, 2);
                                    Landed = true;
                                }
                                jumpDelay -= seconds;
                                if (jumpDelay <= 0)
                                {
                                    jumpCountdown--;
                                    if (jumpCountdown == 0)
                                    {
                                        if (HP < 320*HPmod)
                                            _phase = BossBlobPhase.ShootPhase;
                                        break;
                                    }
                                    if (jumpCountdown <= 0)
                                    {
                                        jumpLeft = !jumpLeft;
                                        jumpCountdown = 2;
                                    }
                                    Jump();
                                    Landed = false;
                                    jumpDelay = 1.5f;
                                }
                            }
                            else
                            {
                                if (jumpLeft)
                                    Move(-0.65f);
                                else
                                    Move(0.65f);
                            }
                            break;
                        #endregion
                        #region ShootPhase
                        case BossBlobPhase.ShootPhase:
                            //if (!FixedDir)
                            //{
                            if (target.MidPosition.X > this.MidPosition.X)
                                Move(0.0001f);
                            else
                                Move(-0.0001f);
                            //    FixedDir = true;
                            //}
                            ShootCountdown -= seconds;
                            if (ShootCountdown <= 0)
                            {
                                _map.ObjectManager.SpawnAttack(ID, 3);
                                ShootDuration -= seconds;
                                if (ShootDuration <= 0)
                                {
                                    if (ShotsCount == 0)
                                    {
                                        ShotsCount++;
                                        ShootDuration = 0.4f;
                                        ShootCountdown = 1.9f;
                                        Jump();
                                    }
                                    else if (ShotsCount == 1)
                                    {
                                        ShotsCount++;
                                        ShootCountdown = 0.4f;
                                        ShootDuration = 0.3f;
                                    }
                                    else
                                    {
                                        ShootDuration = 0.3f;
                                        ShootCountdown = 1.5f;
                                        ShotsCount = 0;
                                        _phase = BossBlobPhase.JumpPhase;
                                        if (HP < 200 * HPmod)
                                            _phase = BossBlobPhase.SpawnPhase;
                                    }
                                    //FixedDir = false;

                                }
                            }
                            break;
                        #endregion
                        #region SpawnPhase
                        case BossBlobPhase.SpawnPhase:
                            spawnCountdown -= seconds;
                            if (spawnCountdown > 1f)
                                _map.ObjectManager.SpawnMob("4 " + (_random.Next(64) - 32 + _rect.X + _rect.Width / 2).ToString() + " " + (-_random.Next(40) + _rect.Y).ToString());
                            if (spawnCountdown < 0)
                            {
                                _phase = BossBlobPhase.JumpPhase;
                                spawnCountdown = 1.2f;
                            }
                            break;
                        #endregion
                        #region Endphase
                        case BossBlobPhase.EndPhase:
                            _invincible = true;
                            countdown -= seconds;
                            spawnCountdown2 -= seconds;
                            if (countdown > 9)
                            {
                                Jump();
                                
                            }
                            if (countdown > 6 && !OnGround )
                            {
                                if (_position.X>startPos.X)
                                    Move(-0.7f);
                                else
                                    Move(0.7f);
                            }
                            if (countdown < 0)
                            {
                                
                                _invincible = false;
                                GetDamage(1000);
                                IGORRProtocol.Messages.DamageMessage dm = (IGORRProtocol.Messages.DamageMessage)IGORRProtocol.Protocol.NewMessage(IGORRProtocol.MessageTypes.Damage);
                                dm.playerID = this._id;
                                dm.posX = this._position.X;
                                dm.posY = this._position.Y;
                                dm.damage = 1000;
                                dm.Encode();
                                _map.ObjectManager.Server.SendAllMap(_map, dm, false);
                                IGORRProtocol.Messages.PlayMessage pm = (IGORRProtocol.Messages.PlayMessage)IGORRProtocol.Protocol.NewMessage(IGORRProtocol.MessageTypes.Play);
                                pm.SongName = "";
                                pm.Loop = false;
                                pm.Queue = false;
                                pm.Encode();
                                _map.ObjectManager.Server.SendAllMap(_map, pm, true);
                                _map.SpawnItem(new PartPickup(new Wings(), _map, new Rectangle((int)this.MidPosition.X, (int)this.MidPosition.Y, 16, 16), ObjectManager.getID(), true));
                            }
                            if (spawnCountdown2 <= 0)
                            {
                                _map.ObjectManager.SpawnMob("4 " + (_random.Next(100) - 50 + _rect.X).ToString() + " " + (-_random.Next(40) + _rect.Y).ToString());
                                spawnCountdown2 = 0.2f;
                            }
                            break;
                        #endregion
                    }
                }
            }
            base.Update(map, seconds);
        }

        public void ChangeHPMod(float hpMod)
        {
            HPmod = hpMod;
        }

        public override Logic.Attack GetAttack(int id)
        {
            Logic.Attack att = null;
            switch (id)
            {
                case 1:
                    att = new Attack(_map,17, new Rectangle((int)_position.X + _rect.Width / 2, (int)_position.Y + _rect.Height - 8, 5, 8), new Vector2(-200, 0), 500, this.ID, this.GroupID, 5004);
                    att.HitOnce = true;
                    att.Knockback = new Vector2(-200, -50);
                    break;
                case 2:
                    att = new Attack(_map,17, new Rectangle((int)_position.X + _rect.Width / 2, (int)_position.Y + _rect.Height - 8, 5, 8), new Vector2(200, 0), 500, this.ID, this.GroupID, 5004);
                    att.HitOnce = true;
                    att.Knockback = new Vector2(200, -50);
                    break;
                case 3:
                    if (LookLeft)
                    {
                        att = new Attack(_map,4, new Rectangle((int)_position.X + _rect.Width / 2, (int)_position.Y + _rect.Height - 16, 5, 8), new Vector2(-200, 0), 2200, this.ID, this.GroupID, 5005);
                    }
                    else
                    {
                        att = new Attack(_map,4, new Rectangle((int)_position.X + _rect.Width / 2, (int)_position.Y + _rect.Height - 16, 5, 8), new Vector2(200, 0), 2200, this.ID, this.GroupID, 5005);
                    }
                    att.HitOnce = true;
                    break;
            }
            return att;
        }

        private void AcquireTarget()
        {
            target = _map.ObjectManager.GetPlayerInArea(new Rectangle(16 * 58, 16 * 13, 16 * 27, 16 * 14));
        }

        public override PartPickup GetDrop(Map map)
        {
            return null;
        }
    }
}
