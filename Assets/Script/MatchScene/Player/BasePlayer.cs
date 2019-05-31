using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasePlayer : MonoBehaviour
{
    [SerializeField] 
    private int curHP = 0;
    [SerializeField] 
    private int maxHP = 0;
    [SerializeField] 
    private float moveSpeed = 0;
    [SerializeField]
    private float attackSpeed = 0;
    [SerializeField] 
    private int attackDamage = 0;
    [SerializeField]
    private int shieldPower = 0;
    [SerializeField]
    private int maxShieldPower = 0;
    [SerializeField] [Range(0f, 1f)]
    private float rotationLerpSpeed = 0f;
    private Vector3 movement = new Vector3(0f, 0f, 0f);

    public new Rigidbody rigidbody { get; private set; } = null;
    public Animator animator { get; private set; } = null;

    public int CurHP { get => curHP; set => curHP = value; }
    public int MaxHP { get => maxHP; set => maxHP = value; }
    public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }
    public float AttackSpeed { get => attackSpeed; set => attackSpeed = value; }
    public int AttackDamage { get => attackDamage; set => attackDamage = value; }
    public int ShieldPower { get => shieldPower; set => shieldPower = value; }
    public int MaxShieldPower { get => maxShieldPower; set => maxShieldPower = value; }
    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }
    
    public void Move()
    {
        float v = Input.GetAxis("Vertical"); // 수직
        float h = Input.GetAxis("Horizontal"); // 수평

        movement = new Vector3(h, 0f, v).normalized * moveSpeed * Time.deltaTime;
    }
    public void MoveRotation()
    {
        if (movement.magnitude < 0.01f)
            return;


        rigidbody.velocity = movement;
        rigidbody.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), rotationLerpSpeed);
    }

    public abstract void Death();
    public abstract void Attack();
    public abstract void UltimateSkill();
}
