using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.AI;
using UnityEngine.UIElements;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    #region variables
    GameObject Chars;
    public Joystick joystick;
    float ForwardValue;
    Rigidbody rb, tgRb;
    public float sensivity, CamSens, groupSens, stopDist;
    public int playerCount = 1;
    NavMeshAgent PlayerNavMesh;
    GameObject cam, tg;
    public Vector3 camOffs;

    [Range(0f,1f)] [SerializeField] private float DistanceFactor, Radius;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        Chars = GameObject.Find("Chars");
        cam = GameObject.Find("Main Camera");
        //tg = GameObject.Find("Toggle");
        //tgRb = tg.GetComponent<Rigidbody>();
        rb = Chars.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        ClickDedector();
        SetPlayersPos();
    }

    void ClickDedector()
    {

    }

    void FixedUpdate()
    {
        CameraController();
        InputsController();
        //PlayersController();
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

    //If op is true, adds opNumber times player. Else, removes opNumber times player.
    public void AddRemove(bool op, int opNumber)
    {
        //Adds players
        if(op)
        {
            float t = Chars.transform.childCount * 0.15f;
            for(int i = 0; i<opNumber; i++)
            {
                Instantiate(Chars.transform.GetChild(0).gameObject,
                            Chars.transform.position,
                            Quaternion.identity,
                            Chars.transform);
            }
            camOffs += new Vector3(0, t, -t);
        }
        //removes players
        else
        {
            int t = Chars.transform.childCount - 1;
            for(int i = 0; i<opNumber; i++)
            {
                Destroy(Chars.transform.GetChild(t-i).gameObject);
            }
            camOffs += new Vector3(0, t, -t);
        }
        playerCount = Chars.transform.childCount;
    }

    void SetPlayersPos()
    {
        for (int i = 0; i < Chars.transform.childCount; i++)
        {
            var x = DistanceFactor * Mathf.Sqrt(i) * Mathf.Cos(i * Radius);
            var z = DistanceFactor * Mathf.Sqrt(i) * Mathf.Sin(i * Radius);
            var NewPos = new Vector3(x, Chars.transform.GetChild(i).position.y, z);

            if (i != 0)
                Chars.transform.GetChild(i).localPosition = Vector3.MoveTowards(Chars.transform.GetChild(i).localPosition, NewPos, groupSens * Time.deltaTime);
        }
    }

    void CameraController()
    {
        cam.transform.position = Vector3.Lerp(cam.transform.position, Chars.transform.GetChild(0).position + camOffs, CamSens * Time.deltaTime);
    }
}
