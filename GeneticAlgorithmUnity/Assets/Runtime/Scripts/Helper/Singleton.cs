using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public abstract class Singleton<T> : MonoBehaviour
        where T : Singleton<T>
    {
        private static bool _instantiated = false;

        private static volatile T _instance = null;

        public static T Instance
        {
            get
            {
                if (_instance == null && !_instantiated)
                {
                    _instance = FindObjectOfType<T>();

                    if (_instance == null)
                    {
                        GameObject go = new GameObject();
                        go.name = typeof(T).ToString();

                        _instance = go.AddComponent<T>();
                    }

                    if (!_instantiated)
                    {
                        _instantiated = true;
                        _instance.SingletonAwake();
                    }
                }

                return _instance;
            }
        }

        private void Awake()
        {
            if (Instance != this)
            {
                Destroy(gameObject);
            }
        }

        protected abstract void SingletonAwake();
    } 
}
