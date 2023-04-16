using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Slime : NetworkBehaviour, IEnemy
{
    //https://www.youtube.com/watch?v=HrNebvxSUsU&list=PLivfKP2ufIK6ToVMtpc_KTHlJRZjuE1z0&index=7&ab_channel=GameGrind TR -- 2023.03.24 15:50
   
    [SerializeField] private NetworkVariable<int> enemyMaxHealth = new NetworkVariable<int>();
    public NetworkVariable<int> EnemyMaxHealth { get => enemyMaxHealth ; set => enemyMaxHealth = value; }

    [SerializeField] private NetworkVariable<int> enemyCurrentHealth = new NetworkVariable<int>();
    public NetworkVariable<int> EnemyCurrentHealth { get => enemyCurrentHealth ; set => enemyCurrentHealth = value; }

    [SerializeField] private NetworkVariable<int> enemyPower = new NetworkVariable<int>();
    public NetworkVariable<int> EnemyPower { get => enemyPower ; set => enemyPower = value; }

    public int ID { get; set; }

    public Transform slimeTransform { get; set; }

    
    void Start()
    {
        EnemyMaxHealth.Value = 100;
        EnemyCurrentHealth.Value = EnemyMaxHealth.Value;
        ID = 0;
    }
    public void PerformAttack()
    {
        throw new System.NotImplementedException();
    }

    public void TakeDamage(int damage)
    {
        /*enemyCurrentHealth -= damage;
        if(enemyCurrentHealth <= 0){
            Die();
        }*/
        Debug.Log("Slime took damage");
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
