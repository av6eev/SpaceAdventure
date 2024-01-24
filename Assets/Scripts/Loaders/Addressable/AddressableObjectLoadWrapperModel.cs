using Loader.Object;

namespace Loaders.Addressable
{
    public class AddressableObjectLoadWrapperModel<T> : IAddressableObjectLoadWrapperModel
    {
        public ILoadObjectToWrapperModel<T> LoadObjectToWrapperModel { get; }

        public AddressableObjectLoadWrapperModel(ILoadObjectToWrapperModel<T> loadObjectToWrapperModel)
        {
            LoadObjectToWrapperModel = loadObjectToWrapperModel;
        }
    }

    public interface IAddressableObjectLoadWrapperModel
    {
    }
}