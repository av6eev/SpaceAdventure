using System;
using UnityEngine;

namespace Input
{
    public abstract class BaseInputModel : IInputModel
    {
        public event Action OnFireInput;
        
        public bool IsFireInputPressed { get; set; }
        public bool IsShipShooting { get; set; }
        
        public float Thrust { get; set; }
        public float Strafe { get; set; }
        public float UpDown { get; set; }
        public float Roll { get; set; }
        public Vector2 PitchYaw { get; set; } = new(0, 0);
        public bool IsBoosted { get; set; }
        public Vector2 MousePosition { get; set; }

        public void CallFire()
        {
            OnFireInput?.Invoke();
        }
    }
}