using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySc : MonoBehaviour
{
    [SerializeField] GameObject playerDeathSplashEffect, enemyDeathSplashEffect;
    GameManager gm;
    EnemyGroupSc parentSc;
    public float enemySplachYOffset;
    bool triggered = false;
    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.Find("Game Manager").GetComponent<GameManager>();
        parentSc = transform.parent.GetComponent<EnemyGroupSc>();
    }

    void OnCollisionEnter(Collision other)
    {
        print("Enemy Collision!" + other.gameObject.tag);
        if (other.gameObject.tag == "Player" && !triggered)
        {
            triggered = true;
            other.gameObject.GetComponent<CapsuleCollider>().enabled = false;
            Instantiate(playerDeathSplashEffect, other.transform.position, Quaternion.identity);
            Instantiate(enemyDeathSplashEffect, this.transform.position + Vector3.up*enemySplachYOffset, Quaternion.identity);
            Destroy(other.gameObject);
            gm.SetPlayerCount();
            parentSc.SetCountText(this.gameObject);
            //Destroy(this.gameObject);       
        }
    }
}
