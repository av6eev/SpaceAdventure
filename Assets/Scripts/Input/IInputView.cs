using System;
using UnityEngine;

namespace Input
{
    public interface IInputView
    {
        event Action OnDebugPanelVisibilityChange;
        event Action<float> OnThrust;
        event Action<float> OnStrafe;
        event Action<float> OnUpDown;
        event Action<float> OnRoll;
        event Action<Vector2> OnPitchYaw;
        event Action OnBoost;
        event Action OnBoostCanceled;
        event Action<Vector2> OnMouseMove;
        
        bool CheckFireInputPressed();
        bool CheckPointerOverUI();
    }
}