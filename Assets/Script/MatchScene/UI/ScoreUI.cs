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
       if (Team == Enums.RoomProperties.BLUDSCORE)
            scoreText.text = ((int)Photon.Pun.PhotonNetwork.CurrentRoom.CustomProperties[Enums.RoomProperties.BLUDSCORE.ToString()]).ToString();
       else
            scoreText.text = ((int)Photon.Pun.PhotonNetwork.CurrentRoom.CustomProperties[Enums.RoomProperties.REDSCORE.ToString()]).ToString();
    }
}
