[System.Serializable]


public class WorkingGameState : GameState
{
    public WorkingGameState(float periodOfState, OnStateComplete onStateCompleteCallback) : base(periodOfState, onStateCompleteCallback)
    {
    }
}
