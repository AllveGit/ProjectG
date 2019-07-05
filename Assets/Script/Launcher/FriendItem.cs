using ExitGames.Client.Photon.Chat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FriendItem : MonoBehaviour
{
    enum Status : int
    {
        Offline = 0,
        Invisible = 1,
        Online = 2,
        Away = 3,
        DND = 4,
        LFG = 5,
        Playing = 6
    }

    private string friendName;

    public string FriendName {
        get => friendName;
        set {
            friendName = value;
            nameField.text = friendName;
        }
    }

    public Text nameField;
    public Text statusField;
    public Text userMessage;

    // Start is called before the first frame update
    void Awake()
    {
        nameField.text = "NAME:NONE";
        statusField.text = "Offline";
        userMessage.text = "상태 메시지가 없습니다.";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnFriendStatusUpdate(int status, bool gotMessage, object message)
    {
        statusField.text = ((Status)status).ToString();
        userMessage.text = message as string;
    }
}
