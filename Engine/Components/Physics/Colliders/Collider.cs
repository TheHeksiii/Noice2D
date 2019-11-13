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
