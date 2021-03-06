﻿using Engine;
using Microsoft.Xna.Framework;
using System;
using System.ComponentModel;

namespace Scripts
{

	public class Transform : Component
	{

		[ShowInEditor]
		public override bool Enabled { get { return true; } }

		[ShowInEditor] public Vector3 Scale { get; set; } = Vector3.One;
		[ShowInEditor] public Vector3 Rotation { get; set; } = Vector3.Zero;

		[ShowInEditor] public Vector3 Anchor { get; set; } = new Vector3(0, 0, 0);
		[ShowInEditor] public Vector3 Position { get; set; } = Vector3.Zero;
		[ShowInEditor] public Vector3 LocalPosition { get { return Position - GetParentPosition(); } set { Position = GetParentPosition() + value; } }
		/*[ShowInEditor]
		public Vector3 LocalPosition
		{
			get { return transform.position - GetParentPosition(); }
			set
			{
				position = value + GetParentPosition();
				localPosition = value;
			}
		}*/
		public Vector3 GetParentPosition()
		{
			if (GameObject?.Parent != null)
			{
				return GameObject.Parent.transform.Position;
			}
			else
			{
				return Vector3.Zero;
			}
		}
		public Vector3 initialAngleDifferenceFromParent = Vector3.Zero;

		public Vector3 TransformVector(Vector3 vec)
		{
			//float sin = (float)Math.Sin(transform.Rotation.X);
			//float cos = (float)Math.Cos(transform.Rotation.X);
			//var zRotation = new Vector3(direction.Y * sin - direction.X * cos, direction.X * sin + direction.Y * cos, transform.Rotation);
			//return zRotation;


			var a = Quaternion.CreateFromRotationMatrix(
	Matrix.CreateRotationX(90 * (float)Math.PI / 180) *
	Matrix.CreateRotationY(0) *
	Matrix.CreateRotationZ(0));
			var b = Matrix.CreateFromQuaternion(a);

			var q = Quaternion.CreateFromRotationMatrix(
				Matrix.CreateRotationX(transform.Rotation.X) *
				Matrix.CreateRotationY(transform.Rotation.Y) *
				Matrix.CreateRotationZ(transform.Rotation.Z));


			// Matrix rotation = Matrix.CreateFromYawPitchRoll(transform.rotation.Y, transform.Rotation, transform.rotation.X);
			//Vector3 translation = Vector3.Transform(vec, rotation);
			return transform.Position + Matrix.CreateFromQuaternion(q).Backward;

		}
		public Vector3 up { get { return Position + TransformVector(new Vector3(0, 1, 0)); } }
	}
}
