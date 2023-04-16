using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class EnemySpawner : NetworkBehaviour
{
    /*public GameObject enemyPrefab;
    private bool hasSpawned = false;

    public override void OnStartServer()
    {
        if (!hasSpawned)
        {
            GameObject enemy = Instantiate(enemyPrefab, transform.position, transform.rotation);
            NetworkServer.Spawn(enemy);
            hasSpawned = true;
        }
    }

    public void DespawnEnemy()
    {
        GameObject enemy = GameObject.FindGameObjectWithTag("Enemy");
        if (enemy != null)
        {
            NetworkServer.Destroy(enemy);
            hasSpawned = false;
        }
    }*/
}
