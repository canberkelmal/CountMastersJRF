using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CylinderSc : MonoBehaviour
{
    GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            Destroy(other.gameObject);
            gm.SetPlayerCount();
        }
    }
}
