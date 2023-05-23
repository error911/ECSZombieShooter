using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace TestGame
{
    public class LocalAssetLoader
    {
        private GameObject _go;

        protected async Task<T> LoadInternal<T>(string assetId)
        {
            var handle = Addressables.InstantiateAsync(assetId);
            _go = await handle.Task;
            if (_go.TryGetComponent(out T component) == false)
            {
                throw new NullReferenceException($"Компонент {typeof(T)} не найден по пути {assetId}");
            }
            return component;
        }

        protected void UnloadInternal()
        {
            if (_go == null) return;

            _go.SetActive(false);
            Addressables.ReleaseInstance(_go);
            _go = null;
        }
    }
}