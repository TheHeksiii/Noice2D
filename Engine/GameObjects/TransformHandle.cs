using Microsoft.Xna.Framework;
using Scripts;
using System;
namespace Engine
{
    [Serializable]
    public class TransformHandle : Component
    {
        public override bool Enabled { get { return true; } }
        public static TransformHandle instance;
        public static TransformHandle GetInstance()
        {
            return instance;
        }
        public enum Axis { X, Y, XY };
        public Axis? CurrentAxisSelected = null;
        public bool clicked = false;

        public bool objectSelected = false;
        public BoxCollider boxColliderXY;
        public BoxCollider boxColliderX;
        public BoxCollider boxColliderY;
        public BoxRenderer boxRendererXY;
        public BoxRenderer boxRendererX;
        public BoxRenderer boxRendererY;

        public TransformHandle() : base()
        {
            //Awoken = false;
            instance = this;
        }
        public override void Awake()
        {
            objectSelected = false;
            GameObject.updateWhenDisabled = true;

            GameObject.AddComponent<Rigidbody>().IsStatic = true;
            GetComponent<Rigidbody>().IsButton = true;

            if (GetComponents<BoxRenderer>().Count > 2)
            {
                boxColliderXY = GetComponent<BoxCollider>(0);
                boxColliderX = GetComponent<BoxCollider>(1);
                boxColliderY = GetComponent<BoxCollider>(2);

                boxRendererXY = GetComponent<BoxRenderer>(0);
                boxRendererX = GetComponent<BoxRenderer>(1);
                boxRendererY = GetComponent<BoxRenderer>(2);
            }
            else
            {
                boxColliderXY = GameObject.AddComponent<BoxCollider>();
                boxColliderXY.rect = new Rectangle(0, 0, 20, 20);

                boxColliderX = GameObject.AddComponent<BoxCollider>();
                boxColliderX.rect = new Rectangle(0, 0, 50, 5);

                boxColliderY = GameObject.AddComponent<BoxCollider>();
                boxColliderY.rect = new Rectangle(0, 0, 5, 50);

                boxRendererXY = GameObject.AddComponent<BoxRenderer>();
                boxRendererX = GameObject.AddComponent<BoxRenderer>();
                boxRendererY = GameObject.AddComponent<BoxRenderer>();

                boxRendererXY.Color = Color.Orange;
                boxRendererX.Color = Color.Red;
                boxRendererY.Color = Color.Cyan;

                boxRendererX.boxCollider = boxColliderX;
                boxRendererXY.boxCollider = boxColliderXY;
                boxRendererY.boxCollider = boxColliderY;
            }
            base.Awake();
        }
        public override void Update()
        {
            if (objectSelected == false)
            {
                GameObject.Active = false;
                return;
            }
            else
            {
                GameObject.Active = true;
            }

            if (MouseInput.Position.In(boxColliderX).intersects)
            {
                boxRendererX.Color = Color.AntiqueWhite;
            }
            else
            {
                boxRendererX.Color = Color.Red;
            }
            if (MouseInput.Position.In(boxColliderY).intersects)
            {
                boxRendererY.Color = Color.AntiqueWhite;
            }
            else
            {
                boxRendererY.Color = Color.Cyan;
            }
            if (MouseInput.Position.In(boxColliderXY).intersects)
            {
                boxRendererXY.Color = Color.AntiqueWhite;
            }
            else
            {
                boxRendererXY.Color = Color.Orange;
            }
            base.Update();
        }

        public void Move(Vector3 deltaVector)
        {
            Vector3 moveVector = Vector3.Zero;
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
