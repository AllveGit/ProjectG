using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class Healer : BasePlayer
{
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

    public override void Attack()
    {
        if (photonView.IsMine == false)
            return;

        animator.SetBool("Attack", true);

        StartCoroutine(DelayAttack(delegate (Vector3 direction)
        {
            if (direction == Vector3.zero)
                return;

            GameObject projectile = PhotonNetwork.Instantiate(
                "Skill/" + basicAttackPrefab.name,
                transform.position + transform.forward + new Vector3(0, 0.5f, 0),
                transform.rotation);

            if (projectile != null)
                projectile.GetComponent<Arrow>().Cast(this, AttackDamage, direction);

        }, SkillJoyStick.Amount, 0.6f));

    }
    public override void UltimateSkill()
    {
        if (!photonView.IsMine) return;
    }
}
