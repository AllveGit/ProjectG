﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healer : BasePlayer
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