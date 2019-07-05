using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FriendAddButton : MonoBehaviour
{
    public FriendTabBase friendTabBase;
    public InputField inputField;

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
        if (string.IsNullOrWhiteSpace(inputField.text))
            return;

        friendTabBase.AddFriend(inputField.text);
        inputField.text = string.Empty;
    }
}
