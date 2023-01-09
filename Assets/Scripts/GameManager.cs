using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region variables
    GameObject Chars;
    public Joystick joystick;
    float ForwardValue;
    Rigidbody rb;
    public float sensivity;


    
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        Chars = GameObject.Find("Chars");
        rb = Chars.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        ClickDedector();
    }

    void ClickDedector()
    {

    }

    void FixedUpdate()
    {
        InputsController();
    }

    //Controls all inputs
    void InputsController()
    {
        //Reloads the current scene
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        //Controlling the char&player
        rb.velocity = new Vector3(joystick.Horizontal * sensivity,
                                0,
                                joystick.Vertical * sensivity);

    }
}
