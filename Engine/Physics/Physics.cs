using Microsoft.Xna.Framework;
using Scripts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Engine
{
    public static class Physics
    {
        public static readonly Vector3 gravity = new Vector3(0, -200, 0);
        public static List<Rigidbody> rigidbodies = new List<Rigidbody>();

        public static readonly int timeStep = 40;//in milliseconds

        private static Task physicsTask;

        public static bool Running = false;

        private static void Wait(double durationSeconds)
        {
            var durationTicks = Math.Round(durationSeconds * Stopwatch.Frequency);
            var sw = Stopwatch.StartNew();

            while (sw.ElapsedTicks < durationTicks)
            {

            }
        }
        public static void StartPhysics()
        {
            if (Running == false)
            {
                physicsTask = Task.Factory.StartNew(FixedUpdate);
            }
        }
        private static void ResetVelocities()
        {
            for (int i = 0; i < rigidbodies.Count; i++)
            {
                rigidbodies[i].Velocity = Vector3.Zero;
            }
        }
        public static void StopPhysics()
        {
            Running = false;
        }
        public static void CheckForCollisionOnNextFrame(Rigidbody rb1, Rigidbody rb2)
        {
            if (rb1 == null || rb2 == null ||
                rb1.collider == null || rb2.collider == null ||
                rb1.transform == null || rb2.transform == null ||
                rb1.GameObject == null || rb2.GameObject == null) { return; }
            if (rb1.Intersects(rb2, rb1.GetPositionOnNextFrame(), rb2.GetPositionOnNextFrame()).intersects)
            {
                if (rb1.IsTrigger == false && rb2.IsTrigger == false)
                {
                    ApplyCorrectVelocities(rb1, rb2);
                }
                if (rb1.touchingRigidbodies.Contains(rb2) == false)
                {
                    if (rb1.IsTrigger || rb2.IsTrigger)
                    {
                        rb1.OnTriggerEnter(rb2);
                        rb2.OnTriggerEnter(rb1);
                    }
                    else
                    {
                        rb1.OnCollisionEnter(rb2);
                        rb2.OnCollisionEnter(rb1);
                    }
                }
            }
            else
            {
                if (rb1.touchingRigidbodies.Contains(rb2))
                {
                    if (rb1.IsTrigger || rb2.IsTrigger)
                    {
                        rb1.OnTriggerExit(rb2);
                        rb2.OnTriggerExit(rb1);
                    }
                    else
                    {
                        rb1.OnCollisionExit(rb2);
                        rb2.OnCollisionExit(rb1);
                    }
                }
            }
        }

        /// <summary>
        /// Sets rb's velocity to such velocity, where on next frame, they won't collide, TODO-distinguish between shapes too
        /// </summary>
        /// <param name="rb1">First rigidbody</param>
        /// <param name="rb2">Second rigidbody</param>
        private static void ApplyCorrectVelocities(Rigidbody rb1, Rigidbody rb2)
        {
            if (rb1.collider is CircleCollider && rb2.collider is CircleCollider)
            {

                rb2.Velocity = gravity * 0;
                rb1.Velocity = gravity * 0;

                Vector3 rb1OldVelocity = rb1.Velocity;
                Vector3 rb2OldVelocity = rb2.Velocity;

                Vector3 velocities = rb1OldVelocity + rb2OldVelocity;

                Vector3 rb1_NextFramePosition = rb1.GetPositionOnNextFrame();
                Vector3 rb2_NextFramePosition = rb2.GetPositionOnNextFrame();

                Vector3 from2to1 = rb2_NextFramePosition - rb1_NextFramePosition;
                from2to1 = from2to1 / (rb1.GetComponent<CircleCollider>().Radius + rb2.GetComponent<CircleCollider>().Radius);
                from2to1.Normalize();
                from2to1 = from2to1 * new Vector3(50 + velocities.Length() / 3, 0, 0);

                rb1.AngularVelocity += -from2to1.X * 0.01f;
                rb2.AngularVelocity += from2to1.X * 0.01f;
            }
            else if (rb1.collider is CircleCollider && rb2.collider is LineCollider)
            {
                CorrectVelocityCircleLine(rb1, rb2);
            }
            else if (rb1.collider is LineCollider && rb2.collider is CircleCollider)
            {
                CorrectVelocityCircleLine(rb2, rb1);
            }
            else if (rb1.collider is BoxCollider && rb2.collider is LineCollider)
            {
                CorrectVelocityRectangleLine(rectangle: rb1, line: rb2);
            }
            else if (rb1.collider is LineCollider && rb2.collider is BoxCollider)
            {
                CorrectVelocityRectangleLine(rectangle: rb2, line: rb1);
            }
        }

        /// <summary>
        /// Bouncy
        /// </summary>
        static void CorrectVelocityRectangleLine(Rigidbody rectangle, Rigidbody line)
        {
            Vector2 oldVelocity = rectangle.Velocity.ToVector2();
            Vector2 circleDir = rectangle.Velocity.ToVector2();
            circleDir.Normalize();

            Vector2 rectangleNextFramePosition = rectangle.GetPositionOnNextFrame().ToVector2();

            var lineStart = line.GetComponent<LineCollider>().GetLineStart();
            var lineEnd = line.GetComponent<LineCollider>().GetLineEnd();

            var rect = rectangle.GetComponent<BoxCollider>().rect;
            rect.Position = rectangleNextFramePosition;

            Vector2 onLine1 = PhysicsExtensions.ClosestPointOnLine(lineStart, lineEnd, rect.TopLeft);
            float dist = Vector2.Distance(onLine1, rect.TopLeft.ToVector2());
            Vector2 closestOnLine = onLine1;
            Vector2 closestVertex = rect.TopLeft;


            Vector2 onLine2 = PhysicsExtensions.ClosestPointOnLine(lineStart, lineEnd, rect.TopLeft + new Vector2(rect.Width, 0));
            var newDistance = Vector2.Distance(onLine2, rect.TopLeft.ToVector2() + new Vector2(rect.Width, 0));
            if (newDistance < dist)
            {
                closestOnLine = onLine2;
                closestVertex = rect.TopLeft + new Vector2(rect.Width, 0);
            }

            Vector2 onLine3 = PhysicsExtensions.ClosestPointOnLine(lineStart, lineEnd, rect.BottomRight);
            newDistance = Vector2.Distance(onLine2, rect.BottomRight.ToVector2());
            dist = newDistance > dist ? newDistance : dist;
            if (newDistance < dist)
            {
                closestOnLine = onLine3;
                closestVertex = rect.BottomRight;
            }

            Vector2 onLine4 = PhysicsExtensions.ClosestPointOnLine(lineStart, lineEnd, rect.BottomRight + new Vector2(-rect.Width, 0));
            newDistance = Vector2.Distance(onLine2, rect.BottomRight.ToVector2() + new Vector2(-rect.Width, 0));
            dist = newDistance > dist ? newDistance : dist;
            if (newDistance < dist)
            {
                closestOnLine = onLine4;
                closestVertex = rect.BottomRight + new Vector2(-rect.Width, 0);
            }

            Vector2 lineDir = Extensions.MaxY(lineStart, lineEnd) - Extensions.MinY(lineStart, lineEnd);
            if (lineDir != Vector2.Zero)
            {
                lineDir.Normalize();
            }

            Vector2 from2To1 = closestVertex - closestOnLine;

            Vector2 from2To1Dir = new Vector2(from2To1.X, from2To1.Y);
            from2To1Dir.Normalize();

            Vector2 reflected = Vector2.Reflect(oldVelocity, from2To1Dir);

            // move rb along collider
            rectangle.Velocity = reflected.ToVector3() + gravity * Time.deltaTime;
        }
        static void CorrectVelocityCircleLine(Rigidbody circle, Rigidbody line)
        {
            Vector2 oldVelocity = circle.Velocity.ToVector2();
            Vector2 circleDir = circle.Velocity.ToVector2();
            circleDir.Normalize();

            Vector2 circleRB_NextFramePosition = circle.GetPositionOnNextFrame().ToVector2();

            var lineStart = line.GetComponent<LineCollider>().GetLineStart();
            var lineEnd = line.GetComponent<LineCollider>().GetLineEnd();
            Vector2 onLine = PhysicsExtensions.ClosestPointOnLine(lineStart, lineEnd, circleRB_NextFramePosition);

            Vector2 lineDir = Extensions.MaxY(lineStart, lineEnd) - Extensions.MinY(lineStart, lineEnd);
            if (lineDir != Vector2.Zero)
            {
                lineDir.Normalize();
            }


            Vector2 from2To1 = circle.transform.Position.ToVector2() - onLine;


            Vector2 from2To1Dir = new Vector2(from2To1.X, from2To1.Y);
            from2To1Dir.Normalize();
            Vector2 reflected = Vector2.Reflect(oldVelocity, from2To1Dir);

            circle.AngularVelocity = lineDir.X;

            // move rb along collider
            //circle.Velocity = lineDir * Time.deltaTime;

            circle.Velocity = gravity * Time.deltaTime + ((circle.transform.Position * lineDir.ToVector3() * lineDir.Y) - circle.transform.Position) * Time.deltaTime;

            //circle.Velocity =gravity * Time.deltaTime + new Vector2(0, -lineDir.Y * 100);


        }
        public static void FixedUpdate()
        {
            ResetVelocities();
            Running = true;
            while (Running)
            {
                Wait(timeStep / 1000f);

                /*var options = new ParallelOptions();
                options.MaxDegreeOfParallelism = Environment.ProcessorCount;

                Parallel.For(0, rigidbodies.Count, options, i =>
                {
                    Parallel.For(0, rigidbodies.Count, options, j =>
                    {
                        if (i == j) { return; }

                        CheckForCollisionOnNextFrame(rigidbodies[i],
                           rigidbodies[j]);
                    });
                });*/
                for (int i = 0; i < rigidbodies.Count; i++)
                {
                    for (int j = i; j < rigidbodies.Count; j++)
                    {
                        if (j == i || rigidbodies[i].Enabled == false || rigidbodies[i].GameObject.Active == false ||
                            rigidbodies[j].Enabled == false || rigidbodies[j].GameObject.Active == false) { continue; }

                        CheckForCollisionOnNextFrame(rigidbodies[i],
                           rigidbodies[j]);
                    }
                }
                for (int i = 0; i < rigidbodies.Count; i++)
                {
                    rigidbodies[i].FixedUpdate();
                }
            }
        }
    }
}