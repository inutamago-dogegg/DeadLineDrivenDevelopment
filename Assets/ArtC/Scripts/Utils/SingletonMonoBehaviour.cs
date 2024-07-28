using System;
using UnityEngine;

namespace ArtC.Utils {
    public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : SingletonMonoBehaviour<T> {
        [SerializeField] protected bool DontDestroy = false;

        private static T _instance;

        public static bool HasInstance => _instance != null;

        public static T Instance {
            get {
                // if singleton already exists, return it
                if (_instance) {
                    return _instance;
                }

                _instance = FindObjectOfType<T>();

                // if found instance, return it
                if (_instance) {
                    return _instance;
                }

                // if not found instance, create log error
                Debug.LogError($"Not found {typeof(T)}");
                return null;
            }
        }

        private void Initialize() {
            var obj = gameObject;

            // if singleton is not this, destroy this instance
            if (Instance != this) {
                Debug.LogWarning("Destroy this instance for singleton");
                Destroy(obj);
                return;
            }

            if (DontDestroy) {
                DontDestroyOnLoad(obj);
            }

            // rename this gameObject for emphasize singleton
            if (obj.name.IndexOf("[Singleton]", StringComparison.Ordinal) < 0) {
                obj.name = $"[Singleton] {obj.name}";
            }
        }

        protected virtual void Awake() {
            Initialize();
        }
    }
}
