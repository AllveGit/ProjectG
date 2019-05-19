using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.Demo.PunBasics;


public class PlayerManager : MonoBehaviourPunCallbacks, IPunObservable // Interface 구현.
{
    [Tooltip("최로로 생성된 플레이어를 저장할 변수입니다.")]
    public static GameObject localPlayerInstance;

    #region IPunOvervable
    // 데이터를 전송하는 CallBack 함수입니다 무조건 PhotonView Observed Compenents에 넣어야만 작동합니다.
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

        // 메시지를 쓸수 있을때
        if (stream.IsWriting)
        {
            stream.SendNext(IsFiring);
            stream.SendNext(health);
        }
        // 메시지를 받아야할때
        else
        {
            this.IsFiring = (bool)stream.ReceiveNext();
            this.health = (float)stream.ReceiveNext();
        }
    }

    #endregion

    [Tooltip("Beam GameObject의 프리팹입니다")]
    [SerializeField]
    private GameObject beams;

    [Tooltip("Player 체력입니다.")]
    public float health = 1f;

    bool IsFiring;

    private void Awake()
    {
        if (photonView.IsMine)
            PlayerManager.localPlayerInstance = this.gameObject;

        // 새로운 플레이어가 들어올때마다 맵 레벨이 바뀌기 때문에 로컬 플레이어가 씬 전환으로 삭제되는걸 방지합니다.
        DontDestroyOnLoad(gameObject);

        if (beams == null)
            Debug.LogError("<Color = Red><a>Missing</a></Color> Beams Reference.", this);
        else
            beams.SetActive(false);
    }

    void Start()
    {
        CameraWork _camreaWork = gameObject.GetComponent<CameraWork>();
        if (_camreaWork != null)
        {
            if (photonView.IsMine)
                _camreaWork.OnStartFollowing();
        }
        else
            Debug.LogError("<Color=Red><a>Missing</a></Color> CameraWork Component on playerPrefab.", this);
    }

    private void Update()
    {
        if (photonView.IsMine)
          ProcessInputs();

        if (health <= 0f)
            GameManager.Instance.LeaveRoom();

        if (beams != null && IsFiring != beams.activeSelf)
            beams.SetActive(IsFiring);
    }

    void ProcessInputs()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (!IsFiring)
                IsFiring = true;
        }

        if (Input.GetButtonUp("Fire1"))
        {
            if (IsFiring)
                IsFiring = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!photonView.IsMine)
            return;

        // 해당 문자열에 인자로 전달된 문자열이 포함되는지 확인합니다.
        if (!other.name.Contains("Beam"))
            return;

        health -= 0.1f * Time.deltaTime;
    }
}
