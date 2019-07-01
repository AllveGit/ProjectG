using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class KillEventHandler : MonoBehaviour
{
    public GameObject RedTeamEventUI;
    public GameObject BlueTeamEventUI;

    public GameObject Canvas;

    public Vector3 RedTeamUIPos;
    public Vector3 BlueTeamUIPos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnKillEvent(BasePlayer killer, BasePlayer slayer)
    {
        if (killer.playerTeam == Enums.TeamOption.RedTeam)
        {
            AddRedTeamUI($"{killer.name}님이 {slayer.name}님을 처치했습니다.");
        }
        else
        {
            AddBlueTeamUI($"{killer.name}님이 {slayer.name}님을 처치했습니다.");
        }
    }

    public void AddRedTeamUI(string context)
    {
        GameObject newUI = Instantiate(RedTeamEventUI, RedTeamUIPos, Quaternion.identity, Canvas.transform);

        newUI.transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = context;
    }

    public void AddBlueTeamUI(string context)
    {
        GameObject newUI = Instantiate(BlueTeamEventUI, BlueTeamUIPos, Quaternion.identity, Canvas.transform);

        newUI.transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = context;
    }
}
