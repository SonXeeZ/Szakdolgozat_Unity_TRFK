using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemy
{
    //https://www.youtube.com/watch?v=HrNebvxSUsU&list=PLivfKP2ufIK6ToVMtpc_KTHlJRZjuE1z0&index=7&ab_channel=GameGrind TR -- 2023.03.24 15:50

    int ID { get; set; }
    void TakeDamage(int damage);
    void PerformAttack();
    void Die();
}
