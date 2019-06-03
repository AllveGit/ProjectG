using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class Archer : BasePlayer
{
    [SerializeField]
    private GameObject ultimateSkillPrefab = null;

    public override void Attack()
    {
    }

    public override void OnPlayerDeath()
    {
    }

    public override void UltimateSkill()
    {
        Vector3 direction = SkillJoyStick.Amount;

        if (direction == Vector3.zero)
            return;

        GameObject projectile = PhotonNetwork.Instantiate(
            "Skill/" + ultimateSkillPrefab.name,
            transform.position,
            transform.rotation);
        
        projectile.GetComponent<IceArrow>().Cast(this, AttackDamage, direction);
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

    public override void OnAttacked(int damage)
    {
        CurHP -= damage;
    }
}
