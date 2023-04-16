using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class Mob : NetworkBehaviour
{
    [SerializeField] private float destinationCooldownWithTarget = 1f;
    [SerializeField] private float destinationCooldownWithoutTarget = 3f;
        
    private float findDestinationCooldown;
    private Transform playerTransform; // reference to the player's Transform
    public Transform targetTransform;
    private NavMeshAgent agent;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        agent = GetComponent<NavMeshAgent>();
        playerTransform = GameObject.Find("PlayerPrefab (1)(Clone)").transform; // find the player object by name
    }

    private void Update()
    {
        if (findDestinationCooldown > 0)
            findDestinationCooldown -= Time.deltaTime;
        else
            FindDestination();

        if (playerTransform) // update targetTransform to follow the player's Transform
        {
            targetTransform = playerTransform;
        }

        if (targetTransform)
        {
            agent.SetDestination(targetTransform.position);
        }
    }

    public void FindDestination()
    {
        if (targetTransform)
        {
            Vector3 direction = (transform.position - targetTransform.position).normalized;
            Vector3 destination = targetTransform.position - direction + (direction * 2f);
            agent.SetDestination(destination);
            findDestinationCooldown = destinationCooldownWithTarget;
        }
        else
        {
            Vector3 position = transform.position;
            Vector3 destination = new Vector3(position.x + Random.Range(-20, 20), 1, position.z + Random.Range(-20, 20));
            agent.SetDestination(destination);
            findDestinationCooldown = destinationCooldownWithoutTarget;
        }
    }
}
