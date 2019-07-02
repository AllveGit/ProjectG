using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class WinnerWindow : BaseWindow
{
    public Text KillTex;
    public Text DeathTex;
    public Text AssistTex;
    public Text ScoreTex;

    public override void CloseWindow()
    {
        base.CloseWindow();
    }

    public override void ShowWindow()
    {
        base.ShowWindow();

        StartCoroutine(RoomQuitCoroutine(3.0f));
    }

    // Start is called before the first frame update
    void Start()
    {
        CloseWindow();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Initialize(int kill, int death, int assist)
    {
        // score 는 내부 계산

        KillTex.text = kill.ToString();
        DeathTex.text = death.ToString();
        AssistTex.text = assist.ToString();

        ScoreTex.text = (kill * 3 + assist * 2 - death ).ToString();
    }

    private IEnumerator RoomQuitCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);

        PlayerManager.Instance.AddExp(40);
        PlayerManager.Instance.AddRankScore(30);

        PhotonNetwork.LeaveRoom();

        yield break;
    }
}
