using Engine;
using Microsoft.Xna.Framework;
using System;
using System.ComponentModel;

namespace Scripts
{

    public class Transform : Component
    {
        [ShowInEditor]public UInt16 xxxx { get; set; }
        [ShowInEditor]
        public override bool Enabled { get { return true; } }

        [ShowInEditor] public Vector2 Scale { get; set; } = Vector2.One;

        // later remove this, use Vector3 position, dont just use Z in renderer or here, i may need stacked objects un future

        //private Vector3 localPosition = Vector3.Zero;
        private float rotation;

        [ShowInEditor] public Vector2 Anchor { get; set; } = new Vector2(0, 0);
        [ShowInEditor] public Vector2 Position { get; set; } = Vector2.Zero;
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

        [ShowInEditor]
        public float Rotation
        {
            get { return rotation + GetRotationFromParentAsPivot(); }
            set { rotation = value; }
        }
        public Vector2 GetParentPosition()
        {
            if (GameObject?.Parent != null)
            {
                return GameObject.Parent.transform.Position;
            }
            else
            {
                return new Vector2(0, 0);
            }
        }
        public float initialAngleDifferenceFromParent = 0;
        public Vector2 GetPositionFromRotatedParent()
        {
            GameObject prnt = GameObject.Parent;
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

            GameObject par = GameObject?.Parent;
            while (par != null)
            {
                rot += par.transform.rotation;
                par = par.Parent;
            }
            return rot;
        }
        public Vector2 TransformVector(Vector2 vec)
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
            var nn = Vector3.Transform(Vector3.Forward, b);
            var f = Vector3.Transform(Vector3.Zero, Matrix.CreateTranslation(new Vector3(10, 10, 10)) * Matrix.CreateRotationZ(90 * (float)Math.PI / 180));

            var q = Quaternion.CreateFromRotationMatrix(
                Matrix.CreateRotationX(transform.rotation) *
                Matrix.CreateRotationY(transform.rotation) *
                Matrix.CreateRotationZ(0));


            // Matrix rotation = Matrix.CreateFromYawPitchRoll(transform.rotation.Y, transform.Rotation, transform.rotation.X);
            //Vector3 translation = Vector3.Transform(vec, rotation);
            return transform.Position + Matrix.CreateFromQuaternion(q).Backward.ToVector2();

        }
        public Vector2 up { get { return Position + TransformVector(new Vector2(0, 1)); } }
    }
}
