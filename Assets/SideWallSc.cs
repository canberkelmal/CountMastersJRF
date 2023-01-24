using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class SideWallSc : MonoBehaviour
{
    public float removeDelay;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<NavMeshAgent>().enabled= false;
            other.AddComponent<Rigidbody>();
            StartCoroutine(CharRemover(other.gameObject));
        }
    }

    IEnumerator CharRemover(GameObject dropingChar)
    {
        yield return new WaitForSeconds(removeDelay);
        Destroy(dropingChar);
    }
}
