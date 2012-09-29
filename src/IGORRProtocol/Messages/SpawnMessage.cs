using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Lidgren.Network;

namespace IGORR.Protocol.Messages
{
    public class SpawnMessage : IgorrMessage
    {
        public Vector2 position;
        public Vector2 move;
        public int objectType;
        public int id;
        public int groupID;
        public string Name;
        public string CharName = "";

        public SpawnMessage(NetOutgoingMessage outgoing, long timestamp)
            : base(outgoing, timestamp, MessageTypes.Spawn)
        { Name = id.ToString(); move = Vector2.Zero; }

        public SpawnMessage(IgorrMessage m) : base(m) { }

        public override void Encode()
        {
            base.Encode();
            _outgoing.Write(position.X);
            _outgoing.Write(position.Y);
            _outgoing.Write(move.X);
            _outgoing.Write(move.Y);
            _outgoing.Write(objectType);
            _outgoing.Write(id);
            _outgoing.Write(groupID);
            _outgoing.Write(Name);
            _outgoing.Write(CharName);
        }

        protected override void Decode()
        {
            position = new Vector2(_incoming.ReadFloat(), _incoming.ReadFloat());
            move = new Vector2(_incoming.ReadFloat(), _incoming.ReadFloat());
            objectType = _incoming.ReadInt32();
            id = _incoming.ReadInt32();
            groupID = _incoming.ReadInt32();
            Name = _incoming.ReadString();
            CharName = _incoming.ReadString();
        }
    }
}
