using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using IGORR.Protocol;
using IGORR.Protocol.Messages;

namespace IGORR.Server.Logic
{
    class ScorePost:EventObject
    {
        public enum ScoreColor
        {
            Neutral=0,
            Blue=1,
            Red=2
        }

        public static int BlueScore = 0;
        public static int RedScore = 0;
        public static int Progress = 0;

        ScoreColor _nativeColor;
        ScoreColor _currentColor;
        Rectangle Area;
        int score = 0;
        bool reset = false;
        static int needToReset = 0;

        public ScorePost(ScoreColor color, IMap map, Rectangle targetRect, int id)
            : base(map, targetRect, id)
        {
            this._nativeColor = color;
            _currentColor = _nativeColor;
            Area = targetRect;
            Area.X -= 64;
            Area.Width += 128;
            Area.Height += 48;
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
            if (needToReset > 0 && !reset)
            {
                _currentColor = _nativeColor;
                switch (_currentColor)
                {
                    case ScoreColor.Neutral:
                        score = 0;
                        break;
                    case ScoreColor.Blue:
                        score = 450;
                        break;
                    case ScoreColor.Red:
                        score = -450;
                        break;
                }
                needToReset--;
                    reset = true;
            }
            if (needToReset == 0)
            {
                reset = false;
            }
            Player RedPlayer = _map.ObjectManager.GetPlayerInArea(Area,2,true);
            Player BluePlayer = _map.ObjectManager.GetPlayerInArea(Area,3,true);
            if (RedPlayer != null)
                score--;
            if (BluePlayer != null)
                score++;
            if (score >= 150)
            {
                if (_currentColor != ScoreColor.Blue)
                {
                    _currentColor = ScoreColor.Blue;
                    Progress++;
                    if (_nativeColor == ScoreColor.Red)
                        Progress++;
                    ObjectInfoMessage oim = (ObjectInfoMessage)ProtocolHelper.NewMessage(MessageTypes.ObjectInfo);
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
                    BlueScore+=1;
                if (score > 450)
                    score = 450;
            }
            else if (score > -150 && score < 150)
            {
                if (_currentColor != ScoreColor.Neutral)
                {
                    if (_currentColor != _nativeColor && _nativeColor != ScoreColor.Neutral)
                    {
                        //Was captured red went neutral
                        if (_currentColor == ScoreColor.Red)
                            Progress += 2;
                        //Was captured Blue went neutral
                        else if (_currentColor == ScoreColor.Blue)
                            Progress -= 2;
                    }
                    else
                        //Was native blue went neutral
                        if (_currentColor == ScoreColor.Blue)
                            Progress -= 1;
                        //Was native red went neutral
                        else if (_currentColor == ScoreColor.Red)
                            Progress += 1;
                    _currentColor = ScoreColor.Neutral;
                    ObjectInfoMessage oim = (ObjectInfoMessage)ProtocolHelper.NewMessage(MessageTypes.ObjectInfo);
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
                    Progress--;
                    if (_nativeColor == ScoreColor.Blue)
                        Progress--;
                    ObjectInfoMessage oim = (ObjectInfoMessage)ProtocolHelper.NewMessage(MessageTypes.ObjectInfo);
                    oim.objectID = _id;
                    oim.info = "r";
                    oim.Encode();
                    int chan = map.ObjectManager.Server.Channel;
                    map.ObjectManager.Server.SetChannel(0);
                    map.ObjectManager.Server.SendAllMapReliable(_map, oim, false);
                    map.ObjectManager.Server.SetChannel(chan);
                }
                RedScore += 1;
                if (_nativeColor == ScoreColor.Blue)
                    RedScore += 1;
                if (score < -450)
                    score = -450;
            }

            if (RedScore - BlueScore > 32400)
            {
                MessageBoard.LastWinner = "Red team won!";
                NewGame();
            }
            else if (BlueScore - RedScore > 32400)
            {
                MessageBoard.LastWinner = "Blue team won!";
                NewGame();
            }
        }

        private void NewGame()
        {
            RedScore = 0;
            BlueScore = 0;
            Progress = 0;
            needToReset = 3;
            MessageBoard.Rounds++;
            for (int x = 0; x < map.ObjectManager.Objects.Count; x++)
            {
                Player player;
                if (map.ObjectManager.Objects[x] is Player)
                {
                    player = map.ObjectManager.Objects[x] as Player;
                    if (player.GroupID == 2)
                        map.ObjectManager.Server.ChangePlayerMap(player, 7, new Vector2(7 * 16, 92 * 16));
                    else if (player.GroupID == 3)
                        map.ObjectManager.Server.ChangePlayerMap(player, 8, new Vector2(92 * 16, 94 * 16));
                    else
                        map.ObjectManager.Server.ChangePlayerMap(player, 6, new Vector2(29 * 16, 27 * 16));
                }
            }
        }

        public override void SendInfo(Lidgren.Network.NetConnection connection)
        {
            ObjectInfoMessage oim = (ObjectInfoMessage)ProtocolHelper.GetContainerMessage(MessageTypes.ObjectInfo, connection);
            oim.objectID = _id;
            oim.info = _currentColor == ScoreColor.Neutral ? "n" : _currentColor == ScoreColor.Red ? "r" : "b";
            ProtocolHelper.SendContainer(oim, connection);
        }

        public override void Event(Player obj)
        {
        }
        
    }
}
