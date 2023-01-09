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
    Rigidbody rb;
    public float sensivity, CamSens, groupSens, stopDist;
    public int playerCount = 1;
    GameObject cam, tg;
    public Vector3 camOffs;
    float x, z;
    Vector3 TargetPos;

    [Range(0f,1f)] [SerializeField] private float Dist, Radius;
    float[] rads;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        Chars = GameObject.Find("Chars");
        cam = GameObject.Find("Main Camera");
        //tg = GameObject.Find("Toggle");
        //tgRb = tg.GetComponent<Rigidbody>();
        rb = Chars.GetComponent<Rigidbody>();
        rads = new float[] {UnityEngine.Random.Range(0, 1)};
    }

    // Update is called once per frame
    void Update()
    {
        CameraController();
        SetPlayersPos();
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

        //Controlling the chars&players
        rb.velocity = new Vector3(joystick.Horizontal * sensivity * Time.deltaTime,
                                0,
                                joystick.Vertical * sensivity * Time.deltaTime);

    }

    //If op is true, adds opNumber times player. Else, removes opNumber times player.
    public void AddRemove(bool op, int opNumber)
    {
        //Adds players
        if (op)
        {
            float t = Chars.transform.childCount * 0.15f;
            for (int i = 0; i < opNumber; i++)
            {
                Instantiate(Chars.transform.GetChild(0).gameObject,
                            Chars.transform.position,
                            Quaternion.identity,
                            Chars.transform);
            }
            camOffs += new Vector3(0, t, -t);
        }

        //Removes players
        else
        {
            int t = Chars.transform.childCount - 1;
            for (int i = 0; i < opNumber; i++)
            {
                Destroy(Chars.transform.GetChild(t - i).gameObject);
            }
            camOffs += new Vector3(0, t, -t);
        }


        //Updates the playerCount according to the operation that done
        playerCount = Chars.transform.childCount;
        rads = new float[playerCount];
        for (int i = 0; i < rads.Length; i++)
        {
            rads[i] = UnityEngine.Random.Range(0, 100) * 0.01f;
            Chars.transform.GetChild(i).GetComponent<NavMeshAgent>().avoidancePriority = i;
        }
    }

    //Sets collected players pos during update
    void SetPlayersPos()
    {
        for (int i = 0; i < Chars.transform.childCount; i++)
        {
            if (Chars.transform.GetChild(i).position.x < -4.5f || Chars.transform.GetChild(i).position.x > 4.5f)
            {
                Chars.transform.GetChild(i).GetComponent<CapsuleCollider>().isTrigger = true;
                Chars.transform.GetChild(i).AddComponent<Rigidbody>();
                Chars.transform.GetChild(i).SetParent(null);
            }
            else
            {
                float rnd = UnityEngine.Random.Range(0, Chars.transform.childCount);
                x = Dist * Mathf.Sqrt(i) * Mathf.Cos(i * rads[i]);
                z = Dist * Mathf.Sqrt(i) * Mathf.Sin(i * rads[i]);
                TargetPos = new Vector3(x, Chars.transform.GetChild(i).localPosition.y, z);

                if (i != 0)
                    Chars.transform.GetChild(i).localPosition = Vector3.MoveTowards(Chars.transform.GetChild(i).localPosition, TargetPos, groupSens * Time.deltaTime);
            }
        }
    }

    void CameraController()
    {
        cam.transform.position = Vector3.Lerp(cam.transform.position, Chars.transform.GetChild(0).position + camOffs, CamSens * Time.deltaTime);
    }
}
