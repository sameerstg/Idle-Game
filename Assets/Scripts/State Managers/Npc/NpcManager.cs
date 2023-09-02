using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NpcManager : MonoBehaviour
{
    public Npc npcPrefab;
    public List<Npc> npcs = new();
    internal static NpcManager _instance;

    private void Awake()
    {
        _instance = this;
        npcs = GetComponentsInChildren<Npc>().ToList();
    }
    public void CreateNpc()
    {
       npcs.Add( Instantiate(npcPrefab, transform));
    }
    public void SendNpc(PlaceName place)
    {
        StartCoroutine(SendNpcDelay(place));
    } 
    public IEnumerator SendNpcDelay(PlaceName place)
    {
        foreach (var item in npcs)
        {
            item.statemachine.SwitchState(new MovingState(item, place));
            yield return null;
        }
    }

}
