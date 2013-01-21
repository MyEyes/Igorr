using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

using IGORR.Protocol;
using IGORR.Protocol.Messages;

namespace IGORR.Client.Logic
{

    interface IWorldController
    {
        IWorldController Controller { get; }
    }
}
