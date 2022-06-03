using Langep.JamKit.Systems;

namespace Langep.JamKit.UI
{
    public interface IUISystem : IService
    {
        IAnimationHelper GetAnimationHelper();
    }
}