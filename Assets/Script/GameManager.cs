using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{
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

        // 플레이어를 생성합니다. PhotonNetwork.Instantiate로 생성해야만 네트워크 통신을 할 수 있습니다.
        if(playerPrefab == null)
            Debug.LogError("플레이어 프리팹이 null 입니다.", this);
        else
        {
            Debug.LogFormat("게임이 시작되어 플레이어를 생성합니다.", Application.loadedLevelName);
            string path = $"Player/{this.playerPrefab.name}/{this.playerPrefab.name}";
            PhotonNetwork.Instantiate(path, playerSpawnPos, Quaternion.identity, 0);
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
