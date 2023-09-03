using UnityEngine;

public class IdleState : State
{
    public IdleState(Npc npc) : base(npc)
    {
    }

    public override  void Enter()
    {
        base.Enter();
        if (npc.togoPlace == null)
        {
            npc.statemachine.SwitchState(new MovingState(npc, PlaceName.Entrance));
        }
        //PlaceName placeToGoName = PlaceName.None;
        //switch (npc.togoPlace.placeName)
        //{
        //    case PlaceName.OuterEntrance:
        //        placeToGoName = PlaceName.Entrance;
        //        break;
        //    case PlaceName.Entrance:
        //        placeToGoName = PlaceName.Cell;
        //        break;
        //    case PlaceName.Cell:
        //        break;
        //    case PlaceName.Entertainment:
        //        break;
        //    case PlaceName.FoodRoom:
        //        break;
        //    case PlaceName.Bathroom:
        //        break;
        //    case PlaceName.FoodPrepartaionRoom:
        //        break;
        //    case PlaceName.ElectricSupply:
        //        break;
        //    default:
        //        break;
        //}
        //if (placeToGoName != PlaceName.None)
        //{
        //    Debug.Log($"going to place {placeToGoName}");
        //    npc.statemachine.SwitchState(new MovingState(npc, placeToGoName));
        //}
        //else
        //{
        //    Debug.Log("place empty");
        //}
    }  
}
