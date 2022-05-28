using System;

namespace Langep.JamKit.Systems
{
    public enum ServiceLocatorLookupMode
    {
        Local,
        Parent,
        Fallback
    }
    
    public interface IServiceLocator
    {
        void RegisterService<T>(T service, Type serviceKey = null) where T : IService;
        T LocateService<T>(Type serviceKey = null, ServiceLocatorLookupMode lookupMode = ServiceLocatorLookupMode.Fallback) where T : IService;
    }
}