using Loader;
using Loader.Object;
using Presenter;

namespace Loaders.Addressable
{
    public class AddressableObjectLoadWrapper : ILoadObjectWrapper
    {
        private readonly PresentersDictionary<IWrapperLoadModel> _presenters = new();

        public void Load<T>(ILoadObjectToWrapperModel<T> model)
        {
            var element = new AddressableObjectLoadWrapperModel<T>(model);
            var presenter = new AddressableObjectLoadWrapperPresenter<T>(element);
            
            presenter.Init();
            _presenters.Add(model, presenter);
        }

        public void Unload(IWrapperLoadModel model)
        {
            _presenters.Remove(model);
        }
    }
}