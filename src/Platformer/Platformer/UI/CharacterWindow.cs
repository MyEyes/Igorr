using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace IGORR.Client.UI
{
    class CharacterWindow:UIWindow,IDroppable
    {
        Label _nameLabel, _nameVLabel;
        Label _LevelLabel, _LevelVLabel;
        Logic.Player _player;

        readonly Vector2 IconDistance = new Vector2(20, 0);
        readonly Vector2 AttackOffset = new Vector2(10, 50);
        readonly Vector2 UtilityOffset = new Vector2(10, 80);
        readonly Vector2 MovementOffset = new Vector2(10, 110);
        readonly Vector2 ArmorOffset = new Vector2(10, 140);

        ItemIcon[] AttackParts;
        ItemIcon[] MovementParts;
        ItemIcon[] UtilityParts;
        ItemIcon[] ArmorParts;

        public CharacterWindow(Vector2 position, Vector2 size, Logic.Player player)
            :base(position, size, Content.ContentInterface.LoadTexture("UITest"),Content.ContentInterface.LoadTexture("UITest"))
        {
            _player = player;
            _nameLabel = new Label(this, new Vector2(10, 4), "Name:");
            _nameVLabel = new Label(this, new Vector2(100, 4), player.Name);
            _LevelLabel = new Label(this, new Vector2(10, 22), "Level:");
            _LevelVLabel = new Label(this, new Vector2(100, 22), player.Level.ToString());
            AddChild(_nameLabel);
            AddChild(_nameVLabel);
            AddChild(_LevelLabel);
            AddChild(_LevelVLabel);
            Update();
        }

        public override void Update(float ms, Microsoft.Xna.Framework.Input.MouseState mouse)
        {
            base.Update(ms, mouse);
        }

        public void Update()
        {
            _nameVLabel.SetText(_player.Name);
            _LevelVLabel.SetText(_player.Level.ToString());

            if (ArmorParts == null || ArmorParts.Length != _player.Body.Armor.Length)
                ArmorParts = new ItemIcon[_player.Body.Armor.Length];
            for (int x = 0; x < _player.Body.Armor.Length; x++)
            {
                this.RemoveChild(ArmorParts[x]);
                ArmorParts[x] = new ItemIcon(this, ArmorOffset+x*IconDistance, _player.Body.Armor[x]);
                this.AddChild(ArmorParts[x]);
            }

            if (UtilityParts == null || UtilityParts.Length != _player.Body.Utility.Length)
                UtilityParts = new ItemIcon[_player.Body.Utility.Length];
            for (int x = 0; x < _player.Body.Utility.Length; x++)
            {
                this.RemoveChild(UtilityParts[x]);
                UtilityParts[x] = new ItemIcon(this, UtilityOffset+x*IconDistance, _player.Body.Utility[x]);
                this.AddChild(UtilityParts[x]);
            }

            if (MovementParts == null || MovementParts.Length != _player.Body.Movement.Length)
                MovementParts = new ItemIcon[_player.Body.Movement.Length];
            for (int x = 0; x < _player.Body.Movement.Length; x++)
            {
                this.RemoveChild(MovementParts[x]);
                MovementParts[x] = new ItemIcon(this, MovementOffset+x*IconDistance, _player.Body.Movement[x]);
                this.AddChild(MovementParts[x]);
            }

            if (AttackParts == null || AttackParts.Length != _player.Body.Attacks.Length)
                AttackParts = new ItemIcon[_player.Body.Attacks.Length];
            for (int x = 0; x < _player.Body.Attacks.Length; x++)
            {
                this.RemoveChild(AttackParts[x]);
                AttackParts[x] = new ItemIcon(this, AttackOffset+x*IconDistance, _player.Body.Attacks[x]);
                this.AddChild(AttackParts[x]);
            }
        }

        public bool Drop(UI.IDraggable obj)
        {
            ItemIcon ic = obj as ItemIcon;
            if (ic != null)
            {
                Logic.Body.BodyPart part = ic.Item as Logic.Body.BodyPart;
                if (part != null)
                {
                    if (_player.Body.CanEquip((int)((_lastMouse.X - TotalOffset.X)/IconDistance.X), part))
                    {
                        ic.Drop(this);
                        _player.Body.TryEquip((int)((_lastMouse.X - TotalOffset.X)/IconDistance.X), part);
                        Update();
                        return true;
                    }
                }
            }
            return false;
        }

        public void Remove(IDraggable obj)
        {
            ItemIcon ic = obj as ItemIcon;
            if (ic != null)
            {
                this.RemoveChild(ic);
                _player.Body.Unequip(ic.Item as Logic.Body.BodyPart);
            }
            Update();
        }

    }
}
