using Engine;
using System.Collections.Generic;

namespace Editor
{
    public struct SceneFile
    {
        public List<Engine.GameObject> GameObjects;
        public List<Scripts.Component> Components;

        public SceneFile CreateForOneGameObject(GameObject go)
        {
            return new SceneFile() { GameObjects = new List<Engine.GameObject>() { go }, Components = go.Components };
        }
    }
}
