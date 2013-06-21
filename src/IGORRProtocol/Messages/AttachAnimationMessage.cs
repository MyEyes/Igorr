using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;
using Microsoft.Xna.Framework;

namespace IGORR.Protocol.Messages
{
    public class AttachAnimationMessage : IgorrMessage
    {
        public int playerID;
        public string animationFile;

        public AttachAnimationMessage(NetOutgoingMessage outgoing, long timestamp)
            : base(outgoing, timestamp, MessageTypes.AttachAnimation)
        { }

        public AttachAnimationMessage(IgorrMessage m) : base(m) { }

        public override void Encode()
        {
            base.Encode();
            _outgoing.Write(playerID);
            _outgoing.Write(animationFile);
        }

        protected override void Decode()
        {
            playerID = _incoming.ReadInt32();
            animationFile = _incoming.ReadString();
        }
    }
}
