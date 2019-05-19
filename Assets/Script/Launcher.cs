using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Launcher : MonoBehaviourPunCallbacks
{
    /// <summary>
    /// Game Verision 변수입니다.
    /// </summary>
    string strGameVersion = "1";

    [Tooltip("한 방당 최대로 접속할 수 있는 유저의 숫자입니다.")]
    [SerializeField]
    private byte maxPlayersPerRooom = 4;

    [Tooltip("InputFiled와 플레이 버튼이 들어가있는 Panel 입니다.")]
    [SerializeField]

    private GameObject controlPanel = null;
    [Tooltip("Connecting 중임을 알리는 Text UI 입니다.")]
    [SerializeField]
    private GameObject progressLabel = null;

    /// <summary>
    /// 사용자가 현재 접속이 진행중인지 알 수 있는 플래그입니다.
    /// </summary>
    public bool isConnected { get; private set; } = false;


    private void Awake()
    {
        // 자동 동기화.
        PhotonNetwork.AutomaticallySyncScene = true;
    
     }

    void Start()
    {
        progressLabel.SetActive(false);
        controlPanel.SetActive(true);

    }


    /// <summary>
    /// 포톤 클라우드와 연결되는 시작점입니다.
    /// </summary>
    public void Connect()
    {
        isConnected = true;

        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            PhotonNetwork.GameVersion = strGameVersion;
            PhotonNetwork.ConnectUsingSettings();
        }

        progressLabel.SetActive(true);
        controlPanel.SetActive(false);
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("마스터 서버에 연결되었습니다.");

        if (isConnected)
            PhotonNetwork.JoinRandomRoom();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarningFormat("PUN Basics Tutorial/Launcher: OnDisconnected() was called by PUN with reason {0}", cause);

        progressLabel.SetActive(false);
        controlPanel.SetActive(true);
    }

    // Room 참가에 실패했을시 호출되는 Callback함수입니다.
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("랜덤 방 참가에 실패하여 OnJoinRandomRoom 호출되었습니다.");

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = maxPlayersPerRooom;
        PhotonNetwork.CreateRoom(null, roomOptions);
    }
    public override void OnJoinedRoom()
    {
        Debug.Log("랜덤 방 참가에 성공하여 OnJoinedRoom 호출되었습니다.");

        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            Debug.Log("1인용 맵을 로드합니다.");
            PhotonNetwork.LoadLevel("RoomFor1");
        }
    }

}
