using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.Demo.PunBasics;


public class PlayerManager : MonoBehaviourPunCallbacks, IPunObservable // Interface 구현.
{
    public static GameObject localPlayerInstance;

    [SerializeField]
    private GameObject beamPrefab = null;
    [SerializeField]
    private GameObject beamFirePos = null;

    // 플레이어 체력
    public float fHealth = 1f;

    // 발사가 가능 여부를 체크하는 변수 입니다. true : 발사 가능.
    private bool bLaunchable = true;

    // 발사 쿨타임
    [SerializeField]
    private float fLaunchWaitTime = 0.5f;

    #region IPunOvervable
    // 데이터를 전송하는 CallBack 함수입니다 무조건 PhotonView Observed Compenents에 넣어야만 작동합니다.
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

        // 메시지를 쓸수 있을때
        if (stream.IsWriting)
        {
            stream.SendNext(fHealth);
        }
        // 메시지를 받아야할때
        else
        {
            this.fHealth = (float)stream.ReceiveNext();
        }
    }

    #endregion

    private void Awake()
    {
        if (photonView.IsMine)
            PlayerManager.localPlayerInstance = this.gameObject;

        if (beamPrefab == null)
            Debug.LogError("PlayerManager beamPrefab 변수가 초기화 되지 않았습니다.");
        if (beamFirePos == null)
            Debug.LogError("PlayerManager beamFirPos 변수가 초기화 되지 않았습니다.");

        // 새로운 플레이어가 들어올때마다 맵 레벨이 바뀌기 때문에 로컬 플레이어가 씬 전환으로 삭제되는걸 방지합니다.
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        PlayerCamera _camreaWork = gameObject.GetComponent<PlayerCamera>();
        if (_camreaWork != null)
        {
            if (photonView.IsMine)
                _camreaWork.OnTargeting();
        }
        else
            Debug.LogError("<Color=Red><a>Missing</a></Color> CameraWork Component on playerPrefab.", this);
    }

    private void Update()
    {
        if (photonView.IsMine)
          ProcessInputs();

        if (fHealth <= 0f)
            GameManager.Instance.LeaveRoom();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!photonView.IsMine)
            return;

        // 해당 문자열에 인자로 전달된 문자열이 포함되는지 확인합니다.
        if (!other.name.Contains("Beam"))
            return;

        fHealth -= 0.1f * Time.deltaTime;
    }

    void ProcessInputs()
    {
        if (Input.GetButtonDown("Fire1") && bLaunchable)
            StartCoroutine("WaitForLaunch");
    }

    IEnumerator WaitForLaunch()
    {
        bLaunchable = false;

        GameObject BeamObject = PhotonNetwork.Instantiate(beamPrefab.name, beamFirePos.transform.position, beamFirePos.transform.rotation);
        yield return new WaitForSeconds(fLaunchWaitTime);

        bLaunchable = true;
    }
}
