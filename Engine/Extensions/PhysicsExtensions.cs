using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using Color = System.Drawing.Color;
using Scripts;
using MonoGame.Extended;
using System.Collections.Generic;

namespace Engine
{
    public static class PhysicsExtensions
    {
        public static float AngleBetween(Vector2 vector1, Vector2 vector2)
        {
            float returnAngle = (float)Math.Acos((vector1.Dot(vector2)) / (vector1.Length() * vector2.Length()));

            if (returnAngle == float.NaN)
            {
                returnAngle = 0;
            }
            return returnAngle;
        }

        // Kód z odpovede na StackOverflow - https://stackoverflow.com/a/9557244/4405279
        public static Vector2 ClosestPointOnLine(Vector2 lineStart, Vector2 lineEnd, Vector2 point)
        {
            Vector2 AP = point - lineStart;       //Vector from A to P   
            Vector2 AB = lineEnd - lineStart;       //Vector from A to B  

            float magnitudeAB = AB.LengthSquared();     //Magnitude of AB vector (it's length squared)     
            float ABAPproduct = Vector2.Dot(AP, AB);    //The DOT product of a_to_p and a_to_b     
            float distance = ABAPproduct / magnitudeAB; //The normalized "distance" from a to your closest point  

            if (distance < 0)     //Check if P projection is over vectorAB     
            {
                return lineStart;

            }
            else if (distance > 1)
            {
                return lineEnd;
            }
            else
            {
                return lineStart + AB * distance;
            }
        }
        public static float DistanceFromLine(Vector2 lineStart, Vector2 lineEnd, Vector2 point)
        {
            return Vector2.Distance(ClosestPointOnLine(lineStart, lineEnd, point), point);
        }
        public static (bool intersects, float distance) In(this Vector2 point, Collider collider)
        {
            bool isIn = false;
            float distance = 0;
            switch (collider)
            {
                case LineCollider lineCollider:
                    if ((distance = DistanceFromLine(lineCollider.GetLineStart(), lineCollider.GetLineEnd(), point)) <
                        lineCollider.GetComponent<LineRenderer>().StrokeSize)
                    {
                        isIn = true;
                    }
                    break;
                case CircleCollider circleCollider:
                    if ((distance = Vector2.Distance(circleCollider.transform.Position, point)) < circleCollider.Radius)
                    {
                        isIn = true;
                    }
                    break;
                case BoxCollider boxCollider:
                    var boxPosition = boxCollider.rect.Center;

                    Vector2 boxCenter = boxCollider.rect.Center;
                    RectangleF rect = boxCollider.rect;
                    rect.Position = boxCollider.transform.Position;

                    isIn = (point.X < rect.Right &&
                            point.X > rect.Left &&
                            point.Y < rect.Bottom &&
                            point.Y > rect.Top);
                    distance = rect.DistanceTo(point);

                    break;
            }
            return (isIn, distance);
        }
        public static (bool intersects, float distance) Intersects(this Rigidbody rb1, Rigidbody rb2, Vector2? rb1Pos = null, Vector2? rb2Pos = null)
        {
            Vector2 rb1Position;
            Vector2 rb2Position;

            if (rb1Pos == null || rb2Pos == null)
            {
                rb1Position = rb1.transform.Position;
                rb2Position = rb1.transform.Position;
            }
            else
            {
                rb1Position = (Vector2)rb1Pos;
                rb2Position = (Vector2)rb2Pos;
            }

            if (rb1.collider is CircleCollider && rb2.collider is CircleCollider)
            {
                float dist = Vector2.Distance(rb1Position, rb2Position);
                return (dist < rb1.GetComponent<CircleCollider>().Radius + rb2.GetComponent<CircleCollider>().Radius, dist);
            }
            else if (rb1.collider is BoxCollider && rb2.collider is BoxCollider)
            {
                return (((BoxCollider)rb1.collider).rect.Intersects(((BoxCollider)rb2.collider).rect), 0);
            }
            else if (rb1.collider is CircleCollider && rb2.collider is LineCollider)
            {
                var dist = Vector2.Distance(ClosestPointOnLine(rb2.GetComponent<LineCollider>().GetLineStart(), rb2.GetComponent<LineCollider>().GetLineEnd(), rb1Position), rb1Position);
                return (dist <= rb1.GetComponent<CircleCollider>().Radius, dist);
            }
            else if (rb2.collider is CircleCollider && rb1.collider is LineCollider)
            {
                var dist = Vector2.Distance(ClosestPointOnLine(rb1.GetComponent<LineCollider>().GetLineStart(), rb1.GetComponent<LineCollider>().GetLineEnd(), rb2Position), rb2Position);
                return (dist <= rb2.GetComponent<CircleCollider>().Radius, dist);
            }

            else if ((rb2.collider is BoxCollider && rb1.collider is LineCollider) || rb2.collider is LineCollider && rb1.collider is BoxCollider)
            {
                BoxCollider boxCollider = (BoxCollider)(rb2.collider is BoxCollider ? rb2.collider : rb1.collider);

                LineCollider lineCollider = (LineCollider)(rb2.collider is LineCollider ? rb2.collider : rb1.collider);

                List<Vector2[]> lines = new List<Vector2[]>() {
                    new Vector2[2] { boxCollider.transform.Position, boxCollider.transform.Position + new Vector2(boxCollider.rect.Width, 0) }, // UP
                    new Vector2[2] { boxCollider.transform.Position, boxCollider.transform.Position + new Vector2(0, boxCollider.rect.Height) }, // LEFT
                    new Vector2[2] { boxCollider.transform.Position + new Vector2(0, boxCollider.rect.Height), boxCollider.transform.Position + new Vector2(boxCollider.rect.Width, boxCollider.rect.Height) }, // BOTTOM
                    new Vector2[2] { boxCollider.transform.Position + new Vector2(boxCollider.rect.Width, 0), boxCollider.transform.Position + new Vector2(boxCollider.rect.Width, boxCollider.rect.Height) }  // RIGHT
                };
                for (int i = 0; i < lines.Count; i++)
                {
                    if (LinesIntersecting((lines[i])[0], (lines[i])[1], lineCollider.GetLineStart(), lineCollider.GetLineEnd()))
                    {
                        return (true, 0);
                    }
                }
            }

            else if (rb2.collider is LineCollider && rb1.collider is LineCollider)
            {
                return (LinesIntersecting(((LineCollider)rb1.collider).GetLineStart(), ((LineCollider)rb1.collider).GetLineEnd(),
                    ((LineCollider)rb2.collider).GetLineStart(), ((LineCollider)rb2.collider).GetLineEnd()), 0);



            }
            return (false, 0);
        }

        // Kód z odpovede na StackOverflow - https://gamedev.stackexchange.com/a/26022
        public static bool LinesIntersecting(Vector2 a, Vector2 b, Vector2 c, Vector2 d)
        {
            float denominator = ((b.X - a.X) * (d.Y - c.Y)) - ((b.Y - a.Y) * (d.X - c.X));
            float numerator1 = ((a.Y - c.Y) * (d.X - c.X)) - ((a.X - c.X) * (d.Y - c.Y));
            float numerator2 = ((a.Y - c.Y) * (b.X - a.X)) - ((a.X - c.X) * (b.Y - a.Y));

            // Detect coincident lines (has a problem, read below)
            if (denominator == 0) return numerator1 == 0 && numerator2 == 0;

            float r = numerator1 / denominator;
            float s = numerator2 / denominator;

            return (r >= 0 && r <= 1) && (s >= 0 && s <= 1);
        }
    }
}
