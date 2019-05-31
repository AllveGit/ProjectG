using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.Demo.PunBasics;

public abstract partial class BasePlayer : MonoBehaviourPunCallbacks, IPunObservable
{
    private Vector3 movementAmount = new Vector3(0f, 0f, 0f); // 플레이어의 이동량

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        playerCamera = GetComponent<PlayerCamera>();
    }

    private void Start()
    {
    }

    public void MoveCalculate()
    {
        float v = Input.GetAxis("Vertical"); // 수직
        float h = Input.GetAxis("Horizontal"); // 수평

        movementAmount = new Vector3(h, 0f, v).normalized * moveSpeed * Time.deltaTime;
    }
    public void RotateCalculate()
    {
        if (movementAmount.magnitude < 0.01f)
        {
            animator.SetFloat("Speed", 0);
            return;
        }

        animator.SetFloat("Speed", movementAmount.magnitude);
        rigidbody.velocity = movementAmount;
        rigidbody.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movementAmount), rotationLerpSpeed);
    }

    public abstract void OnPlayerDeath();
    public abstract void Attack();
    public abstract void UltimateSkill();

    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
    }
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
    [Range(0f, 1f)]
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

    public int CurHP { get => curHP; set => curHP = value; }
    public int MaxHP { get => maxHP; set => maxHP = value; }
    public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }
    public float AttackSpeed { get => attackSpeed; set => attackSpeed = value; }
    public int AttackDamage { get => attackDamage; set => attackDamage = value; }
    public int ShieldPower { get => shieldPower; set => shieldPower = value; }
    public int MaxShieldPower { get => maxShieldPower; set => maxShieldPower = value; }
}