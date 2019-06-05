using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BunnyGirl : BasePlayer
{
    public override void Attack()
    {
        if (!photonView.IsMine) return;

    }

    public override void OnPlayerDeath()
    {
        if (!photonView.IsMine) return;

    }

    public override void UltimateSkill()
    {
        if (!photonView.IsMine) return;

    }

    void Start()
    {
    }

    void Update()
    {
        MoveCalculate();
    }

    private void LateUpdate()
    {
        RotateCalculate();
    }
}
