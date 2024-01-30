using System;
using UnityEngine;

namespace Input
{
    public interface IInputModel
    {
        event Action OnFireInput;

        bool IsFireInputPressed { get; set; }
        bool IsShipShooting { get; set; }
        
        float Thrust { get; }
        float Strafe { get; }
        float UpDown { get; }
        float Roll { get; }
        Vector2 PitchYaw { get; }
        bool IsBoosted { get; set; }
        Vector2 MousePosition { get; set; }
    }
}