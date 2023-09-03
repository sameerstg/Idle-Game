using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NpcManager : MonoBehaviour
{
    public Npc npcPrefab;
    public List<Npc> npcs = new();
    internal static NpcManager _instance;
    public PlaceName currentNpcPlace;

    private void Awake()
    {
        _instance = this;
        //npcs = GetComponentsInChildren<Npc>().ToList();
    }
    public void CreateNpc()
    {
        //npcs.Add( Instantiate(npcPrefab, transform));
    }
    public void AdmitNpc(Npc npc)
    {

        npcs.Add(npc);
        npc.statemachine.SwitchState(new MovingState(npc, currentNpcPlace));
    }
    public void SendAllNpc(PlaceName place)
    {
        currentNpcPlace = place;
        StartCoroutine(SendAllNpcDelay());
    } 
    public IEnumerator SendAllNpcDelay()
    {
        foreach (var item in npcs)
        {
            item.statemachine.SwitchState(new MovingState(item, currentNpcPlace));
            yield return null;
        }
    }

}
