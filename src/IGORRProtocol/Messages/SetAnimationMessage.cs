using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;
using Microsoft.Xna.Framework;

namespace IGORR.Protocol.Messages
{
    public class SetAnimationMessage : IgorrMessage
    {
        public int objectID;
        public int animationNumber;
        public bool force;

        public SetAnimationMessage(NetOutgoingMessage outgoing, long timestamp)
            : base(outgoing, timestamp, MessageTypes.SetAnimation)
        {

        }

        public SetAnimationMessage(IgorrMessage incoming) : base(incoming) { }

        public override void Encode()
        {
            base.Encode();
            _outgoing.Write(objectID);
            _outgoing.Write(animationNumber);
            _outgoing.Write(force);
        }

        protected override void Decode()
        {
            objectID = _incoming.ReadInt32();
            animationNumber = _incoming.ReadInt32();
            force = _incoming.ReadBoolean();
        }
    }
}
