using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleField : MonoBehaviour
{
    [Header("Enemies")]
    public int enemyCnt = 8;
    public GameObject[] enemies;

    [Header("Environments")]
    public GameObject barrier;
    public int itemsToDestroyCnts;
    public GameObject[] itemsToDestroy;

    [Header("Cameras")]
    public RotatingCam innerRing;
    private FacingCam fac;
    
    [Header("Combat Settings")]
    public int killed = 0;
    public bool killAll = true;
    private bool cleared = false;
    private bool inBattle = false;
    private bool finished = false;

    [Header("Displays")]
    public GameObject winUI;
    public AudioClip winMusic;
    void Start()
    {
        fac = this.gameObject.GetComponent<FacingCam>();
        for (int i = 0; i < enemyCnt; i++)
        {
            EnemyHealthSys hsys = enemies[i].GetComponent<EnemyHealthSys>();
            if (hsys != null) hsys.spawnPoint = enemies[i].transform.position;
        }
    }

    void Update()
    {
        if (killed >= enemyCnt && !finished) { finished = true;  stageClear(); }
        if (Input.GetKeyDown(KeyCode.R)) Respawn();
    }

    public void Respawn()
    {
        if (cleared) return;
        for (int i = 0; i < enemyCnt; i++)
        {
            EnemyHealthSys hsys = enemies[i].GetComponent<EnemyHealthSys>();
            if (hsys != null) hsys.Respawn();
            enemies[i].SetActive(false);
        }
        killed = 0;
        barrier.SetActive(false);
        inBattle = false;
    }

    public void stageClear()
    {
        if (fac != null) Destroy(fac);
        cleared = true;
        StartCoroutine(clearAnim(3));
    }

    public IEnumerator clearAnim(float time)
    {
        float number = 60 * time;
        winUI.SetActive(true);
        AudioSource.PlayClipAtPoint(winMusic, transform.position);
        innerRing.switchMode(true);
        for (int i = 0; i < number; i++)
        {
            yield return new WaitForFixedUpdate();
        }
        for (int i = 0; i < itemsToDestroyCnts; i++)
            Destroy(itemsToDestroy[i]);
        winUI.SetActive(false);
        Destroy(barrier);
        //Destroy(this);
    }

    private void OnTriggerEnter2D(Collider2D player)
    {
        if(player.gameObject.tag == "Player" && !cleared && !inBattle)
        {
            for (int i = 0; i < enemyCnt; i++)
                enemies[i].SetActive(true);
            barrier.SetActive(true);
            innerRing.switchMode(false);
            inBattle = true;
        }
    }
}
