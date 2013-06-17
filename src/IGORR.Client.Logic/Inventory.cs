using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using IGORR.Protocol;
using IGORR.Protocol.Messages;

namespace IGORR.Client.Logic
{
    public interface ICollectible
    {
        Texture2D Texture { get; }
        int MaxStacks { get; }
        int GetID();
    }

    public class Inventory
    {
        Player _owner;
        List<ICollectible> _items;
        int _actions = 0;

        public Inventory(Player owner)
        {
            _items = new List<ICollectible>();
            _owner = owner;
        }

        public void Add(ICollectible c, bool dropped=false)
        {
            if (dropped)
            {
                MoveItemMessage mim = (MoveItemMessage)_owner.Map.ProtocolHelper.NewMessage(MessageTypes.MoveItem);
                mim.Slot = -1;
                mim.Quantity = 1;
                mim.id = c.GetID();
                mim.To = MoveTarget.Inventory;
                mim.Encode();

                if (_owner.Map != null)
                    _owner.Map.SendMessage(mim, true);
            }
            _items.Add(c);
            _actions++;
        }

        public void Remove(ICollectible c, bool dropped)
        {
            if (dropped)
            {
                MoveItemMessage mim = (MoveItemMessage)_owner.Map.ProtocolHelper.NewMessage(MessageTypes.MoveItem);
                mim.Slot = -1;
                mim.Quantity = 1;
                mim.id = c.GetID();
                mim.From = MoveTarget.Inventory;
                mim.Encode();

                if (_owner.Map != null)
                    _owner.Map.SendMessage(mim, true);
            }
            _items.Remove(c);
            _actions++;
        }

        public void Insert(ICollectible c, int index, bool dropped)
        {
            if (index < 0) index = 0;
            if (index >= _items.Count)
                index = _items.Count;

            if (dropped)
            {
                MoveItemMessage mim = (MoveItemMessage)_owner.Map.ProtocolHelper.NewMessage(MessageTypes.MoveItem);
                mim.Slot = index;
                mim.Quantity = 1;
                mim.id = c.GetID();
                mim.To = MoveTarget.Inventory;
                mim.Encode();
                if (_owner.Map != null)
                    _owner.Map.SendMessage(mim, true);
            }
            _items.Insert(index, c);
            _actions++;
        }

        public ICollectible this[int i]
        {
            get { return _items[i]; }
        }

        public int Count
        {
            get { return _items.Count; }
        }

        public int Actions { get { return _actions; } }
    }
}
