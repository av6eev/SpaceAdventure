using Presenter;
using Specification.Scene;
using UnityEngine;

namespace LocationBuilder
{
    public class LocationLogicBuilder
    {
        private readonly GameModel _environment;
        private readonly PresentersList _presenters;
        private readonly SceneSpecification _specification;

        public LocationLogicBuilder(GameModel environment, PresentersList presenters, SceneSpecification specification)
        {
            _environment = environment;
            _presenters = presenters;
            _specification = specification;
        }

        public void Build()
        {
            var sceneView = GameObject.Find(_specification.PrefabId).GetComponent<LocationSceneView>();
            
            switch (_specification.SceneId)
            {
                case "dock_scene":
                    break;
                case "home_scene":
                    break;
                case "session_scene":
                    break;
            }
        }
    }
}