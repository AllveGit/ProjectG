using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FriendTab : MonoBehaviour
{
    public string FriendName { get => friendName;
        set {
            NameField.text = value;
            friendName = value;
        }
    }
    public string FriendStatus { get => friendStatus;
        set {
            StatusField.text = value;
            friendStatus = value;
        }
    }
    public string FriendComment { get => friendComment;
        set {
            CommentField.text = value;
            friendComment = value;
        }
    }

    public Text NameField;
    public Text StatusField;
    public Text CommentField;

    private string friendName = string.Empty;
    private string friendStatus = string.Empty;
    private string friendComment = string.Empty;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnStatusUpdate(int status, bool getMessage, object comment)
    {
        switch (status)
        {
            case 1:
                FriendStatus = "보이지 않음";
                break;
            case 2:
                FriendStatus = "온라인";
                break;
            case 3:
                FriendStatus = "AWAY";
                break;
            case 4:
                FriendStatus = "방해금지";
                break;
            case 5:
                FriendStatus = "게임/그룹";
                break;
            case 6:
                FriendStatus = "게임 플레이 중";
                break;
            default:
                FriendStatus = "오프라인";
                break;
        }

        if (getMessage)
        {
            if (comment is string messages)
            {
                FriendComment = messages;
            }
        }
    }
}
