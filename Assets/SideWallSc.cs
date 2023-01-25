using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class SideWallSc : MonoBehaviour
{
    public float removeDelay;
    GameManager gm;

    void Start()
    {
        gm = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            gm.distortions.Remove(gm.distortions.Count - 1);
            other.transform.parent = null;
            other.gameObject.GetComponent<NavMeshAgent>().enabled= false;
            other.AddComponent<Rigidbody>();
            gm.SetPlayerCount();
            StartCoroutine(CharRemover(other.gameObject));
        }
    }

    IEnumerator CharRemover(GameObject dropingChar)
    {
        yield return new WaitForSeconds(removeDelay);
        Destroy(dropingChar);
    }
}
