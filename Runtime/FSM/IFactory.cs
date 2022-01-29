namespace Langep.JamKit.FSM
{
    public interface IFactory<TState>
    {
        TState Create<TCreate>() where TCreate : TState;
    }
}