using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.Demo.PunBasics;

public abstract partial class BasePlayer : MonoBehaviourPun, IPunObservable
{
    private void Awake()
    {
        MoveJoyStick    = GameObject.FindGameObjectWithTag("JoyStick").GetComponent<JoyStick>();
        SkillJoyStick   = GameObject.FindGameObjectWithTag("SkillJoyStick").GetComponent<JoyStick>();

        rigidbody       = GetComponent<Rigidbody>();
        if (rigidbody == null)
            Debug.LogError("BasePlayer.cs / rigidbody을 가져오지 못했습니다.");
        animator        = GetComponent<Animator>();
        if (animator == null)
            Debug.LogError("BasePlayer.cs / animator를 가져오지 못했습니다.");
        collider = GetComponent<CapsuleCollider>();
        if (collider == null)
            Debug.LogError("BasePlayer.cs / collider를 가져오지 못했습니다.");

        if (photonView.IsMine)
        {
            playerCamera = GetComponent<PlayerCamera>();
            playerCamera.TargetObject   = gameObject;
            playerCamera.IsTargeting    = true;
        }

        if (MoveJoyStick == null)
            Debug.LogError("BasePlayer.cs / JoyStick을 가져오지 못했습니다.");
        if (SkillJoyStick == null)
            Debug.LogError("BasePlayer.cs / SkillJoyStick을 가져오지 못했습니다.");


        // 이벤트 핸들러에 등록
        SkillJoyStick.OnStickUp     += OnSkillJoyStickUp;
        SkillJoyStick.OnStickDown   += OnSkillJoyStickDown;


        if (photonView.IsMine)
        {
            GameObject line = Instantiate(attackLinePrefab);
            line.transform.parent = transform ;
            attackLine = line.GetComponent<AttakLine>();
            attackLine.gameObject.SetActive(false);
        }
    }
    public void PlayerInit(Enums.TeamOption team, Vector3 pos)
    {
        transform.position = pos;
        playerTeam = team;

        photonView.RPC("RPCTranslatePosition", RpcTarget.Others, transform.position);
    }

    protected void FixedUpdate()
    {
        if (photonView.IsMine)
        {
            MoveCalculate();
            RotateCalculate();
            AttackRangeCalculate();
        }
        else
        {
            if (Vector3.Distance(transform.position, currentPosition) > 3)
            {
                transform.position = currentPosition;
                photonView.RPC("RPCTranslatePosition", RpcTarget.Others, transform.position);
            }

            transform.position = Vector3.MoveTowards(transform.position, currentPosition, moveSpeed * Time.deltaTime);
            transform.rotation = currentRotation;
        }
    }


    public void MoveCalculate()
    {
        if (animator.GetBool("Attack") || animator.GetBool("Death"))
        {
            movementAmount = Vector3.zero;
            animator.SetFloat("Speed", 0f);
    
            return;
        }


        movementAmount = MoveJoyStick.JoyDir * (moveSpeed * MoveJoyStick.JoyScale) * Time.deltaTime;
        animator.SetFloat("Speed", movementAmount.magnitude * 10);

        rigidbody.MovePosition(transform.position + movementAmount);
    }

    public virtual void RotateCalculate()
    {
        if (animator.GetBool("Attack") || animator.GetBool("Death")) return;

        if (movementAmount != Vector3.zero)
            rigidbody.rotation = Quaternion.LookRotation(movementAmount);
    }

    /*
     * 공격 시 공격 범위 뜨는 계산.
     */
    public virtual void AttackRangeCalculate()
    {
        if (isFocusOnAttack)
        {
            attackLine.transform.rotation = Quaternion.LookRotation(SkillJoyStick.JoyDir);

            Vector3 vlocalPosition = SkillJoyStick.JoyDir * ((AttackDistance / 2f) + 1f);
            vlocalPosition.y = 0.1f;
            attackLine.transform.position = transform.position + vlocalPosition;
        }
    }

    private void AttackBehavior()
    {
        if (photonView.IsMine == false)
            return;

        if (animator.GetBool("Attack") || animator.GetBool("Death"))
            return;

        attackDirection = SkillJoyStick.JoyDir;
        rigidbody.rotation = Quaternion.LookRotation(attackDirection);
        Attack();
    }

    private void UltimateBehavior()
    {
        if (photonView.IsMine == false)
            return;

        if (animator.GetBool("Attack") || animator.GetBool("Death"))
            return;

        UltimateSkill();
    }
    public void OnDamaged(int damage)
    {
        photonView.RPC("RPCOnDamage", RpcTarget.Others, damage);
    }

    public void OnHeal(int heal)
    {
        photonView.RPC("RPCOnHeal", RpcTarget.Others, heal);
    }

    public abstract void Attack();          // 기본공격을 사용하기 위한 함수
    public abstract void UltimateSkill();   // 궁극기 스킬을 사용하기 위한 함수
    public virtual void OnPlayerDeath()   // 플레이어가 죽을 때 호출됨
    {
        if (animator.GetBool("Death") == true)
            return;

        
        GameManager.Instance.DeathPlayer();
   
        collider.enabled = false;
        rigidbody.useGravity = false;

        animator.SetBool("Death", true);
        StartCoroutine(Respawn(5f));
    }

