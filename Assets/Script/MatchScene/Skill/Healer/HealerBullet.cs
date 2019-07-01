using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealerBullet : BaseAttack
{
    void Update()
    {
        if (!photonView.IsMine) return;

        rigidbody.MovePosition(
        transform.position
        + direction * ProjectileSpeed * Time.deltaTime);
    }
}
