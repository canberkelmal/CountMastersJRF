using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySc : MonoBehaviour
{
    [SerializeField] GameObject playerDeathSplashEffect, enemyDeathSplashEffect;
    GameManager gm;
    EnemyGroupSc parentSc;
    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.Find("Game Manager").GetComponent<GameManager>();
        parentSc = transform.parent.GetComponent<EnemyGroupSc>();
    }

    void OnCollisionEnter(Collision other)
    {
        print("Enemy Collision!" + other.gameObject.tag);
        if (other.gameObject.tag == "Player")
        {
            Instantiate(playerDeathSplashEffect, other.transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            Instantiate(enemyDeathSplashEffect, this.transform.position, Quaternion.identity);
            Destroy(this.gameObject);

            gm.SetPlayerCount();
            parentSc.SetCountText();
        }
    }
}
