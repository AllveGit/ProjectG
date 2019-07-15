using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enums : MonoBehaviour
{
    public enum MatchType
    {
        Match_None = 0,
        Match_Debug = 1,
        Match_1vs1 = 2,
        Match_2vs2 = 4,
        Match_3vs3 = 6,
    }

    public enum TeamOption
    {
        NoneTeam = 0,
        RedTeam = 1,
        BlueTeam = 2,
        Solo = 3,
    }

    public enum RoomProperties
    {
        MATCHTYPE,
        REDTEAMSCORE,
        BLUETEAMSCORE,
    }

    public enum PlayerProperties
    {
        TEAM,
        SPAWNPOS,
        CHARACTER
    }

    //이름 변경 금지 
    // 경로로 쓰임
    public enum CharacterIndex
    {
        Healer,
        Maid,
        Archer,
    }

    public enum RankType : int
    {
        Bronze,
        Silver,
        Gold,
        Diamond,
        Master,
        Unknown
    }

    
}
