using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : BasePlayer
{
    public override void Attack()
    {
    }

    public override void OnPlayerDeath()
    {
    }

    public override void UltimateSkill()
    {
    }

    
    void Start()
    {
        playerCamera.TargetObject = this.gameObject;

        if (photonView.IsMine)
        {
            playerCamera.IsTargeting = true;
        }
    }
    void Update()
    {
        MoveCalculate();
    }

    private void FixedUpdate()
    {
        RotateCalculate();
    }
}
