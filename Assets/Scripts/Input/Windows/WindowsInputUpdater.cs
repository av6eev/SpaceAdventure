namespace Input.Windows
{
    public class WindowsInputUpdater : BaseInputUpdater
    {
        private readonly IWindowsInputView _inputView;
        private readonly WindowsInputModel _inputModel;

        public WindowsInputUpdater(IWindowsInputView inputView, WindowsInputModel inputModel)
        {
            _inputView = inputView;
            _inputModel = inputModel;
        }
    
        public override void Update(float deltaTime)
        {
            if (_inputView.CheckFireInputPressed() && !_inputView.CheckPointerOverUI())
            {
                _inputModel.IsFireInputPressed = true;
                _inputModel.CallFire();
            }
            else
            {
                _inputModel.IsFireInputPressed = false;
            }
        }
    }
}