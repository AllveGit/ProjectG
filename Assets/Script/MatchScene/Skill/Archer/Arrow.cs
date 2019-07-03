using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class Arrow : BaseAttack
{
    [SerializeField]
    private float rotationSpeedDegree = 10f;

    
    new void Update()
    {
        if (!photonView.IsMine) return;

        Quaternion q = new Quaternion();

        if (transform.rotation.y >= 360)
            transform.rotation.SetEulerAngles(new Vector3(0f, 0f, 0f));

        transform.Rotate(new Vector3(0f, 0f, rotationSpeedDegree));

        base.Update();
    }
    
}
