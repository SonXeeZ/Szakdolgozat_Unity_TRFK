using Unity.Netcode;
using UnityEngine;

public class ClimbingSystem : NetworkBehaviour,IClimbing
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Vector3 climbPositionTo;
    [SerializeField] private Vector3 climbPositionFrom;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private LayerMask teleportLayer;

    public Vector3 ClimbPositionTo { get => climbPositionTo; set => climbPositionTo = value; }
    public Vector3 ClimbPositionFrom { get => climbPositionFrom; set => climbPositionFrom = value; }
    public Camera PlayerCamera { get => playerCamera; set => playerCamera = value; }
    public LayerMask TeleportLayer { get => teleportLayer; set => teleportLayer = value; }

    public void Climb(ClimbObject climbObject)
    {
        if(IsServer){
            playerTransform.position = climbObject.ClimbPositionTo;
        }
        if(IsOwner && IsLocalPlayer){
            climbPositionTo = climbObject.ClimbPositionTo;
            climbPositionFrom = climbObject.ClimbPositionFrom;

            RequestClimbingServerRpc(OwnerClientId, climbPositionFrom, climbPositionTo);
        }
    }

    [ServerRpc]
    public void RequestClimbingServerRpc(ulong clientId, Vector3 climbPositionFrom, Vector3 climbPositionTo)
    {
        var playerObject = NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject;
        var currentPosition = playerObject.GetComponent<Transform>().position;
        var playerClimbingSystemTransform = playerObject.GetComponent<ClimbingSystem>().playerTransform;
        if(Vector3.Distance(currentPosition, climbPositionFrom) < 3.5f){
            Debug.Log("Climbing to position");
            //playerClimbingSystemTransform.position = climbPositionTo;
            RequestClimbingClientRpc(clientId);
        }
    }
    [ClientRpc]
    private void RequestClimbingClientRpc(ulong clientId){
        if(OwnerClientId == clientId){
            playerTransform.position = climbPositionTo;
        }
    }

    [ServerRpc]
    private void SetPlayerTransformServerRpc(ulong clientId){
        var playerobject = NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject;
        var playerClimbingSystem = playerobject.GetComponent<ClimbingSystem>();
        playerClimbingSystem.playerTransform = playerobject.GetComponent<Transform>();
    }

    [ServerRpc]
    private void SetPlayerCameraServerRpc(ulong clientId){
        var playerObject = NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject;
        var playerClimbingSystem = playerObject.GetComponent<ClimbingSystem>();
        playerClimbingSystem.PlayerCamera = playerObject.GetComponentInChildren<Camera>();
    }

    void Start()
    {
        if(IsOwner){
            //SetPlayerTransformServerRpc(OwnerClientId);
            //SetPlayerCameraServerRpc(OwnerClientId);
            playerTransform = GetComponent<Transform>();
            playerCamera = GetComponentInChildren<Camera>();
            
        }
        
    }

    void Update()
    {
        
    }
}
