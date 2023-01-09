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
            float t = Chars.transform.childCount * 0.15f;
            for(int i = 0; i<opNumber; i++)
            {
                Instantiate(Chars.transform.GetChild(0).gameObject,
                            Chars.transform.position + new Vector3(UnityEngine.Random.Range(-t, t),0, UnityEngine.Random.Range(-t, t)),
                            Quaternion.identity,
                            Chars.transform);

                
            }
            camOffs += new Vector3(0, t, -t);
        }
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
    void PlayersController()
    {
        for (int i = 0; i < Chars.transform.childCount; i++)
        {
            PlayerNavMesh = Chars.transform.GetChild(i).GetComponent<NavMeshAgent>();
            PlayerNavMesh.avoidancePriority = i>=99 ? 99 : i;

            /*if (i > 0 && i <= 20)
                PlayerNavMesh.stoppingDistance = stopDist * 1;
            if (i > 20 && i <= 50)
                PlayerNavMesh.stoppingDistance = stopDist * 2;
            if (i > 50 && i <= 90)
                PlayerNavMesh.stoppingDistance = stopDist * 3;
            //if (i > 60 && i <= 80)
            //    PlayerNavMesh.stoppingDistance = stopDist * 4;
            if (i > 90)
                PlayerNavMesh.stoppingDistance = stopDist * 4;*/
            var x = DistanceFactor * Mathf.Sqrt(i) * Mathf.Cos(i * Radius);
            var z = DistanceFactor * Mathf.Sqrt(i) * Mathf.Sin(i * Radius);
            var NewPos = new Vector3(x, Chars.transform.GetChild(i).position.y, z);

            if (i != 0)
                Chars.transform.GetChild(i).localPosition = Vector3.MoveTowards(Chars.transform.GetChild(i).localPosition, NewPos, groupSens * Time.deltaTime);



            //PlayerNavMesh.stoppingDistance = i<=50 ? stopDist * PlayerNavMesh.avoidancePriority : stopDist * 50;
            //PlayerNavMesh.destination = i==0 ? Chars.transform.position : Chars.transform.GetChild(0).position;
        }
    }
    void CameraController()
    {
        cam.transform.position = Vector3.Lerp(cam.transform.position, Chars.transform.GetChild(0).position + camOffs, CamSens * Time.deltaTime);
    }
}
