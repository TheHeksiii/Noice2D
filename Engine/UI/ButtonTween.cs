using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine;
using Microsoft.Xna.Framework;
using Scripts;
namespace Engine.UI
{
	public class ButtonTween : Component
	{
		public override bool AllowMultiple { get; set; } = false;

		private bool clicked = false;
		private float scaleSpeed = 20;
		private float scaleTarget = 0.9f;
		private bool needToScale = false;
		public override void Awake()
		{
			Button btn = GetComponent<Button>();
			if (btn)
			{
				GetComponent<Button>().onReleasedAction += () =>
				{
				};
			}
			base.Awake();
		}
		public override void Update()
		{
			if (needToScale == false) { return; }
			if (clicked)
			{
				transform.Scale = Vector3.Lerp(transform.Scale, Vector3.One * scaleTarget, Time.deltaTime * scaleSpeed);
				if (transform.Scale == Vector3.One * scaleTarget)
				{
					needToScale = false;
				}
			}
			else
			{
				transform.Scale = Vector3.Lerp(transform.Scale, Vector3.One, Time.deltaTime * scaleSpeed);
				if (transform.Scale == Vector3.One)
				{
					needToScale = false;
				}
			}
		}
		public override void OnMouse1Down()
		{
			clicked = true;
			needToScale = true;

			base.OnMouse1Down();
		}
		public override void OnMouse1Up()
		{
			transform.Scale = Vector3.One * scaleTarget;

			clicked = false;
			needToScale = true;

			base.OnMouse1Down();
		}

	}
}
