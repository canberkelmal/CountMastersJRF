using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawSc2 : MonoBehaviour
{
    GameManager gm;
    public GameObject playerDeathSplashEffect;
    public float speed = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    private void FixedUpdate()
    {
        transform.GetChild(0).Rotate(0, 0, 128 * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Instantiate(playerDeathSplashEffect, other.transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            gm.SetPlayerCount();
        }
    }
}
