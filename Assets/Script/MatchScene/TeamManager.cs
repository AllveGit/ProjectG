using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TeamOption = Enums.TeamOption;

public class TeamManager : MonoBehaviour
{
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

    public void ClearTeamMemberCount()
    {
        RedTeamCount = 0;
        BlueTeamCount = 0;
    }
    public void AddTeamMember(TeamOption inPlayerTeam, int iAddCount = 1)
    {
        if (inPlayerTeam == TeamOption.RedTeam) RedTeamCount += iAddCount;
        else if (inPlayerTeam == TeamOption.BlueTeam) BlueTeamCount += iAddCount;
    }

}
