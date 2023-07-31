
[System.Serializable]

public class SleepingGameState : GameState
{
    public SleepingGameState(float periodOfState, OnStateComplete onStateCompleteCallback) : base(periodOfState, onStateCompleteCallback)
    {
    }
}
