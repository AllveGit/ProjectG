using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankViewer : MonoBehaviour
{
    public Sprite[] rankImgs;

    public Enums.RankType rankType;

    // Start is called before the first frame update
    void Start()
    {
        OnRankChanged();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnRankChanged()
    {
        rankType = PlayerManager.Instance.PlayerRankType;

        if (rankImgs[(int)rankType] == null)
            return;

        GetComponent<UnityEngine.UI.Image>().sprite = rankImgs[(int)rankType];
    }
}
