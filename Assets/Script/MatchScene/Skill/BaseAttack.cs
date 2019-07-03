using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

/*
 * BaseAttack 객체의 변수와 Propertie 입니다.
 */
public abstract partial class BaseAttack : MonoBehaviourPun, IPunObservable
{ 
    [SerializeField]
    protected float projectileSpeed = 10.0f;
    [SerializeField]
    protected int attackDamage = 0;
    [SerializeField]
    protected float destroyTime = 10;                 // 삭제까지의 대기시간

    protected Vector3 direction = default;

    #region Propertie
    public float ProjectileSpeed { get => projectileSpeed; }
    public int AttackDamage { get => attackDamage; }
    public float DestroyTime { get => destroyTime; }
    public Vector3 Direction { get => direction; }

    public new Rigidbody rigidbody { get; protected set; }
    public BasePlayer ownerPlayer { get; private set; }
    #endregion
}
public abstract partial class BaseAttack : MonoBehaviourPun, IPunObservable
{

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        
    }

    public void Update()
    {
        if (photonView.IsMine)
            Move();
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, currentPosition, ProjectileSpeed * Time.deltaTime);
            transform.rotation = currentRotation;
        }
    }

    protected virtual void Move()
    {
        rigidbody.MovePosition(
        transform.position
        + direction * ProjectileSpeed * Time.deltaTime);
    }

    public virtual void Cast(BasePlayer inOwnerPlayer, int inAttackDamage, Vector3 inDirection)
    {
        ownerPlayer = inOwnerPlayer;
        attackDamage = inAttackDamage;
        direction = inDirection;

        transform.rotation = Quaternion.LookRotation(inDirection);

        if (photonView.IsMine)
            StartCoroutine(Timer());
    }

    protected bool IsAttackable(Photon.Realtime.Player me, Photon.Realtime.Player enumy)
    {
        bool Attackable = false; ;

        Enums.TeamOption myTeamOption = (Enums.TeamOption)me.CustomProperties[Enums.PlayerProperties.TEAM.ToString()];
        Enums.TeamOption enumyTeamOption = (Enums.TeamOption)enumy.CustomProperties[Enums.PlayerProperties.TEAM.ToString()];

        switch (myTeamOption)
        {
            case Enums.TeamOption.RedTeam:
                if (enumyTeamOption == Enums.TeamOption.BlueTeam)
                    Attackable = true;
                break;
            case Enums.TeamOption.BlueTeam:
                if (enumyTeamOption == Enums.TeamOption.RedTeam)
                    Attackable = true;
                break;
            case Enums.TeamOption.Solo:
                Attackable = true;
                break;
        }
        return Attackable;
    }

    public virtual void BaseCollisionProcess(BasePlayer player)
    {
        if (IsAttackable(PhotonNetwork.LocalPlayer, player.photonView.Owner))
        {
            player.OnDamaged(AttackDamage);
            PhotonNetwork.Destroy(this.gameObject);
        }
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(destroyTime);

        if (photonView.IsMine) PhotonNetwork.Destroy(this.gameObject);

        yield break;
    }


    public void OnTriggerEnter(Collider other)
    {
        if (photonView.IsMine == false)
            return;

        if (other.CompareTag("Player"))
        {

            if (other.gameObject == this.gameObject)
                return;

            BasePlayer player = other.GetComponent<BasePlayer>();

            if (player.animator.GetBool("Death"))
                return;

            BaseCollisionProcess(player);
        }
        else
            PhotonNetwork.Destroy(this.gameObject);
    }

}

public abstract partial class BaseAttack : MonoBehaviourPun, IPunObservable
{
    private Vector3 currentPosition = Vector3.zero;
    private Quaternion currentRotation = Quaternion.identity;

    public virtual void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(projectileSpeed);

            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            projectileSpeed = (float)stream.ReceiveNext();

            currentPosition = (Vector3)stream.ReceiveNext();
            currentRotation = (Quaternion)stream.ReceiveNext();
        }
    }

}
