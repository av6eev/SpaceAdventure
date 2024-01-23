using System;
using System.Collections.Generic;
using SimpleJson;

namespace Specifications.Asteroid
{
    [Serializable]
    public class AsteroidSpecification : Specification.Specification
    {
        public AsteroidType Type;
        public float Health;
        public float Speed;
        public float Damage;
        public float ChanceToSpawn;
        public string PrefabKey2D;
        public string PrefabKey3D;

        public override void Fill(IDictionary<string, object> node)
        {
            _id = node.GetString("id");
            Type = (AsteroidType)node.GetInt("type");
            Health = node.GetFloat("health");
            Speed = node.GetFloat("speed");
            Damage = node.GetFloat("damage");
            ChanceToSpawn = node.GetFloat("chance_to_spawn");
            PrefabKey2D = node.GetString("prefab_key_2d");
            PrefabKey3D = node.GetString("prefab_key_3d");
        }
    }
}