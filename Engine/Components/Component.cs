using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Engine;

namespace Scripts
{
    public class Component : IDestroyable
    {
        [System.Xml.Serialization.XmlIgnore]
        public GameObject gameObject { get; set; }

        [System.Xml.Serialization.XmlIgnore]
        [System.ComponentModel.DefaultValue(false)]
        public bool Awoken { get; set; } = false;

        public readonly int ID;
        public Component()
        {
            ID = IDsManager.gameObjectNextID;
            IDsManager.componentNextID++;
        }
        [System.Xml.Serialization.XmlIgnore]
        public Transform transform
        {
            get { return gameObject.transform; }
            set { gameObject.transform = value; }
        }
        public virtual bool Enabled { get; set; } = true;

        public T GetComponent<T>(int? index = null) where T : Component
        {
            return gameObject.GetComponent<T>(index);
        }
        public List<T> GetComponents<T>() where T : Component
        {
            return gameObject.GetComponents<T>();
        }


        public Vector2 TransformToWorld(Vector2 localPoint)
        {
            return localPoint + transform.Position;
        }
        public Vector2 TransformToLocal(Vector2 worldPoint)
        {
            return worldPoint - transform.Position;
        }


        public virtual void OnDestroyed()
        {
        }

        // Callbacks

        public virtual void Awake() { Awoken = true; }
        public virtual void Update() { }

        public virtual void OnCollisionEnter(Rigidbody rigidbody) { }
        public virtual void OnCollisionExit(Rigidbody rigidbody) { }

        public virtual void OnTriggerEnter(Rigidbody rigidbody) { }
        public virtual void OnTriggerExit(Rigidbody rigidbody) { }
    }
}
