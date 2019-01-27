using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;
using System.IO;
using static System.Net.Mime.MediaTypeNames;
using Engine;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using System.Linq;
using Scripts;
using System.Reflection;

namespace Editor
{
    public class Serializer
    {
        public static Serializer instance;
        public static Serializer GetInstance()
        {
            return instance;
        }
        List<Type> SerializableTypes = new List<Type>();
        public Serializer()
        {
            instance = this;
        }
        void UpdateSerializableTypes()
        {
            var typs = ScriptsManager.ScriptsAssembly.GetTypes().Where(t => t.IsSubclassOf(typeof(Component)));
            SerializableTypes = new List<Type>();

            SerializableTypes.AddRange(typeof(Engine.GameObject).Assembly.GetTypes()
                     .Where(type => (type.IsSubclassOf(typeof(Scripts.Component)))));

            SerializableTypes.AddRange(ScriptsManager.ScriptsAssembly.GetTypes().Where(t => t.IsSubclassOf(typeof(Component))));

            SerializableTypes.AddRange(typeof(Scripts.Component).Assembly.GetTypes()
                 .Where(type => (type.IsSubclassOf(typeof(Scripts.Component)) ||
                 (type.IsSubclassOf(typeof(GameObject)))))
                 .ToList());

            SerializableTypes.AddRange(typs);
        }
        public void Serialize(List<GameObject> goList)
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "list.xml");
            using (StreamWriter sw = new StreamWriter(path))
            {
                for (int i = 0; i < goList.Count; i++)
                {
                    goList[i].Awoken = false;
                    for (int j = 0; j < goList[i].Components.Count; j++)
                    {
                        goList[i].Components[j].Awoken = false;
                    }
                }
                UpdateSerializableTypes();
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<GameObject>),
                    SerializableTypes.ToArray());
                //SerializableTypes.ToArray());

                xmlSerializer.Serialize(sw, goList);

                for (int i = 0; i < goList.Count; i++)
                {
                    goList[i].Awoken = true;
                    for (int j = 0; j < goList[i].Components.Count; j++)
                    {
                        goList[i].Components[j].Awoken = true;
                    }
                }
            }
        }
        public List<GameObject> Deserialize()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "list.xml");
            using (StreamReader sw = new StreamReader(path))
            {
                UpdateSerializableTypes();
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<GameObject>),
    SerializableTypes.ToArray());
                return ((List<GameObject>)xmlSerializer.Deserialize(sw));
            }
        }
    }
}
