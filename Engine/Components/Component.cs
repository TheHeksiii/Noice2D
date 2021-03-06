﻿using Engine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Scripts
{
	public class Component : IDestroyable
	{
		private GameObject gameObject;
		[System.Xml.Serialization.XmlIgnore]
		public GameObject GameObject { get { return gameObject; } set { gameObject = value; gameObjectID = value.ID; } }
		[ShowInEditor]
		public int gameObjectID { get; set; }

		[System.Xml.Serialization.XmlIgnore]
		[System.ComponentModel.DefaultValue(false)]
		public bool Awoken { get; set; } = false;

		public virtual bool AllowMultiple { get; set; } = true;
		public Component()
		{
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

		// Callbacks
		private void RegisterInputCallbacks()
		{
			MouseInput.Mouse1Down += () =>
			{
				if (GameObject.mouseOver)
				{
					OnMouse1Down();
				}
			};
			MouseInput.Mouse1Up += () =>
			{
				if (GameObject.mouseOver)
				{
					OnMouse1Up();
				}
			};
			MouseInput.Mouse1 += OnMouse1;
		}
		public virtual void Awake()
		{
			Awoken = true;
			RegisterInputCallbacks();
		}
		public virtual void Start() { }
		public virtual void Update() { }
		public virtual void OnDestroyed() { }

		public virtual void OnCollisionEnter(Rigidbody rigidbody) { }
		public virtual void OnCollisionExit(Rigidbody rigidbody) { }

		public virtual void OnTriggerEnter(Rigidbody rigidbody) { }
		public virtual void OnTriggerExit(Rigidbody rigidbody) { }

		public virtual void OnMouse1Down() { }

		public virtual void OnMouse1Up() { }

		public virtual void OnMouse1() { }
		public virtual void OnNewComponentAdded() { }

		public int CompareTo(bool other)
		{
			if (this == null) { return 0; }
			else { return 1; }
		}

		public static implicit operator bool(Component instance)
		{
			if (instance == null)
			{
				return false;
			}
			return true;
		}
	}
}
