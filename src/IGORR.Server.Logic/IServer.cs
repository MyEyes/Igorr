using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IGORR.Protocol;
using Lidgren.Network;
using Microsoft.Xna.Framework;

namespace IGORR.Server.Logic
{
    public interface IServer
    {
        void ChangePlayerMap(Logic.Player player, int mapID, Vector2 position);

        void SendClient(Player player, IgorrMessage message);
        void SendAllMap(IMap map, IgorrMessage message, bool Reliable);
        void SendAllMapReliable(IMap map, IgorrMessage message, bool ordered);
        void SendAllExcept(IMap map, Player player, IgorrMessage message, bool Reliable);
        
        void SetChannel(int channel);
        int Channel { get; }

        bool Enabled { get;}
        void Enable();
        void Disable();
    }
}
