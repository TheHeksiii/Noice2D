using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;
using Engine;
using System;

namespace Scripts
{
    public class Transform : Component
    {
        public override bool Enabled { get { return true; } }

        [ShowInEditor] public Vector2 Position { get; set; } = Vector2.Zero;
        [ShowInEditor] public Vector2 Scale { get; set; } = Vector2.One;

        // later remove this, use Vector3 position, dont just use Z in renderer or here, i may need stacked objects un future

        private float rotation;


        [ShowInEditor]
        public float Rotation
        {
            get { return rotation; }
            set { rotation = MathHelper.WrapAngle(value); }
        }
    }
}
