using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    #region variables
    GameObject Chars;
    public Joystick joystick;
    float ForwardValue;
    Rigidbody rb, tgRb;
    public float sensivity, CamSens, stopDist;
    public int playerCount = 1;
    NavMeshAgent PlayerNavMesh;
    GameObject cam, tg;
    public Vector3 camOffs;


    #endregion

    // Start is called before the first frame update
    void Start()
    {
        Chars = GameObject.Find("Chars");
        cam = GameObject.Find("Main Camera");
        tg = GameObject.Find("Toggle");
        tgRb = tg.GetComponent<Rigidbody>();
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
        CameraController();
        InputsController();
        PlayersController();
    }

    //Controls all inputs
    void InputsController()
    {
        //Reloads the current scene
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        //Controlling the chars&players
        rb.velocity = new Vector3(joystick.Horizontal * sensivity * Time.deltaTime,
                                0,
                                joystick.Vertical * sensivity * Time.deltaTime);

    }
    public void AddRemove(bool op, int opNumber)
    {
        if(op)
        {
            for(int i = 0; i<opNumber; i++)
            {
                Instantiate(Chars.transform.GetChild(0).gameObject,
                            Chars.transform.position + new Vector3(UnityEngine.Random.Range(-0.1f, 0.1f),0, UnityEngine.Random.Range(-0.1f, 0.1f)),
                            Quaternion.identity,
                            Chars.transform);
            }
        }
        else
        {
            int t = Chars.transform.childCount - 1;
            for(int i = 0; i<opNumber; i++)
            {
                Destroy(Chars.transform.GetChild(t-i).gameObject);
            }
        }
        playerCount = Chars.transform.childCount;
    }
    void PlayersController()
    {
        for (int i = 0; i < Chars.transform.childCount; i++)
        {
            PlayerNavMesh = Chars.transform.GetChild(i).GetComponent<NavMeshAgent>();
            PlayerNavMesh.avoidancePriority = i;
            PlayerNavMesh.stoppingDistance = stopDist * Chars.transform.childCount;
            //PlayerNavMesh.stoppingDistance = 0.1f + 0.05f * i;
            PlayerNavMesh.destination = Chars.transform.position;
            //Chars.transform.GetChild(i).position = Vector3.Lerp(Chars.transform.GetChild(i).position, tg.transform.position, sensivity* 0.7f * Time.deltaTime);
        }
    }
    void CameraController()
    {
        cam.transform.position = Vector3.Lerp(cam.transform.position, Chars.transform.GetChild(0).position + camOffs, CamSens * Time.deltaTime);
    }
}
