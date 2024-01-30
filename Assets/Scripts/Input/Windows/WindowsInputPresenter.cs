using UnityEngine;

namespace Input.Windows
{
    public class WindowsInputPresenter : BaseInputPresenter
    {
        private readonly IGameModel _gameModel;
        private readonly WindowsInputModel _model;
        private readonly IWindowsInputView _view;

        public WindowsInputPresenter(IGameModel gameModel, WindowsInputModel model, IWindowsInputView view) : base(gameModel, model, view)
        {
            _gameModel = gameModel;
            _model = model;
            _view = view;
        }

        public override void Init()
        {
            _view.Initialize();
            
            _view.OnDebugPanelVisibilityChange += HandleDebugPanelVisibilityChangeInput;
            _view.OnThrust += HandleThrustInput;
            _view.OnRoll += HandleRollInput;
            _view.OnStrafe += HandleStrafeInput;
            _view.OnUpDown += HandleUpDownInput;
            _view.OnPitchYaw += HandlePitchYawInput;
            _view.OnBoost += HandleBoostInput;
            _view.OnBoostCanceled += HandleBoostCanceledInput;
            _view.OnMouseMove += HandleMouseMoveInput;
        }
        
        public override void Dispose()
        {
            _view.Dispose();
            
            _view.OnDebugPanelVisibilityChange -= HandleDebugPanelVisibilityChangeInput;
            _view.OnThrust -= HandleThrustInput;
            _view.OnRoll -= HandleRollInput;
            _view.OnStrafe -= HandleStrafeInput;
            _view.OnUpDown -= HandleUpDownInput;
            _view.OnPitchYaw -= HandlePitchYawInput;
            _view.OnBoost -= HandleBoostInput;
            _view.OnBoostCanceled -= HandleBoostCanceledInput;
            _view.OnMouseMove -= HandleMouseMoveInput;
        }

        private void HandleDebugPanelVisibilityChangeInput()
        {
        }

        private void HandleMouseMoveInput(Vector2 input)
        {
            _model.MousePosition = input;
        }

        private void HandleBoostInput()
        {
            _model.IsBoosted = true;
        }
        
        private void HandleBoostCanceledInput()
        {
            _model.IsBoosted = false;
        }

        private void HandlePitchYawInput(Vector2 input)
        {
            _model.PitchYaw = input;
        }

        private void HandleUpDownInput(float input)
        {
            _model.UpDown = input;
        }

        private void HandleStrafeInput(float input)
        {
            _model.Strafe = input;
        }

        private void HandleRollInput(float input)
        {
            _model.Roll = input;
        }

        private void HandleThrustInput(float input)
        {
            _model.Thrust = input;
        }
    }
}