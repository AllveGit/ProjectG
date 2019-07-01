using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    public Enums.RoomProperties Team;
    private UnityEngine.UI.Text scoreText;

    // Start is called before the first frame update
    void Start()
    {
        scoreText = GetComponent<UnityEngine.UI.Text>();
    }

    void LateUpdate()
    {
        scoreText.text = Photon.Pun.PhotonNetwork.CurrentRoom.CustomProperties[Team.ToString()].ToString();
    }
}
