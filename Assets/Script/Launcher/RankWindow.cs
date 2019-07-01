using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankWindow : BaseWindow
{
    public Sprite[] rankWindows;
    public UnityEngine.UI.Image windowImage;
    public UnityEngine.UI.Text rankScoreTex;

    // Start is called before the first frame update
    void Start()
    {
        CloseWindow();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void ShowWindow()
    {
        int rankType = (int)PlayerManager.Instance.PlayerRankType;

        if (rankWindows[rankType] == null)
            return;

        windowImage.sprite = rankWindows[rankType];
        rankScoreTex.text = PlayerManager.Instance.PlayerRankScore.ToString();

        base.ShowWindow();
    }
}
