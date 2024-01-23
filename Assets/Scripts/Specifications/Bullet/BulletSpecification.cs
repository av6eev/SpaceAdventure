using System;
using System.Collections.Generic;
using SimpleJson;

namespace Specifications.Bullet
{
    [Serializable]
    public class BulletSpecification : Specification.Specification
    {
        public float Damage;
        public float MaxHealth;
        public float Speed;
        public string PrefabKey2D; 
        public string PrefabKey3D;

        public override void Fill(IDictionary<string, object> node)
        {
            _id = node.GetString("id");
            Damage = node.GetFloat("damage");
            MaxHealth = node.GetFloat("max_health");
            Speed = node.GetFloat("speed");
            PrefabKey2D = node.GetString("prefab_key_2d");
            PrefabKey3D = node.GetString("prefab_key_3d");
        }
    }
}