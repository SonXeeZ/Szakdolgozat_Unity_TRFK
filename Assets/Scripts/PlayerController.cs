
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;
using System;
using Unity.Netcode;
using Unity.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class PlayerController : NetworkBehaviour
{
    [SerializeField] private Transform spawnedObjectPrefab;
    [SerializeField] private InputAction movement = new InputAction();
    [SerializeField] private LayerMask layerMask = new LayerMask();
    private NavMeshAgent agent = null;
    private Camera cam = null;
    private Transform spawnedObjectTransform;

    private NetworkVariable<MyCustomData> randomNumber = new NetworkVariable<MyCustomData>(
        new MyCustomData
        {
            _int = 56,
            _bool = true,
        },
        NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public struct MyCustomData : INetworkSerializable
    {
        public int _int;
        public bool _bool;
        public FixedString128Bytes message;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref _int);
            serializer.SerializeValue(ref _bool);
            serializer.SerializeValue(ref message);


        }
    }

    public override void OnNetworkSpawn()
    {
        randomNumber.OnValueChanged += (MyCustomData previousValue, MyCustomData newValue) =>
        {
            Debug.Log(OwnerClientId + "; " + newValue._int + "; " + newValue._bool + "; " + newValue.message);
        };

        // set the camera to the player after spawning
        
        Camera.main.GetComponent<CameraController>().SetTarget();
    }

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
        if (!IsOwner) return;

        if (Input.GetKeyDown(KeyCode.T))
        {
            spawnedObjectTransform = Instantiate(spawnedObjectPrefab);
            spawnedObjectTransform.GetComponent<NetworkObject>().Spawn(true);
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            Destroy(spawnedObjectTransform.gameObject);
        }

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
