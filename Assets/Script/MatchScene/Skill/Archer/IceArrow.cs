using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon;
using Photon.Pun;

public class IceArrow : BaseAttack
{

    // Update is called once per frame
    new void Update()
    {
        if (!photonView.IsMine) return;

        rigidbody.MovePosition(
        transform.position
        + direction * ProjectileSpeed * Time.deltaTime);
    }

}
