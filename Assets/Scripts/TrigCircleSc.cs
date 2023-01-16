using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrigCircleSc : MonoBehaviour
{
    GameManager gm;
    EnemyGroupSc enSc;
    GameObject enemies, chars;
    void Start()
    {
        enemies = GameObject.Find("Enemies");
        chars = GameObject.Find("Chars");
        enSc = enemies.GetComponent<EnemyGroupSc>();
        gm = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        print("Enemy trigger circle is triggered by " + other.name);
        if(other.tag == "Player")
        {
            enSc.Triggered();
        }
    }
}
