using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon;
using Photon.Pun;

public class IceArrow : MonoBehaviourPunCallbacks, IPunObservable
{
    private const int    BaseDamage         = 150;      // 기본 스킬 데미지
    private const float  Coefficient        = 2.0f;     // 스킬 계수

    private const float  ProjectileSpeed    = 10.0f;    // 투사체 속도

    private new Rigidbody  rigidbody    = null;
    private     PhotonView photonView   = null;

    private int         skillDamage     = 0;        // 스킬의 최종 데미지
    private Vector3     direction       = default;  // 스킬의 방향

    private BasePlayer  ownerPlayer     = null;     // 시전한 플레이어

    [SerializeField]
    private float       destroyTime     = 10;       // 삭제까지의 대기시간

    public void Cast(BasePlayer inOwnerPlayer, int attackDamage, Vector3 inDirection)
    {
        ownerPlayer = inOwnerPlayer;
        skillDamage = (int)(attackDamage * Coefficient) + BaseDamage;
        direction   = inDirection;

        transform.rotation = Quaternion.LookRotation(inDirection);
        StartCoroutine(Timer());
    }

    void Awake()
    {
        rigidbody   = GetComponent<Rigidbody>();
        photonView  = GetComponent<PhotonView>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        rigidbody.MovePosition(
            transform.position
            + direction * ProjectileSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (TeamManager.Instance.IsAttackable(ownerPlayer.tag, collision.gameObject.tag))
        {
            BasePlayer player = collision.gameObject.GetComponent<BasePlayer>();
            player.OnAttacked(skillDamage);

            PhotonNetwork.Destroy(this.gameObject);
        }
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(destroyTime);

        PhotonNetwork.Destroy(this.gameObject);

        yield break;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

    }
}
