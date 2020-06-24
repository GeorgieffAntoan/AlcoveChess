using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;

public class ChangeOwner : Photon.PunBehaviour
{
    bool a = true;
 
    public void Click()
    {
        if (!photonView.isMine && !PhotonNetwork.isMasterClient)  
        base.photonView.RequestOwnership();
    }

    public override void OnOwnershipRequest (object[] viewAndPlayer)
    {
        PhotonView view = viewAndPlayer[0] as PhotonView;
        PhotonPlayer requestingPlayer = viewAndPlayer[1] as PhotonPlayer;

        base.photonView.TransferOwnership(requestingPlayer);
    }
}
