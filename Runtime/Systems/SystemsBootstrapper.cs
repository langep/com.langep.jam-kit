using System.Collections.Generic;
using Langep.JamKit.Utility;
using UnityEngine;

namespace Langep.JamKit.Systems
{
    public class SystemsBootstrapper : Singleton<SystemsBootstrapper>
    {
        [SerializeField] private List<System> systems;
        private GameObject _holderObject;

        public Transform HolderTransform => _holderObject.transform;

        protected void Awake()
        {
            InitializeHolderObject();
            InitializeSystems();
        }
        
        private void InitializeHolderObject()
        {
            var holder = new GameObject {name = "Systems"};
            DontDestroyOnLoad ( holder );
            _holderObject = holder;
        }
        
        private void InitializeSystems()
        {
            foreach (var system in systems)
            {
                system.Initialize(_holderObject.transform);
            }
        }
    }
}