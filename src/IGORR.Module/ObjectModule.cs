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
        private Dictionary<int, ObjectTemplate> _objectTemplates;
        private Dictionary<int, AttackTemplate> _attackTemplates;
        private Dictionary<int, EffectTemplate> _effectTemplates;

        public ObjectModule()
        {
            _objectTemplates = new Dictionary<int, ObjectTemplate>();
            _attackTemplates = new Dictionary<int, AttackTemplate>();
            _effectTemplates = new Dictionary<int, EffectTemplate>();
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
                else if (types[x].IsClass && types[x].IsSubclassOf(typeof(EffectTemplate)))
                {
                    EffectTemplate template = (EffectTemplate)types[x].GetConstructor(new Type[0]).Invoke(new object[0]);
                    Register(template);
                    Counter++;
                }
            }
            Console.WriteLine("Registered {0} templates in module.", Counter);
        }

        public Server.Logic.GameObject SpawnByIdServer(Server.Logic.IMap map, int typeId, int objectID, Point p, BinaryReader bin)
        {
            if(_objectTemplates.ContainsKey(typeId))
                return _objectTemplates[typeId].CreateServer(map,objectID,p,bin);
            return null;
        }

        public Client.Logic.GameObject SpawnByIdClient(Client.Logic.IMap map, int typeId, int objectID, Point p, string info)
        {
            if(_objectTemplates.ContainsKey(typeId))
                return _objectTemplates[typeId].CreateClient(map,objectID,p,info);
            return null;
        }

        public Client.Logic.Attack SpawnByIdClient(int typeID, int objectID, Vector2 dir, Point position, string info)
        {
            if (_attackTemplates.ContainsKey(typeID))
                return _attackTemplates[typeID].CreateClient(objectID, dir, position, info);
            return null;
        }

        public void DoEffect(int typeID, IGORR.Client.Logic.IMap map, Vector2 dir, Point position, string info)
        {
            if (_effectTemplates.ContainsKey(typeID))
                _effectTemplates[typeID].DoEffect(map, dir, position, info);
        }

        public ObjectControl GetControl(int typeID, BinaryReader reader)
        {
            if (_objectTemplates.ContainsKey(typeID))
                return _objectTemplates[typeID].GetEditorControl(reader);
            return null;
        }

        public void Register(ObjectTemplate template)
        {
            if (!_objectTemplates.ContainsKey(template.TypeID))
                _objectTemplates.Add(template.TypeID, template);
            else
                throw new InvalidOperationException(Environment.NewLine+"IDs must be unique! Conflicting: " + _objectTemplates[template.TypeID].ToString() + " and " + template.ToString()+Environment.NewLine);
        }

        public void Register(AttackTemplate template)
        {
            if (!_attackTemplates.ContainsKey(template.TypeID))
                _attackTemplates.Add(template.TypeID, template);
            else
                throw new InvalidOperationException(Environment.NewLine + "IDs must be unique! Conflicting: " + _attackTemplates[template.TypeID].ToString() + " and " + template.ToString() + Environment.NewLine);
        }

        public void Register(EffectTemplate template)
        {
            if (!_effectTemplates.ContainsKey(template.EffectID))
                _effectTemplates.Add(template.EffectID, template);
            else
                throw new InvalidOperationException(Environment.NewLine + "IDs must be unique! Conflicting: " + _effectTemplates[template.EffectID].ToString() + " and " + template.ToString() + Environment.NewLine);
        }

        public List<ObjectTemplate> GetObjectTemplates()
        {
            return _objectTemplates.Values.ToList();
        }

        public List<AttackTemplate> GetAttackTemplates()
        {
            return _attackTemplates.Values.ToList();
        }

        public List<EffectTemplate> GetEffectTemplates()
        {
            return _effectTemplates.Values.ToList();
        }
    }
}
