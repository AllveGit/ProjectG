using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Launcher : MonoBehaviourPunCallbacks
{
    enum AutoMatchType
    {
        AutoMatch_None = 0,
        AutoMatch_1vs1 = 2,
        AutoMatch_2vs2 = 4,
        AutoMatch_3vs3 = 6,
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

    private AutoMatchType currentMatchType = AutoMatchType.AutoMatch_None; 

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
        Debug.Log("매칭 시작!");
        onMatching = true;
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("매칭 실패!");

        ExitGames.Client.Photon.Hashtable roomProperty
            = new ExitGames.Client.Photon.Hashtable() { { "MT", currentMatchType } };

        RoomOptions roomOption = new RoomOptions();
        roomOption.MaxPlayers = (byte)currentMatchType;
        roomOption.CustomRoomProperties = roomProperty;
        roomOption.CustomRoomPropertiesForLobby = new string[] {"MT"};

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
            else
            {

            }
        }
    }
    public override void OnLeftRoom()
    {
        onMatching = false;
    }

    #endregion

    public void Matching1vs1()
    {
        currentMatchType = AutoMatchType.AutoMatch_1vs1;
        Matching();
    }

    public void Matching2vs2()
    {
        currentMatchType = AutoMatchType.AutoMatch_2vs2;
        Matching();
    }

    public void Matching3vs3()
    {
        currentMatchType = AutoMatchType.AutoMatch_3vs3;
        Matching();
    }

    private void Matching()
    {
        if (currentMatchType == AutoMatchType.AutoMatch_None)
            return;

        progressLabel.SetActive(true);
        controlPanel.SetActive(false);

        ExitGames.Client.Photon.Hashtable roomProperty
            = new ExitGames.Client.Photon.Hashtable() { { "MT", currentMatchType} };

        PhotonNetwork.JoinRandomRoom(roomProperty, 0);
    }

}
