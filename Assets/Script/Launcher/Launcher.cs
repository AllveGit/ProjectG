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
    public bool IsConnected { get; private set; } = false;

    private void Awake()
    {
        // 자동 동기화.
        PhotonNetwork.AutomaticallySyncScene = true;

        SoundManager.instance.PlayMusic(Resources.Load<AudioClip>("Sounds/happy"));
    }

    // [System.Diagnostics.Conditional("UNITY_DEBUG")]
    public void MatchingDebug()
    {
        PhotonNetwork.LocalPlayer.SetCustomProperties(
            new PhotonHashTable()
            {
                { PlayerProperties.TEAM.ToString(), TeamOption.Solo },
                { PlayerProperties.SPAWNPOS.ToString(), 0 } ,
                { PlayerProperties.CHARACTER.ToString(), PlayerManager.Instance.CharacterType.ToString()}
            });

        PhotonNetwork.JoinRandomRoom(null, 0);
    }

    public void MatchingStart(MatchOption matchType)
    {
        PlayerManager.Instance.CurrentMatchType = matchType;

        if (matchType == MatchOption.Match_None)
            return;

        if (matchType == MatchOption.Match_Debug)
        {
            MatchingDebug();
            return;
        }

        PhotonHashTable playerProperties = new PhotonHashTable
            {
                { PlayerProperties.CHARACTER.ToString().ToString(), PlayerManager.Instance.CharacterType.ToString() },
                { PlayerProperties.SPAWNPOS.ToString().ToString(), 0 },
                { PlayerProperties.TEAM.ToString(), TeamOption.NoneTeam }
            };
        PhotonNetwork.SetPlayerCustomProperties(playerProperties);

        PhotonHashTable roomProperties
            = new PhotonHashTable() { { RoomPropoerties.MATCHTYPE.ToString(), PlayerManager.Instance.CurrentMatchType } };

        PhotonNetwork.JoinRandomRoom(roomProperties, 0);
    }

    public void MatchingCancel()
    {
        PhotonNetwork.LeaveRoom();
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
            PhotonHashTable oldHashTable = PhotonNetwork.LocalPlayer.CustomProperties;

            TeamManager.Instance.ClearTeamMemberCount();
            TeamOption localTeam = TeamManager.Instance.CollocateTeam();
            TeamManager.Instance.AddTeamMember(localTeam);

            PhotonHashTable newHashTable = new PhotonHashTable();
            newHashTable.Add(PlayerProperties.CHARACTER.ToString(), oldHashTable[PlayerProperties.CHARACTER.ToString()]);
            newHashTable.Add(PlayerProperties.TEAM.ToString(), localTeam);

            if (localTeam.Equals(TeamOption.BlueTeam)) newHashTable.Add(PlayerProperties.SPAWNPOS.ToString(), TeamManager.Instance.BlueTeamCount);
            else if (localTeam.Equals(TeamOption.RedTeam)) newHashTable.Add(PlayerProperties.SPAWNPOS.ToString(), (3 + TeamManager.Instance.RedTeamCount));

            PhotonNetwork.LocalPlayer.SetCustomProperties(newHashTable);
        }
        else if (PhotonNetwork.IsMasterClient && PlayerManager.Instance.CurrentMatchType == MatchOption.Match_Debug)
        {
            PhotonNetwork.LoadLevel("Match");
            return;
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        PhotonHashTable roomProperties = new PhotonHashTable();
        roomProperties.Add(Enums.RoomProperties.MATCHTYPE.ToString(), PlayerManager.Instance.CurrentMatchType);
        roomProperties.Add(Enums.RoomProperties.BLUETEAMSCORE.ToString(), 5);
        roomProperties.Add(Enums.RoomProperties.REDTEAMSCORE.ToString(), 5);

        RoomOptions options = new RoomOptions
        {
            CustomRoomProperties = roomProperties,
            CustomRoomPropertiesForLobby = new string[] { RoomPropoerties.MATCHTYPE.ToString() }
        };

        if (PlayerManager.Instance.CurrentMatchType == MatchOption.Match_None)
            return;
        else if (PlayerManager.Instance.CurrentMatchType == MatchOption.Match_Debug)
            options.MaxPlayers = 0;
        else
            options.MaxPlayers = (byte)PlayerManager.Instance.CurrentMatchType;

        PhotonNetwork.CreateRoom(null, options);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (PhotonNetwork.IsMasterClient && !PlayerManager.Instance.CurrentMatchType.Equals(MatchOption.Match_Debug))
        {
            TeamOption playerTeam = TeamManager.Instance.CollocateTeam();
            TeamManager.Instance.AddTeamMember(playerTeam);

            PhotonHashTable newHashTable = new PhotonHashTable();
            newHashTable.Add(Enums.PlayerProperties.TEAM.ToString(), playerTeam);
            newHashTable.Add(PlayerProperties.CHARACTER.ToString(), newPlayer.CustomProperties[PlayerProperties.CHARACTER.ToString()]);

            if (playerTeam.Equals(TeamOption.BlueTeam)) newHashTable.Add(PlayerProperties.SPAWNPOS.ToString(), TeamManager.Instance.BlueTeamCount);
            else if (playerTeam.Equals(TeamOption.RedTeam)) newHashTable.Add(PlayerProperties.SPAWNPOS.ToString(), 3 + TeamManager.Instance.RedTeamCount);

            newPlayer.SetCustomProperties(newHashTable);

            if (PhotonNetwork.CurrentRoom.PlayerCount == (int)PlayerManager.Instance.CurrentMatchType)
            {
                Debug.Log("매칭이 완료되어 게임을 시작합니다.");
                PhotonNetwork.LoadLevel("Match");
            }
        }
    }
}

