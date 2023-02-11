using UnityEngine;
using Unity.Netcode;

public class CameraController : NetworkBehaviour
{ // -- FK --
    [SerializeField] private Transform target = null; // target for the camera to Look At.
    [SerializeField] private Vector3 offset = new Vector3(); // Camera distance compared to the player
    [SerializeField] private float pitch = 2f; // Camera distance from player feet upwards

    private void LateUpdate()
    {
        FollowPlayer();

    }

    public override void OnNetworkSpawn()
    {
        SetTarget();
    }

    public void FollowPlayer()
    {
        transform.position = target.position - offset;
        transform.LookAt(target.position + Vector3.up *pitch);
    }

    public void SetTarget()
    {
        target = NetworkManager.LocalClient.PlayerObject.transform;
    }
}
