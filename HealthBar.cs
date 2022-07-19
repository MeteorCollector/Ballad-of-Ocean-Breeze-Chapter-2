using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public PlayerMovement plr;
    public Text bar;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        int health = (int) plr.health;
        bar.text = "生命值：";
        for (int i = 0; i < health; i++) { bar.text += "♥"; }
    }
}