    public IEnumerator DelayAttack(AttackCallback attackCallback, Vector3 direction, float delay)
    {
        yield return new WaitForSeconds(delay);
        attackCallback(direction);
        yield break;
    }
    public IEnumerator Respawn(float delay)
    {
        yield return new WaitForSeconds(delay);
        animator.SetBool("Death", false);

        CurHP = MaxHP;
        ShieldPower = MaxShieldPower;

        transform.position = GameManager.Instance.GetRespawnPos();
        photonView.RPC("RPCTranslatePosition", RpcTarget.Others, transform.position);

        collider.enabled = true;
        rigidbody.useGravity = true;

        yield break;
    }

    //Debug함수입니다.
    void OnGUI()
    {
        if (photonView.IsMine)
            GUILayout.TextField(playerTeam.ToString() + " HP : " + CurHP.ToString() + " Shield : " + ShieldPower.ToString());
    }
}

public abstract partial class BasePlayer
{
    private Vector3 currentPosition = Vector3.zero;
    private Quaternion currentRotation = Quaternion.identity;
    public virtual void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // 전송, 수신은 순서 맞춰서
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);

            stream.SendNext(ShieldPower);
            stream.SendNext(CurHP);
        }
        else
        {
            currentPosition = (Vector3)stream.ReceiveNext();
            currentRotation = (Quaternion)stream.ReceiveNext();

            ShieldPower = (int)stream.ReceiveNext();
            CurHP = (int)stream.ReceiveNext();
        }
    }

    [PunRPC]
    protected virtual void RPCOnDamage(int damage)
    {
        if (photonView.IsMine)
        {
            ShieldPower -= damage;

            if (ShieldPower < 0)
            {
                CurHP -= Mathf.Abs(ShieldPower);
                ShieldPower = 0;
            }

            if (CurHP <= 0)
            {
                CurHP = 0;
                OnPlayerDeath();
            }
        }
    }
    [PunRPC]
    protected virtual void RPCOnHeal(int heal)
    {
        if (photonView.IsMine)
        {
            CurHP += heal;

            if (CurHP > maxHP)
                CurHP = maxHP;
        }
    }
    [PunRPC]
    protected virtual void RPCTranslatePosition(Vector3 translationPos)
    {
        currentPosition = translationPos; 
        transform.position = translationPos;
    }

}

public abstract partial class BasePlayer
{
    protected JoyStick MoveJoyStick = null;

    protected JoyStick SkillJoyStick = null;


    public void OnSkillJoyStickUp(Vector3 pos, Vector3 dir)
    {
        if (!photonView.IsMine) return;

        attackLine.gameObject.SetActive(false);
        isFocusOnAttack = false;

        AttackBehavior();

    }

    public void OnSkillJoyStickDown(Vector3 pos, Vector3 dir)
    {
        if (!photonView.IsMine) return;

        float scale = AttackDistance * 0.35f;
        attackLine.gameObject.SetActive(true);
        attackLine.transform.localScale = new Vector3(0.1f, 0.1f, scale);

        isFocusOnAttack = true;
    }
}


/*
 * BasePlayer 클래스의
 * 인스펙터 상 표기될 변수 목록입니다.
 */
public abstract partial class BasePlayer
{
    public delegate void AttackCallback(Vector3 direction);

    public Enums.TeamOption playerTeam { get; set; }



    protected Vector3 movementAmount = Vector3.zero; // 플레이어의 이동량

    protected Vector3 attackDirection = Vector3.zero;

    protected bool isFocusOnAttack = false;

    [SerializeField]
    private GameObject attackLinePrefab = null;
    private AttakLine attackLine = null;

    [SerializeField]
    protected GameObject ultimateSkillPrefab = null; // 궁극기 프리펩
    [SerializeField]
    protected GameObject basicAttackPrefab = null; // 평타 프리팹

    [SerializeField]
    private int curHP = 0; // 플레이어의 HP

    [SerializeField]
    private int maxHP = 0; // 플레이어의 HP 최대치

    [SerializeField]
    private float moveSpeed = 0; // 플레이어의 이동 속도

    [SerializeField]
    private int attackDamage = 0; // 플레이어의 공격력

    [SerializeField]
    private float attackDistance = 10f;

    [SerializeField]
    private int shieldPower = 0; // 플레이어의 쉴드(추가 체력)

    [SerializeField]
    private int maxShieldPower = 0; // 플레이어의 쉴드(추가 체력) 최대치


}

/*
 * BasePlayer 클래스의 프로퍼티 모음입니다.
 */
public abstract partial class BasePlayer
{
    public new Rigidbody rigidbody { get; private set; } = null;
    public Animator animator { get; private set; } = null;
    public CapsuleCollider collider { get; private set; } = null;
    public PlayerCamera playerCamera { get; private set; } = null;

    public bool IsBush { get; set; } = false;


    public int CurHP { get => curHP; set => curHP = value; }
    public int MaxHP { get => maxHP; set => maxHP = value; }
    public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }
    public int AttackDamage { get => attackDamage; set => attackDamage = value; }
    public float AttackDistance { get => attackDistance; set => attackDistance = value; }
    public int ShieldPower { get => shieldPower; set => shieldPower = value; }
    public int MaxShieldPower { get => maxShieldPower; set => maxShieldPower = value; }
}