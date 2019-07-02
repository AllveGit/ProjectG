using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Events;

using MatchOption = Enums.MatchType;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance = null;

    public MatchOption CurrentMatchType { get; set; } = MatchOption.Match_None;

    public Enums.CharacterIndex CharacterType { get; set; }

    private string playerName;

    public string PlayerName {
        get => playerName;
        set
        {
            PhotonNetwork.NickName = value;
            playerName = value;

            OnNameChanged?.Invoke();
        }
    }

    private int playerLevel = 1;

    public int PlayerLevel {
        get => playerLevel;
        private set
        {
            OnLevelChanged?.Invoke();
            playerLevel = value;
        }
    }

    public int PlayerRankScore { get; private set; } = 0;

    public int PlayerExp { get; private set; } = 0;

    public int PlayerMaxExp { get; private set; } = 0;

    public Enums.RankType PlayerRankType { get; private set; } = Enums.RankType.Bronze;

    public UnityEvent OnRankUp;
    public UnityEvent OnRankDown;
    public UnityEvent OnNameChanged;
    public UnityEvent OnLevelChanged;

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Initialize();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Start()
    {

    }

    public void Initialize()
    {
        PlayerRankType = Enums.RankType.Bronze;
        PlayerLevel = 1;
        PlayerExp = 0;
        PlayerName = PhotonNetwork.NickName;
    }

    public void AddExp(int plusExp)
    {
        PlayerExp += plusExp;

        // 레벨업 공식 
        // 수정 가능

        PlayerMaxExp = (int)(PlayerLevel * 1.5f) + 15;

        if (PlayerExp >= PlayerMaxExp)
        {
            PlayerExp -= PlayerMaxExp;

            PlayerLevel++;
        }
    }

    public void AddRankScore(int plusScore)
    {
        PlayerRankScore += plusScore;

        Enums.RankType newRank = SiftRank(PlayerRankScore);

        if (newRank > PlayerRankType)
        {
            PlayerRankType = newRank;
            OnRankUp?.Invoke();
            return;
        }
        else
            PlayerRankType = newRank;
    }

    public void MinusRankScore(int minusScore)
    {
        PlayerRankScore -= minusScore;

        if (PlayerRankScore < 0)
            PlayerRankScore = 0;

        Enums.RankType newRank = SiftRank(PlayerRankScore);

        if (newRank < PlayerRankType)
        {
            PlayerRankType = newRank;
            OnRankDown?.Invoke();
            return;
        }
        else
            PlayerRankType = newRank;
    }

    public static Enums.RankType SiftRank(int score)
    {
        if (score >= 0 &&
                score < 199)
        {
            return Enums.RankType.Bronze;
        }
        else if (score >= 200 &&
            score < 399)
        {
            return Enums.RankType.Silver;
        }
        else if (score >= 400 &&
           score < 599)
        {
            return Enums.RankType.Gold;
        }
        else if (score >= 600 &&
           score < 799)
        {
            return Enums.RankType.Diamond;
        }
        else if (score >= 800)
        {
            return Enums.RankType.Master;
        }
        else
        {
            return Enums.RankType.Unknown;
        }
    }
}
