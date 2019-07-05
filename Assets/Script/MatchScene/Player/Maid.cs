﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class Maid : BasePlayer
{
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

        }, SkillJoyStick.JoyDir, 0.4f));
    }

    public override void UltimateSkill()
    {
        if (!photonView.IsMine) return;
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
                projectile.GetComponent<MaidBullet>().Cast(this, AttackDamage, 
                    transform.position + new Vector3(0f, 1f, 0f) + transform.forward * 1.5f, direction);
            ++currentSpeakerCount;

            yield return new WaitForSeconds(speakerDelay);
        }

        yield break;
    }
}
