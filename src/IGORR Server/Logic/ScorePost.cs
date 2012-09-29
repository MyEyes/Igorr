using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using IGORRProtocol;
using IGORRProtocol.Messages;

namespace IGORR_Server.Logic
{
    class ScorePost:EventObject
    {
        public enum ScoreColor
        {
            Neutral=0,
            Blue=1,
            Red=2
        }

        static int BlueScore = 0;
        static int RedScore = 0;

        ScoreColor _nativeColor;
        ScoreColor _currentColor;
        Rectangle Area;
        int score = 1000;

        public ScorePost(ScoreColor color, Map map, Rectangle targetRect, int id)
            : base(map, targetRect, id)
        {
            this._nativeColor = color;
            _currentColor = _nativeColor;
            Area = targetRect;
            Area.X -= 64;
            Area.Width += 128;
            Area.Y -= 64;
            Area.Height += 64;
            switch (color)
            {
                case ScoreColor.Neutral:
                    _objectType = 29;
                    score = 0;
                    break;
                case ScoreColor.Blue:
                    _objectType = 23;
                    score = 450;
                    break;
                case ScoreColor.Red:
                    _objectType = 22;
                    score = -450;
                    break;
            }
        }

        public override void Update(float ms)
        {
            base.Update(ms);
            Player RedPlayer = _map.ObjectManager.GetPlayerInArea(Area,2);
            Player BluePlayer = _map.ObjectManager.GetPlayerInArea(Area,3);
            if (RedPlayer != null)
                score--;
            if (BluePlayer != null)
                score++;
            if (score >= 150)
            {
                if (_currentColor != ScoreColor.Blue)
                {
                    _currentColor = ScoreColor.Blue;
                    ObjectInfoMessage oim = (ObjectInfoMessage)Protocol.NewMessage(MessageTypes.ObjectInfo);
                    oim.objectID=_id;
                    oim.info="b";
                    oim.Encode();
                    int chan = map.ObjectManager.Server.Channel;
                    map.ObjectManager.Server.SetChannel(0);
                    map.ObjectManager.Server.SendAllMapReliable(_map, oim, false);
                    map.ObjectManager.Server.SetChannel(chan);
                }
                BlueScore += 1;
                if (_nativeColor == ScoreColor.Red)
                    BlueScore += 1;
                if (score > 450)
                    score = 450;
            }
            else if (score > -150 && score < 150)
            {
                if (_currentColor != ScoreColor.Neutral)
                {
                    _currentColor = ScoreColor.Neutral;
                    ObjectInfoMessage oim = (ObjectInfoMessage)Protocol.NewMessage(MessageTypes.ObjectInfo);
                    oim.objectID = _id;
                    oim.info = "n";
                    oim.Encode();
                    int chan = map.ObjectManager.Server.Channel;
                    map.ObjectManager.Server.SetChannel(0);
                    map.ObjectManager.Server.SendAllMapReliable(_map, oim, false);
                    map.ObjectManager.Server.SetChannel(chan);
                }
            }
            else if (score < -150)
            {
                if (_currentColor != ScoreColor.Red)
                {
                    _currentColor = ScoreColor.Red;
                    ObjectInfoMessage oim = (ObjectInfoMessage)Protocol.NewMessage(MessageTypes.ObjectInfo);
                    oim.objectID = _id;
                    oim.info = "r";
                    oim.Encode();
                    int chan = map.ObjectManager.Server.Channel;
                    map.ObjectManager.Server.SetChannel(0);
                    map.ObjectManager.Server.SendAllMapReliable(_map, oim, false);
                    map.ObjectManager.Server.SetChannel(chan);
                }
                RedScore += 1;
                if (_nativeColor == ScoreColor.Red)
                    RedScore += 1;
                if (score < -450)
                    score = -450;
            }
        }

        public override void SendInfo(Lidgren.Network.NetConnection connection)
        {
            ObjectInfoMessage oim = (ObjectInfoMessage)Protocol.GetContainerMessage(MessageTypes.ObjectInfo, connection);
            oim.objectID = _id;
            oim.info = _currentColor == ScoreColor.Neutral ? "n" : _currentColor == ScoreColor.Red ? "r" : "b";
            Protocol.SendContainer(oim, connection);
        }

        public override void Event(Player obj)
        {
        }
        
    }
}
