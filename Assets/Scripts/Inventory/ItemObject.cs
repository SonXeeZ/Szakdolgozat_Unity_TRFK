using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ItemObject : NetworkBehaviour
{
    public Items itemStats;
    public int amount;
}
