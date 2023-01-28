using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.AI;
using UnityEngine.UIElements;
using Unity.VisualScripting;
using static UnityEngine.InputManagerEntry;

public class GameManager : MonoBehaviour
{
    #region variables
    GameObject Chars, Tower, FinishLine, FinishPanel, Stairs;

    public Joystick joystick;
    public float  FinishForwardSens, enemyFightSpeed, towerIncreseSens, setCharPosDur, targetLocPosSense, towerAnimDur, towerSense, towerHorizontalDistance, towerVerticalDistance, jsSensivity, forwardSpeed, CamSens, toweringCamSens, distortionRate, distortion,  groupWalkSens, stopDist;
    public int startMultiplier = 1;
    public bool bossLv = false;
    public List<float> distortions = new List<float>();

    public int playerCount = 1;
    public Vector3 camOffs;
    public int setGroupDuration = 25;
    public float spawnSense;
    GameObject cam, camFallower;
    float x, z, camX, camZ;
    Vector3 towerTargerPos, TargetPos, CamTargetPos;
    Quaternion CamTargetRot;

    NavMeshAgent PlayerNavMesh;

    [Range(0f, 1f)][SerializeField] private float DistanceFactor, Radius;
    //float[] rads;
    bool groupTrig = true, towering = false, lockTowerY = false;

    public Canvas PlayerCountCv;
    public Text PlayerCountText;
    public bool runToEnemy = false;
    public GameObject targetEnemy;
    float mainPlayerAgentSpeed;

    public int row = 1, charCountonRow = 1;
    float endZ;
    float towerLastY;
    public float lockTowerOffs = 0;
    int finishCharCount = 0;

    public float camTimeLeft = 4.0f;

