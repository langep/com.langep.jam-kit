using UnityEngine;

namespace Langep.JamKit.Utility
{
    public class Component<T> : MonoBehaviour where T : MonoBehaviour
    {
        public T ComponentRoot { get; private set; }

        private bool _initialized;
        
        public virtual void Init(T entity)
        {
            if (_initialized) return;
            _initialized = true;
            ComponentRoot = entity;
        }
    }
}