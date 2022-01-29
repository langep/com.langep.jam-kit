namespace Langep.Unity.Tools.FSM
{
    public struct StateChange<TState>
    {
        public readonly TState PreviousState;
        public readonly TState CurrentState;

        public StateChange(TState previousState, TState currentState)
        {
            PreviousState = previousState;
            CurrentState = currentState;
        }
    }
}