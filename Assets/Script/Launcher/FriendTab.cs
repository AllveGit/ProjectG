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
}
