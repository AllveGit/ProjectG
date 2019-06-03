using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon;

public class IceArrow : MonoBehaviour
{
    private const int    BaseDamage  = 150;
    private const float  Coefficient = 2.0f;

    private const float  ProjectileSpeed = 10.0f;


    private new Rigidbody rigidbody = null;

    private int skillDamage     = 0;
    private Vector3 direction   = default;

    public BasePlayer ownerPlayer = null;

    [SerializeField]
    private float destroyTime   = 10;

    public void Cast(BasePlayer inOwnerPlayer, int attackDamage, Vector3 inDirection)
    {
        ownerPlayer = inOwnerPlayer;
        skillDamage = (int)(attackDamage * Coefficient) + BaseDamage;
        direction   = inDirection;

        StartCoroutine(Timer());
    }

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
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

    void OnCollisionEnter(Collider collider)
    {
        if (TeamManager.Instance.IsAttackable(ownerPlayer.tag, collider.tag))
        {

        }
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(destroyTime);

        Photon.Pun.PhotonNetwork.Destroy(this.gameObject);

        yield break;
    }
}
