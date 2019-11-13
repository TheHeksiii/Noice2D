using Engine;
using Microsoft.Xna.Framework;
using System;

namespace Scripts
{
    public class Transform : Component
    {
        public override bool Enabled { get { return true; } }

        [ShowInEditor] public Vector3 Scale { get; set; } = Vector3.One;

        // later remove this, use Vector3 position, dont just use Z in renderer or here, i may need stacked objects un future

        //private Vector3 localPosition = Vector3.Zero;
        private Vector3 rotation;

        //[ShowInEditor] public Vector3 Position { get { return position; /*+ GetPositionFromRotatedParent();*/ } set { position = value; } }
        [ShowInEditor] public Vector3 Position { get; set; } = Vector3.Zero;
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
        public Vector3 Rotation
        {
            get { return rotation + GetRotationFromParentAsPivot(); }
            set { rotation = value; }
        }
        public Vector3 GetParentPosition()
        {
            if (GameObject?.Parent != null)
            {
                return GameObject.Parent.transform.Position;
            }
            else
            {
                return new Vector3(0, 0, 0);
            }
        }
        public Vector3 initialAngleDifferenceFromParent = Vector3.Zero;
        public Vector3 GetPositionFromRotatedParent()
        {
            GameObject prnt = GameObject.Parent;
            if (prnt == null) { return new Vector3(0, 0, 0); }
            Vector3 point = Vector3.Zero;
            while (prnt != null)
            {
                Vector3 angle = GetRotationFromParentAsPivot() + initialAngleDifferenceFromParent;
                Vector3 pivot = prnt.transform.Position - transform.Position;

                float sin = (float)Math.Sin(angle.Z);
                float cos = (float)Math.Cos(angle.Z);

                var v = new Vector3(-pivot.X, -pivot.Y, 0);

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
        public Vector3 GetRotationFromParentAsPivot()
        {
            Vector3 rot = Vector3.Zero;

            GameObject par = GameObject?.Parent;
            while (par != null)
            {
                rot += par.transform.rotation;
                par = par.Parent;
            }
            return rot;
        }
        public Vector3 TransformVector(Vector3 vec)
        {
            //float sin = (float)Math.Sin(transform.Rotation.X);
            //float cos = (float)Math.Cos(transform.Rotation.X);
            //var zRotation = new Vector3(direction.Y * sin - direction.X * cos, direction.X * sin + direction.Y * cos, transform.Rotation.Z);
            //return zRotation;


            var a = Quaternion.CreateFromRotationMatrix(
    Matrix.CreateRotationX(90 * (float)Math.PI / 180) *
    Matrix.CreateRotationY(0) *
    Matrix.CreateRotationZ(0));
            var b = Matrix.CreateFromQuaternion(a);
            var nn = Vector3.Transform(Vector3.Forward, b);
            var f = Vector3.Transform(Vector3.Zero, Matrix.CreateTranslation(new Vector3(10, 10, 10)) * Matrix.CreateRotationZ(90 * (float)Math.PI / 180));

            var q = Quaternion.CreateFromRotationMatrix(
                Matrix.CreateRotationX(transform.rotation.X) *
                Matrix.CreateRotationY(transform.rotation.Y) *
                Matrix.CreateRotationZ(0));


            // Matrix rotation = Matrix.CreateFromYawPitchRoll(transform.rotation.Y, transform.rotation.Z, transform.rotation.X);
            //Vector3 translation = Vector3.Transform(vec, rotation);
            return transform.Position + Matrix.CreateFromQuaternion(q).Backward;

        }
        public Vector3 forward { get { return Position + TransformVector(new Vector3(0, 1, 0)); } }
        public Vector3 up { get { return Position + TransformVector(new Vector3(0, 0, 1)); } }
    }
}
