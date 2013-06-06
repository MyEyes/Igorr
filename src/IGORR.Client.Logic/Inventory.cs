using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace IGORR.Client.Logic
{
    public interface ICollectible
    {
        Texture2D Texture { get; }
        int MaxStacks { get; }
    }

    public class Inventory
    {
        Player _owner;
        List<ICollectible> _items;

        public Inventory(Player owner)
        {
            _items = new List<ICollectible>();
            _owner = owner;
        }

        public void Add(ICollectible c)
        {
            _items.Add(c);
        }

        public void Remove(ICollectible c)
        {
            _items.Remove(c);
        }

        public void Insert(ICollectible c, int index)
        {
            if (index < 0) index = 0;
            if (index >= _items.Count)
                index = _items.Count;
            _items.Insert(index, c);
        }

        public ICollectible this[int i]
        {
            get { return _items[i]; }
        }

        public int Count
        {
            get { return _items.Count; }
        }
    }
}
