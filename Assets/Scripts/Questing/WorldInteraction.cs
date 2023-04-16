using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldInteraction : MonoBehaviour
{
    private Transform playerTransform;
    //UnityEngine.AI.NavMeshAgent playerAgent;
    [SerializeField]
    private LayerMask interactLayer;

    [SerializeField] private Camera cam;

    void Start()
    {
        //playerAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        playerTransform =  GetComponent<Transform>();
        cam = transform.GetComponentInChildren<Camera>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            Debug.Log("mouse down");
            GetInteraction();

        Debug.DrawRay(transform.position, transform.forward * 5f, Color.red);
    }

    void GetInteraction()
    {
        Ray interactionRay = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit interactionInfo;
        if (Physics.SphereCast(interactionRay, 1f, out interactionInfo, Mathf.Infinity, interactLayer))
        {
            //playerAgent.updateRotation = true;
            GameObject interactedObject = interactionInfo.collider.gameObject;
            if (interactedObject.tag == "Enemy")
            {
                Debug.Log("move to enemy");
                //interactedObject.GetComponent<Interactable>().MoveToInteraction(transform);
            }
            else if (interactedObject.tag == "Interactable Object")
                interactedObject.GetComponent<Interactable>().MoveToInteraction(playerTransform);
                Debug.Log("move to interactable object");
            /*else
            {
                //playerAgent.stoppingDistance = 0;
                //playerAgent.destination = interactionInfo.point;
                Debug.Log("move to point");
            }*/
        }
    }
}
