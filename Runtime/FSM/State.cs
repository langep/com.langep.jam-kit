namespace Langep.JamKit.FSM
{
    public abstract class State<TState, TContext>: IState<TState, TContext> where TContext : IContext<TState, TContext> where TState : State<TState, TContext>
    {
        private bool _isRootState = true;
        private TContext _ctx;
        private IFactory<TState> _factory;
        private State<TState, TContext> _currentSuperState;
        private State<TState, TContext> _currentSubState;

        protected bool IsRootState
        {
            get => _isRootState;
            set => _isRootState = value;
        }

        protected TContext Ctx => _ctx;
        protected IFactory<TState> Factory => _factory;


        protected State(TContext ctx, IFactory<TState> factory)
        {
            _ctx = ctx;
            _factory = factory;
        }

        public abstract void InitializeSubState();
        protected abstract void EnterState();
        protected abstract void ExitState();
        protected abstract void CheckSwitchStates();

        public bool Is<TOtherState>() where TOtherState : State<TState, TContext> => GetType().IsSubclassOf(typeof(TOtherState));

        public void EnterStates()
        {
            EnterState();
            _currentSubState?.EnterStates();
        }

        public void ExitStates()
        {
            ExitState();
            _currentSubState?.ExitStates();
        }

        public void UpdateStates()
        {
            UpdateState();
            _currentSubState?.UpdateStates();
        }

        protected virtual void UpdateState()
        {
            CheckSwitchStates();
        }

        protected void SwitchState(TState newState)
        {
            ExitStates();
            newState.EnterStates();

            if (_isRootState)
            {
                _ctx.CurrentState = newState;
            }
            else
            {
                _currentSuperState?.SetSubState(newState);
            }
        }

        protected void SetSuperState(State<TState, TContext> newSuperState)
        {
            _currentSuperState = newSuperState;
        }

        protected void SetSubState(State<TState, TContext> newSubState)
        {
            _currentSubState = newSubState;
            newSubState.SetSuperState(this);
        }
    }
}