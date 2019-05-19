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
    public GameObject playerPrefab;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        // 플레이어를 생성합니다. PhotonNetwork.Instancte로 생성해야만 네트워크 통신을 할 수 있습니다.
        if(playerPrefab == null)
            Debug.LogError("플레이어 프리팹이 NULL 입니다.'", this);
        else
        {
            // 씬이 새로로드되어 GameManager가 새로 생성되어도 이미 생성한 플레이어가 있는지 체크합니다.
            if (PlayerManager.localPlayerInstance == null)
            {
                Debug.LogFormat("플레이어를 생성합니다.", Application.loadedLevelName);
                PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(0f, 5f, 0f), Quaternion.identity, 0);
            }
            else
                Debug.LogFormat("플레이어가 이미 로드 되었습니다.{0}", SceneManagerHelper.ActiveSceneName);
        }
    }

    #region Public Methods
    public void LeaveRoom()
    {
        //방에서 나갑니다.
        PhotonNetwork.LeaveRoom();
    }

    //private void LoadArena()
    //{
    //    // 맵을 생성하는 것은 마스터 클라이언트의 일이기 때문에 마스터클라이언트인지 체크한다.
    //    if (!PhotonNetwork.IsMasterClient)
    //        Debug.LogError("PhotonNetwork : 우리는 마스터 클라이언트가 아닙니다.");
    //
    //    Debug.LogFormat("래밸이 로드되었습니다. : {0}", PhotonNetwork.CurrentRoom.PlayerCount);
    //    /*해당 레벨을 로드합ㄴ이다.  PhotonNetwork.automaticallySyncScene 을 사용하도록 해놓았기 때문에 룸 안의 모든 접속한 클라이언트에 
    //    대해 이 레벨 로드를 유니티가 직접하는 것이 아닌 Photon이 하도록 하였습니다.*/
    //    PhotonNetwork.LoadLevel("RoomFor" + PhotonNetwork.CurrentRoom.PlayerCount);
    //}  
    #endregion

    #region Photon
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        // 다른 player가 접속될때 현재 방인원수의 따라 새로 맵이 로드됩니다.
        Debug.LogFormat("다른 플레이어가 참가하였습니다() {0}", newPlayer.NickName);

        // 자신이 마스터 클라이언트일 경우만(방장)
        //if (PhotonNetwork.IsMasterClient)
        //{
        //    Debug.LogFormat("방장이 방에 참가하였습니다.{0}", PhotonNetwork.IsMasterClient);
        //    LoadArena();
        //}
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.LogFormat("다른 플레이어가 나갔습니다() {0}", otherPlayer.NickName);

        // 자신이 마스터 클라이언트일 경우만(방장)
        //if (PhotonNetwork.IsMasterClient)
        //{
        //    Debug.LogFormat("방장이 방에서 나갔습니다.{0}", PhotonNetwork.IsMasterClient);
        //    LoadArena();
        //}
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }
    #endregion
}
