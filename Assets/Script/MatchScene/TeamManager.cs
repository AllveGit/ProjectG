using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamManager : MonoBehaviour
{
    public enum PlayerTeam
    {
        NoneTeam = 0,
        RedTeam = 1,
        BlueTeam = 2,
        Solo = 3,
    }

    public static TeamManager Instance = null;

    private PlayerTeam PrevCollocateTeam = PlayerTeam.NoneTeam;
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

    public void CollocateTeam(GameObject player)
    {
        if (player.tag.Equals("Player"))
        {

        }
    }

    public bool IsAttackable(PlayerTeam myTeam, PlayerTeam targetTeam)
    {
        if (!myTeam.Equals(targetTeam))
            return true;

        return false;
    }
}
