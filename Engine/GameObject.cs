using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Scripts;
using System.Reflection;
using System.Linq;
using System.Xml.Serialization;

namespace Engine
{
    //[System.Xml.Serialization.XmlInclude(typeof(Engine.TransformHandle))]
    //[System.Xml.Serialization.XmlInclude(typeof(Engine.Camera))]
    [XmlRootAttribute("GameObject")]
    public class GameObject
    {
        public bool updateWhenDisabled = false;
        private object ComponentsLock = new object();
        public delegate void ComponentAdded(GameObject gameObject, Component component);
        public event ComponentAdded OnComponentAdded;

        //Destroying 
        public delegate void Destroyed(GameObject gameObject);
        public event Destroyed OnDestroyed;
        public float destroyTimer = 2;
        private bool destroy = false;

        [System.ComponentModel.DefaultValue(false)]
        public bool Awoken { get; set; } = false;
        public int? ID = null;
        public string name = "";
        public bool selected = false;

        public bool Active { get; set; } = true;

        //[System.Xml.Serialization.XmlArrayItem(type: typeof(Component))]
        [XmlElement("Components")]
        public List<Component> Components = new List<Component>();

        public Transform Transform { get; set; }

        public bool silentInScene;


        public GameObject()
        {
            this.name = "Game Object";


            TryToAddToGameObjectsList();

            OnComponentAdded = LinkComponents;

            OnDestroyed = RemoveFromLists;

            if (ID == null)
            {
                ID = IDsManager.gameObjectNextID;
                IDsManager.gameObjectNextID++;
            }
        }
        public GameObject(Vector2? position = null, Vector2? scale = null, string name = "", bool linkComponents = true)
        {
            this.name = name;

            Transform = AddComponent<Transform>();

            if (position != null)
            {
                Transform.Position = position.Value;
            }
            if (position != null)
            {
                Transform.Scale = scale.Value;
            }

            if (linkComponents)
            {
                OnComponentAdded += LinkComponents;
            }
            OnDestroyed += RemoveFromLists;

            ID = IDsManager.gameObjectNextID;
            IDsManager.gameObjectNextID++;

            TryToAddToGameObjectsList();
        }
        public void LinkComponents(GameObject gameObject, Component component)
        {
            for (int index1 = 0; index1 < Components.Count; index1++)
            {
                for (int index2 = 0; index2 < Components.Count; index2++)
                {
                    if (index1 == index2) { continue; }

                    Type sourceType1 = Components[index1].GetType();
                    Type sourceType2 = Components[index2].GetType();
                    if (sourceType1.Name == "Rigidbody" && GetComponent<TransformHandle>() == null)
                    {
                        var a = 0;
                    }
                    FieldInfo fieldInfo = null;
                    FieldInfo[] infos = sourceType1.GetFields();
                    for (int i = 0; i < infos.Length; i++)
                    {
                        if (infos[i].GetCustomAttribute<LinkableComponent>() != null)
                        {
                            fieldInfo = infos[i];
                            break;
                        }
                    }
                    while (fieldInfo == null && sourceType1.BaseType != null && sourceType1.BaseType.Name.Equals("Component") == false)
                    {
                        infos = sourceType1.BaseType.GetFields();
                        for (int i = 0; i < infos.Length; i++)
                        {
                            if (infos[i].GetCustomAttribute<LinkableComponent>() != null)
                            {
                                fieldInfo = infos[i];
                                break;
                            }
                        }
                        sourceType1 = sourceType1.BaseType;
                    }

                    // get deepest class,but not Component, so if we have CircleCollider, we get Collider
                    while (sourceType2.BaseType != null && sourceType2.BaseType.Name.Equals("Component") == false)
                    {
                        sourceType2 = sourceType2.BaseType;
                    }
                    if (fieldInfo != null && (fieldInfo.FieldType.IsSubclassOf(sourceType2) || fieldInfo.FieldType == sourceType2) && fieldInfo.GetValue(Components[index1]) == null)
                    {
                        if (GetComponents(Components[index1].GetType()).IndexOf(Components[index1]) == GetComponents(Components[index2].GetType()).IndexOf(Components[index2]))
                        {
                            fieldInfo.SetValue(Components[index1], Components[index2]);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// give every found component in class its gameobject and transform reference
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="component"></param>
        public void InitializeMemberComponents(Component component)
        {
            Type sourceType = component.GetType();

            // fields that are derived from Component
            List<FieldInfo> componentFields = new List<FieldInfo>();

            // Find all fields that derive from Component
            componentFields.AddRange(sourceType.GetFields().Where(info => info.FieldType.IsSubclassOf(typeof(Component))));

            List<FieldInfo> gameObjectFields = new List<FieldInfo>();
            List<FieldInfo> transformFields = new List<FieldInfo>();


            for (int i = 0; i < componentFields.Count; i++)
            {
                PropertyInfo gameObjectFieldInfo = componentFields[0].FieldType.GetProperty("gameObject");
                PropertyInfo transformFieldInfo = componentFields[0].FieldType.GetProperty("transform");

                gameObjectFieldInfo.SetValue(component, Convert.ChangeType(this, gameObjectFieldInfo.PropertyType), null);
                transformFieldInfo.SetValue(component, Convert.ChangeType(Transform, transformFieldInfo.PropertyType), null);
            }
        }

        private void TryToAddToGameObjectsList()
        {
            if (silentInScene == false)
            {
                EditorSceneView.GetInstance().OnGameObjectCreated(this);
            }
        }
        public virtual void Awake()
        {

            if (Transform == null)
            {
                Transform = AddComponent<Transform>();
            }

            for (int i = 0; i < Components.Count; i++)
            {
                if (Components[i].Awoken == false)
                {
                    Components[i].Awake();
                    Components[i].Awoken = true;
                }
            }

            Awoken = true;
        }
        private void RemoveFromLists(GameObject gameObject)
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                for (int i = 0; i < rb.touchingRigidbodies.Count; i++)
                {
                    rb.touchingRigidbodies[i].touchingRigidbodies.Remove(rb);
                }

                Physics.rigidbodies.Remove(rb);

            }
            lock (ComponentsLock)
            {
                for (int i = 0; i < Components.Count; i++)
                {
                    Components[i].OnDestroyed();
                }
                Components.Clear();
            }

            EditorSceneView.GetInstance().OnGameObjectDestroyed(this);
        }
        public void Destroy(float? delay = null)
        {
            if (delay == null)
            {
                OnDestroyed(this);
            }
            else
            {
                destroy = true;
                destroyTimer = (float)delay;
            }
        }

        public virtual void Update()
        {
            if (destroy == true)
            {
                destroyTimer -= Time.deltaTime;
                if (destroyTimer < 0)
                {
                    destroy = false;
                    OnDestroyed(this);
                    return;
                }
            }

            UpdateComponents();
        }
        public Component AddComponent<Component>() where Component : Scripts.Component, new()
        {
            Component component = new Component();
            component.gameObject = this;

            Components.Add(component);

            OnComponentAdded?.Invoke(this, component);
            if (Awoken) { component.Awake(); }

            return component;
        }
        public Component AddComponent(Type type)
        {
            var component = (Scripts.Component)Activator.CreateInstance(type);
            component.gameObject = this;

            Components.Add(component);

            OnComponentAdded?.Invoke(this, component);
            if (Awoken) { component.Awake(); }
            return component;
        }
        public void RemoveComponent(int index)
        {
            Active = false;
            Components.RemoveAt(index);
            Active = true;

        }
        public T GetComponent<T>(int? index = null) where T : Component
        {
            int k = index == null ? 0 : (int)index;
            for (int i = 0; i < Components.Count; i++)
            {
                if (Components[i] is T)
                {
                    if (k == 0)
                    {
                        return Components[i] as T;
                    }
                    else
                    {
                        k--;
                    }
                }
            }
            return null;
        }
        public List<T> GetComponents<T>() where T : Component
        {
            List<T> componentsToReturn = new List<T>();
            for (int i = 0; i < Components.Count; i++)
            {
                if (Components[i] is T)
                {
                    componentsToReturn.Add(Components[i] as T);
                }
            }
            return componentsToReturn;
        }

        public List<Component> GetComponents(Type type)
        {
            List<Component> componentsToReturn = new List<Component>();
            for (int i = 0; i < Components.Count; i++)
            {
                if (Components[i].GetType() == type)
                {
                    componentsToReturn.Add(Components[i]);
                }
            }
            return componentsToReturn;
        }
        private void UpdateComponents()
        {
            lock (ComponentsLock)
            {
                for (int i = 0; i < Components.Count; i++)
                {
                    if (Components[i].Enabled && Components[i].Awoken)
                    {
                        Components[i].Update();
                    }
                }
            }
        }
        public void Draw(SpriteBatch batch)
        {
            for (int i = 0; i < Components.Count; i++)
            {
                if (Components[i] is Renderer && Components[i].Enabled && Components[i].Awoken && Active)
                    (Components[i] as Renderer).Draw(batch);
            }
        }

        public Vector2 TransformToWorld(Vector2 localPoint)
        {
            return localPoint + Transform.Position;
        }
        public Vector2 TransformToLocal(Vector2 worldPoint)
        {
            return worldPoint - Transform.Position;
        }
    }
}
