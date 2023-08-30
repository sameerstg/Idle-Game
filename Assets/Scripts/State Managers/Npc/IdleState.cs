using UnityEngine;

public class IdleState : State
{
    public IdleState(Npc npc) : base(npc)
    {
    }

    public override  void Enter()
    {
        base.Enter();
        Place placeToGo = null;
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
            Debug.Log($"going to place {placeToGo}");
            npc.statemachine.SwitchState(new MovingState(npc, placeToGo));
        }
        else
        {
            Debug.Log("place empty");
        }
    }  
}
