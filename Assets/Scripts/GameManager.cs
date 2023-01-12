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
    public float jsSensivity, CamSens, spawnSense, groupWalkSens, stopDist;

    public int playerCount = 1;
    public int setGroupDuration = 25;
    GameObject cam;
    public Vector3 camOffs;
    float x, z, camX, camZ;
    Vector3 TargetPos, CamTargetPos;
    NavMeshAgent PlayerNavMesh;

    [Range(0f, 1f)][SerializeField] private float DistanceFactor, Radius;
    float[] rads;
    bool groupTrig = true;

    public Canvas PlayerCountCv;
    public Text PlayerCountText;

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
                Chars.transform.GetChild(i).localPosition = Vector3.MoveTowards(Chars.transform.GetChild(i).localPosition, Vector3.zero, groupWalkSens * Time.deltaTime);

                /*Chars.transform.GetChild(i).position = Vector3.MoveTowards(Chars.transform.GetChild(i).position,
                                                                           PlayerCountCv.transform.position - (Vector3.up * PlayerCountCv.transform.position.y) + (Vector3.up*0.1f),
                                                                           groupWalkSens * Time.deltaTime);*/

                //Chars.transform.GetChild(i).GetComponent<NavMeshAgent>().destination = Chars.transform.position + Vector3.forward;
            }
        }
        Chars.GetComponent<NavMeshAgent>().destination = Chars.GetComponent<NavMeshAgent>().destination + Vector3.forward;
    }

    //Controls the inputs
    void InputsController()
    {
        //Controls char's x position
        Chars.transform.position += new Vector3(joystick.Horizontal * jsSensivity * Time.deltaTime, 0, 0);
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
                            PlayerCountCv.transform.position,
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
        SetPlayerCount();        

        /*rads = new float[playerCount];
        for (int i = 0; i < rads.Length; i++)
            rads[i] = UnityEngine.Random.Range(0, i) * 0.01f;*/
    }


    //Updates the playerCount
    public void SetPlayerCount()
    {
        playerCount = Chars.transform.childCount;
        PlayerCountText.text = playerCount.ToString();
    }

    //Sets collected players pos during update
    void SetPlayersPos()
    {
        for (int i = 0; i < Chars.transform.childCount; i++)
        {
            StartCoroutine(SetGroupPos(Chars.transform.GetChild(i).gameObject, i));
            //PlayerNavMesh = Chars.transform.GetChild(i).GetComponent<NavMeshAgent>();
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
        for(int i=0; i < Chars.transform.childCount; i++)
        {
            //camX += Chars.transform.GetChild(i).position.x < 0 ? Chars.transform.GetChild(i).position.x * -1 : Chars.transform.GetChild(i).position.x;
            camX += Chars.transform.GetChild(i).position.x;
            camZ += Chars.transform.GetChild(i).position.z;
        }

        CamTargetPos = new Vector3(camX/Chars.transform.childCount, 0, camZ / Chars.transform.childCount) + camOffs;
        PlayerCountCv.transform.position = CamTargetPos - camOffs + (Vector3.up * 1.52f);

        camX = 0;
        camZ = 0;

        cam.transform.position = Vector3.Lerp(cam.transform.position, CamTargetPos, CamSens * Time.deltaTime);
    }

    IEnumerator SetGroupPos(GameObject Runner, int ind)
    {
        groupTrig = false;
        for (int i = 0; i < setGroupDuration; i++)
        {
            //Must be modified as written ourself
            x = DistanceFactor * Mathf.Sqrt(ind) * Mathf.Cos(ind * Radius);
            z = DistanceFactor * Mathf.Sqrt(ind) * Mathf.Sin(ind * Radius);
            TargetPos = new Vector3(x, Chars.transform.GetChild(ind).localPosition.y, z);

            Runner.transform.localPosition = Vector3.MoveTowards(Runner.transform.localPosition, TargetPos, spawnSense * Time.deltaTime);
            yield return new WaitForSeconds(0.01f);
        }
        groupTrig = true;
    }
}
