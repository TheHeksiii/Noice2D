using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Scripts;
using MonoGame.Extended;
using Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
namespace Editor
{
    public class ColliderEditor
    {
        public static bool editing = false;
        public static ColliderEditor instance;
        public static ColliderEditor GetInstance() { return instance; }

        private List<GameObject> verticeGameObjects = new List<GameObject>();
        private GameObject dynamicVertex;
        private GameObject selectedPoint;
        private Collider collider;
        private PolygonCollider polygonCollider;

        private Vector2 verticeBoxOffset = new Vector2(5, 5);
        public ColliderEditor()
        {
            instance = this;

            // Dynamic vertex <<<<<<<<<<<<<<<
            dynamicVertex = new SilentGameObject(name: "ColliderEditor dynamicVertex");
            BoxCollider boxCollider = dynamicVertex.AddComponent<BoxCollider>();
            boxCollider.rect = new RectangleF(0, 0, 8, 8);
            dynamicVertex.AddComponent<BoxRenderer>().Fill = true;
            dynamicVertex.GetComponent<BoxRenderer>().Color = Color.HotPink;
            dynamicVertex.Awake();
            // Dynamic vertex >>>>>>>>>>>>>>>

            MouseInput.Mouse1Clicked += OnMouseLeftClick;
            MouseInput.Mouse1Released += () =>
            {
                if (selectedPoint != null)
                {
                    selectedPoint.GetComponent<Renderer>().Color = Color.White;
                    selectedPoint = null;
                }
            };

            MouseInput.Mouse2Clicked += OnMouseRightClick;

        }

        private void OnMouseRightClick()
        {
            if (polygonCollider == null || editing == false) { return; }

            if (verticeGameObjects.Count > 2)
            {
                // check if mouse is hovering over some vertex
                int? vertexIndex = null;
                vertexIndex = GetVerticeIndexOnMousePosition();

                if (vertexIndex != null)
                {
                    DestroyVertex((int)vertexIndex);
                }
            }
        }
        private int? GetVerticeIndexOnMousePosition()
        {
            for (int i = 0; i < verticeGameObjects.Count; i++)
            {
                if (MouseInput.Position.In(verticeGameObjects[i].GetComponent<BoxCollider>()).intersects)
                {
                    return i;
                }
            }
            return null;
        }
        void DestroyVertex(int index)
        {
            polygonCollider.Points.RemoveAt(index);
            polygonCollider.BuildEdges();
            GameObject pointCached = verticeGameObjects[index];
            selectedPoint = null;
            verticeGameObjects.Remove(pointCached);
            pointCached.Destroy();
        }
        private void OnMouseLeftClick()
        {
            if (polygonCollider == null || editing == false) { return; }

            int? vertexIndex = GetVerticeIndexOnMousePosition();
            if (vertexIndex != null)
            {
                verticeGameObjects[(int)vertexIndex].GetComponent<Renderer>().Color = Color.DeepSkyBlue;
                selectedPoint = verticeGameObjects[(int)vertexIndex];
                offset = -selectedPoint.TransformToLocal(MouseInput.Position);
            }


            if (vertexIndex == null && dynamicVertex.Active) // find line on which the point is, and at that position,insert new point
            {

                int newPointIndex = 0;
                float minDistance = float.PositiveInfinity;
                for (int i = 0; i < polygonCollider.Points.Count; i++)
                {
                    Vector2 point1 = polygonCollider.TransformToWorld(polygonCollider.Points[i]);
                    Vector2 point2 = i + 1 >= polygonCollider.Points.Count ? polygonCollider.TransformToWorld(polygonCollider.Points[0]) : polygonCollider.TransformToWorld(polygonCollider.Points[i + 1]);
                    var dist = PhysicsExtensions.DistanceFromLine(point1, point2, MouseInput.Position);
                    if (dist < minDistance)
                    {
                        minDistance = dist;
                        newPointIndex = i;
                    }
                }

                newPointIndex = newPointIndex + 1 >= polygonCollider.Points.Count ? 0 : newPointIndex + 1;
                polygonCollider.Points.Insert(newPointIndex, polygonCollider.TransformToLocal(MouseInput.Position));
                polygonCollider.BuildEdges();

                //polygonCollider.localPoints.Insert(newPointIndex,MouseInput.Position-polygonCollider.Transform.Position);

                // creating new Point
                SilentGameObject pointGO = new SilentGameObject(name: "ColliderEditor point");
                pointGO.transform.Position = MouseInput.Position;
                BoxCollider boxCollider = pointGO.AddComponent<BoxCollider>();
                boxCollider.rect = new RectangleF(0, 0, 10, 10);

                pointGO.AddComponent<Rigidbody>().IsStatic = true;
                pointGO.GetComponent<Rigidbody>().IsButton = true;

                pointGO.AddComponent<BoxRenderer>().Fill = true;

                verticeGameObjects.Insert(newPointIndex, pointGO);
                verticeGameObjects[newPointIndex].GetComponent<Renderer>().Color = Color.DeepSkyBlue;
                selectedPoint = verticeGameObjects[newPointIndex];

                dynamicVertex.Active = false;
                pointGO.Awake();

            }
            if (vertexIndex == null && dynamicVertex.Active == false && selectedPoint == null) // we areon on vertex or line
            {
                editing = true;
                ToggleEditing();
                Tools.SetSelectTool();
            }
        }

