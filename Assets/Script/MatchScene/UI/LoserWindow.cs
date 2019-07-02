using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class LoserWindow : BaseWindow
{
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

    private IEnumerator RoomQuitCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);

        PlayerManager.Instance.AddExp(30);
        PlayerManager.Instance.MinusRankScore(15);

        PhotonNetwork.LeaveRoom();

        yield break;
    }
}
