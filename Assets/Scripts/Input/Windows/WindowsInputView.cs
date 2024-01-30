using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Input.Windows
{
    public class WindowsInputView : MonoBehaviour, IWindowsInputView
    {
        public event Action<float> OnThrust;
        public event Action<float> OnStrafe;
        public event Action<float> OnUpDown;
        public event Action<float> OnRoll;
        public event Action OnDebugPanelVisibilityChange;
        public event Action OnBoost;
        public event Action OnBoostCanceled;
        public event Action<Vector2> OnPitchYaw;
        public event Action<Vector2> OnMouseMove; 

        public InputActionAsset ShipInputAsset;

        public void Initialize()
        {
            ShipInputAsset.Enable();
            
            ShipInputAsset["Thrust"].performed += OnThrustInput;
            ShipInputAsset["Strafe"].performed += OnStrafeInput;
            ShipInputAsset["UpDown"].performed += OnUpDownInput;
            ShipInputAsset["Roll"].performed += OnRollInput;
            ShipInputAsset["PitchYaw"].performed += OnPitchYawInput;
            ShipInputAsset["Boost"].performed += OnBoostInput;
            ShipInputAsset["Boost"].canceled += OnBoostCanceledInput;
            ShipInputAsset["MousePosition"].performed += OnMouseMoveInput;
            ShipInputAsset["ShowHide"].performed += OnDebugPanelVisibilityChangeInput;
        }

        public void Dispose()
        {
            ShipInputAsset.Disable();
            
            ShipInputAsset["Thrust"].performed -= OnThrustInput;
            ShipInputAsset["Strafe"].performed -= OnStrafeInput;
            ShipInputAsset["UpDown"].performed -= OnUpDownInput;
            ShipInputAsset["Roll"].performed -= OnRollInput;
            ShipInputAsset["PitchYaw"].performed -= OnPitchYawInput;
            ShipInputAsset["Boost"].performed -= OnBoostInput;
            ShipInputAsset["Boost"].canceled -= OnBoostCanceledInput;
            ShipInputAsset["MousePosition"].performed -= OnMouseMoveInput;
            ShipInputAsset["ShowHide"].performed -= OnDebugPanelVisibilityChangeInput;
        }

        private void OnDebugPanelVisibilityChangeInput(InputAction.CallbackContext ctx) => OnDebugPanelVisibilityChange?.Invoke();
        private void OnMouseMoveInput(InputAction.CallbackContext ctx) => OnMouseMove?.Invoke(ctx.ReadValue<Vector2>());
        private void OnPitchYawInput(InputAction.CallbackContext ctx) => OnPitchYaw?.Invoke(ctx.ReadValue<Vector2>());
        private void OnRollInput(InputAction.CallbackContext ctx) => OnRoll?.Invoke(ctx.ReadValue<float>());
        private void OnUpDownInput(InputAction.CallbackContext ctx) => OnUpDown?.Invoke(ctx.ReadValue<float>());
        private void OnStrafeInput(InputAction.CallbackContext ctx) => OnStrafe?.Invoke(ctx.ReadValue<float>());
        private void OnThrustInput(InputAction.CallbackContext ctx) => OnThrust?.Invoke(ctx.ReadValue<float>());
        private void OnBoostInput(InputAction.CallbackContext ctx) => OnBoost?.Invoke();
        private void OnBoostCanceledInput(InputAction.CallbackContext ctx) => OnBoostCanceled?.Invoke();

        public bool CheckFireInputPressed()
        {
            throw new NotImplementedException();
        }

        public bool CheckPointerOverUI()
        {
            throw new NotImplementedException();
        }
    }
}