
[System.Serializable]

public class SleepingGameState : GameState
{
    public SleepingGameState() : base()
    {
        gameStateName = GameStateName.Sleeping;
        placeName = PlaceName.Cell;
        
    }
}
