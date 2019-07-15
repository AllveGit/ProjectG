using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealSkill : BaseAttack
{
    private bool isCanHeal = false;
    private bool useHealTick = false;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (useHealTick == true)
        {
            isCanHeal = false;
            useHealTick = false;
        }

        transform.position = ownerPlayer.transform.position;
    }

    public override void Cast(BasePlayer inOwnerPlayer, int inAttackDamage, float attackDistance, Vector3 vStartPosition, Vector3 direction)
    {
        useDistance = false;
        useAutoDeleteOnObstacle = false;
        base.Cast(inOwnerPlayer, inAttackDamage, attackDistance, vStartPosition, direction);

        StartCoroutine("Healing", 23);
    }

    public override bool BaseCollisionProcess(BasePlayer player)
    {
        if (IsAttackable(PhotonNetwork.LocalPlayer, player.photonView.Owner) == false)
        {
            if (isCanHeal == true)
            {
                useHealTick = true;
                player.OnHeal(AttackDamage);
            }
        }

        return false;
    }

    public IEnumerator Healing(int tickCount)
    {
        for (int i = 0; i < tickCount; i++)
        {
            isCanHeal = true;

            yield return new WaitForSeconds(0.2f);
        }

        if (photonView.IsMine == true)
        {
            PhotonNetwork.Destroy(gameObject);
        }

        yield break;
    }

    public void OnDestroy()
    {
        
    }
}
