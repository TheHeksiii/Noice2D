using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Engine;
namespace Scripts
{
    public class Collider : Component
    {
        [System.Xml.Serialization.XmlIgnore]
        [LinkableComponent]
        public Rigidbody rigidbody;
    }
}
