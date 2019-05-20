using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class PlayerAnimatorManager : MonoBehaviourPun
{
    private Animator animator;

    [SerializeField]
    private float directionDampTime = 0.25f;

    void Start()
    {
        animator = GetComponent<Animator>();
        if (!animator)
            Debug.LogError("PlayerAnimator is Missing Animator Component", this);
    }
    void Update()
    {
        if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
            return;

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        if (v < 0)
            v = 0;

        animator.SetFloat("Speed", h * h + v * v);
        // 세번째인자 : 두번째 인자까지 도달하는시간
        animator.SetFloat("Direction", h, directionDampTime, Time.deltaTime);

        // 현재 애니메이션의 정보를 얻어옵니다.
        AnimatorStateInfo stateinfo = animator.GetCurrentAnimatorStateInfo(0);

        // 현재애니메이터가 해당 상태인지 판단합니다. (LayerName.StateName)
        if (stateinfo.IsName("Base Layer.Run"))
        {
            if (Input.GetButtonDown("Fire2"))
                animator.SetTrigger("Jump");
        }
    }
}
