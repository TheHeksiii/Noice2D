using Engine;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Scripts
{
    public class Component : IDestroyable
    {
        private GameObject gameObject;
        [System.Xml.Serialization.XmlIgnore]
        public GameObject GameObject { get { return gameObject; } set { gameObject = value; gameObjectID = value.ID; } }
        public int gameObjectID;

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
            get { return GameObject.transform; }
            set { GameObject.transform = value; }
        }
        public virtual bool Enabled { get; set; } = true;

        public T GetComponent<T>(int? index = null) where T : Component
        {
            return GameObject.GetComponent<T>(index);
        }
        public List<T> GetComponents<T>() where T : Component
        {
            return GameObject.GetComponents<T>();
        }


        public Vector2 TransformToWorld(Vector2 localPoint)
        {
            return localPoint + transform.Position;
        }

        public Vector2 TransformToLocal(Vector2 worldPoint)
        {
            return worldPoint - transform.Position;
        }
        // Callbacks

        public virtual void Awake() { Awoken = true; }
        public virtual void Start() { }
        public virtual void Update() { }
        public virtual void OnDestroyed() { }

        public virtual void OnCollisionEnter(Rigidbody rigidbody) { }
        public virtual void OnCollisionExit(Rigidbody rigidbody) { }

        public virtual void OnTriggerEnter(Rigidbody rigidbody) { }
        public virtual void OnTriggerExit(Rigidbody rigidbody) { }
    }
}
