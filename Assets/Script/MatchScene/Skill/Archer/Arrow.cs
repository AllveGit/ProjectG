using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class Arrow : BaseAttack
{
    [SerializeField]
    private float rotationSpeedDegree = 10f;

    public override void GiveAttack(BasePlayer player)
    { 
         base.GiveAttack(player);
    }

    
    void Update()
    {
        Quaternion q = new Quaternion();

        if (transform.rotation.y >= 360)
            transform.rotation.SetEulerAngles(new Vector3(0f, 0f, 0f));

        transform.Rotate(new Vector3(0f, 0f, rotationSpeedDegree));

        if (!photonView.IsMine) return;

        rigidbody.MovePosition(
           transform.position
           + direction * ProjectileSpeed * Time.deltaTime);
    }
    
}
