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
        if (Photon.Pun.PhotonNetwork.CurrentRoom != null)
        {
            ExitGames.Client.Photon.Hashtable roomProperties = Photon.Pun.PhotonNetwork.CurrentRoom.CustomProperties;
            scoreText.text = ((int)roomProperties[Team.ToString()]).ToString();
        }
     }
}
