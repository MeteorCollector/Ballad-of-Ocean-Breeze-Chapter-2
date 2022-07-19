using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthSys : MonoBehaviour
{
    new private Rigidbody2D rigidbody;
    private Animator animator;

    //life & combat
    public float healthBound = 10;
    private float health = 100;
    public BattleField field;
    public bool alive = true;

    //private Vector3 offset;
    void Start()
    {
        //offset = Camera.main.transform.position - transform.position;
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        health = healthBound;
    }

    public void harm(float damage)
    {
        if (!alive) return;
        if (health - damage <= 0) { health = 0; die(); return; }
        else { health -= damage; return; }
    }

    void die()
    {
        animator.SetBool("die", true);
        field.killed++;
        alive = false;
        return;
    }
    void Update()
    {
    }
}
