using System;
using UnityEngine;

namespace Langep.JamKit.Systems
{
    public abstract class System : ScriptableObject
    {
        [SerializeField] protected string systemName;

        [NonSerialized] protected GameObject holderObject;
        [NonSerialized] private bool _isInitialized = false;
        
        /// <summary>
        /// Initializes the system.
        /// </summary>
        /// <param name="systemsHolder">The parent transform in the hierarchy.</param>
        public void Initialize(Transform systemsHolder)
        {
            if (_isInitialized)
            {
                Debug.Log($"{systemName} is already initialized");
                return;
            }

            InitializeHolderObject(systemsHolder.transform);

            _isInitialized = true;
            OnInitialize();
        }
        
        /// <summary>
        /// Override this to implement custom initialization logic.
        /// </summary>
        protected virtual void OnInitialize()
        {
        }

        /// <summary>
        /// Instantiates the prefab under holderObject.
        /// </summary>
        /// <param name="prefab">The prefab to instantiate.</param>
        /// <returns>The created instance.</returns>
        protected T InstantiatePrefab<T>(T prefab) where T : UnityEngine.Object
        {
            return Instantiate(prefab, holderObject.transform);
        }
        
        /// <summary>
        /// Returns the passed in property.
        /// <para>Initializes the System and SystemsBootstrapper if needed.</para> 
        /// </summary>
        /// <param name="property">Property value to return.</param>
        /// <returns></returns>
        protected T SystemProperty<T>(T property)
        {
            LazyInitialize();
            return property;
        }
        
        /// <summary>
        /// Returns the passed in property.
        /// <para>Initializes the System and SystemsBootstrapper if needed.</para> 
        /// </summary>
        /// <param name="propertyGetter">Function returning property to return.</param>
        /// <returns></returns>
        protected T SystemProperty<T>(Func<T> propertyGetter)
        {
            LazyInitialize();
            return propertyGetter();
        }

        private void InitializeHolderObject(Transform parent)
        {
            var holder = new GameObject {name = systemName};
            holder.transform.parent = parent;
            holderObject = holder;
        }
        
        private void LazyInitialize()
        {
            if (_isInitialized) return;

            var systemsHolder = SystemsBootstrapper.Instance.HolderTransform;
            Initialize(systemsHolder);
        }
    }
}