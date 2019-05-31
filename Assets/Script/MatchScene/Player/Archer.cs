using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : BasePlayer
{
    public override void Attack()
    {
    }

    public override void Death()
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
        Move();
    }

    private void FixedUpdate()
    {
        MoveRotation();
    }
}
