using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendTabBase : MonoBehaviour
{
    Dictionary<string, FriendTab> friendContainter = new Dictionary<string, FriendTab>();

    public GameObject friendViewButtonPrefab;
    public Transform buttonBaseTransform;

    // Start is called before the first frame update
    void Start()
    {
        ChatManager.Instance.OnStatusUpdate.AddListener(OnStatusUpdate);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddFriend(string userName)
    {
        bool result = ChatManager.Instance.AddFriend(userName);

        if (result == false)
            return;

        GameObject newTab = InstantiateFriendItem(userName);

        FriendTab item = newTab.GetComponent<FriendTab>();

        item.FriendName = userName;
        item.OnStatusUpdate(7, true, "코멘트 없음.");

        friendContainter.Add(userName, item);
    }

    public void RemoveFriend(string userName)
    {
        bool result = ChatManager.Instance.RemoveFriend(userName);

        if (result == false)
            return;

        if (friendContainter.TryGetValue(userName, out FriendTab item) == false)
            return;

        Destroy(item.gameObject);
        friendContainter.Remove(userName);
    }

    public GameObject InstantiateFriendItem(string userName)
    {
        GameObject newTab = Instantiate(friendViewButtonPrefab, buttonBaseTransform);

        return newTab;
    }

    public void OnStatusUpdate(string userName, int status, bool getMessage, object comment)
    {
        if (friendContainter.TryGetValue(userName, out FriendTab friendTab) == false)
            return;

        friendTab.OnStatusUpdate(status, getMessage, comment);
    }
}
