using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //movement part reference: bilibili, BV1DT4y1A7DJ
    public float speed;
    new private Rigidbody2D rigidbody;
    private Animator animator;
    private float inputX, inputY;
    private float stopX, stopY;

    //life & combat
    public float healthBound = 10;
    public float health = 100;
    public bool alive = true;
    public Transform gun;
    public GameObject bulletPrefab;
    private Vector3 mousePos;
    private Vector2 direction;
    public RotatingCam innerRing;
    public AudioClip hitSound;
    public GameObject dieUI;

    //save & load
    public Vector3 spawn = new Vector3(0, 0, 0);

    //private Vector3 offset;
    void Start()
    {
        //offset = Camera.main.transform.position - transform.position;
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spawn = this.transform.position;
        health = healthBound;
    }

    public void harm(float damage)
    {
        if (!alive) return;
        if (health - damage <= 0) { health = 0; die(); return; }
        else { 
            health -= damage;
            StartCoroutine(innerRing.FightFX(0.1f));
            StartCoroutine(innerRing.CameraShake(1f));
            AudioSource.PlayClipAtPoint(hitSound, transform.position);
            return;          
        }

    }

    void die()
    {
        animator.SetBool("die", true);
        alive = false;
        dieUI.SetActive(true);
        return;
    }

    void shoot()
    {
        direction = new Vector2(mousePos.x - gun.position.x, mousePos.y - gun.position.y).normalized;
        gun.up = direction;
        Bullet blt = bulletPrefab.GetComponent<Bullet>();
        if (blt != null) { blt.damage = 5; blt.shooterIndex = 0; }
        GameObject bgo = Instantiate(bulletPrefab, gun.position, gun.rotation);
    }

    void respawn()
    {
        this.transform.position = spawn;
        health = healthBound;
        alive = true;
    }
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.R)) respawn();// respawn, 待后续存档机制改进
        mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Mathf.Abs(Camera.main.transform.position.z)));
        //mousePos.z = 0;
        if (!alive) { rigidbody.velocity = new Vector3(0, 0, 0); return; } //以下内容死亡后无法启用

        if (Input.GetMouseButtonDown(0)) shoot();
        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetAxisRaw("Vertical");
        Vector2 input = (transform.right * inputX + transform.up * inputY).normalized;// 输入需要乘以本身坐标系来确保行进方向正确
        rigidbody.velocity = input * speed;

        if (input != Vector2.zero)
        {
            animator.SetBool("isMoving", true);
            stopX = inputX;
            stopY = inputY;
        }
        else
        {
            animator.SetBool("isMoving", false);
        }
        animator.SetFloat("InputX", stopX);
        animator.SetFloat("InputY", stopY);

        //Camera.main.transform.position = transform.position + offset;
    }
}
