﻿using Scripts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace Engine
{
	public class Serializer
	{
		public static string lastScene = "";
		public static Serializer instance;
		public static Serializer GetInstance()
		{
			return instance;
		}
		List<Type> SerializableTypes = new List<Type>();
		public Serializer()
		{
			instance = this;

			lastScene = Properties.Settings.Default.lastScene.ToString();
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
		public void SaveGameObjects(SceneFile sceneFile, string scenePath, bool isPrefab = false)
		{
			if (isPrefab == false)
			{
				lastScene = scenePath;

				Properties.Settings.Default.lastScene = lastScene;
				Properties.Settings.Default.Save();
			}

			using (StreamWriter sw = new StreamWriter(scenePath))
			{
				for (int i = 0; i < sceneFile.GameObjects.Count; i++)
				{
					sceneFile.GameObjects[i].Awoken = false;
					for (int j = 0; j < sceneFile.GameObjects[i].Components.Count; j++)
					{
						sceneFile.GameObjects[i].Components[j].Awoken = false;
					}
				}
				UpdateSerializableTypes();

				XmlSerializer xmlSerializer = new XmlSerializer(typeof(SceneFile),
							SerializableTypes.ToArray());

				//SerializableTypes.ToArray());
				xmlSerializer.Serialize(sw, sceneFile);

				for (int i = 0; i < sceneFile.GameObjects.Count; i++)
				{
					sceneFile.GameObjects[i].Awoken = true;
					for (int j = 0; j < sceneFile.GameObjects[i].Components.Count; j++)
					{
						sceneFile.GameObjects[i].Components[j].Awoken = true;
					}
				}
			}
		}
		public SceneFile LoadGameObjects(string scenePath)
		{
			using (StreamReader sr = new StreamReader(scenePath))
			{
				UpdateSerializableTypes();

				XmlSerializer xmlSerializer = new XmlSerializer(typeof(SceneFile),
					SerializableTypes.ToArray());

				var a = ((SceneFile)xmlSerializer.Deserialize(sr));
				return a;
			}
		}
		public void ConnectGameObjectsWithComponents(SceneFile sf)
		{
			GameObject[] gos = sf.GameObjects.ToArray();
			Component[] comps = sf.Components.ToArray();

			for (int i = 0; i < gos.Length; i++)
			{
				for (int j = 0; j < comps.Length; j++)
				{
					if (comps[j].gameObjectID == gos[i].ID)
					{
						gos[i].AddExistingComponent(comps[j]);
						gos[i].LinkComponents(gos[i],comps[j]);
						/*gos[i].Components.Add(comps[j]);
						gos[i].Awake();
						comps[j].GameObject = gos[i];*/
					}
				}
			}
			return;
			for (int i = 0; i < gos.Length; i++)
			{
				for (int j = 0; j < gos[i].Components.Count; j++)
				{
					gos[i].InitializeMemberComponents(gos[i].Components[j]);

					gos[i].LinkComponents(gos[i], gos[i].Components[j]);

					gos[i].Components[j].GameObject = gos[i];
					gos[i].Components[j].transform.GameObject = gos[i];

					gos[i].Components[j].Awake();
					gos[i].Components[j].Awoken = true;

					gos[i].Components[j].Start();


				}
			}
		}
	}
}
