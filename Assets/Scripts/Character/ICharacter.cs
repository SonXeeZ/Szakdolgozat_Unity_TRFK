using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICharacter
{
    void DealDamage(int damage, Collision collision);
    void DealDamageServerRpc(ulong targetClientId, int damage);
    void TakeDamage(int damage);
    void TakeDamageClientRpc(int damage);
    void Die();
}
