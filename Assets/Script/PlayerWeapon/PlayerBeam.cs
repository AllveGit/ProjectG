using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class PlayerBeam : MonoBehaviour
{
    private float fSpeed { get; set; } = 20f;
    private float fRangeDistance { get; set; } = 100f;
    private float fMoveDistance = 0;
    private void Update()
    {
        if (fRangeDistance <= fMoveDistance)
            PhotonNetwork.Destroy(this.gameObject);

        Vector3 vDirection = transform.forward;
        vDirection.y = 0;

        float fDistance = fSpeed * Time.deltaTime;

        transform.position += vDirection * fDistance;

        fMoveDistance += fDistance;
    }
}
