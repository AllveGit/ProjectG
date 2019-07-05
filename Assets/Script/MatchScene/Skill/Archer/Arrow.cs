using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class Arrow : BaseAttack
{
    [SerializeField]
    private float rotationSpeedDegree = 10f;

    public override void Move()
    {
        base.Move();

        if (transform.rotation.y >= 360)
            transform.rotation = Quaternion.Euler(Vector3.zero);

        transform.Rotate(new Vector3(0f, 0f, rotationSpeedDegree));
    }
    
}
