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
    GameObject Chars, EndPoint;
    public Joystick joystick;
    Rigidbody rb;
    public float sensivity, CamSens, groupSens, groupSens2, stopDist;
    public int playerCount = 1;
    public int setGroupDuration = 25;
    GameObject cam;
    public Vector3 camOffs;
    float x, z;
    Vector3 TargetPos;
    NavMeshAgent PlayerNavMesh;

    [Range(0f, 1f)][SerializeField] private float DistanceFactor, Radius;
    float[] rads;
    bool groupTrig = true;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        Chars = GameObject.Find("Chars");
        cam = GameObject.Find("Main Camera");
        EndPoint = GameObject.Find("End Point");
        //tgRb = tg.GetComponent<Rigidbody>();
        rb = Chars.GetComponent<Rigidbody>();
        rads = new float[] {UnityEngine.Random.Range(0, 1)};
    }

    // Update is called once per frame
    void Update()
    {
        //Reloads the current scene
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

    }

    void FixedUpdate()
    {
        InputsController();
        CameraController();

        if (groupTrig)
        {
            for (int i = 0; i < Chars.transform.childCount; i++)
            {
                Chars.transform.GetChild(i).localPosition = Vector3.MoveTowards(Chars.transform.GetChild(i).localPosition, Vector3.zero, groupSens2 * Time.deltaTime);
                //Chars.transform.GetChild(i).GetComponent<NavMeshAgent>().destination = Chars.transform.position + Vector3.forward;
            }
        }
        Chars.GetComponent<NavMeshAgent>().destination = Chars.GetComponent<NavMeshAgent>().destination + Vector3.forward;
    }

    //Controls the inputs
    void InputsController()
    {
        

        Chars.transform.position += new Vector3(joystick.Horizontal * sensivity * Time.deltaTime, 0, 0);

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
                            Chars.transform.GetChild(0).position,
                            Quaternion.identity,
                            Chars.transform);
            }
            //camOffs += new Vector3(0, t, -t);
        }

        //Removes players
        else
        {
            int t = Chars.transform.childCount - 1;
            for (int i = 0; i < opNumber; i++)
            {
                Destroy(Chars.transform.GetChild(t - i).gameObject);
            }
            //camOffs += new Vector3(0, t, -t);
        }

        SetPlayersPos();


        //Updates the playerCount according to the operation that done
        playerCount = Chars.transform.childCount;
        rads = new float[playerCount];
        for (int i = 0; i < rads.Length; i++)
            rads[i] = UnityEngine.Random.Range(0, i) * 0.01f;
    }

    //Sets collected players pos during update
    void SetPlayersPos()
    {
        for (int i = 0; i < Chars.transform.childCount; i++)
        {
            StartCoroutine(SetGroupPos(Chars.transform.GetChild(i).gameObject, i));
            PlayerNavMesh = Chars.transform.GetChild(i).GetComponent<NavMeshAgent>();
            //PlayerNavMesh.avoidancePriority = i % 2 == 0 ? i : i + 1;
            //PlayerNavMesh.avoidancePriority = i >= 99 ? 99 : i;

            /*if (Chars.transform.GetChild(i).position.x < -4.5f || Chars.transform.GetChild(i).position.x > 4.5f)
            {
                Chars.transform.GetChild(i).GetComponent<CapsuleCollider>().isTrigger = true;
                Chars.transform.GetChild(i).AddComponent<Rigidbody>();
                Chars.transform.GetChild(i).SetParent(null);
            }
            else
            {
                //Chars.transform.GetChild(i).GetComponent<NavMeshAgent>().destination = EndPoint.transform.position + Vector3.forward;
                //Chars.transform.GetChild(0).GetComponent<NavMeshAgent>().destination = Chars.transform.GetChild(0).position + Vector3.forward;

                Vector3 dir = Chars.transform.GetChild(0).position + Chars.transform.GetChild(0).forward;
                Chars.transform.GetChild(i).GetComponent<NavMeshAgent>().destination = i != 0 ? dir : Chars.transform.GetChild(0).position + Vector3.forward;
            }*/


            //PlayerNavMesh.avoidancePriority = i % 2 == 0 ? i : i + 1;

            /*
            x = DistanceFactor * Mathf.Sqrt(i) * Mathf.Cos(i * Radius);
            z = DistanceFactor * Mathf.Sqrt(i) * Mathf.Sin(i * Radius);
            TargetPos = Chars.transform.GetChild(0).localPosition + new Vector3(x, Chars.transform.GetChild(i).localPosition.y, z);

            Chars.transform.GetChild(i).localPosition = Vector3.MoveTowards(Chars.transform.GetChild(i).localPosition, TargetPos, groupSens * Time.deltaTime);
            */

        }
    }

    void CameraController()
    {
        cam.transform.position = Vector3.Lerp(cam.transform.position,
                                              new Vector3(Chars.transform.GetChild(0).position.x, 0, Chars.transform.GetChild(0).position.z) + camOffs,
                                              CamSens * Time.deltaTime);
    }

    IEnumerator SetGroupPos(GameObject Runner, int ind)
    {
        groupTrig = false;
        for (int i = 0; i < setGroupDuration; i++)
        {
            //Must be modified as written ourself
            x = DistanceFactor * Mathf.Sqrt(ind) * Mathf.Cos(ind * Radius);
            z = DistanceFactor * Mathf.Sqrt(ind) * Mathf.Sin(ind * Radius);
            TargetPos = Chars.transform.position + new Vector3(x, Chars.transform.GetChild(ind).localPosition.y, z);

            Runner.transform.localPosition = Vector3.MoveTowards(Runner.transform.localPosition, TargetPos, groupSens * Time.deltaTime);
            yield return new WaitForSeconds(0.01f);
        }
        groupTrig = true;
    }
}
