namespace Langep.Unity.Tools.FSM
{
    public interface IState<TState, TContext> where TState : State<TState, TContext> where TContext: IContext<TState, TContext>
    {
        bool Is<TOtherState>() where TOtherState : State<TState, TContext>;
    }
}