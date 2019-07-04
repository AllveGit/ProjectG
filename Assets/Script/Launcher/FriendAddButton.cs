using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FriendAddButton : MonoBehaviour
{
    public InputField friendNameField;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
        ChatManager.Instance.AddFriend(friendNameField.text);
        friendNameField.text = string.Empty;
    }
}
