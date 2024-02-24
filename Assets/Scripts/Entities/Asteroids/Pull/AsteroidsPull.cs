using Entities.Asteroids.Asteroid;
using Pulls;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Entities.Asteroids.Pull
{
    public class AsteroidsPull : Pull<IAsteroidView>
    {
        private readonly Transform _root;
        public readonly GameObject Element;

        private const string RootGoName = "AsteroidsPull";

        public AsteroidsPull(GameObject element)
        {
            Element = element;
            _root = GameObject.Find(RootGoName).transform;
        }
        
        protected override IAsteroidView CreateElement()
        {
            var go = Object.Instantiate(Element, _root).GetComponent<IAsteroidView>();
            ModifyPutObject(go);
            return go;
        }

        protected override void RemoveElement(IAsteroidView element)
        {
            Object.Destroy(element as MonoBehaviour);
        }

        protected override void ModifyPutObject(IAsteroidView element)
        {
            element.Hide();
        }

        protected override void ModifyGetObject(IAsteroidView element)
        {
            element.Show();
        }
    }
}