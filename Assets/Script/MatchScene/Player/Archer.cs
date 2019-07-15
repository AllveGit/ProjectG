using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class Archer : BasePlayer
{
    public AudioClip AttackSound;

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
                projectile.GetComponent<Arrow>().Cast(this, AttackDamage, AttackDistance,
                    transform.position + new Vector3(0f, 1.0f, 0f) + transform.forward, direction);

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
                projectile.GetComponent<IceArrow>().Cast(this, AttackDamage * 5, AttackDistance,
                    transform.position + transform.forward + new Vector3(0, 1.0f, 0), direction);

            SoundManager.instance.PlayEffect(AttackSound);

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

    public override void OnPlayerDeath()   // 플레이어가 죽을 때 호출됨
    {
        if (animator.GetBool("Death") == true || OnDeath)
            return;

        bushUnActiveRendererEvent?.Invoke();

        GameManager.Instance.DeathPlayer();

        OnDeath = true;
        collider.enabled = false;
        rigidbody.useGravity = false;
        bushCollider.DieProccess();

        if (TempDieEffect == null)
        {
            TempDieEffect = Instantiate(DieEffect, transform.position, Quaternion.identity, Camera.main.transform) as GameObject;
            TempDieEffect.transform.localPosition = Vector3.zero;
            TempDieEffect.transform.localEulerAngles = Vector3.zero;
            TempDieEffect.transform.localPosition += Vector3.forward * 3;
        }
        StartCoroutine(Respawn(5f));
        StartCoroutine(TempDieEffect.GetComponent<DeathEffect>().ActiveFalse(4.9f));
    }

    public override IEnumerator Respawn(float delay)
    {
        yield return new WaitForSeconds(delay);
        bushActiveRendererEvent?.Invoke();

        CurHP = MaxHP;
        ShieldPower = MaxShieldPower;
        photonView.RPC("RPCSetHpAndShield", RpcTarget.All, CurHP, ShieldPower);

        transform.position = GameManager.Instance.GetRespawnPos();
        photonView.RPC("RPCTranslatePosition", RpcTarget.Others, transform.position);

        OnDeath = false;
        collider.enabled = true;
        rigidbody.useGravity = true;
        bushCollider.RespawnProccess();

        yield break;
    }
}
