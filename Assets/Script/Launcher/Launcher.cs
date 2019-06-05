using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Launcher : MonoBehaviourPunCallbacks
{
    enum MatchType
    {
        Match_None = 0,
        Match_Debug = 1,
        Match_1vs1 = 2,
        Match_2vs2 = 4,
        Match_3vs3 = 6,
    }


    private string gameVersion = "1";
    private bool onMatching = false;

    [Tooltip("InputFiled와 플레이 버튼이 들어가있는 Panel 입니다.")]
    [SerializeField]
    private GameObject controlPanel = null;

    [Tooltip("Connecting 중임을 알리는 Text UI 입니다.")]
    [SerializeField]
    private GameObject progressLabel = null;

    public bool IsConnected { get; private set; } = false;

    private MatchType currentMatchType = MatchType.Match_None; 

    private void Awake()
    {
        // 자동 동기화.
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    void Start()
    {
        progressLabel.SetActive(true);
        controlPanel.SetActive(false);

        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.GameVersion = gameVersion;
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public void Matching1vs1()
    {
        currentMatchType = MatchType.Match_1vs1;
        Matching();
    }

    public void Matching2vs2()
    {
        currentMatchType = MatchType.Match_2vs2;
        Matching();
    }

    public void Matching3vs3()
    {
        currentMatchType = MatchType.Match_3vs3;
        Matching();
    }


    public void MatchingDebug()
    {
        progressLabel.SetActive(true);
        controlPanel.SetActive(false);

        currentMatchType = MatchType.Match_Debug;
        PhotonNetwork.JoinRandomRoom(null, 0);
    }


    private void Matching()
    {
        if (currentMatchType == MatchType.Match_None)
            return;

        progressLabel.SetActive(true);
        controlPanel.SetActive(false);

        ExitGames.Client.Photon.Hashtable roomProperty
            = new ExitGames.Client.Photon.Hashtable() { { "MT", currentMatchType } };

        PhotonNetwork.JoinRandomRoom(roomProperty, 0);
    }

    #region PhotonCallBack
    public override void OnConnectedToMaster()
    {
        Debug.Log("마스터 서버에 연결되었습니다.");
        PhotonNetwork.JoinLobby();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarningFormat("서버와의 연결이 종료되었습니다.", cause);
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("로비에 연결되었습니다.");

        progressLabel.SetActive(false);
        controlPanel.SetActive(true);
    }

    public override void OnJoinedRoom()
    {
        if (Photon.Pun.PhotonNetwork.IsMasterClient)
            TeamManager.Instance.

        if(currentMatchType == MatchType.Match_Debug)
        {
            PhotonNetwork.LoadLevel("Match");
            return;
        }

        Debug.Log("매칭 시작!");
        onMatching = true;
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("매칭 실패!");

        ExitGames.Client.Photon.Hashtable roomProperty = null;
        RoomOptions roomOption = new RoomOptions();

        roomProperty = new ExitGames.Client.Photon.Hashtable() { { "MT", currentMatchType } };

        if (currentMatchType == MatchType.Match_Debug)
            roomOption.MaxPlayers = 0;
        else
            roomOption.MaxPlayers = (byte)currentMatchType;
    
        roomOption.CustomRoomProperties = roomProperty;
        roomOption.CustomRoomPropertiesForLobby = new string[] { "MT" };

        PhotonNetwork.CreateRoom(null, roomOption);
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            
            if (PhotonNetwork.CurrentRoom.PlayerCount == (int)currentMatchType)
            {
                Debug.Log("매칭이 완료되어 게임을 시작합니다.");
                PhotonNetwork.LoadLevel("Match");
            }
        }
    }
    public override void OnLeftRoom()
    {
        onMatching = false;
    }

    #endregion

}
