using Presenter;

namespace Entities.Asteroids.Asteroid.Damage
{
    public class AsteroidDamagePresenter : IPresenter
    {
        private readonly IGameModel _gameModel;
        private readonly AsteroidModel _model;
        private readonly IAsteroidView _view;

        public AsteroidDamagePresenter(IGameModel gameModel, AsteroidModel model, IAsteroidView view)
        {
            _gameModel = gameModel;
            _model = model;
            _view = view;
        }

        public void Init()
        {
        }

        public void Dispose()
        {
        }
    }
}