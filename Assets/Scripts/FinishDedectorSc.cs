using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishDedectorSc : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            GameObject.Find("Game Manager").GetComponent<GameManager>().ReachtoFinish();
            this.gameObject.GetComponent<BoxCollider>().enabled = false;
        }
    }
}
