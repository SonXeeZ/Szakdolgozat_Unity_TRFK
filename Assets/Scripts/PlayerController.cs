
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;
using System;

[RequireComponent(typeof(NavMeshAgent))]
public class PlayerController : MonoBehaviour
{

    [SerializeField] private InputAction movement = new InputAction();
    [SerializeField] private LayerMask layerMask = new LayerMask();
    private NavMeshAgent agent = null;
    private Camera cam = null;

    private void Start()
    {
        cam = Camera.main;

        // Properties of the Player character
        agent = GetComponent<NavMeshAgent>();
        agent.speed = 5.5f;
        agent.acceleration = 999.9f;
    }
    private void OnEnable()
    {
        movement.Enable();
        Debug.Log("Movement's enabled.");
    }
    private void OnDisable()
    {
        movement.Disable();
        Debug.Log("Movement's disabled.");
    }
    private void Update()
    {
        HandleInput();
    }



    private void HandleInput()
    {
        if (movement.ReadValue<float>() == 1)
        {
            Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
            Debug.Log("Mouse position's read.");
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 1000,layerMask))
            {
                Debug.Log("Trying to move player to destination if under MaxDistance.");
                PlayerMove(hit.point);
                Debug.Log("Moving to destination.");
            }
        }
    }

    private void PlayerMove(Vector3 locationToGoWithPlayer)
    {
        agent.SetDestination(locationToGoWithPlayer);
    }
}
