using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

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

    class Body
    {
        Player owner;

        BaseBody BaseBody = null;
        public AttackPart[] Attacks = new AttackPart[4];
        public UtilityPart[] Utility = new UtilityPart[2];
        public MovementPart[] Movement = new MovementPart[2];
        public ArmorPart[] Armor = new ArmorPart[2];

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
            for (int x = 0; x < Attacks.Length; x++)
            {
                owner.Inventory.Add(Attacks[x]);
            }
        }

        public bool TryEquip(int slot, BodyPart part)
        {
            BodyPartType type = part.Type;
            switch (type)
            {
                case BodyPartType.Armor:
                    for (int x = 0; x < Armor.Length; x++)
                        if ((Armor[x] == null && slot < 0) || x == slot)
                        {
                            Armor[x] = part as ArmorPart;
                            return true;
                        }
                    break;
                case BodyPartType.Attack:
                    for (int x = 0; x < Attacks.Length; x++)
                        if ((Attacks[x] == null && slot < 0) || x == slot)
                        {
                            Attacks[x] = part as AttackPart;
                            return true;
                        }
                    break;
                case BodyPartType.Movement:
                    for (int x = 0; x < Movement.Length; x++)
                        if ((Movement[x] == null && slot < 0) || x == slot)
                        {
                            Movement[x] = part as MovementPart;
                            return true;
                        }
                    break;
                case BodyPartType.Utility:
                    for (int x = 0; x < Utility.Length; x++)
                        if ((Utility[x] == null && slot < 0) || x == slot)
                        {
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

        public void Update(float ms) 
        {
            for (int x = 0; x < Movement.Length; x++)
            {
                if (Movement[x] == null) break;
                Movement[x].Update(owner, ms);
            }
            
            for (int x = 0; x < Utility.Length; x++)
            {
                if (Utility[x] == null) break;
                Utility[x].Update(owner, ms);
            }

            for (int x = 0; x < Attacks.Length; x++)
            {
                if (Attacks[x] == null) break;
                Attacks[x].Update(owner, ms);
            }

            for (int x = 0; x < Armor.Length; x++)
            {
                if (Armor[x] == null) break;
                Armor[x].Update(owner, ms);
            }
        }

        public void Move(float dir)
        {
            for (int x = 0; x < Movement.Length; x++)
            {
                if (Movement[x] == null) break;
                Movement[x].Move(owner, dir);
            }
        }

        public void Jump(float strength)
        {
            for (int x = 0; x < Movement.Length; x++)
            {
                if (Movement[x] == null) break;
                Movement[x].Jump(owner, strength);
            }
        }
    }
}
