using UnityEngine;

namespace Langep.JamKit.Singletons
{
    public abstract class Singleton<T> : MonoBehaviour where T : Component
    {
        /// <summary>
        /// The instance.
        /// </summary>
        private static T _instance;


        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static T Instance
        {
            get
            {
                if (_instance != null) return _instance;
                
                _instance = FindObjectOfType<T> ();
                if (_instance == null)
                {
                    var obj = new GameObject {name = typeof(T).Name};
                    _instance = obj.AddComponent<T> ();
                }

                return _instance;
            }
        }

        /// <summary>
        /// Use this for initialization.
        /// </summary>
        protected virtual void Awake ()
        {
            if ( _instance == null )
            {
                _instance = this as T;
                DontDestroyOnLoad ( gameObject );
            }
            else
            {
                Destroy ( gameObject );
            }
        }
    }
}