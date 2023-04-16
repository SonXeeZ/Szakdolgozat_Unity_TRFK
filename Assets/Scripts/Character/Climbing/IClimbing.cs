using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IClimbing
{
    Camera PlayerCamera { get; set; }
    Vector3 ClimbPositionTo { get; set; }
    Vector3 ClimbPositionFrom { get; set; }
    LayerMask TeleportLayer { get; set; }
    void Climb(ClimbObject climbObject);
    void RequestClimbingServerRpc(ulong clientId, Vector3 climbPositionFrom,Vector3 climbPositionTo);

}
