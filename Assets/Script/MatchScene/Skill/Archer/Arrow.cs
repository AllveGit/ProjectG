using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class Arrow : BaseAttack
{ 
    public override void GiveAttack(BasePlayer player)
    { 
         base.GiveAttack(player);
    }

    
    void Update()
    {
        if (!photonView.IsMine) return;

        rigidbody.MovePosition(
           transform.position
           + direction * ProjectileSpeed * Time.deltaTime);
    }
    
}
