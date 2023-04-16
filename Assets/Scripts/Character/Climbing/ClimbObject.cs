using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ClimbObject : NetworkBehaviour
{
    private Transform portalTransform;
    public Vector3 ClimbPositionTo { get; set; }
    public Vector3 ClimbPositionFrom { get; set; }

    private void Awake(){
        portalTransform = GetComponent<Transform>();
        ClimbPositionFrom = portalTransform.position;
        ClimbPositionTo = transform.GetChild(0).position;
    }

}
