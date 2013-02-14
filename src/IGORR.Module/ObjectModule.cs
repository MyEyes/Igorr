using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using Microsoft.Xna.Framework;

namespace IGORR.Modules
{
    public class ObjectModule
    {
        private Dictionary<int, ObjectTemplate> _templates;
        private Dictionary<int, AttackTemplate> _attackTemplates;

        public ObjectModule()
        {
            _templates = new Dictionary<int, ObjectTemplate>();
            _attackTemplates = new Dictionary<int, AttackTemplate>();
            Assembly thisAssembly = System.Reflection.Assembly.GetCallingAssembly();
            Type[] types = thisAssembly.GetTypes();
            int Counter = 0;

            for (int x = 0; x < types.Length; x++)
            {
                if (types[x].IsClass && types[x].IsSubclassOf(typeof(ObjectTemplate)))
                {
                    ObjectTemplate template = (ObjectTemplate)types[x].GetConstructor(new Type[0]).Invoke(new object[0]);
                    Register(template);
                    Counter++;
                }
                else if (types[x].IsClass && types[x].IsSubclassOf(typeof(AttackTemplate)))
                {
                    AttackTemplate template = (AttackTemplate)types[x].GetConstructor(new Type[0]).Invoke(new object[0]);
                    Register(template);
                    Counter++;
                }
            }
            Console.WriteLine("Registered {0} templates in module.", Counter);
        }

        public Server.Logic.GameObject SpawnByIdServer(Server.Logic.IMap map, int typeId, int objectID, Point p, BinaryReader bin)
        {
            if(_templates.ContainsKey(typeId))
                return _templates[typeId].CreateServer(map,objectID,p,bin);
            return null;
        }

        public Client.Logic.GameObject SpawnByIdClient(Client.Logic.IMap map, int typeId, int objectID, Point p, string info)
        {
            if(_templates.ContainsKey(typeId))
                return _templates[typeId].CreateClient(map,objectID,p,info);
            return null;
        }

        public Client.Logic.Attack SpawnByIdClient(int typeID, int objectID, Vector2 dir, Point position, string info)
        {
            if (_attackTemplates.ContainsKey(typeID))
                return _attackTemplates[typeID].CreateClient(objectID, dir, position, info);
            return null;
        }

        public void Register(ObjectTemplate template)
        {
            if (!_templates.ContainsKey(template.TypeID))
                _templates.Add(template.TypeID, template);
            else
                throw new InvalidOperationException(Environment.NewLine+"IDs must be unique! Conflicting: " + _templates[template.TypeID].ToString() + " and " + template.ToString()+Environment.NewLine);
        }

        public void Register(AttackTemplate template)
        {
            if (!_attackTemplates.ContainsKey(template.TypeID))
                _attackTemplates.Add(template.TypeID, template);
            else
                throw new InvalidOperationException(Environment.NewLine + "IDs must be unique! Conflicting: " + _attackTemplates[template.TypeID].ToString() + " and " + template.ToString() + Environment.NewLine);
        }
    }
}
