using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;

public class ScoreAdmin : MonoBehaviourPun , IPunObservable
{
 
    public int redTeamCount = 5;
    public int bludTeamCount = 5;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(redTeamCount);
            stream.SendNext(bludTeamCount);
        }
        else
        {
            redTeamCount = (int)stream.ReceiveNext();
            bludTeamCount = (int)stream.ReceiveNext();
        }
    }
}
