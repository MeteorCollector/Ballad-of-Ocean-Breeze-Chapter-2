using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SealMovement : MonoBehaviour
{
    public float speed;
    new private Rigidbody2D rigidbody;
    private Animator animator;
    private float inputX, inputY;
    private float stopX, stopY;
    public Transform target;
    public float attackRange = 4;
    private bool attacking = false;

    //life & combat
    public EnemyHealthSys hsys;
    public float attackCD = 3;
    private float attackTimer = 3;
    public GameObject bulletPrefab;
    public Transform gun;
    private Vector2 direction;

    //private Vector3 offset;
    void Start()
    {
        //offset = Camera.main.transform.position - transform.position;
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!hsys.alive) { rigidbody.velocity = new Vector3(0, 0, 0); return; }
        Vector2 delta = (target.position - transform.position);// 计算速度方向
        rigidbody.velocity = delta.normalized * speed;

        if (rigidbody.velocity.x > 0) {animator.SetFloat("xSpeed", 1); }
        else animator.SetFloat("xSpeed", -1);

        attacking = (delta.x * delta.x + delta.y * delta.y < attackRange * attackRange);
        animator.SetBool("attack", attacking);

        if (attacking) {
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0) { shoot(); attackTimer = attackCD; }
        }
        //Camera.main.transform.position = transform.position + offset;
    }

    private void shoot()
    {
        direction = new Vector2(target.position.x - gun.position.x, target.position.y - gun.position.y).normalized;
        gun.up = direction;
        Bullet blt = bulletPrefab.GetComponent<Bullet>();
        if (blt != null) { blt.damage = 3; blt.shooterIndex = 1; }
        GameObject bgo = Instantiate(bulletPrefab, gun.position, gun.rotation);
    }
}