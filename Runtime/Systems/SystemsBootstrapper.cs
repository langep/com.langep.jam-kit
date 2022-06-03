using System;
using System.Collections.Generic;
using Langep.JamKit.Utility;
using UnityEngine;

namespace Langep.JamKit.Systems
{
    public class SystemsBootstrapper : Singleton<SystemsBootstrapper>
    {
        [SerializeField] private List<System> systems;
        public Transform HolderTransform => _holderObject.transform;
        
        private GameObject _holderObject;
        private IServiceLocator _systemLocator;

        public T GetService<T>(Type serviceKey = null) where T : IService
        {
            Debug.Log($"GetService: {GetInstanceID()}" );
            return _systemLocator.LocateService<T>(serviceKey);
        }
        
        protected override void Awake()
        {
            base.Awake();
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
            Debug.Log($"InitializeSystems: {GetInstanceID()}" );
            _systemLocator = new BasicServiceLocator();
            foreach (var system in systems)
            {
                system.Initialize(_holderObject.transform);
                _systemLocator.RegisterService(system);
                foreach (var providedService in system.ProvidedServices())
                {
                    _systemLocator.RegisterService(system, providedService);
                }
            }
        }
    }
}