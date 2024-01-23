using System;
using System.Collections.Generic;
using SimpleJson;

namespace Specifications.Weapon
{
    [Serializable]
    public class WeaponSpecification : Specification.Specification
    {
        public string BulletId;
        public int AmmoInHorn;
        public int MaxAmmo;
        public float Damage;
        public float ReloadTime;
        public float FireRate;
        public string PrefabKey2D; 
        public string PrefabKey3D;

        public override void Fill(IDictionary<string, object> node)
        {
            _id = node.GetString("id");
            BulletId = node.GetString("bullet_id");
            AmmoInHorn = node.GetInt("ammo_in_horn");
            MaxAmmo = node.GetInt("max_ammo");
            Damage = node.GetFloat("damage");
            ReloadTime = node.GetFloat("reload_time");
            FireRate = node.GetFloat("fire_rate");
            PrefabKey2D = node.GetString("prefab_key_2d");
            PrefabKey3D = node.GetString("prefab_key_3d");
        }
    }
}