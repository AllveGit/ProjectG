using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

using Photon.Pun;

public class TouchToScreen : MonoBehaviourPunCallbacks
{
    //
    public GameObject Character2D = null;

    public float Character2DHeight = 1f;
    public float Character2DSpeed = 1f;

    private Vector3 Character2DOriginPos;
    private float Character2DSin = 0f;

    //
    public GameObject Logo = null;

    public float LogoHeight = 1f;
    public float LogoSpeed = 1f;

    private Vector3 LogoOriginPos;
    private float LogoSin = Mathf.PI;

    // 
    public GameObject TouchToScreenSprite;
    public GameObject LoadingSprite;

    public int SetWidth = 16;
    public int SetHeight = 9;



    private void Awake()
    {
        Application.targetFrameRate = 60;

        Screen.sleepTimeout = SleepTimeout.NeverSleep;

#if UNITY_ANDROID
        Screen.SetResolution(Screen.width, Screen.height * (SetWidth / SetHeight), true);
#else
        Screen.SetResolution(Screen.width, Screen.height * (SetWidth / SetHeight), false);
#endif


    }

    void Start()
    {
        Character2DOriginPos = Character2D.transform.position;
        LogoOriginPos = Logo.transform.position;

        
    }

    // 임시로 사용할 이름을 만드는 함수입니다.
    public string GetRandomNickName()
    {
        string[] namePool = new string[20] {
            "Aiden",
            "Alex",
            "Antonio",
            "Brody",
            "Bruno",
            "Chistopher",
            "Charles",
            "Covy",
            "Chico",
            "Danial",
            "Derek",
            "Dominick",
            "Drew",
            "Evan",
            "Enzo",
            "Fabian",
            "Floyd",
            "Gabriel",
            "Garrett",
            "George"
        };

        return namePool[Random.Range(0, 19)];
    }
    public void Connect()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.GameVersion = "1.0";
            PhotonNetwork.NickName = GetRandomNickName();
            PhotonNetwork.ConnectUsingSettings();
        }
    }
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    void LateUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            TouchToScreenSprite.SetActive(false);
            LoadingSprite.SetActive(true);

            Connect();
        }

        if (Character2DSin >= (Mathf.PI * 2))
            Character2DSin = 0f;

        float y = Character2DOriginPos.y + (Mathf.Sin(Character2DSin) * Character2DHeight);
        Character2D.transform.position = new Vector3(Character2DOriginPos.x, y, 0f);

        Character2DSin += Time.deltaTime * Character2DSpeed;

        if (LogoSin >= (Mathf.PI * 2))
            LogoSin = 0f;

        y = LogoOriginPos.y + (Mathf.Sin(LogoSin) * LogoHeight);
        Logo.transform.position = new Vector3(LogoOriginPos.x, y, 0f);

        LogoSin += Time.deltaTime * LogoSpeed;
    }

    public override void OnJoinedLobby()
    {
        SceneManager.LoadScene("Launcher", LoadSceneMode.Single);
    }
}
