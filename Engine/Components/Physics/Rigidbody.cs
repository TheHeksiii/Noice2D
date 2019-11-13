using Engine;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
namespace Scripts
{
    public sealed class Rigidbody : Component
    {
        [System.Xml.Serialization.XmlIgnore]
        public List<Rigidbody> touchingRigidbodies = new List<Rigidbody>();

        [System.Xml.Serialization.XmlIgnore]
        [LinkableComponent]
        public Collider collider;
        [ShowInEditor] public bool UseGravity { get; set; } = true;
        [ShowInEditor] public bool IsStatic { get; set; } = false;
        [ShowInEditor] public bool IsTrigger { get; set; } = false;
        [ShowInEditor] public bool IsButton { get; set; } = false;
        [ShowInEditor] public Vector3 Velocity { get; set; } = new Vector3(0, 0, 0);

        public float velocityDrag = 0.99f;
        [ShowInEditor] public float Bounciness { get; set; } = 0f;

        [ShowInEditor] public float AngularVelocity { get; set; } = 0;
        public float angularDrag = 1f;

        public float friction = 1;
        public float mass = 1;


        public override void Awake()
        {
            //gameObject.OnComponentAdded += CheckForColliderAdded;
            if (IsButton == false)
            {
                Physics.rigidbodies.Add(this);
            }
        }
        /// <summary>
        /// Called from Physics thread after collision check
        /// </summary>
        public void FixedUpdate()
        {
            if (Scene.GetInstance()?.GraphicsDevice == null || IsStatic || IsButton)
            {
                return;
            }
            //velocity *= velocityDrag;
            //angularVelocity *= angularDrag;

            if (UseGravity)
            {
                ApplyGravity();
            }
            TranslateVelocityToTransform();
            TranslateAngularRotationToTransform();

        }

        public Vector3 GetPositionOnNextFrame()
        {
            if (IsStatic)
            {
                return transform.Position;
            }
            Vector3 pos = transform.Position;
            Vector3 vel = new Vector3(Velocity.X, Velocity.Y, Velocity.Z);

            if (UseGravity)
            {
                vel -= Physics.gravity * Time.deltaTime * mass;
            }
            //vel = new Vector2(vel.X * velocitySlowDown, vel.Y* velocitySlowDown);
            pos += Velocity * Time.deltaTime;
            return pos;
        }
        public void ApplyGravity()
        {
            Velocity -= Physics.gravity * Time.deltaTime;
        }
        public void ApplyVelocity(Vector3 vel)
        {
            if (IsStatic == false)
            {
                Velocity += vel;
            }
        }
        public void TranslateVelocityToTransform()
        {
            transform.Position += Velocity * Time.deltaTime;
        }
        public void TranslateAngularRotationToTransform()
        {
            transform.Rotation += new Vector3(0, 0, AngularVelocity * Time.deltaTime);
            if (touchingRigidbodies.Count > 0) // only move from rotating, when friciton from other object
            {
                transform.Position += new Vector3(AngularVelocity, 0, 0);
            }
        }
        public override void OnDestroyed()
        {
            for (int i = 0; i < touchingRigidbodies.Count; i++)
            {
                touchingRigidbodies[i].OnCollisionExit(this);
                OnCollisionExit(touchingRigidbodies[i]);
            }
            if (Physics.rigidbodies.Contains(this))
            {
                Physics.rigidbodies.Remove(this);
            }
        }

        public override void OnCollisionEnter(Rigidbody rigidbody) // TODO-TRANSLATE CURRENT VELOCITY TO COLLIDED RIGIDBODY, ADD FORCE (MassRatio2/MassRatio1)
        {
            touchingRigidbodies.Add(rigidbody);

            // Call callback on components that implement interface IPhysicsCallbackListener
            for (int i = 0; i < GameObject.Components.Count; i++)
            {
                if ((GameObject.Components[i] is Rigidbody) == false)
                {
                    GameObject.Components[i].OnCollisionEnter(rigidbody);
                }
            }
        }
        public override void OnCollisionExit(Rigidbody rigidbody)
        {
            if (touchingRigidbodies.Contains(rigidbody))
            {
                touchingRigidbodies.Remove(rigidbody);
            }

            for (int i = 0; i < GameObject.Components.Count; i++)
            {
                if ((GameObject.Components[i] is Rigidbody) == false)
                {
                    GameObject.Components[i].OnCollisionExit(rigidbody);
                }
            }
        }
        public override void OnTriggerEnter(Rigidbody rigidbody)
        {
            touchingRigidbodies.Add(rigidbody);

            // Call callback on components that implement interface IPhysicsCallbackListener
            for (int i = 0; i < GameObject.Components.Count; i++)
            {
                if ((GameObject.Components[i] is Rigidbody) == false)
                {
                    GameObject.Components[i].OnTriggerEnter(rigidbody);
                }
            }
        }
        public override void OnTriggerExit(Rigidbody rigidbody)
        {
            if (touchingRigidbodies.Contains(rigidbody))
            {
                touchingRigidbodies.Remove(rigidbody);
            }

            for (int i = 0; i < GameObject.Components.Count; i++)
            {
                if ((GameObject.Components[i] is Rigidbody) == false)
                {
                    GameObject.Components[i].OnTriggerExit(rigidbody);
                }
            }
        }
    }
}
