using Engine;
using Microsoft.Xna.Framework;
using System;

namespace Scripts
{
    public class Transform : Component
    {
        public override bool Enabled { get { return true; } }

        [ShowInEditor] public Vector2 Scale { get; set; } = Vector2.One;

        // later remove this, use Vector3 position, dont just use Z in renderer or here, i may need stacked objects un future

        //private Vector2 localPosition = Vector2.Zero;
        private float rotation;

        //[ShowInEditor] public Vector2 Position { get { return position; /*+ GetPositionFromRotatedParent();*/ } set { position = value; } }
        [ShowInEditor] public Vector2 Position { get; set; } = Vector2.Zero;
        /*[ShowInEditor]
        public Vector2 LocalPosition
        {
            get { return transform.position - GetParentPosition(); }
            set
            {
                position = value + GetParentPosition();
                localPosition = value;
            }
        }*/

        [ShowInEditor]
        public float Rotation
        {
            get { return rotation + GetRotationFromParentAsPivot(); }
            set { rotation = MathHelper.WrapAngle(value); }
        }
        public Vector2 GetParentPosition()
        {
            if (gameObject?.Parent != null)
            {
                return gameObject.Parent.transform.Position;
            }
            else
            {
                return new Vector2(0, 0);
            }
        }
        public float initialAngleDifferenceFromParent = 0;
        public Vector2 GetPositionFromRotatedParent()
        {
            GameObject prnt = gameObject.Parent;
            if (prnt == null) { return new Vector2(0, 0); }
            Vector2 point = Vector2.Zero;
            while (prnt != null)
            {
                float angle = GetRotationFromParentAsPivot() + initialAngleDifferenceFromParent;
                Vector2 pivot = prnt.transform.Position - transform.Position;

                float sin = (float)Math.Sin(angle);
                float cos = (float)Math.Cos(angle);

                var v = new Vector2(-pivot.X, -pivot.Y);

                // rotate point
                float xnew = v.Y * sin - v.X * cos;
                float ynew = v.X * sin + v.Y * cos;

                // translate point back:
                v.X = xnew + pivot.X;
                v.Y = ynew + pivot.Y;

                point += v;
                prnt = prnt.Parent;
            }
            return point;
        }
        public float GetRotationFromParentAsPivot()
        {
            float rot = 0;
            if (gameObject == null) { return 0; }
            GameObject par = gameObject.Parent;
            while (par != null)
            {
                rot += par.transform.rotation;
                par = par.Parent;
            }
            return rot;
        }
        public Vector2 TransformDirection(Vector2 direction)
        {
            float sin = (float)Math.Sin(transform.Rotation);
            float cos = (float)Math.Cos(transform.Rotation);

            return new Vector2(direction.Y * sin - direction.X * cos, direction.X * sin + direction.Y * cos);
        }
    }
}
