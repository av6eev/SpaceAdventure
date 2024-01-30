using Presenter;

namespace Input
{
    public abstract class BaseInputPresenter : IPresenter
    {
        private readonly IGameModel _gameModel;
        private readonly BaseInputModel _model;
        private readonly IInputView _view;

        protected BaseInputPresenter(IGameModel gameModel, BaseInputModel model, IInputView view)
        {
            _gameModel = gameModel;
            _model = model;
            _view = view;
        }

        public abstract void Init();

        public abstract void Dispose();
    }
}