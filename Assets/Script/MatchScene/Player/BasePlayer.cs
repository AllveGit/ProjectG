using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.Demo.PunBasics;

public abstract partial class BasePlayer : MonoBehaviourPunCallbacks, IPunObservable
{
    protected GameObject DieEffect;
    protected GameObject TempDieEffect = null;
    public GameObject DmgPopup;
    
    private void Awake()
    {
        DmgPopup = Resources.Load<GameObject>("Effect/DmgPopup/PopupText");
        DieEffect = Resources.Load<GameObject>("Effect/DieEffect/DieEffect");

        rigidbody = GetComponent<Rigidbody>();
        if (rigidbody == null)
            Debug.LogError("BasePlayer.cs / rigidbody을 가져오지 못했습니다.");
        animator = GetComponent<Animator>();
        if (animator == null)
            Debug.LogError("BasePlayer.cs / animator를 가져오지 못했습니다.");
        collider = GetComponent<CapsuleCollider>();
        if (collider == null)
            Debug.LogError("BasePlayer.cs / collider를 가져오지 못했습니다.");
        bushCollider = transform.FindChild("BushCollCheck").GetComponent<BushCollider>();
        if (bushCollider == null)
            Debug.LogError("BushPlayer.cs / BushCollCheck 자식 오브젝트를 가져오지 못했습니다.");


        if (photonView.IsMine)
        {
            playerCamera = GetComponent<PlayerCamera>();
            playerCamera.TargetObject = gameObject;
            playerCamera.IsTargeting = true;


            MoveJoyStick = GameObject.FindGameObjectWithTag("JoyStick").GetComponent<JoyStick>();
            if (MoveJoyStick == null)
                Debug.LogError("BasePlayer.cs / JoyStick을 가져오지 못했습니다.");
            SkillJoyStick = GameObject.FindGameObjectWithTag("SkillJoyStick").GetComponent<JoyStick>();
            if (SkillJoyStick == null)
                Debug.LogError("BasePlayer.cs / SkillJoyStick을 가져오지 못했습니다.");
            UltimateStick = GameObject.FindGameObjectWithTag("UltimateAttackStick").GetComponent<JoyStick>();
            if (UltimateStick == null)
                Debug.LogError("BasePlayer.cs / UltimateStick 가져오지 못했습니다.");

            UltimateStick.transform.GetChild(0).transform.GetChild(0).GetComponent<CooldownRender>().targetPlayer = this;

            // 이벤트 핸들러에 등록
            SkillJoyStick.OnStickUp += OnSkillJoyStickUp;
            SkillJoyStick.OnStickDown += OnSkillJoyStickDown;

            UltimateStick.OnStickUp += OnUltimateStickUp;
            UltimateStick.OnStickDown += OnUltimateStickDown;

            currentUltimateCooldown = ultimateCooldown;

            GameObject line = Instantiate(attackLinePrefab);
            line.transform.parent = transform;
            attackLine = line.GetComponent<AttakLine>();
            attackLine.gameObject.SetActive(false);

            photonView.RPC("CreateBar", RpcTarget.All, (Enums.TeamOption)PhotonNetwork.LocalPlayer.CustomProperties[Enums.PlayerProperties.TEAM.ToString()]);
        }


        bushCollider.onBushEnter += () => 
        {
            if (photonView.IsMine)
            {
                OnBush = true;
                photonView.RPC("RPCOnBushEnter", RpcTarget.Others);
            }
        };
        bushCollider.onBushExit += () => 
        {
            if (photonView.IsMine)
            {
                OnBush = false;
                photonView.RPC("RPCOnBushExit", RpcTarget.Others);
            }
        };

        bushCollider.onBushColliderEnter += () =>
        {
            if (photonView.IsMine)
                photonView.RPC("RPCOnBushColliderEnter", RpcTarget.Others);
        };
        bushCollider.OnBushColliderExit += () =>
        {
            if (photonView.IsMine)
                photonView.RPC("RPCOnBushColliderExit", RpcTarget.Others);
        };
    }
    public void PlayerInit(Enums.TeamOption team, Vector3 pos)
    {
        transform.position = pos;

        photonView.RPC("RPCTranslatePosition", RpcTarget.Others, transform.position);
    }

