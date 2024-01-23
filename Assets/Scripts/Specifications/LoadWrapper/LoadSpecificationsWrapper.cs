using Awaiter;
using Loader.Object;
using SimpleJson;
using Specification;
using Specification.Startup;
using Specifications.Collection;
using UnityEngine;

namespace Specifications.LoadWrapper
{
    public class LoadSpecificationsWrapper<T> where T : ISpecification, new()
    {
        private readonly IPrimitiveSpecificationsCollection _specificationsCollection;
        public readonly CustomAwaiter LoadAwaiter = new();
    
        public LoadSpecificationsWrapper(ILoadObjectsModel loadObjectsModel, string key, IPrimitiveSpecificationsCollection specificationsCollection, StartupSpecification startupSpecification)
        {
            _specificationsCollection = specificationsCollection;

            switch (startupSpecification.StartupSpecificationType)
            {
                case StartupSpecificationType.Json:
                    LoadFromJson(loadObjectsModel, key);
                    break;
                case StartupSpecificationType.SerializeObject:
                    LoadFromAsset(loadObjectsModel, key);
                    break;
            }
        }

        private async void LoadFromJson(ILoadObjectsModel loadObjectsModel, string key)
        {
            var objectModel = loadObjectsModel.Load<TextAsset>(key);
            await objectModel.LoadAwaiter;
            
            var result = new JsonParser(objectModel.Result.text).ParseAsDictionary();

            foreach (var element in result.GetNodes(key))
            {
                var specification = new T();
                
                specification.Fill(element);
                _specificationsCollection.Add(element.GetString("id"), specification);
            }
            
            LoadAwaiter.Complete();
        }
        
        private async void LoadFromAsset(ILoadObjectsModel loadObjectsModel, string key)
        {
            var objectModel = loadObjectsModel.Load<SpecificationCollectionScrObj<T>>(key);
            await objectModel.LoadAwaiter;

            foreach (var element in objectModel.Result.Collection)
            {
                _specificationsCollection.Add(element.Specification.Id, element.Specification);
            }
            
            LoadAwaiter.Complete();
        }
    }
}