using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;
using UnityEngine.Events;

public class ScoreAdmin : MonoBehaviourPun , IPunObservable
{
    public int redTeamCount = 5;
    public int blueTeamCount = 5;

    public UnityEvent OnScoreAdminLoad;

    public void Awake()
    {
        OnScoreAdminLoad?.Invoke();
    }

    public void RedTeamCounting()
    {
        photonView.RPC("RPCRedTeamCounting", RpcTarget.Others);
    }

    [PunRPC]
    void RPCRedTeamCounting()
    {
        if (!photonView.IsMine) return;
        redTeamCount--;
    }

    public void BlueTeamCounting()
    {
        photonView.RPC("RPCBlueTeamCounting", RpcTarget.Others);
    }

    [PunRPC]
    void RPCBlueTeamCounting()
    {
        if (!photonView.IsMine) return;
        blueTeamCount--;
    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(redTeamCount);
            stream.SendNext(blueTeamCount);
        }
        else
        {
            redTeamCount = (int)stream.ReceiveNext();
            blueTeamCount = (int)stream.ReceiveNext();
        }
    }
}
