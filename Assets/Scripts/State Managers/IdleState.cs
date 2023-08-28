using UnityEngine;

public class IdleState : State
{
    public IdleState(Npc npc) : base(npc)
    {
    }

    public override  void Enter()
    {
        Debug.Log("Idle state");
        Place placeToGo = null;
        Debug.Log(npc.togoPlace.placeName);
        switch (npc.togoPlace.placeName)
        {
            case PlaceName.OuterEntrance:
                placeToGo = PathManager._instance.GetPlace(PlaceName.Entrance);
                break;
            case PlaceName.Entrance:
                placeToGo = PathManager._instance.GetPlace(PlaceName.Cell);
                break;
            case PlaceName.Cell:
                break;
            case PlaceName.Entertainment:
                break;
            case PlaceName.FoodRoom:
                break;
            case PlaceName.Bathroom:
                break;
            case PlaceName.FoodPrepartaionRoom:
                break;
            case PlaceName.ElectricSupply:
                break;
            default:
                break;
        }
        if (placeToGo != null)
        {
            npc.Move(placeToGo);
        }
        else
        {
            Debug.Log("place empty");
        }
    }
    public override void Exit()
    {
       
    }

  
}
