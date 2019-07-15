using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class Maid : BasePlayer
{
    public AudioClip AttackSound;

    // 연사 카운트
    [SerializeField]
    private int currentSpeakerCount = 0;
    [SerializeField]
    private int maxSpeakerCount = 5;

    private float speakerDelay = 0.1f;

    public override void Attack()
    {
        animator.SetBool("Attack", true);

        StartCoroutine(DelayAttack(delegate (Vector3 direction)
        {
            if (direction == Vector3.zero)
                return;

            StartCoroutine(MaidAttack(direction));

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
                projectile.GetComponent<Bam>().Cast(this, AttackDamage * 10, AttackDistance * 10,
                    transform.position + transform.forward + new Vector3(0, 1.0f, 0), direction);

        }, UltimateStick.JoyDir, 0.6f));
    }


    // 연사 기본공격
    IEnumerator MaidAttack(Vector3 direction)
    {
        currentSpeakerCount = 0;

        while(currentSpeakerCount < maxSpeakerCount)
        {
            GameObject projectile = PhotonNetwork.Instantiate(
            "Skill/" + basicAttackPrefab.name,
            Vector3.zero,
            transform.rotation);

            if (projectile != null)
                projectile.GetComponent<MaidBullet>().Cast(this, AttackDamage, AttackDistance,
                    transform.position + new Vector3(0f, 1f, 0f) + transform.forward * 1.5f, direction);
            ++currentSpeakerCount;

            SoundManager.instance.PlayEffect(AttackSound);

            yield return new WaitForSeconds(speakerDelay);
        }

        yield break;
    }
}
