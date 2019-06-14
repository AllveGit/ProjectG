using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;

/*
 * 이 매니져는 매치씬 안에서만 유효한 매니져입니다. 절대 다른 씬에서 사용할 생각하지 마세요.
 */
public class GameManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject spawnZone = null;

    public static GameManager Instance;

    [Tooltip("네트워크를 통해 생성할 플레이어 프리팹입니다.")]
    public GameObject playerPrefab = null;

    public Vector3 playerSpawnPos = default;

    private void Awake()
    {
        Instance = this;
    }

    [System.Obsolete]
    private void Start()
    {
        if (spawnZone == null)
            Debug.LogError("GameManager에  spawnZone이란 변수가 초기화 되지 않았습니다.");

        // 플레이어를 생성합니다. PhotonNetwork.Instantiate로 생성해야만 네트워크 통신을 할 수 있습니다.
        if (playerPrefab == null)
            Debug.LogError("플레이어 프리팹이 null 입니다.", this);
        else
        {
            Debug.LogFormat("게임이 시작되어 플레이어를 생성합니다.", Application.loadedLevelName);
            string path = $"Player/{this.playerPrefab.name}/{this.playerPrefab.name}";

            int spawnZoneIndex= (int)PhotonNetwork.LocalPlayer.CustomProperties["SPAWN"];
            Transform spawnZoneElement = spawnZone.transform.GetChild(spawnZoneIndex);

            GameObject player = PhotonNetwork.Instantiate(path, spawnZoneElement.position 
                , spawnZoneElement.rotation, 0);

            /*
             * Team을 지정합니다. Team 런쳐씬에서 CustomProperties HashTable로 설정했습니다.
             */
            player.GetComponent<BasePlayer>().playerTeam = (TeamManager.PlayerTeam)PhotonNetwork.LocalPlayer.CustomProperties["TEAM"];
        }
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    #region Photon
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
    #endregion
}
