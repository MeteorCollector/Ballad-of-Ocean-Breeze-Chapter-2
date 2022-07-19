using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleField : MonoBehaviour
{
    public GameObject[] enemies;
    public GameObject barrier;
    public RotatingCam innerRing;
    private FacingCam fac;
    public int enemyCnt = 9;
    public int killed = 0;
    public bool killAll = true;
    private bool cleared = false;
    private bool inBattle = false;
    private bool finished = false;
    void Start()
    {
        fac = this.gameObject.GetComponent<FacingCam>();
    }

    void Update()
    {
        if (killed >= enemyCnt && !finished) { finished = true;  stageClear(); }
    }

    public void stageClear()
    {
        if (fac != null) Destroy(fac);
        cleared = true;
        innerRing.switchMode(true);
        //for (int i = 0; i < enemyCnt; i++)
        //    Destroy(enemies[i]);
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
