using System;
using Presenter;
using Session;
using UnityEngine;

namespace Entities.Ship.BoostEffect
{
    public class ShipBoostEffectPresenter : IPresenter
    {
        private readonly SessionLocationGameModel _gameModel;
        private readonly ShipModel _model;
        private readonly IShipView _view;

        public ShipBoostEffectPresenter(SessionLocationGameModel gameModel, ShipModel model, IShipView view)
        {
            _gameModel = gameModel;
            _model = model;
            _view = view;
        }
        
        public void Init()
        {
            _model.CurrentSpeed.OnChanged += ChangeBoostEffect;
        }

        public void Dispose()
        {
            _model.CurrentSpeed.OnChanged += ChangeBoostEffect;
        }

        private void ChangeBoostEffect(Vector3 speed)
        {
            var firstSpeed = 40f;
            var secondSpeed = 65f;
            var thirdSpeed = 80f;
            var absX = Math.Abs(speed.x);
            var absY = Math.Abs(speed.y);
            var absZ = Math.Abs(speed.z);
            Vector2 effectSpeed;
            float effectSize;
            
            if (absX > thirdSpeed || absY > thirdSpeed || absZ > thirdSpeed)
            {
                effectSpeed = new Vector2(0f, -1.8f);
                effectSize = 1f;
            }
            else if (absX > secondSpeed || absY > secondSpeed || absZ > secondSpeed)
            {
                effectSpeed = new Vector2(0f, -1.4f);
                effectSize = .7f;
            }
            else if (absX > firstSpeed || absY > firstSpeed || absZ > firstSpeed)
            {
                effectSpeed = new Vector2(0f, -1f);
                effectSize = .3f;
            }
            else
            {
                effectSpeed = new Vector2(0f, -.6f);
                effectSize = 0f;
            }
            
            _view.ChangeBoostEffectSpeed(effectSpeed);
            _view.ChangeBoostEffectSize(effectSize);
        }
    }
}