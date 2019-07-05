using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChattingPanel : MonoBehaviour
{
    public Text chattingView;
    public InputField chattingInput;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void AddChatMessage(string context)
    {
        chattingView.text += context;
    }

    public void Submit()
    {
        ChatManager.Instance.Send(chattingInput.text);
        chattingInput.text = string.Empty;
    }

    public void OnEndReturn()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            Submit();
        }
    }
}
