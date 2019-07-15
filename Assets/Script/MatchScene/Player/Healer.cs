using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class Healer : BasePlayer
{
    public AudioClip AttackSound;

    public override void Attack()
    {
        animator.SetBool("Attack", true);

        StartCoroutine(DelayAttack(delegate (Vector3 direction)
        {
            if (direction == Vector3.zero)
                return;

            // 회전 행렬을 만듭니다.
            Matrix4x4 matRot = Matrix4x4.identity;
            matRot = Matrix4x4.Rotate(transform.rotation);

            GameObject projectile = PhotonNetwork.Instantiate(
                "Skill/" + basicAttackPrefab.name,
              Vector3.zero,
              Quaternion.identity);

            if (projectile != null)
                projectile.GetComponent<HealerBullet>().Cast(this, AttackDamage, AttackDistance,
                    transform.position + matRot.MultiplyPoint(new Vector3(0.2f, 1f, 1.0f)),  direction);

     
            projectile = PhotonNetwork.Instantiate(
                "Skill/" + basicAttackPrefab.name,
              Vector3.zero,
              Quaternion.identity);

            if (projectile != null)
                projectile.GetComponent<HealerBullet>().Cast(this, AttackDamage, AttackDistance,
                    transform.position + matRot.MultiplyPoint(new Vector3(-0.2f, 1f, 1.0f)), direction);

            SoundManager.instance.PlayEffect(AttackSound);

        }, SkillJoyStick.JoyDir, AttackSpeed));
    }
    public override void UltimateSkill()
    {
        if (animator.GetBool("Attack"))
            return;

        animator.SetBool("Attack", true);

        StartCoroutine(DelayAttack(delegate (Vector3 direction)
        {
            GameObject heal = PhotonNetwork.Instantiate(
                "Skill/" + ultimateSkillPrefab.name,
               transform.position,
                transform.rotation);

            if (heal != null)
                heal.GetComponent<HealSkill>().Cast(this, AttackDamage, 0, transform.position, direction);

        }, UltimateStick.JoyDir, 0.6f));
    }
}
