using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;

namespace IGORR.Protocol.Messages
{
    public enum InteractAction
    {
        StartInteract,
        EndInteract,
        Ask,
        Respond
    }
    public class InteractMessage : IgorrMessage
    {
        public int objectID;
        public InteractAction action;
        public int info;
        public string sinfo;

        public InteractMessage(NetOutgoingMessage outgoing, long timestamp)
            : base(outgoing, timestamp, MessageTypes.Interact)
        {

        }

        public InteractMessage(IgorrMessage incoming) : base(incoming) { }

        public override void Encode()
        {
            base.Encode();
            _outgoing.Write(objectID);
            _outgoing.Write((int)action);
            _outgoing.Write(info);
            _outgoing.Write(sinfo);
        }

        protected override void Decode()
        {
            objectID = _incoming.ReadInt32();
            action = (InteractAction)_incoming.ReadInt32();
            info = _incoming.ReadInt32();
            sinfo = _incoming.ReadString();
        }
    }
}
