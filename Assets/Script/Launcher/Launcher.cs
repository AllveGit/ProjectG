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
    private string gameVersion = "1.0";
    public bool IsConnected { get; private set; } = false;

    private void Awake()
    {
        // 자동 동기화.
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    void Start()
    {
    }

    public void MatchingDebug()
    {
        PlayerManager.Instance.CurrentMatchType = MatchOption.Match_Debug;
        PhotonNetwork.JoinRandomRoom(null, 0);

        PhotonNetwork.LocalPlayer.SetCustomProperties(
            new PhotonHashTable()
            {
                { PlayerProperties.TEAM.ToString(), TeamOption.Solo },
                { PlayerProperties.SPAWNPOS.ToString(), 0 } ,
                { PlayerProperties.CHARACTER.ToString(), PlayerManager.Instance.CharacterType.ToString()}
            });
    }

    public void MatchingStart(MatchOption matchType)
    {
        PlayerManager.Instance.CurrentMatchType = matchType;

        if (matchType == MatchOption.Match_Debug)
        {
            MatchingDebug();
            return;
        }

        if (PlayerManager.Instance.CurrentMatchType == MatchOption.Match_None)
            return;
        
        PhotonHashTable roomProperty
            = new PhotonHashTable() { { RoomPropoerties.MATCHTYPE.ToString(), PlayerManager.Instance.CurrentMatchType } };
        
        PhotonNetwork.JoinRandomRoom(roomProperty, 0);
    }

    public void MatchingCancel()
    {
        PhotonNetwork.LeaveRoom();
    }

    private RoomOptions CreateMatchingRoomOption(MatchOption type)
    {
        PhotonHashTable roomProperty = null;
        RoomOptions roomOption = new RoomOptions();

        roomProperty = new PhotonHashTable() { { RoomPropoerties.MATCHTYPE.ToString(), PlayerManager.Instance.CurrentMatchType } };

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

        playerProperties.Add(PlayerProperties.CHARACTER.ToString(), PlayerManager.Instance.CharacterType.ToString());

        return playerProperties;
    }


}

/*
 * 포톤 네트워크의 콜백함수들입니다.
 */
public partial class Launcher : MonoBehaviourPunCallbacks
{
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarningFormat("서버와의 연결이 종료되었습니다.", cause);
    }

    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.IsMasterClient && !PlayerManager.Instance.CurrentMatchType.Equals(MatchOption.Match_Debug))
        {
            TeamManager.Instance.ClearTeamMemberCount();

            TeamOption teamOption = TeamManager.Instance.CollocateTeam();
            TeamManager.Instance.AddTeamMember(teamOption);

            PhotonHashTable properties = CreatePlayerProperties(teamOption);
            PhotonNetwork.LocalPlayer.SetCustomProperties(properties);
        }
        else if (PhotonNetwork.IsMasterClient && PlayerManager.Instance.CurrentMatchType == MatchOption.Match_Debug)
        {
            PhotonNetwork.LoadLevel("Match");
            return;
        }

        Debug.Log("매칭 시작!");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("매칭 실패!");

        PhotonNetwork.CreateRoom(null, CreateMatchingRoomOption(PlayerManager.Instance.CurrentMatchType));
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (PhotonNetwork.IsMasterClient && !PlayerManager.Instance.CurrentMatchType.Equals(MatchOption.Match_Debug))
        {
            TeamOption playerTeam = TeamManager.Instance.CollocateTeam();
            TeamManager.Instance.AddTeamMember(playerTeam);

            newPlayer.SetCustomProperties(CreatePlayerProperties(playerTeam));

            if (PhotonNetwork.CurrentRoom.PlayerCount == (int)PlayerManager.Instance.CurrentMatchType)
            {
                Debug.Log("매칭이 완료되어 게임을 시작합니다.");
                PhotonNetwork.LoadLevel("Match");
            }
        }
    }
}

