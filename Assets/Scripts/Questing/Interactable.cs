using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Interactable : MonoBehaviour
{
     [HideInInspector]
   // public UnityEngine.AI.NavMeshAgent playerAgent;
    private bool hasInteracted;
    bool isEnemy;
    private Transform playerTransform;

    public virtual void MoveToInteraction(Transform transform)
    {
        isEnemy = gameObject.tag == "Enemy";
        //hasInteracted = false;
        playerTransform = transform;
        Debug.Log(playerTransform.GetComponent<Transform>());
        //this.playerAgent = playerAgent;
        //playerAgent.stoppingDistance = 3f;
        //playerAgent.destination = GetTargetPosition();
        //EnsureLookDirection();
    }

    void Update()
    {
        float distance = Vector3.Distance(playerTransform.position, transform.position);
        if (!hasInteracted && distance <= 3f && Input.GetKeyDown(KeyCode.Mouse1))
        {

            //playerAgent.destination = GetTargetPosition();
            //TODO: Make sure the player is facing the NPC
            //EnsureLookDirection();
            /*if (playerAgent.remainingDistance <= playerAgent.stoppingDistance)
            {
                
            }*/
            Debug.Log("Interactable.cs update");
            if (!isEnemy){
                hasInteracted = true;
                Interact();
            }
        }
        else if (distance > 3f)
        {
            hasInteracted = false;
        }
    }

    /*void EnsureLookDirection()
    {
        playerAgent.updateRotation = false;
        Vector3 lookDirection = new Vector3(transform.position.x, playerAgent.transform.position.y, transform.position.z);
        playerAgent.transform.LookAt(lookDirection);
        playerAgent.updateRotation = true;
    }*/

    public virtual void Interact()
    {
        Debug.Log("Interacting with base class.");
    }

    private Vector3 GetTargetPosition()
    {
        return transform.position;
    }
}