    Vector3 startPos = Vector2.zero;
    Vector3 startPos2 = Vector2.zero;
    public float smoothSpeed = 0.25f;
    public float sens;
    Vector3 smoothedPosition;
    Vector3 mousePos;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        FinishLine = GameObject.Find("FinishLine");
        Chars = GameObject.Find("Chars");
        cam = GameObject.Find("Main Camera");
        Tower = GameObject.Find("Tower");
        Stairs = GameObject.Find("Stairs");
        FinishPanel = joystick.transform.parent.GetChild(2).gameObject;
        camFallower = GameObject.Find("CameraFallower");
        CamTargetRot = GameObject.Find("CameraRotation").transform.rotation;
        //rads = new float[] {UnityEngine.Random.Range(0, 1)};
        distortions.Add(UnityEngine.Random.Range(-distortionRate, distortionRate));
        mainPlayerAgentSpeed = Chars.transform.GetChild(0).GetComponent<NavMeshAgent>().speed;
        endZ = FinishLine.transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        //Reloads the current scene
        if (Input.GetKeyDown(KeyCode.R))
        {
            Restart();
        }

    }

    void FixedUpdate()
    {
        CameraController();
        if(Chars.transform.position.z > endZ && !towering)
        {
            Chars.transform.position = Vector3.MoveTowards(Chars.transform.position,
                                                    new Vector3(0, 0, Stairs.transform.GetChild(Stairs.transform.childCount - 1).position.z),
                                                    FinishForwardSens * Time.deltaTime);
            finishCharCount = Chars.transform.childCount;
            ReachtoFinish();
        }
        else if (towering)
        {
            Chars.transform.position = Vector3.MoveTowards(Chars.transform.position,
                                                    new Vector3(0, 0, Stairs.transform.GetChild(Stairs.transform.childCount - 1).position.z),
                                                    FinishForwardSens * Time.deltaTime);
            SetTheTowerPos();
        }
        else if(!runToEnemy)
        {
            //InputsController();
            DragController();
        }

        if (groupTrig)
        {
            for (int i = 0; i < Chars.transform.childCount; i++)
            {
                //Chars.transform.GetChild(i).localPosition = Vector3.MoveTowards(Chars.transform.GetChild(i).localPosition, Vector3.zero, groupWalkSens * Time.deltaTime);

                /*Chars.transform.GetChild(i).position = Vector3.MoveTowards(Chars.transform.GetChild(i).position,
                                                                           PlayerCountCv.transform.position - (Vector3.up * PlayerCountCv.transform.position.y) + (Vector3.up*0.1f),
                                                                           groupWalkSens * Time.deltaTime);*/

                Chars.transform.GetChild(i).GetComponent<NavMeshAgent>().destination = runToEnemy ? targetEnemy.transform.position : Chars.transform.position;
                Chars.transform.GetChild(i).GetComponent<NavMeshAgent>().speed = runToEnemy ? enemyFightSpeed : mainPlayerAgentSpeed;

                /*x = DistanceFactor * Mathf.Sqrt(i) * Mathf.Cos(i * Radius) + distortions[i];
                z = DistanceFactor * Mathf.Sqrt(i) * Mathf.Sin(i * Radius) + distortions[i];
                TargetPos = new Vector3(x, 0, z);

                Chars.transform.GetChild(i).localPosition = Vector3.MoveTowards(Chars.transform.GetChild(i).localPosition, TargetPos, spawnSense * Time.deltaTime);*/
            }
        }
        //Chars.GetComponent<NavMeshAgent>().destination = Chars.GetComponent<NavMeshAgent>().destination + Vector3.forward;
    }

    //Controls the inputs
    void InputsController()
    {
        //Controls char's x position
        //Chars.transform.position += new Vector3(joystick.Horizontal * jsSensivity * Time.deltaTime, 0, forwardSpeed * jsSensivity * Time.deltaTime);
        Chars.transform.position += new Vector3(0, 0, forwardSpeed * jsSensivity * Time.deltaTime);

    }

    void DragController()
    {
        Chars.transform.position += new Vector3(0, 0, forwardSpeed * jsSensivity * Time.deltaTime);
        mousePos = new Vector3( Remap(Input.mousePosition.x, -sens + 10, Screen.width + sens - 10, -2.8f, 2.8f), 0, 0 );
        if (Input.GetMouseButtonDown(0))
        {
            startPos = mousePos;
            startPos2 = Chars.transform.position;
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 push = mousePos - startPos;

            if (mousePos.x < startPos.x && Chars.transform.position.x > -2.8f)
            {
                smoothedPosition = Vector3.Lerp(Chars.transform.position, startPos2 + push, smoothSpeed);
                Chars.transform.position = new Vector3(smoothedPosition.x, Chars.transform.position.y, Chars.transform.position.z);
            }
            else if (mousePos.x > startPos.x && Chars.transform.position.x < 2.8f)
            {
                smoothedPosition = Vector3.Lerp(Chars.transform.position, startPos2 + push, smoothSpeed);
                Chars.transform.position = new Vector3(smoothedPosition.x, Chars.transform.position.y, Chars.transform.position.z);
            }
        }
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

                distortions.Add(UnityEngine.Random.Range(-distortionRate, distortionRate));
            }
            //camOffs += new Vector3(0, t, -t);
        }

        //Removes players
        else
        {
            for (int i = 0; i < opNumber; i++)
            {
                //Destroy(Chars.transform.GetChild(t - i).gameObject);
                distortions.Remove(distortions.Count - 1);
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

            PlayerNavMesh = Chars.transform.GetChild(i).GetComponent<NavMeshAgent>();
            //PlayerNavMesh.avoidancePriority = i >= 99 ? 99 : i + 1;

            PlayerNavMesh.avoidancePriority = i % 2 == 0 ? i+1 : i + 2;

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

        CamTargetPos = !towering ? new Vector3(camX/Chars.transform.childCount, 0, camZ / Chars.transform.childCount) + camOffs
                                 : Tower.transform.position + camOffs - (Tower.transform.position.y * Vector3.up/2) + new Vector3(15.17f, 0, 2.7f);
        PlayerCountCv.transform.position = CamTargetPos - camOffs + (Vector3.up * 1.52f);

        camX = 0;
        camZ = 0;
        cam.transform.position = !towering ? Vector3.Lerp(cam.transform.position, CamTargetPos, CamSens * Time.deltaTime) : Vector3.Lerp(cam.transform.position, CamTargetPos, toweringCamSens * Time.deltaTime);

        if (towering)
        {
            camTimeLeft -= Time.deltaTime;
            if(camTimeLeft <= 4f &&  camTimeLeft > 2f)
            {
                //cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, CamTargetRot, CamSens * Time.deltaTime);
                camFallower.transform.position = cam.transform.position;
                camFallower.transform.LookAt(Tower.transform.GetChild(22));
                cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, camFallower.transform.rotation, toweringCamSens * Time.deltaTime);
            }
            else if (camTimeLeft <= 2f)
            {
                //cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, CamTargetRot, CamSens * Time.deltaTime);
                camFallower.transform.position = cam.transform.position;
                camFallower.transform.LookAt(Tower.transform.GetChild(22));
                cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, camFallower.transform.rotation, toweringCamSens * Time.deltaTime);
            }
        }
    }

    public void ReachtoFinish()
    {
        //jsSensivity = 1;
        groupTrig = false;
        

        for (int i = 0; i < Chars.transform.childCount; i++)
        {
            PlayerNavMesh = Chars.transform.GetChild(i).GetComponent<NavMeshAgent>();
            PlayerNavMesh.enabled = false;
        }

        towering = true;
        Tower.transform.position = Chars.transform.position;
        StartCoroutine(SetTowerPos());

        /*for (int i = 0; i < Chars.transform.childCount; i++)
        {
            //Chars.transform.GetChild(i).SetParent(Tower.transform); //tower script
            Chars.transform.GetChild(i).localPosition = new Vector3(((charCountonRow - 1) * towerHorizontalDistance), (1 - row) * towerVerticalDistance, 0);
            for (int j = 1; j < charCountonRow; j++)
            {
                i++;
                Chars.transform.GetChild(i).localPosition = new Vector3(((charCountonRow - 1) * towerHorizontalDistance) - (2 * j * towerHorizontalDistance), (1 - row) * towerVerticalDistance, 0);
            }
            //row++;
            //Chars.transform.position += Vector3.up;
            row++;
            Chars.transform.position += Vector3.up * towerVerticalDistance;
            for (int j = 0; j < charCountonRow; j++)
            {
                i++;
                Chars.transform.GetChild(i).localPosition = new Vector3(((charCountonRow - 1) * towerHorizontalDistance) - (2 * j * towerHorizontalDistance), (1 - row) * towerVerticalDistance, 0);
            }
            row++;
            Chars.transform.position += Vector3.up * towerVerticalDistance;
            charCountonRow++;
        }*/

        //SetTowerPos();

        //StartCoroutine(SetTowerPos());

        /*for (int i = 0; i < Chars.transform.childCount; i++)
        {
            //Chars.transform.GetChild(i).SetParent(Tower.transform); //tower script
            Chars.transform.GetChild(i).localPosition = new Vector3(((charCountonRow - 1) * towerHorizontalDistance), (1-row) * towerVerticalDistance, 0);
            for (int j = 1; j < charCountonRow; j++)
            {
                i++;
                Chars.transform.GetChild(i).localPosition = new Vector3(((charCountonRow - 1) * towerHorizontalDistance) - (2*j * towerHorizontalDistance), (1 - row) * towerVerticalDistance, 0);
            }
            //row++;
            //Chars.transform.position += Vector3.up;
            row++;
            Chars.transform.position += Vector3.up * towerVerticalDistance;
            for (int j = 0; j < charCountonRow; j++)
            {
                i++;
                Chars.transform.GetChild(i).localPosition = new Vector3(((charCountonRow - 1) * towerHorizontalDistance) - (2*j * towerHorizontalDistance), (1 - row) * towerVerticalDistance, 0);
            }
            row++;
            Chars.transform.position += Vector3.up * towerVerticalDistance;
            charCountonRow++;
        }*/
    }

    IEnumerator SetTowerPos()
    {
        while(charCountonRow < Chars.transform.childCount) { 
            int i = 0;
            //Chars.transform.GetChild(i).localPosition = new Vector3(((charCountonRow - 1) * towerHorizontalDistance), (1 - row) * towerVerticalDistance, 0);
            

            SetTowerElement(Chars.transform.GetChild(i).gameObject,
                 new Vector3(((charCountonRow - 1) * towerHorizontalDistance), (1 - row) * towerVerticalDistance, 0));

            for (int j = 1; j < charCountonRow; j++)
            {
                //i++;
                //Chars.transform.GetChild(i).localPosition = new Vector3(((charCountonRow - 1) * towerHorizontalDistance) - (2 * j * towerHorizontalDistance), (1 - row) * towerVerticalDistance, 0);
                SetTowerElement(Chars.transform.GetChild(i).gameObject,
                     new Vector3( ((charCountonRow - 1) * towerHorizontalDistance) - (2 * j * towerHorizontalDistance), (1 - row) * towerVerticalDistance, 0 ));
            }
            yield return new WaitForSeconds(0.01f * (setCharPosDur));//wait for the row position set
            //row++;
            //Chars.transform.position += Vector3.up;
            //Chars.transform.position += Vector3.up * towerVerticalDistance;

            row++;
            /*if (charCountonRow > Chars.transform.childCount)
            {
                while (Chars.transform.childCount > 0)
                {
                    Destroy(Chars.transform.GetChild(0).gameObject);
                }
            }*/
            //StartCoroutine(SetTheTowerPos());
            //yield return new WaitForSeconds(0.01f * (setCharPosDur));//wait for the tower position set

            for (int j = 0; j < charCountonRow && Chars.transform.childCount>0; j++)
            {
                //i++;
                //Chars.transform.GetChild(i).localPosition = new Vector3(((charCountonRow - 1) * towerHorizontalDistance) - (2 * j * towerHorizontalDistance), (1 - row) * towerVerticalDistance, 0);
                SetTowerElement(Chars.transform.GetChild(i).gameObject,
                     new Vector3(((charCountonRow - 1) * towerHorizontalDistance) - (2 * j * towerHorizontalDistance), (1 - row) * towerVerticalDistance, 0 ));
            }
            //Chars.transform.position += Vector3.up * towerVerticalDistance;

            yield return new WaitForSeconds(0.01f * (setCharPosDur));//wait for the tower position set


            /*if (charCountonRow > Chars.transform.childCount)
            {
                while (Chars.transform.childCount > 0)
                {
                    Destroy(Chars.transform.GetChild(0).gameObject);
                }
            }*/


            //StartCoroutine(SetTheTowerPos());
            charCountonRow++;
            if (Chars.transform.childCount < charCountonRow * 2)
            {
                for(int f=Chars.transform.childCount-1; f>=0; f--)
                {
                    Destroy(Chars.transform.GetChild(f).gameObject);
                }
                //lockTowerOffs = towerVerticalDistance * 2;
                break;
            }
            else
            {
                row++;
            }
        }
        /*while (Chars.transform.childCount > 0)
        {
            Destroy(Chars.transform.GetChild(0).gameObject);
        }*/
        lockTowerY = true;
    }

    void SetTowerElement(GameObject towerElement, Vector3 targetLocPos)
    {
        towerElement.transform.SetParent(Tower.transform);
        StartCoroutine( SetCharPos(towerElement.gameObject, targetLocPos) );

    }

    IEnumerator SetCharPos(GameObject curChar, Vector3 targetLocPos)
    {
        for (int i = 0; i < setCharPosDur; i++)
        {
            curChar.transform.localPosition = Vector3.MoveTowards(curChar.transform.localPosition, targetLocPos, targetLocPosSense  * Time.deltaTime /2.5f);
            //SetTheTowerPos();
            yield return new WaitForSeconds(0.01f);
        }        
    }

    void SetTheTowerPos()
    {
        towerLastY = !lockTowerY ? Tower.transform.position.y + towerIncreseSens * Time.deltaTime : row * towerVerticalDistance + lockTowerOffs;
        towerTargerPos = new Vector3(0, towerLastY, Chars.transform.position.z);
        
        Tower.transform.position = Vector3.MoveTowards(Tower.transform.position, towerTargerPos, targetLocPosSense * Time.deltaTime);
        if (Tower.transform.childCount<1)
        {
            EndOfGame();
        }
    }

    IEnumerator SetGroupPos(GameObject Runner, int ind)
    {
        if(ind == 0)
            groupTrig = false;

        for (int i = 0; i < setGroupDuration; i++)
        {
            //Must be modified as written ourself
            x = DistanceFactor * Mathf.Sqrt(ind) * Mathf.Cos(ind * Radius) + distortions[ind];
            z = DistanceFactor * Mathf.Sqrt(ind) * Mathf.Sin(ind * Radius) + distortions[ind];
            TargetPos = new Vector3(x, 0, z);

            Runner.transform.localPosition = Vector3.MoveTowards(Runner.transform.localPosition, TargetPos, spawnSense * Time.deltaTime);
            yield return new WaitForSeconds(0.01f);
        }

        /*for (int i = 0; i < setGroupDuration; i++)
        {
            Runner.transform.localPosition = Vector3.MoveTowards(Runner.transform.localPosition, Vector3.zero, spawnSense * Time.deltaTime);
            yield return new WaitForSeconds(0.01f);
        }*/
        if(ind == Chars.transform.childCount-1)
            groupTrig = true;
    }

    void EndOfGame()
    {
        Time.timeScale = 0;
        FinishPanel.SetActive(true);
        startMultiplier = (int)(1 + (row - 2) * 0.2f);
        PlayerPrefs.SetInt("coin", PlayerPrefs.GetInt("coin", 0) + finishCharCount * startMultiplier);
        FinishPanel.transform.GetChild(5).GetComponent<Text>().text = PlayerPrefs.GetInt("coin").ToString();
        FinishPanel.transform.GetChild(2).GetComponent<Text>().text = "+" + finishCharCount * startMultiplier;
    }
    public static float Remap(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

    public void ResetCoinCaunt()
    {
        PlayerPrefs.SetInt("coin", 0);
        FinishPanel.transform.GetChild(5).GetComponent<Text>().text = PlayerPrefs.GetInt("coin").ToString();
    }

    public void NextLevel()
    {

    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
    }
}
