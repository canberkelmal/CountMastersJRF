using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StairSc : MonoBehaviour
{
    private void Start()
    {
        for(int i=0; i<transform.childCount; i++)
        {
            transform.GetChild(i).GetChild(0).GetChild(0).GetComponent<Text>().text = (i * 0.2f + 1).ToString();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
            other.gameObject.transform.SetParent(null);
    }
}
