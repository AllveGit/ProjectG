using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class Archer : BasePlayer
{
    private GameObject ExplosionPrefab;
    delegate void AttackCallback(Vector3 direction);
 
    void Start()
    {
        ExplosionPrefab = Resources.Load<GameObject>("Effect/Explosion/Explosion");
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
        if (photonView.IsMine == false)
            return;

        animator.SetBool("Attack", true);

        StartCoroutine(DelayAttack(delegate (Vector3 direction)
        {
            if (direction == Vector3.zero)
                return;

            GameObject projectile = PhotonNetwork.Instantiate(
                "Skill/" + ultimateSkillPrefab.name,
                transform.position + transform.forward + new Vector3(0, 0.5f, 0),
                transform.rotation);

            if (projectile != null)
                projectile.GetComponent<IceArrow>().Cast(this, AttackDamage, direction);
        }, SkillJoyStick.Amount, 0.6f));
    }

    // Archer 캐릭터의 애니메이션이 90도 돌아가있어서 재정의해서 특수화함.
    public override void RotateCalculate()
    {
        // 플레이어 조작에 해당되는 구문은 이 조건문을 꼭 씌어줄것

        if (!photonView.IsMine) return;

        if (isFocusOnAttack)
        {
            rigidbody.rotation = Quaternion.Slerp(transform.rotation,
                Quaternion.LookRotation(attackDirection), RotationLerpSpeed);
        }

        if (movementAmount.magnitude < 0.01f)
        {
            animator.SetFloat("Speed", 0);
            return;
        }

        animator.SetFloat("Speed", movementAmount.magnitude * 10);
        rigidbody.MovePosition(transform.position + movementAmount);

        Vector3 rot = Quaternion.LookRotation(movementAmount.normalized).eulerAngles;
        rot += new Vector3(0, -90, 0);

        if (isFocusOnAttack == false)
            rigidbody.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(rot), RotationLerpSpeed);

    }


    IEnumerator DelayAttack(AttackCallback attackCallback, Vector3 direction, float delay)
    {
        yield return new WaitForSeconds(delay);
        attackCallback(direction);
        yield break;
    }
}
