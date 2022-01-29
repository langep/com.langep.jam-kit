namespace Langep.JamKit.FSM
{
    public interface IContext<TState, TContext> where TContext : IContext<TState, TContext>
    {
        public TState CurrentState { get; set; }
    }
}