using UnityEngine;

namespace Langep.JamKit.Utility
{

    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;
        private static readonly object _instanceLock = new object();
        private static bool _quitting = false;

        public static T Instance {
            get {
                lock(_instanceLock){
                    if (_instance != null || _quitting) return _instance;
                    _instance = GameObject.FindObjectOfType<T>();
                    if (_instance != null) return _instance;
                    var go = new GameObject(typeof(T).ToString());
                    _instance = go.AddComponent<T>();

                    DontDestroyOnLoad(_instance.gameObject);

                    return _instance;
                }
            }
        }

        protected virtual void Awake()
        {
            if(_instance==null) _instance = gameObject.GetComponent<T>();
            else if(_instance.GetInstanceID()!=GetInstanceID()){
                Destroy(gameObject);
                Debug.LogWarning($"Instance of {GetType().FullName} already exists, removing {ToString()}");
            }
        }

        protected virtual void OnApplicationQuit() 
        {
            _quitting = true;
        }
    }
}
