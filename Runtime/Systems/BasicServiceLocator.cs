using System;
using System.Collections.Concurrent;
using UnityEngine;

namespace Langep.JamKit.Systems
{

    
    public class BasicServiceLocator : IServiceLocator
    {
        private readonly ConcurrentDictionary<Type, IService> _services = new ConcurrentDictionary<Type, IService>();

        private IServiceLocator _parentLocator;
        private bool _hasParentLocator = false;
        
        public BasicServiceLocator(IServiceLocator parentLocator = null)
        {
            _parentLocator = parentLocator;
            _hasParentLocator = _parentLocator != null;
        }
        
        public void RegisterService<T>(T service, Type serviceKey = null) where T : IService
        {
            var key = serviceKey ?? service.GetType();
            _services[key] = service;
        }

        public T LocateService<T>(Type serviceKey = null, ServiceLocatorLookupMode lookupMode = ServiceLocatorLookupMode.Fallback) where T : IService
        {
            var key = serviceKey ?? typeof(T);

            IService service = null;
            switch (lookupMode)
            {
                case ServiceLocatorLookupMode.Local:
                    _services.TryGetValue(key, out service);
                    break;
                case ServiceLocatorLookupMode.Parent:
                    _services.TryGetValue(key, out service);
                    if (_services != null) break;
                    if (!_hasParentLocator) break;
                    service = _parentLocator.LocateService<T>(serviceKey, ServiceLocatorLookupMode.Local);
                    break;
                case ServiceLocatorLookupMode.Fallback:
                    _services.TryGetValue(key, out service);
                    if (_services != null) break;
                    if (!_hasParentLocator) break;
                    service = _parentLocator.LocateService<T>(serviceKey, ServiceLocatorLookupMode.Fallback);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(lookupMode), lookupMode, null);
            }

            if (service == null)
            {
                Debug.Log($"couldn't find {key}");
                foreach (var k in _services.Keys)
                {
                    Debug.Log($"contains {k}: {_services[k]}");    
                }
            }
            
            return service != null ? (T) service : default(T);

        }
    }
}
