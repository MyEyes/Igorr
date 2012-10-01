using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;

namespace IGORR.Protocol.Messages
{
    public class DamageMessage:IgorrMessage
    {       
        public int damage;
        public int playerID;
        public float posX;
        public float posY;

        public DamageMessage(NetOutgoingMessage outgoing, long timestamp)
            : base(outgoing, timestamp, MessageTypes.Damage)
        { }

        public DamageMessage(IgorrMessage m) : base(m) { }

        public override void Encode()
        {
            base.Encode();
            _outgoing.Write(damage);
            _outgoing.Write(playerID);
            _outgoing.Write(posX);
            _outgoing.Write(posY);
        }

        protected override void Decode()
        {
            damage = _incoming.ReadInt32();
            playerID=_incoming.ReadInt32();
            posX = _incoming.ReadFloat();
            posY = _incoming.ReadFloat();
        }
    }
}
