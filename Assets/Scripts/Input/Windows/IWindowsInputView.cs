namespace Input.Windows
{
    public interface IWindowsInputView : IInputView
    {
        void Initialize();
        void Dispose();
    }
}