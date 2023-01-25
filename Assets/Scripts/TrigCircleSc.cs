using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrigCircleSc : MonoBehaviour
{
    EnemyGroupSc enSc;
    GameObject enemies;
    void Start()
    {
        enemies = GameObject.Find("Enemies");
        enSc = enemies.GetComponent<EnemyGroupSc>();
    }

    private void OnTriggerEnter(Collider other)
    {
        print("Enemy trigger circle is triggered by " + other.name);
        if(other.tag == "Player")
        {
            this.gameObject.GetComponent<MeshCollider>().enabled = false;
            enSc.Triggered();
        }
    }
}
