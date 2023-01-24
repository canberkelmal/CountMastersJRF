using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairSc : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
            other.gameObject.transform.SetParent(null);
    }
}