        public void ToggleEditing()
        {
            editing = !editing;
            if (editing)
            {
                for (int i = 0; i < verticeGameObjects.Count; i++)
                {
                    verticeGameObjects[i].Active = false;
                }
                verticeGameObjects.Clear();
                var a = EditorSceneView.GetInstance().GetSelectedGameObject();
                collider = EditorSceneView.GetInstance().GetSelectedGameObject()?.GetComponent<Collider>();

                EditorSceneView.GetInstance().transformHandle.gameObject.Active = false;

                polygonCollider = (PolygonCollider)collider;
                if (polygonCollider != null)
                {
                    for (int i = 0; i < polygonCollider.Points.Count; i++)
                    {
                        SilentGameObject pointGO = new SilentGameObject(name: "ColliderEditor point");
                        pointGO.transform.Position = polygonCollider.TransformToWorld(polygonCollider.Points[i]);
                        BoxCollider boxCollider = pointGO.AddComponent<BoxCollider>();
                        boxCollider.rect = new RectangleF(0, 0, 10, 10);

                        pointGO.AddComponent<Rigidbody>().IsStatic = true;
                        pointGO.GetComponent<Rigidbody>().IsButton = true;

                        pointGO.AddComponent<BoxRenderer>().Fill = true;

                        pointGO.Awake();
                        verticeGameObjects.Add(pointGO);
                    }
                }
            }
            else
            {
                selectedPoint = null;
                for (int i = 0; i < verticeGameObjects.Count; i++)
                {
                    verticeGameObjects[i].Active = false;
                }
            }
        }
        private Vector2 offset = Vector2.Zero;
        public void Update()
        {
            if (polygonCollider == null || editing == false) return;

            for (int i = 0; i < verticeGameObjects.Count; i++) // update all vertice DOTS positions
            {
                verticeGameObjects[i].transform.Position = polygonCollider.TransformToWorld(polygonCollider.Points[i]) - verticeBoxOffset +
                    ((selectedPoint != null && verticeGameObjects[i] == selectedPoint) ? offset : Vector2.Zero);
            }

            if (selectedPoint != null) // we have selected vertex
            {
                dynamicVertex.Active = false;
                if (Keyboard.GetState().IsKeyDown(Keys.LeftControl))
                {
                    selectedPoint.transform.Position = Extensions.TranslateToGrid(MouseInput.Position);
                }
                else
                {
                    selectedPoint.transform.Position = MouseInput.Position + offset;
                }
                //polygonCollider.localPoints[verticeGameObjects.IndexOf(selectedPoint)] = selectedPoint.GetComponent<BoxCollider>().rect.Center - polygonCollider.Transform.Position;
                polygonCollider.Points[verticeGameObjects.IndexOf(selectedPoint)] = polygonCollider.TransformToLocal(MouseInput.Position) + verticeBoxOffset + offset;
            }
            else// no point selected
            {
                bool overVertex = false;
                for (int i = 0; i < verticeGameObjects.Count; i++) // loop through vertices, if it is under mouse, highlight it, and also disable dynamicVertex
                {
                    if (MouseInput.Position.In(verticeGameObjects[i].GetComponent<BoxCollider>()).intersects)
                    {
                        verticeGameObjects[i].GetComponent<Renderer>().Color = Color.DeepSkyBlue;
                        overVertex = true;
                    }
                    else
                    {
                        verticeGameObjects[i].GetComponent<Renderer>().Color = Color.White;
                    }
                    dynamicVertex.Active = !overVertex;
                }

                // Move dynamicVertex on line
                if (overVertex == false)
                {
                    float minDistance = float.PositiveInfinity;
                    Vector2 pointOnLine = Vector2.Zero;
                    for (int i = 0; i < polygonCollider.Points.Count; i++) // find closest line
                    {
                        Vector2 point1 = polygonCollider.TransformToWorld(polygonCollider.Points[i]);
                        Vector2 point2 = i + 1 >= polygonCollider.Points.Count ? polygonCollider.TransformToWorld(polygonCollider.Points[0]) : polygonCollider.TransformToWorld(polygonCollider.Points[i + 1]);
                        var dist = PhysicsExtensions.DistanceFromLine(point1, point2, MouseInput.Position);
                        if (dist < minDistance)
                        {
                            minDistance = dist;
                            pointOnLine = PhysicsExtensions.ClosestPointOnLine(point1, point2, MouseInput.Position);
                        }
                    }
                    if (Vector2.Distance(pointOnLine, MouseInput.Position) < 10) // dont do anything if we are too far away from line
                    {
                        dynamicVertex.Active = true;
                        dynamicVertex.transform.Position = pointOnLine - dynamicVertex.GetComponent<BoxCollider>().rect.Size.ToVector2() / 2;
                    }
                    else
                    {
                        dynamicVertex.Active = false;
                    }
                }
            }

        }
    }
}
