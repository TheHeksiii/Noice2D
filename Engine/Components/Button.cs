﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Scripts;
using Engine.UI;
using System.Xml.Serialization;

namespace Engine
{
	public class Button : Component
	{
		public override bool AllowMultiple { get; set; } = false;

		public delegate void MouseAction();
		[XmlIgnore] MouseAction onClickedAction;
		[XmlIgnore] public MouseAction onReleasedAction;

		[LinkableComponent]
		public Renderer renderer;
		[LinkableComponent]
		public Shape collider;
		bool mouseIsOver = false;
		bool clicked = false;
		public override void Awake()
		{
			onClickedAction += () => renderer.Color = Color.Red;
			onReleasedAction += () => renderer.Color = Color.White;
			base.Awake();


			GameObject.AddComponent<ButtonTween>().Awake();
		}

		public override void Update()
		{
			if (renderer == false || collider == false) { return; }
			mouseIsOver = Camera.Instance.ScreenToWorld(MouseInput.Position).In(collider).intersects;

			if (clicked == false)
			{
				renderer.Color = mouseIsOver ? Color.Gray : Color.White;
			}

			if (clicked && mouseIsOver == false) // up event when me move out of button bounds, even when clicked
			{
				OnMouse1Up();
			}
		}

		public override void OnMouse1Down()
		{
			onClickedAction?.Invoke();
			clicked = true;
		}
		public override void OnMouse1Up()
		{
			clicked = false;
			onReleasedAction?.Invoke();
		}
	}
}
