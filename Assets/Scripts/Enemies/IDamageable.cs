using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public interface IDamageable
{
    void Damage(uint attackerID, int value);
}
