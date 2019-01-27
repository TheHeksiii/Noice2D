using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Editor
{

    public class TransformHandle : EditorGameObject
    {
        public enum Axis { X, Y, XY };
        public Axis? CurrentAxisSelected = null;
        public bool clicked = false;

        public BoxCollider boxColliderXY;
        public BoxCollider boxColliderX;
        public BoxCollider boxColliderY;

        public BoxRenderer boxRendererXY;
        public BoxRenderer boxRendererX;
        public BoxRenderer boxRendererY;

        public TransformHandle(Vector2 position, Vector2 scale, string name = "") : base(position, scale, name)
        {
        }
        public override void Update()
        {
            if (MouseInput.Position.In(boxColliderX).intersects || CurrentAxisSelected==Axis.X)
            {
                boxRendererX.color=Color.AntiqueWhite;
            }
            else
            {
                boxRendererX.color = Color.Red;
            }
            if (MouseInput.Position.In(boxColliderY).intersects || CurrentAxisSelected == Axis.Y)
            {
                boxRendererY.color = Color.AntiqueWhite;
            }
            else
            {
                boxRendererY.color = Color.Cyan;
            }
            if (MouseInput.Position.In(boxColliderXY).intersects || CurrentAxisSelected == Axis.XY)
            {
                boxRendererXY.color = Color.AntiqueWhite;
            }
            else
            {
                boxRendererXY.color = Color.Orange;
            }
            base.Update();
        }
        internal override void Awake()
        {
            boxColliderXY = AddComponent<BoxCollider>();
            boxColliderXY.rect = new Rectangle(4, -20, 20, 20);

            boxColliderX = AddComponent<BoxCollider>();
            boxColliderX.rect = new Rectangle(0, 0, 50, 5);

            boxColliderY = AddComponent<BoxCollider>();
            boxColliderY.rect = new Rectangle(0, -50, 5, 50);

            boxRendererXY = AddComponent<BoxRenderer>();
            boxRendererX = AddComponent<BoxRenderer>();
            boxRendererY = AddComponent<BoxRenderer>();

            boxRendererXY.color = Color.Orange;
            boxRendererX.color = Color.Red;
            boxRendererY.color = Color.Cyan;

            AddComponent<Rigidbody>().isStatic = true;
            GetComponent<Rigidbody>().isButton = true;

            base.Awake();
        }
        public void Move(Vector2 deltaVector)
        {
            Vector2 moveVector = Vector2.Zero;
            switch (CurrentAxisSelected)
            {
                case Axis.X:
                    moveVector += deltaVector.VectorX();
                    break;
                case Axis.Y:
                    moveVector += deltaVector.VectorY();
                    break;
                case Axis.XY:
                    moveVector += deltaVector;
                    break;
            }
            transform.Position += moveVector;// we will grab it with offset, soe we want to move it only by change of mouse position
        }
    }
}
