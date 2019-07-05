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

        item.FriendName     = userName;
        item.FriendStatus   = "OFFLINE";
        item.FriendComment  = "등록된 코멘트가 없습니다.";

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
}
