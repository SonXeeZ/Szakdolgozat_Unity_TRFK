using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Networking.Transport;
using System;

//TR -2023.03.21. 17:55
//Mirror Udemy Course
public class Targeter : NetworkBehaviour
{
   /* [SerializeField] private Targetable target;

    #region Server

    [Command]
    public void CmdSetTarget(GameObject targetGameObject)
    {
        if (!targetGameObject.TryGetComponent<Targetable>(out Targetable newTarget)) { return; }

        target = newTarget;
    }

    [ServerRpc]
    public void ClearTarget()
    {
        target = null;
    }

    #endregion

    #region Client

    #endregion*/
}
