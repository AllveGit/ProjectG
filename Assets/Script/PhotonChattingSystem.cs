using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Chat;
using Photon.Pun;

public class PhotonChattingSystem : MonoBehaviour, IChatClientListener
{
    private ChatClient chatClient;
    private string userName;
    private string currentChannelName;

    public InputField inputField;
    public Text outputText;

    // Use this for initialization
    void Start()
    {
        Application.runInBackground = true;

        userName = PhotonNetwork.NickName;
        currentChannelName = "001";

        chatClient = new ChatClient(this);

        chatClient.Connect(
            PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat,
            PhotonNetwork.GameVersion,
            new AuthenticationValues(userName));

        // AddLine(string.Format("연결시도", userName));
    }

    public void AddLine(string lineString)
    {
        outputText.text += lineString + "\r\n";
    }

    public void OnApplicationQuit()
    {
        if (chatClient != null)
        {
            chatClient.Disconnect();
        }
    }

    public void DebugReturn(ExitGames.Client.Photon.DebugLevel level, string message)
    {
        if (level == ExitGames.Client.Photon.DebugLevel.ERROR)
        {
            Debug.LogError(message);
        }
        else if (level == ExitGames.Client.Photon.DebugLevel.WARNING)
        {
            Debug.LogWarning(message);
        }
        else
        {
            Debug.Log(message);
        }
    }

    public void OnConnected()
    {
        AddLine("채팅 서버에 연결되었습니다.");

        chatClient.Subscribe(new string[] { currentChannelName }, 10);
    }

    public void OnDisconnected()
    {
        AddLine("채팅 서버와의 연결이 끊어졌습니다.");
    }

    public void OnChatStateChange(ChatState state)
    {
        Debug.Log("OnChatStateChange = " + state);
    }

    public void OnSubscribed(string[] channels, bool[] results)
    {
        AddLine(string.Format("[{0}] 채널에 \"{1}\"님이 입장하셨습니다.", string.Join(",", channels), userName));
    }

    public void OnUnsubscribed(string[] channels)
    {
        AddLine(string.Format("[{0}] 채널에 \"{1}\"님이 퇴장하셨습니다.", string.Join(",", channels), userName));
    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        for (int i = 0; i < messages.Length; i++)
        {
            AddLine(string.Format("{0} : {1}", senders[i], messages[i].ToString()));
        }
    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    {
        Debug.Log("OnPrivateMessage : " + message);
    }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
        Debug.Log("status : " + string.Format("{0} is {1}, Msg : {2} ", user, status, message));
    }

    void Update()
    {
        chatClient.Service();
    }

    public void Input_OnEndEdit(string text)
    {
        if (chatClient.State != ChatState.ConnectedToFrontEnd)
            return;

        // 서버에 메시지 전송
        chatClient.PublishMessage(currentChannelName, inputField.text);
        // 입력 내용 초기화
        inputField.text = "";
    }

    public void OnUserSubscribed(string channel, string user)
    {
        throw new System.NotImplementedException();
    }

    public void OnUserUnsubscribed(string channel, string user)
    {
        throw new System.NotImplementedException();
    }
}
