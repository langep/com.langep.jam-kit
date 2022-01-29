namespace Langep.Unity.Tools.FSM
{
    public interface IFactory<TState>
    {
        TState Create<TCreate>() where TCreate : TState;
    }
}