    protected void FixedUpdate()
    {
        if (photonView.IsMine)
        {
            MoveCalculate();
            RotateCalculate();
            AttackRangeCalculate();
            UltimateRangeCalculate();
        }
        else
        {
            if (Vector3.Distance(transform.position, currentPosition) > 3)
            {
                transform.position = currentPosition;
                photonView.RPC("RPCTranslatePosition", RpcTarget.Others, transform.position);
            }
            else
            {
                if (Vector3.Distance(transform.position, currentPosition) > 0.1f)
                    transform.position = Vector3.MoveTowards(transform.position, currentPosition, moveSpeed * Time.deltaTime);

                    transform.rotation = currentRotation;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (photonView.IsMine) return;

        if (other.CompareTag("Bush") && OnBush)
        {
            BasePlayer otherBasePlayer = other.transform.parent.GetComponent<BasePlayer>();

            if (otherBasePlayer.photonView.IsMine)
            {
                if (otherBasePlayer.OnBush)
                {
                    otherBasePlayer.bushActiveRendererEvent?.Invoke();
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (photonView.IsMine) return;

        if (other.CompareTag("Bush") && OnBush)
        {
            BasePlayer otherBasePlayer = other.transform.parent.GetComponent<BasePlayer>();
            
            if (otherBasePlayer.photonView.IsMine)
            {
                if (otherBasePlayer.OnBush)
                {
                    otherBasePlayer.bushActiveRendererEvent?.Invoke();
                }
            }


        }
    }


    public void MoveCalculate()
    {
        if (animator.GetBool("Attack") || animator.GetBool("Death") || OnDeath)
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

        if (currentUltimateCooldown > 0)
        {
            currentUltimateCooldown -= Time.fixedDeltaTime;
        }
    }
    public virtual void UltimateRangeCalculate()
    {
        if (isFocusOnUltimateAttack)
        {
            attackLine.transform.rotation = Quaternion.LookRotation(UltimateStick.JoyDir);

            Vector3 vlocalPosition = UltimateStick.JoyDir * ((AttackDistance / 2f) + 1f);
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

    public IEnumerator DelayAttack(AttackCallback attackCallback, Vector3 direction, float delay)
    {
        yield return new WaitForSeconds(delay);
        attackCallback(direction);
        yield break;
    }

    private void UltimateBehavior()
    {
        if (photonView.IsMine == false)
            return;

        if (animator.GetBool("Attack") || animator.GetBool("Death"))
            return;

        if (currentUltimateCooldown > 0)
            return;

        attackDirection = UltimateStick.JoyDir;
        rigidbody.rotation = Quaternion.LookRotation(attackDirection);
        UltimateSkill();

        currentUltimateCooldown = ultimateCooldown;
    }
    public void OnDamaged(int damage)
    {
        photonView.RPC("RPCOnDamage", RpcTarget.All, damage) ;
    }

    public void OnHeal(int heal)
    {
        photonView.RPC("RPCOnHeal", RpcTarget.All, heal);
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
        bushCollider.DieProccess();

        animator.SetBool("Death", true);

        if (TempDieEffect == null)
        {
            TempDieEffect = Instantiate(DieEffect, transform.position, Quaternion.identity, Camera.main.transform) as GameObject;
            TempDieEffect.transform.localPosition = Vector3.zero;
            TempDieEffect.transform.localEulerAngles = Vector3.zero;
            TempDieEffect.transform.localPosition += Vector3.forward * 3;
        }
        StartCoroutine(Respawn(5f));
        StartCoroutine(TempDieEffect.GetComponent<DeathEffect>().ActiveFalse(4.9f));
    }
    public virtual IEnumerator Respawn(float delay)
    {
        yield return new WaitForSeconds(delay);

        CurHP = MaxHP;
        ShieldPower = MaxShieldPower;
        photonView.RPC("RPCSetHpAndShield", RpcTarget.All, CurHP, ShieldPower);

        transform.position = GameManager.Instance.GetRespawnPos();
        photonView.RPC("RPCTranslatePosition", RpcTarget.Others, transform.position);

        OnDeath = false;
        collider.enabled = true;
        rigidbody.useGravity = true;
        bushCollider.RespawnProccess();
        animator.SetBool("Death", false);

        yield break;
    }

    //Debug함수입니다.
    //void OnGUI()
    //{
    //    if (photonView.IsMine)
    //        GUILayout.TextField(playerTeam.ToString() + " HP : " + CurHP.ToString() + " Shield : " + ShieldPower.ToString());
    //}
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

            stream.SendNext(OnBush);
           
        }
        else
        {
            currentPosition = (Vector3)stream.ReceiveNext();
            currentRotation = (Quaternion)stream.ReceiveNext();

            OnBush = (bool)stream.ReceiveNext();
        }
    }

    public override void OnLeftRoom()
    {
        if (photonView.IsMine)
            photonView.RPC("DestroyBar", RpcTarget.All, (Enums.TeamOption)PhotonNetwork.LocalPlayer.CustomProperties[Enums.PlayerProperties.TEAM.ToString()]);
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
            //StartCoroutine(FindObjectOfType<PlayerCameraShake>().Shake(0.1f, 0.01f));
            photonView.RPC("RPCSetHpAndShield", RpcTarget.All, CurHP, ShieldPower);
            //photonView.RPC("RPCCreateDamageHit", RpcTarget.All, CurHP, damage);
        }
    }
    [PunRPC]
    protected virtual void RPCCreateDamageHit(int damage)
    {
        GameObject m_Canvas = GameObject.FindGameObjectWithTag("Canvas");
        GameObject Temp = Instantiate(DmgPopup, Vector3.zero, Quaternion.identity, m_Canvas.transform) as GameObject;
        Temp.transform.localPosition = Vector3.zero;

        Temp.transform.GetChild(0).GetComponent<DamagePopup>().SetText(damage, this.transform.position);
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
    protected virtual void RPCOnBushEnter()
    {
        OnBush = true;

        Enums.TeamOption localTeamOption 
            = (Enums.TeamOption)PhotonNetwork.LocalPlayer.CustomProperties[Enums.PlayerProperties.TEAM.ToString()];

        if (!localTeamOption.Equals(playerTeam))
        {
            if (playerBar)
                playerBar.gameObject.SetActive(false);

            bushUnActiveRendererEvent?.Invoke();
        }
        
    }
    [PunRPC]
    protected virtual void RPCOnBushColliderEnter()
    {
        if (!OnBush) return;

        Enums.TeamOption localTeamOption
        = (Enums.TeamOption)PhotonNetwork.LocalPlayer.CustomProperties[Enums.PlayerProperties.TEAM.ToString()];

        if (!localTeamOption.Equals(playerTeam))
        {
            if (playerBar)
                playerBar.gameObject.SetActive(true);

            bushActiveRendererEvent?.Invoke();
        }
    }
    [PunRPC]
    protected virtual void RPCOnBushExit()
    {
        OnBush = false;

        if (playerBar)
            playerBar.gameObject.SetActive(true);

        bushActiveRendererEvent?.Invoke();
    }

    [PunRPC]
    protected virtual void RPCOnBushColliderExit()
    {
        if (!OnBush) return;

        if (playerBar)
            playerBar.gameObject.SetActive(false);

        bushUnActiveRendererEvent?.Invoke();
    }


    [PunRPC]
    protected virtual void RPCTranslatePosition(Vector3 translationPos)
    {
        currentPosition = translationPos; 
        transform.position = translationPos;
    }
    [PunRPC]
    protected virtual void CreateBar(Enums.TeamOption teamOption)
    {
        playerTeam = teamOption;

        playerUIParent = GameObject.FindGameObjectWithTag("PlayerUI");
        GameObject barPrefab = Instantiate(Resources.Load("Player/PlayerBar"), playerUIParent.transform) as GameObject;
        playerBar = barPrefab.GetComponent<PlayerBar>();
        playerBar.BarInit(gameObject, playerTeam, maxHP, maxShieldPower);
    }
    [PunRPC]
    protected virtual void DestroyBar()
    {
        GameObject.Destroy(playerBar.gameObject);
        playerBar = null;
    }
    [PunRPC]
    protected virtual void RPCSetHpAndShield(int inHpBar, int inShield)
    {
        CurHP = inHpBar;
        ShieldPower = inShield;

        playerBar.UpdateHpBar(inHpBar);
        playerBar.UpdateShieldBar(inShield);
    }
}

public abstract partial class BasePlayer
{ 
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

    public void OnUltimateStickUp(Vector3 pos, Vector3 dir)
    {
        if (!photonView.IsMine) return;

        attackLine.gameObject.SetActive(false);
        isFocusOnUltimateAttack = false;

        UltimateBehavior();
    }
    public void OnUltimateStickDown(Vector3 pos, Vector3 dir)
    {
        if (!photonView.IsMine) return;

        float scale = AttackDistance * 0.35f;
        attackLine.gameObject.SetActive(true);
        attackLine.transform.localScale = new Vector3(0.1f, 0.1f, scale);

        isFocusOnUltimateAttack = true;
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
    protected bool isFocusOnUltimateAttack = false;

    protected JoyStick MoveJoyStick = null;
    protected JoyStick SkillJoyStick = null;
    protected JoyStick UltimateStick = null;
    

    public UnityEvent bushUnActiveRendererEvent = new UnityEvent(); // 부쉬에 들어갈 경우 렌더러를 끄는 이벤트입니다
    public UnityEvent bushActiveRendererEvent = new UnityEvent();// 위와 반대

    [SerializeField]
    private GameObject attackLinePrefab = null;
    private AttakLine attackLine = null;

    private GameObject playerUIParent; // Bar를 생성하여 모아놓는 게임오브젝트입니다.
    private PlayerBar playerBar;

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

    [SerializeField]
    private float attackSpeed = 0.0f; // 공속

    [SerializeField]
    private float ultimateCooldown = 0.0f; // 궁극기 쿨타임

    [SerializeField]
    private float currentUltimateCooldown = 0.0f; // 현재 궁극기 쿨타임
}

/*
 * BasePlayer 클래스의 프로퍼티 모음입니다.
 */
public abstract partial class BasePlayer
{
    public new Rigidbody rigidbody { get; private set; } = null;
    public Animator animator { get; private set; } = null;
    public new CapsuleCollider collider { get; private set; } = null;
    public PlayerCamera playerCamera { get; private set; } = null;
    public BushCollider bushCollider { get; private set; } = null;
    public new Renderer renderer { get; private set; } = null;
    public bool OnBush { get; set; } = false; // BushCollider에서도 조작합니다.
    public bool OnDeath { get; set; } = false;

    public int CurHP { get => curHP; set => curHP = value; }
    public int MaxHP { get => maxHP; set => maxHP = value; }
    public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }
    public int AttackDamage { get => attackDamage; set => attackDamage = value; }
    public float AttackDistance { get => attackDistance; set => attackDistance = value; }
    public int ShieldPower { get => shieldPower; set => shieldPower = value; }
    public int MaxShieldPower { get => maxShieldPower; set => maxShieldPower = value; }
    public float AttackSpeed { get => attackSpeed; set => attackSpeed = value; }
    public float UltimateCooldown { get => currentUltimateCooldown; }
}