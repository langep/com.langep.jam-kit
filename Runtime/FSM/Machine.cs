namespace Langep.JamKit.FSM
{
    public class Machine<TState, TContext> where TState : State<TState, TContext> where TContext : IContext<TState, TContext>
    {
        protected TContext _ctx;
        protected IFactory<TState> _factory;

        public Machine(TContext ctx, IFactory<TState> factory)
        {
            _ctx = ctx;
            _factory = factory;
        }

        public void Start<TInitialState>() where TInitialState : TState
        {
            var initialState = _factory.Create<TInitialState>();
            
            initialState.EnterStates();
            
            _ctx.CurrentState = initialState;
        }
        
        public virtual void Update()
        {
            _ctx.CurrentState.UpdateStates();
        }
    }
}