using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

using TeamOption = Enums.TeamOption;

using MatchOption = Enums.MatchType;

using PlayerProperties = Enums.PlayerProperties;

using RoomPropoerties = Enums.RoomProperties;

using PhotonHashTable = ExitGames.Client.Photon.Hashtable;

public partial class Launcher : MonoBehaviourPunCallbacks
{

    private string gameVersion = "1";
    private bool onMatching = false;

    [Tooltip("InputFiled와 플레이 버튼이 들어가있는 Panel 입니다.")]
    [SerializeField]
    private GameObject controlPanel = null;

    [Tooltip("TouchToScreen Object 입니다")]
    [SerializeField]
    private GameObject touchToScreen = null;

    [Tooltip("Connecting 중임을 알리는 Text UI 입니다.")]
    [SerializeField]
    private GameObject progressLabel = null;

    [Tooltip("플레이어 선택 UI")]
    [SerializeField]
    private GameObject characterSeleter = null;

    private MatchOption currentMatchType = MatchOption.Match_None;

    public bool IsConnected { get; private set; } = false;

    private void Awake()
    {
        // 자동 동기화.
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    void Start()
    {
        controlPanel.SetActive(false);
        touchToScreen.SetActive(true);
        characterSeleter.SetActive(false);
    }

    public void Connect()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.GameVersion = gameVersion;
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public void Matching1vs1()
    {
        currentMatchType = MatchOption.Match_1vs1;
        Matching();
    }

    public void Matching2vs2()
    {
        currentMatchType = MatchOption.Match_2vs2;
        Matching();
    }

    public void Matching3vs3()
    {
        currentMatchType = MatchOption.Match_3vs3;
        Matching();
    }


    public void MatchingDebug()
    {
        controlPanel.SetActive(true);
        touchToScreen.SetActive(false);

        currentMatchType = MatchOption.Match_Debug;
        PhotonNetwork.JoinRandomRoom(null, 0);

        PhotonNetwork.LocalPlayer.SetCustomProperties(new PhotonHashTable() { { PlayerProperties.TEAM.ToString(), TeamOption.Solo },
            { PlayerProperties.SPAWNPOS.ToString(), 0 } });
    }


    private void Matching()
    {
        if (currentMatchType == MatchOption.Match_None)
            return;

        progressLabel.SetActive(true);
        controlPanel.SetActive(false);

        PhotonHashTable roomProperty
            = new PhotonHashTable() { { RoomPropoerties.MATCHTYPE.ToString(), currentMatchType } };

        PhotonNetwork.JoinRandomRoom(roomProperty, 0);
    }


    private RoomOptions CreateMatchingRoomOption(MatchOption type)
    {
        PhotonHashTable roomProperty = null;
        RoomOptions roomOption = new RoomOptions();

        roomProperty = new PhotonHashTable() { { RoomPropoerties.MATCHTYPE.ToString(), currentMatchType } };

        if (type.Equals(MatchOption.Match_Debug)) roomOption.MaxPlayers = 0;
        else roomOption.MaxPlayers = (byte)type;

        roomOption.CustomRoomProperties = roomProperty;
        roomOption.CustomRoomPropertiesForLobby = new string[] { RoomPropoerties.MATCHTYPE.ToString() };

        return roomOption;
    }

    private PhotonHashTable CreatePlayerProperties(TeamOption playerTeam)
    {
        PhotonHashTable playerProperties = new PhotonHashTable();

        playerProperties.Add(PlayerProperties.TEAM.ToString(), playerTeam);

        if (playerTeam.Equals(TeamOption.BlueTeam))
            playerProperties.Add(PlayerProperties.SPAWNPOS.ToString(), TeamManager.Instance.BlueTeamCount);
        else if (playerTeam.Equals(TeamOption.RedTeam))
            playerProperties.Add(PlayerProperties.SPAWNPOS.ToString(), 3 + TeamManager.Instance.RedTeamCount);
        else
            playerProperties.Add(PlayerProperties.SPAWNPOS.ToString(), 0);

        return playerProperties;
    }


}

/*
 * 포톤 네트워크의 콜백함수들입니다.
 */
public partial class Launcher : MonoBehaviourPunCallbacks
{
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

        touchToScreen.SetActive(false);
        controlPanel.SetActive(true);
        characterSeleter.SetActive(true);
    }

    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.IsMasterClient && !currentMatchType.Equals(MatchOption.Match_Debug))
        {
            TeamManager.Instance.ClearTeamMemberCount();

            TeamOption teamOption = TeamManager.Instance.CollocateTeam();
            TeamManager.Instance.AddTeamMember(teamOption);

            PhotonHashTable properties = CreatePlayerProperties(teamOption);
            PhotonNetwork.LocalPlayer.SetCustomProperties(properties);
        }
        else if (PhotonNetwork.IsMasterClient && currentMatchType == MatchOption.Match_Debug)
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

        PhotonNetwork.CreateRoom(null, CreateMatchingRoomOption(currentMatchType));
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (PhotonNetwork.IsMasterClient && !currentMatchType.Equals(MatchOption.Match_Debug))
        {
            TeamOption playerTeam = TeamManager.Instance.CollocateTeam();
            TeamManager.Instance.AddTeamMember(playerTeam);

            newPlayer.SetCustomProperties(CreatePlayerProperties(playerTeam));

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

}

