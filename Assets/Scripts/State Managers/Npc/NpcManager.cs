using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NpcManager : MonoBehaviour
{
    public Npc npc;
    public List<Npc> npcs = new();
    private void Awake()
    {
        npcs = GetComponentsInChildren<Npc>().ToList();
    }
    public void CreateNpc()
    {
       npcs.Add( Instantiate(npc, transform));
    }
}
