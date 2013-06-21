using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using IGORR.Server.Logic;
using IGORR.Protocol;
using IGORR.Protocol.Messages;

namespace IGORR.Server.Logic
{

    public interface IObjectManager
    {
        void Add(Vector2 position, int id, int type);
        void Add(GameObject obj);
        void SpawnAttack(int playerID, Vector2 dir, int attackID, int info);
        void Remove(GameObject obj);
        void RemoveQuiet(GameObject obj);
        void Remove(int id);

        bool UpdatePosition(Vector2 newPos, Vector3 newMove, int id, long timestamp);

        GameObject GetObject(int id);
        Player GetPlayer(int id);
        Player GetPlayerAround(Vector2 position, float radius);
        Player GetPlayerInArea(Rectangle area);
        Player GetPlayerInArea(Rectangle area, int groupID, bool sameGroup);
        int getID();
        List<GameObject> Objects { get; }
        IServer Server { get; }
        IMap Map { get; }
        IAttackManager AttackManager { get; }
    }

}
