﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.Demo.PunBasics;

public abstract partial class BasePlayer : MonoBehaviourPunCallbacks, IPunObservable
{

    #region PhotonCallBack
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // 전송, 수신은 순서 맞춰서
        if (stream.IsWriting)
        {
            // 메시지 전송
            // stream.SendNext()
        }
        else
        {
            // 메시지 수신
            // stream.ReceiveNext();
        }
    }

    #endregion
    protected JoyStick MoveJoyStick = null;
    protected JoyStick SkillJoyStick = null;
    protected Vector3 movementAmount = Vector3.zero; // 플레이어의 이동량
    protected Vector3 attackDirection = Vector3.zero;

    protected bool isFocusOnAttack = false;

    [SerializeField]
    protected GameObject ultimateSkillPrefab = null; // 궁극기 프리펩
    [SerializeField]
    protected GameObject basicAttackPrefab = null; // 평타 프리팹
    private void Awake()
    {
        MoveJoyStick    = GameObject.FindGameObjectWithTag("JoyStick").GetComponent<JoyStick>();
        SkillJoyStick   = GameObject.FindGameObjectWithTag("SkillJoyStick").GetComponent<JoyStick>();

        rigidbody       = GetComponent<Rigidbody>();
        if (rigidbody == null)
            Debug.LogError("BasePlayer.cs : 19 / rigidbody을 가져오지 못했습니다.");
        animator        = GetComponent<Animator>();
        if (animator == null)
            Debug.LogError("BasePlayer.cs : 22 / animator를 가져오지 못했습니다.");
        photonView      = GetComponent<PhotonView>();
        if (photonView == null)
            Debug.LogError("BasePlayer.cs : 25 / photonView을 가져오지 못했습니다.");

        if(photonView.IsMine)
        {
            playerCamera = GetComponent<PlayerCamera>();
            playerCamera.TargetObject   = gameObject;
            playerCamera.IsTargeting    = true;
        }

        if (MoveJoyStick == null)
            Debug.LogError("BasePlayer.cs : 35 / JoyStick을 가져오지 못했습니다.");
        if (SkillJoyStick == null)
            Debug.LogError("BasePlayer.cs : 39 / SkillJoyStick을 가져오지 못했습니다.");

        // 이벤트 핸들러에 등록
        SkillJoyStick.OnUpEvent     += OnSkillJoyStickUp;
        SkillJoyStick.OnDownEvent   += OnSkillJoyStickDown;
    }

    public void MoveCalculate()
    {
        // 플레이어 조작에 해당되는 구문은 이 조건문을 꼭 씌워줄것
        if (photonView.IsMine)
        {
            // 키보드
            float v = Input.GetAxis("Vertical"); // 수직
            float h = Input.GetAxis("Horizontal"); // 수평
            movementAmount = new Vector3(h, 0f, v).normalized * moveSpeed * Time.deltaTime;
            
            movementAmount = MoveJoyStick.Amount * moveSpeed * Time.deltaTime;

            attackDirection = SkillJoyStick.Amount;
        }
    }

    public virtual void RotateCalculate()
    {
        // 플레이어 조작에 해당되는 구문은 이 조건문을 꼭 씌어줄것
        if (photonView.IsMine)
        {
            if(isFocusOnAttack)
                rigidbody.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(attackDirection), rotationLerpSpeed);

            if (movementAmount.magnitude < 0.01f)
            {
                animator.SetFloat("Speed", 0);
                return;
            }

            animator.SetFloat("Speed", movementAmount.magnitude * 10);
            rigidbody.MovePosition(transform.position + movementAmount);

            if(isFocusOnAttack == false)
                rigidbody.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movementAmount), rotationLerpSpeed);

            Debug.Log(isFocusOnAttack);
        }
    }

    public void OnSkillJoyStickUp(Vector3 pedPosition)
    {
        Attack();
        isFocusOnAttack = false;
    }

    public void OnSkillJoyStickDown(Vector3 pedPosition)
    {
        isFocusOnAttack = true;
    }

    public abstract void Attack();          // 기본공격을 사용하기 위한 함수
    public abstract void UltimateSkill();   // 궁극기 스킬을 사용하기 위한 함수
    public abstract void OnPlayerDeath();   // 플레이어가 죽을 때 호출됨

    // 플레이어가 공격당할 때 호출됨
    public void OnAttacked(int damage)
    {
        shieldPower -= damage;

        if (shieldPower < 0)
        {
            CurHP -= shieldPower;
            shieldPower = 0;
        }

        if (CurHP < 0)
            CurHP = 0;
    }

    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {}
}

/*
 * BasePlayer 클래스의
 * 인스펙터 상 표기될 변수 목록입니다.
 */
public abstract partial class BasePlayer
{
    [SerializeField]
    private int curHP = 0; // 플레이어의 HP

    [SerializeField]
    private int maxHP = 0; // 플레이어의 HP 최대치

    [SerializeField]
    private float moveSpeed = 0; // 플레이어의 이동 속도

    [SerializeField]
    private float attackSpeed = 0; // 플레이어의 공격 속도

    [SerializeField]
    private int attackDamage = 0; // 플레이어의 공격력

    [SerializeField]
    private int shieldPower = 0; // 플레이어의 쉴드(추가 체력)

    [SerializeField]
    private int maxShieldPower = 0; // 플레이어의 쉴드(추가 체력) 최대치

    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float rotationLerpSpeed = 0f; // 플레이어의 회전 보간 속도

}

/*
 * BasePlayer 클래스의 프로퍼티 모음입니다.
 */
public abstract partial class BasePlayer
{
    public new Rigidbody rigidbody { get; private set; } = null;
    public Animator animator { get; private set; } = null;
    public PlayerCamera playerCamera { get; private set; } = null;
    public new PhotonView photonView { get; private set; } = null;

    public int CurHP { get => curHP; set => curHP = value; }
    public int MaxHP { get => maxHP; set => maxHP = value; }
    public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }
    public float AttackSpeed { get => attackSpeed; set => attackSpeed = value; }
    public int AttackDamage { get => attackDamage; set => attackDamage = value; }
    public int ShieldPower { get => shieldPower; set => shieldPower = value; }
    public int MaxShieldPower { get => maxShieldPower; set => maxShieldPower = value; }
    public float RotationLerpSpeed { get => rotationLerpSpeed; set => rotationLerpSpeed = value; }
}