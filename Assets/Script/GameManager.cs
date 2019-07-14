using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;

/*
 * 이 매니져는 매치씬 안에서만 유효한 매니져입니다. 절대 다른 씬에서 사용할 생각하지 마세요.
 */

public partial class GameManager : MonoBehaviourPunCallbacks
{
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.LogFormat("다른 플레이어가 참가하였습니다() {0}", newPlayer.NickName);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.LogFormat("다른 플레이어가 나갔습니다() {0}", otherPlayer.NickName);
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }
}

public partial class GameManager 
{
    public static GameManager Instance;

    [SerializeField]
    private GameObject spawnZone = null;
    [SerializeField]
    private LoserWindow loseWindow = null;
    [SerializeField]
    private WinnerWindow winnerWindow = null;
   
    public bool GameEnd { get; private set; } = false;

    private void Awake()
    {
        Instance = this;
    }

    [System.Obsolete]
    private void Start()
    {
        if (spawnZone == null)
            Debug.LogError("GameManager에  spawnZone이란 변수가 초기화 되지 않았습니다.");

        ExitGames.Client.Photon.Hashtable localHash = PhotonNetwork.LocalPlayer.CustomProperties;

        string prefabName = (string)localHash[Enums.PlayerProperties.CHARACTER.ToString()];
        string path = $"Player/{prefabName}/{prefabName}";

        int spawnZoneIndex = (int)localHash[Enums.PlayerProperties.SPAWNPOS.ToString()];
        Transform spawnZoneTransform = spawnZone.transform.GetChild(spawnZoneIndex);

        // 플레이어를 생성합니다. PhotonNetwork.Instantiate로 생성해야만 네트워크 통신을 할 수 있습니다.
        GameObject player = PhotonNetwork.Instantiate(path, Vector3.zero
            ,spawnZoneTransform.rotation, 0);

        player.GetComponent<BasePlayer>().PlayerInit((Enums.TeamOption)localHash[Enums.PlayerProperties.TEAM.ToString()], spawnZoneTransform.position);
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public Vector3 GetRespawnPos()
    {
        int spawnIndex = (int)PhotonNetwork.LocalPlayer.CustomProperties[Enums.PlayerProperties.SPAWNPOS.ToString()];
        Transform spawnZoneElement = spawnZone.transform.GetChild(spawnIndex);
        return spawnZoneElement.position;
    }

    public void DeathPlayer()
    {
        if (GameEnd)
            return;

        ExitGames.Client.Photon.Hashtable oldHashTable = PhotonNetwork.CurrentRoom.CustomProperties;
        ExitGames.Client.Photon.Hashtable newHashTable = new ExitGames.Client.Photon.Hashtable();

        newHashTable.Add(Enums.RoomProperties.MATCHTYPE.ToString(), oldHashTable[Enums.RoomProperties.MATCHTYPE.ToString()]);

        Enums.TeamOption local = (Enums.TeamOption)PhotonNetwork.LocalPlayer.CustomProperties[Enums.PlayerProperties.TEAM.ToString()];
        if (local.Equals(Enums.TeamOption.BlueTeam))
        {
            int i = (int)oldHashTable[Enums.RoomProperties.BLUETEAMSCORE.ToString()];
            newHashTable.Add(Enums.RoomProperties.BLUETEAMSCORE.ToString(), i - 1);
        }
        else if (local.Equals(Enums.TeamOption.RedTeam))
        {
            int i = (int)oldHashTable[Enums.RoomProperties.REDTEAMSCORE.ToString()];
            newHashTable.Add(Enums.RoomProperties.REDTEAMSCORE.ToString(), i - 1);
        }
        PhotonNetwork.CurrentRoom.SetCustomProperties(newHashTable);

        photonView.RPC("WinLoseCheck", RpcTarget.MasterClient);
    }

    [PunRPC]
    public void WinLoseCheck()
    {
        int BlueTeamScord = (int)PhotonNetwork.CurrentRoom.CustomProperties[Enums.RoomProperties.BLUETEAMSCORE.ToString()];
        int RedTeamScord = (int)PhotonNetwork.CurrentRoom.CustomProperties[Enums.RoomProperties.REDTEAMSCORE.ToString()];

        Enums.TeamOption winnerTeam = Enums.TeamOption.NoneTeam;

        if (BlueTeamScord == 0)
        {
            // 레드팀 승리
            winnerTeam = Enums.TeamOption.RedTeam;
        }
        else if (RedTeamScord == 0)
        {
            // 블루팀 승리.
            winnerTeam = Enums.TeamOption.BlueTeam;
        }

        if (!winnerTeam.Equals(Enums.TeamOption.NoneTeam))
            photonView.RPC("GameResult", RpcTarget.All, winnerTeam);
    }

    [PunRPC]
    public void GameResult(Enums.TeamOption winnerTeam)
    {
        GameEnd = true;

        Enums.TeamOption myTeam = (Enums.TeamOption)PhotonNetwork.LocalPlayer.CustomProperties[Enums.PlayerProperties.TEAM.ToString()];

        if (winnerTeam.Equals(myTeam))
            winnerWindow.ShowWindow();
        else
            loseWindow.ShowWindow();

    }


}
