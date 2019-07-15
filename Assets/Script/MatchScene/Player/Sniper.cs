using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class Sniper : BasePlayer
{
    public override void Attack()
    {
        animator.SetBool("Attack", true);

        StartCoroutine(DelayAttack(delegate (Vector3 direction)
        {
            if (direction == Vector3.zero)
                return;

            GameObject projectile = PhotonNetwork.Instantiate(
                "Skill/" + basicAttackPrefab.name,
                Vector3.zero,
                transform.rotation);

            if (projectile != null)
                projectile.GetComponent<Arrow>().Cast(this, AttackDamage, AttackDistance,
                    transform.position + transform.forward + new Vector3(0, 0.5f, 0), direction);

        }, SkillJoyStick.JoyDir, AttackSpeed));
    }

    public override void UltimateSkill()
    {
        if (!photonView.IsMine) return;

    }
}
