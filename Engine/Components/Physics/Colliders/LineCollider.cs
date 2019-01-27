using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using Engine;
namespace Scripts
{
    public class LineCollider : Collider
    {
        //[ShowInEditor]
        //[System.ComponentModel.Editor(typeof(Editor.MethodEditor), typeof(System.Drawing.Design.UITypeEditor))]
        //public bool EditPoints { get; set; } = false;
        public float length = 0;

        public float? staticAngle;


        public Vector2 GetLineStart()// put both methods into tuple method?
        {
            if (staticAngle != null)
            {
                return transform.Position + new Vector2((float)Math.Cos((float)staticAngle), (float)Math.Sin((float)staticAngle));
            }
            else
            {
                return transform.Position + new Vector2((float)Math.Cos(transform.Rotation), (float)Math.Sin(transform.Rotation));
            }
        }
        public Vector2 GetLineEnd()
        {
            if (staticAngle != null)
            {
                return transform.Position + new Vector2(-(float)Math.Cos((float)staticAngle), (float)Math.Sin((float)staticAngle)) * length;
            }
            else
            {
                return transform.Position + new Vector2(-(float)Math.Cos(transform.Rotation), (float)Math.Sin(transform.Rotation)) * length;
            }
        }


    }
}
