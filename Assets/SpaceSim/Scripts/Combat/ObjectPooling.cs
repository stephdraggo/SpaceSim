using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UObject = UnityEngine.Object;

namespace SpaceSim
{
    [Serializable]
    public class ObjectPooling<T> where T : Component
    {
        //[SerializeField] private T prefab;
        [SerializeField] private List<T> pool;

        public ObjectPooling()
        {
            pool ??= new List<T>();
        }

        public T Spawn(T prefab, Vector3 position = default, Quaternion rotation = default)
        {
            if (pool.Count == 0)
            {
                return UObject.Instantiate(prefab, position, rotation);
            }

            //remember that if the object has stats
            //use an on enable method in that object to reset them
            T prefabComponent = pool[0];
            pool.RemoveAt(0);
            prefabComponent.gameObject.SetActive(true);
            Transform prefabTransform = prefabComponent.transform;
            prefabTransform.position = position;
            prefabTransform.rotation = rotation;
            return prefabComponent;
        }

        public void Despawn(T component)
        {
            component.gameObject.SetActive(false);
            pool.Add(component);
        }
    }
}