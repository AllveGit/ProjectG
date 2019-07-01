using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankViewer : MonoBehaviour
{
    public Sprite[] rankImgs;

    public UnityEngine.UI.Image currentImg;

    public Enums.RankType rankType;

    // Start is called before the first frame update
    void Start()
    {
        rankType = PlayerManager.Instance.PlayerRankType;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnRankUp()
    {
        rankType = PlayerManager.Instance.PlayerRankType;

        if (rankImgs[(int)rankType] == null)
            return;

        currentImg.sprite = rankImgs[(int)rankType];
    }

    public void OnRankDown()
    {
        rankType = PlayerManager.Instance.PlayerRankType;

        if (rankImgs[(int)rankType] == null)
            return;

        currentImg.sprite = rankImgs[(int)rankType];
    }
}
