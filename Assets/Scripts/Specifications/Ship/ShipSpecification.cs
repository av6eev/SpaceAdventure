using System;
using System.Collections.Generic;
using Entities.Ship;
using SimpleJson;

namespace Specifications.Ship
{
     [Serializable]
     public class ShipSpecification : Specification.Specification
     {
          public ShipType Type;
          public string Name;
          public string Description;
          public int Price;
          public float Health;
          public int BulletCount;
          public float ReloadTime;
          public float ShootRate;
          public bool IsAutomatic;
          public float Speed;
          public string PrefabKey2D; 
          public string PrefabKey3D;
          public string SpriteKey;
          public string RequirementId;
          public float YawTorque;
          public float PitchTorque;
          public float RollTorque;
          public float Thrust;
          public float UpThrust;
          public float StrafeThrust;
          public float ThrustGlideReduction;
          public float UpDownGlideReduction;
          public float LeftRightGlideReduction;
          public float MaxBoostAmount;
          public float BoostDeprecationRate;
          public float BoostRechargeRate;
          public float BoostMultiplier;

          public override void Fill(IDictionary<string, object> node)
          {
               _id = node.GetString("id");
               Type = (ShipType)node.GetInt("type");
               Name = node.GetString("name");
               Description = node.GetString("description");
               Price = node.GetInt("price");
               Health = node.GetFloat("health");
               BulletCount = node.GetInt("bullet_count");
               ReloadTime = node.GetFloat("reload_time");
               ShootRate = node.GetFloat("shoot_rate");
               IsAutomatic = node.GetBoolean("is_automatic");
               Speed = node.GetFloat("speed");
               PrefabKey2D = node.GetString("prefab_key_2d");
               PrefabKey3D = node.GetString("prefab_key_3d");
               SpriteKey = node.GetString("icon_key");
               RequirementId = node.GetString("requirement_id");
               YawTorque = node.GetFloat("yaw_torque");
               PitchTorque = node.GetFloat("pitch_torque");
               RollTorque = node.GetFloat("roll_torque");
               Thrust = node.GetFloat("thrust");
               UpThrust = node.GetFloat("up_thrust");
               StrafeThrust = node.GetFloat("strafe_thrust");
               ThrustGlideReduction = node.GetFloat("thrust_glide_reduction");
               UpDownGlideReduction = node.GetFloat("up_down_glide_reduction");
               LeftRightGlideReduction = node.GetFloat("left_right_glide_reduction");
               MaxBoostAmount = node.GetFloat("max_boost_amount");
               BoostDeprecationRate = node.GetFloat("boost_deprecation_rate");
               BoostRechargeRate = node.GetFloat("boost_recharge_rate");
               BoostMultiplier = node.GetFloat("boost_multiplier");
          }

          public int GetPrice() => Price;
     }
}