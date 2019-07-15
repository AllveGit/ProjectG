using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class Archer : BasePlayer
{
    private GameObject ExplosionPrefab;
 
    void Start()
    {
        ExplosionPrefab = Resources.Load<GameObject>("Effect/Explosion/Explosion");
    }

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
                projectile.GetComponent<Arrow>().Cast(this, AttackDamage, AttackDistance, transform.position + new Vector3(0f, 1f, 0f) + transform.forward * 1, direction);

        }, SkillJoyStick.JoyDir, AttackSpeed));
    }


    public override void UltimateSkill()
    {
        if (animator.GetBool("Attack"))
            return;

        animator.SetBool("Attack", true);

        StartCoroutine(DelayAttack(delegate (Vector3 direction)
        {
            if (direction == Vector3.zero)
                return;

            GameObject projectile = PhotonNetwork.Instantiate(
                "Skill/" + ultimateSkillPrefab.name,
               Vector3.zero,
                transform.rotation);

            if (projectile != null)
                projectile.GetComponent<IceArrow>().Cast(this, AttackDamage, AttackDistance,
                    transform.position + transform.forward + new Vector3(0, 0.5f, 0), direction);

        }, UltimateStick.JoyDir, 0.6f));
    }

    // Archer 캐릭터의 애니메이션이 90도 돌아가있어서 재정의해서 특수화함.
    public override void RotateCalculate()
    {
        if (animator.GetBool("Attack") == true) return;

        if (movementAmount != Vector3.zero)
        {
            Vector3 rot = Quaternion.LookRotation(movementAmount.normalized).eulerAngles;
            rot += new Vector3(0, -90, 0);
            rigidbody.rotation = Quaternion.Euler(rot.x,rot.y,rot.z);
        }
    }

}
