using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Project.Scripts
{
    public class ObjectPool<T>
        where T : MonoBehaviour
    {
        private readonly Transform _container;
        private readonly T _prefab;

        private List<T> _pool;

        public ObjectPool(T prefab, int count, Transform container)
        {
            _prefab = prefab;
            _container = container;
            CrateObjectPool(count);
        }

        public bool AutoExpand { get; set; }

        public T GetFreeElement()
        {
            if (HasFreeElement(out var element))
                return element;

            if (AutoExpand)
                return CreateObject(true);

            throw new Exception($"There is no free elements in pool of type {typeof(T)}");
        }

        private bool HasFreeElement(out T element)
        {
            foreach (var objects in _pool)
            {
                if (!objects.gameObject.activeInHierarchy)
                {
                    element = objects;
                    objects.gameObject.SetActive(true);

                    return true;
                }
            }

            element = null;

            return false;
        }

        private void CrateObjectPool(int count)
        {
            _pool = new List<T>();

            for (int i = 0; i < count; i++)
            {
                CreateObject();
            }
        }

        private T CreateObject(bool isActiveByDefault = false)
        {
            var createdObject = Object.Instantiate(_prefab, _container);
            createdObject.gameObject.SetActive(isActiveByDefault);

            _pool.Add(createdObject);

            return createdObject;
        }
    }
}