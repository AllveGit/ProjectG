using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamManager : MonoBehaviour
{
    public enum PlayerTeam
    {
        RedTeam = 0,
        BlueTeam = 1,
        Solo = 2,
        NoneTeam = 3,
    }

    public static TeamManager Instance = null;

    public int RedTeamCount { get; set; } = 0;
    public int BlueTeamCount { get; set; } = 0;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);

        DontDestroyOnLoad(this.gameObject);
    }

    public void InitializeTeam()
    {
        RedTeamCount = 0;
        BlueTeamCount = 0;
    }

    public void ClearTeamMemberCount()
    {
        RedTeamCount = 0;
        BlueTeamCount = 0;
    }
    public void AddTeamMember(PlayerTeam inPlayerTeam, int iAddCount = 1)
    {
        if (inPlayerTeam == PlayerTeam.RedTeam) RedTeamCount += iAddCount;
        else if (inPlayerTeam == PlayerTeam.BlueTeam) BlueTeamCount += iAddCount;
    }

    public bool IsAttackable(PlayerTeam myTeam, PlayerTeam targetTeam)
    {
        if (!myTeam.Equals(targetTeam))
            return true;

        return false;
    }
}
