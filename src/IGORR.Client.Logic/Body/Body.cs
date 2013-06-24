using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using IGORR.Protocol.Messages;
using IGORR.Protocol;

namespace IGORR.Client.Logic.Body
{
    public enum BodyPartType
    {
        BaseBody,
        Attack,
        Utility,
        Movement,
        Armor,
        None
    }

    public class Body
    {
        Player owner;
        BaseBody BaseBody = null;
        public AttackPart[] Attacks = new AttackPart[4];
        public UtilityPart[] Utility = new UtilityPart[2];
        public MovementPart[] Movement = new MovementPart[2];
        public ArmorPart[] Armor = new ArmorPart[2];
        public int changes = 0;

        public Body(Player owner)
        {
            this.owner = owner;
        }

        void ChangeBaseBody(BaseBody newBaseBody)
        {
            ReturnToInventory();
        }

        void ReturnToInventory()
        {
            changes++;
            for (int x = 0; x < Attacks.Length; x++)
            {
                owner.Inventory.Add(Attacks[x]);
            }
        }

        public bool TryEquip(int slot, BodyPart part, bool dropped=false)
        {
            changes++;
            if (owner.Map != null && dropped)
            {
                MoveItemMessage mim = (MoveItemMessage)owner.Map.ProtocolHelper.NewMessage(MessageTypes.MoveItem);
                mim.Slot = slot;
                mim.Quantity = 1;
                mim.id = part.GetID();
                mim.To = MoveTarget.Body;
                mim.Encode();
                owner.Map.SendMessage(mim, true);
            }

            BodyPartType type = part.Type;
            switch (type)
            {
                case BodyPartType.Armor:
                    for (int x = 0; x < Armor.Length; x++)
                        if ((Armor[x] == null && (slot < 0 || slot >= Armor.Length)) || x == slot)
                        {
                            if (x == slot && Armor[x] != null && Armor[x] != part)
                                owner.Inventory.Add(Armor[x]);
                            Armor[x] = part as ArmorPart;
                            return true;
                        }
                    break;
                case BodyPartType.Attack:
                    for (int x = 0; x < Attacks.Length; x++)
                        if ((Attacks[x] == null && (slot < 0 || slot >= Attacks.Length)) || x == slot)
                        {
                            if (x == slot && Attacks[x] != null && Attacks[x] != part)
                                owner.Inventory.Add(Attacks[x]);
                            Attacks[x] = part as AttackPart;
                            return true;
                        }
                    break;
                case BodyPartType.Movement:
                    for (int x = 0; x < Movement.Length; x++)
                        if ((Movement[x] == null && (slot < 0 || slot >= Movement.Length)) || x == slot)
                        {
                            if (x == slot && Movement[x] != null && Movement[x] != part)
                                owner.Inventory.Add(Movement[x]);
                            Movement[x] = part as MovementPart;
                            return true;
                        }
                    break;
                case BodyPartType.Utility:
                    for (int x = 0; x < Utility.Length; x++)
                        if ((Utility[x] == null && (slot < 0 ||slot>=Utility.Length)) || x == slot)
                        {
                            if (x == slot && Utility[x]!=null && Utility[x]!=part)
                                owner.Inventory.Add(Utility[x]);
                            Utility[x] = part as UtilityPart;
                            return true;
                        }
                    break;
                case BodyPartType.BaseBody:

                    this.ChangeBaseBody(part as BaseBody);
                    return true;
            }
            return false;
        }

        public bool CanEquip(int slot, BodyPart part)
        {
            BodyPartType type = part.Type;
            switch (type)
            {
                case BodyPartType.Armor:
                    for (int x = 0; x < Armor.Length; x++)
                        if ((Armor[x] == null && (slot < 0 || slot >= Armor.Length)) || x == slot)
                        {
                            return true;
                        }
                    break;
                case BodyPartType.Attack:
                    for (int x = 0; x < Attacks.Length; x++)
                        if ((Attacks[x] == null && (slot < 0 || slot >= Attacks.Length)) || x == slot)
                        {
                            return true;
                        }
                    break;
                case BodyPartType.Movement:
                    for (int x = 0; x < Movement.Length; x++)
                        if ((Movement[x] == null && (slot < 0 || slot >= Movement.Length)) || x == slot)
                        {
                            return true;
                        }
                    break;
                case BodyPartType.Utility:
                    for (int x = 0; x < Utility.Length; x++)
                        if ((Utility[x] == null && (slot < 0 || slot >= Utility.Length)) || x == slot)
                        {
                            return true;
                        }
                    break;
                case BodyPartType.BaseBody:
                    return true;
            }
            return false;
        }

        public void Unequip(BodyPart part, bool dropped = false)
        {
            changes++;
            if (owner.Map != null && dropped)
            {
                MoveItemMessage mim = (MoveItemMessage)owner.Map.ProtocolHelper.NewMessage(MessageTypes.MoveItem);
                mim.Slot = -1;
                mim.Quantity = 1;
                mim.id = part.GetID();
                mim.From = MoveTarget.Body;
                mim.Encode();
                owner.Map.SendMessage(mim, true);
            }

            for (int x = 0; x < Movement.Length; x++)
            {
                if (part.Equals(Movement[x]))
                {
                    Movement[x] = null;
                    return;
                }
            }

            for (int x = 0; x < Utility.Length; x++)
            {
                if (part.Equals(Utility[x]))
                {
                    Utility[x] = null;
                    return;
                }
            }

            for (int x = 0; x < Armor.Length; x++)
            {
                if (part.Equals(Armor[x]))
                {
                    Armor[x] = null;
                    return;
                }
            }

            for (int x = 0; x < Attacks.Length; x++)
            {
                if (part.Equals(Attacks[x]))
                {
                    Attacks[x] = null;
                    return;
                }
            }
        }

        public void Update(float ms) 
        {
            if (BaseBody != null)
            {
                BaseBody.Update(ms);
            }
            for (int x = 0; x < Movement.Length; x++)
            {
                if (Movement[x] == null) continue;
                Movement[x].Update(owner, ms);
            }
            
            for (int x = 0; x < Utility.Length; x++)
            {
                if (Utility[x] == null) continue;
                Utility[x].Update(owner, ms);
            }

            for (int x = 0; x < Attacks.Length; x++)
            {
                if (Attacks[x] == null) continue;
                Attacks[x].Update(owner, ms);
            }

            for (int x = 0; x < Armor.Length; x++)
            {
                if (Armor[x] == null) continue;
                Armor[x].Update(owner, ms);
            }
        }

        public void Move(float dir, float yDir)
        {
            if (BaseBody != null)
            {
                BaseBody.Move(dir,yDir);
            }

            for (int x = 0; x < Movement.Length; x++)
            {
                if (Movement[x] == null) continue;
                Movement[x].Move(owner, dir, yDir);
            }
        }

        public void Jump(float strength)
        {
            if (BaseBody != null)
            {
                BaseBody.Jump(strength);
            }
            for (int x = 0; x < Movement.Length; x++)
            {
                if (Movement[x] == null) continue;
                Movement[x].Jump(owner, strength);
            }
        }
    }
}
