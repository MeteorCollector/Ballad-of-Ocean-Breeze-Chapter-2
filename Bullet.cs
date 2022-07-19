using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Bullet : MonoBehaviour
{
    public int shooterIndex = 0;// 0 - player; 1 - enemy
    public float speed = 20;
    public float life = 10;
    public GameObject self;
    public GameObject gunFire;
    public float damage = 5;
    public AudioClip bang;
    void Start()
    {
        AudioSource.PlayClipAtPoint(bang, transform.position);
        Debug.Log("new bullet, shooter index = " + shooterIndex + ", damage = " + damage + ";");//
        Instantiate(gunFire, transform.position, transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.up * speed * Time.deltaTime;
        life -= Time.deltaTime;
        if (life < 0) Destroy(self);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Environment" || collision.gameObject.tag == "Bullet") { return; }
        if (collision.gameObject.tag == "Player")
        {
            PlayerMovement plr = collision.gameObject.GetComponent<PlayerMovement>();
            if (plr != null && shooterIndex == 1)
            {
                plr.harm(damage);
                Destroy(this.gameObject);
                Debug.Log("hit player");
            }
        }
        else if (collision.gameObject.tag == "Enemy")
        {
            EnemyHealthSys esys = collision.gameObject.GetComponent<EnemyHealthSys>();
            if (esys != null && shooterIndex == 0)
            {
                esys.harm(damage);
                Destroy(this.gameObject);
                Debug.Log("hit enemy");
            }
        }
        else { Destroy(this.gameObject); Debug.Log("hit " + collision.gameObject.name + ", tag = " + collision.gameObject.tag); }
    }
}
