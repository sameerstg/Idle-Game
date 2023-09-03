using UnityEngine;
[System.Serializable]
public class PrisonersEntryState : GameState
{

    public PrisonersEntryState() : base()
    {
        gameStateName = GameStateName.PrisonersEntry;
        placeName = PlaceName.Cell;
        Debug.Log(placeName);
    }

   



}
