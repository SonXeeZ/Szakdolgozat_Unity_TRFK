using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform target = null; // target for the camera to Look At.
    [SerializeField] private Vector3 offset = new Vector3(); // Camera distance compared to the player
    [SerializeField] private float pitch = 2f; // Camera distance from player feet upwards

    private void LateUpdate()
    {
        transform.position = target.position - offset;
        transform.LookAt(target.position + Vector3.up *pitch);

    }
}
