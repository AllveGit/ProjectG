using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class PlayerBeam : MonoBehaviour
{
    private float moveSpeed { get; set; } = 20f;
    private float rangeDistance { get; set; } = 100f;
    private float moveDistance = 0;
    private void Update()
    {
        if (rangeDistance <= moveDistance)
            PhotonNetwork.Destroy(this.gameObject);

        Vector3 vDirection = transform.forward;
        vDirection.y = 0;

        float fDistance = moveSpeed * Time.deltaTime;

        transform.position += vDirection * fDistance;

        moveDistance += fDistance;
    }
}
