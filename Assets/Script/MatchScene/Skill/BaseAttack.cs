﻿using System.Collections;
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

    protected float attackDistance = 0f;
    protected float AccumulateDistance = 0f;
    protected int attackDamage = 0;

    protected Vector3 direction = default;

    #region Propertie
    public float ProjectileSpeed { get => projectileSpeed; }
    public int AttackDamage { get => attackDamage; }
    public Vector3 Direction { get => direction; }

    public new Rigidbody rigidbody { get; protected set; }
    public BasePlayer ownerPlayer { get; private set; }

    #endregion
}
public abstract partial class BaseAttack
{
    private Vector3 currentPosition = Vector3.zero;
    private Quaternion currentRotation = Quaternion.identity;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            currentPosition = (Vector3)stream.ReceiveNext();
            currentRotation = (Quaternion)stream.ReceiveNext();
        }
    }

    [PunRPC]
    protected virtual void RPCTranslatePosition(Vector3 translationPos)
    {
        currentPosition = translationPos;
        transform.position = translationPos;
    }

}

public abstract partial class BaseAttack
{
    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        
    }

    public void FixedUpdate()
    {
        if (photonView.IsMine)
        {
            Move();
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, currentPosition, ProjectileSpeed * Time.fixedDeltaTime);
            transform.rotation = currentRotation;
        }
    }

    public virtual void Move()
    {
        float moveDistance = ProjectileSpeed * Time.deltaTime;
        AccumulateDistance += moveDistance;

        rigidbody.MovePosition(
        transform.position
        + direction * moveDistance);
    }

    public virtual void Cast(BasePlayer inOwnerPlayer, int inAttackDamage, float attackDistance, Vector3 vStartPosition, Vector3 inDirection)
    {
        ownerPlayer         = inOwnerPlayer;
        attackDamage        = inAttackDamage;
        direction           = inDirection;
        this.attackDistance  = attackDistance;

        transform.position  = vStartPosition;
        transform.rotation  = Quaternion.LookRotation(inDirection);

        photonView.RPC("RPCTranslatePosition", RpcTarget.Others, transform.position);

        if (photonView.IsMine)
            StartCoroutine(DistanceCheck());
    }

    // 공격방식마다 추가적으로 해야할 작업이 있다면 이 함수를 오버라이딩 하세요

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


    public void OnTriggerEnter(Collider other)
    {
        if (photonView.IsMine == false)
            return;
        if (other.gameObject == this.gameObject)
            return;

        if (other.CompareTag("Player"))
        {
            BasePlayer player = other.GetComponent<BasePlayer>();
            if (player.animator.GetBool("Death"))
                return;

            BaseCollisionProcess(player);
        }
        else if (other.CompareTag("Attack"))
        {
            BaseAttack attack = other.GetComponent<BaseAttack>();

            if (attack.ownerPlayer == this.ownerPlayer)
                return;
            else
                PhotonNetwork.Destroy(this.gameObject);
        }
        else 
            PhotonNetwork.Destroy(this.gameObject);
    }
    
    /*
     * 만약 구현하는 공격의 프로세스가 기존 프로세스와 다를경우
     * 이 함수를 상속받아 사용한다.
     */
    public virtual void BaseCollisionProcess(BasePlayer player)
    {
        if (IsAttackable(PhotonNetwork.LocalPlayer, player.photonView.Owner))
        {
            player.OnDamaged(AttackDamage);
            PhotonNetwork.Destroy(this.gameObject);
        }
    }

    IEnumerator DistanceCheck()
    {
        while (true)
        {
            if (AccumulateDistance >= attackDistance)
                break;

            yield return null;
        }

        if (photonView.IsMine) PhotonNetwork.Destroy(this.gameObject);
        yield break;
    }
}
