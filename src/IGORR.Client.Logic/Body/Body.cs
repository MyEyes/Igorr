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

        void Update(float ms) { }

        public void Move(float dir)
        {
            for (int x = 0; x < Movement.Length; x++)
                Movement[x].Move(owner, dir);
        }

        public void Jump(float strength)
        {
            for (int x = 0; x < Movement.Length; x++)
                Movement[x].Jump(owner, strength);
        }
    }
}
