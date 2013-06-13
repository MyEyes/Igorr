using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using IGORR.Server;
using IGORR.Server.Logic;

namespace IGORR.Server.Logic.AI
{
    public class LuaNPC : NPC
    {
        string _script_file;

        public bool reacts_attackReady = false;
        public bool reacts_idle = false;
        public bool reacts_spotEnemy = false;
        public bool reacts_targetDead = false;
        public bool reacts_spotFriendly = false;
        public bool reacts_damageTaken = false;

        public bool lookingForTarget = false;
        public bool lookingForFriend = false;

        public bool interacts = false;

        bool _could_Attack = true;
        int _lastHP = 0;

        AIQueue queue;

        public LuaNPC(string script_file, IMap map, string charfile, Rectangle spawnPos, int id)
            : base(map, charfile, spawnPos, id)
        {
            _invincibleTime = 0;
            //_baseBody.speedBonus = 30;

            _script_file="Content/scripts/"+script_file;
            queue = new AIQueue(this);
            _objectType = 10001;
            _info = charfile;
            LuaVM.SetValue("me", this);
            LuaVM.DoString(script_file + " = require \"" + _script_file + "\"\n"+script_file+".spawn()\n"+script_file+".spawn_equip()");
            _script_file=script_file;
            if (interacts)
                _info += ";i";
        }

        public override void Update(IMap map, float seconds)
        {
            #region ReactToEvents
            if (reacts_attackReady && !_could_Attack && CanAttack(0))
            {
                LuaVM.SetValue("me", this);
                LuaVM.DoString(_script_file + ".react_spawn()\n");
            }

            if (reacts_idle && queue.Empty)
            {
                LuaVM.SetValue("me", this);
                LuaVM.DoString(_script_file + ".react_idle()\n");
            }
            Player player;
            if (reacts_spotEnemy && lookingForTarget && (player=_map.ObjectManager.GetPlayerInArea(new Rectangle(this._rect.Left-10,this._rect.Top-10,this._rect.Width+20,this._rect.Height+20),_groupID,false))!=null)
            {
                LuaVM.SetValue("me", this);
                LuaVM.SetValue("enemy", player);
                LuaVM.DoString(_script_file+".react_spotEnemy()\n");
            }
            if (reacts_spotFriendly && lookingForFriend && (player = _map.ObjectManager.GetPlayerInArea(new Rectangle(this._rect.Left - 10, this._rect.Top - 10, this._rect.Width + 20, this._rect.Height + 20), _groupID, true)) != null)
            {
                LuaVM.SetValue("me", this);
                LuaVM.SetValue("enemy", player);
                LuaVM.DoString(_script_file+".react_spotFriendly()\n");
            }
            if (reacts_damageTaken&&(_lastHP>_hp))
            {
                int damage = _lastHP - _hp;
                LuaVM.SetValue("me", this);
                LuaVM.DoString(_script_file+".react_damageTaken("+damage+")\n");
            }
            #endregion
            #region State Machine
            queue.Update(seconds*1000);
            #endregion
            _could_Attack = CanAttack(0);
            _lastHP = _hp;
            base.Update(map, seconds);
        }

        public void ClearState()
        {
            queue.Clear();
        }

        public void Move(int time, float dir)
        {
            queue.Add(new MoveCommand(this, time, dir));
        }

        public void Wait(int time)
        {
            queue.Add(new WaitCommand(this, time));
        }

        public void JumpCmd()
        {
            queue.Add(new JumpCommand(this));
        }


        public override void Interact(Player player, string sinfo, int info)
        {
            if (!interacts)
                return;
            LuaVM.SetValue("me", this);
            LuaVM.SetValue("player", player);
            LuaVM.DoString(_script_file + ".interact(\"" + sinfo + "\"," + info.ToString() + ")\n");
        }
        
    }
}
