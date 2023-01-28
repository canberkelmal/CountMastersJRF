using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyGroupSc : MonoBehaviour
{
    public int numberOfEnemies;
    public float groupSens;
    public Text EnemyCountTx;
    public GameObject refEnemy, chars;
    GameManager gm;
    float tempJsSensivity, tempAgentSpeed;

    [Range(0f, 1f)][SerializeField] private float DistanceFactor, Radius;

    // Start is called before the first frame update
    void Start()
    {
        chars = GameObject.Find("Chars");
        refEnemy = transform.GetChild(2).gameObject;
        EnemyCountTx.text = numberOfEnemies.ToString();
        gm = GameObject.Find("Game Manager").GetComponent<GameManager>();
        for (int i = 2; i <= numberOfEnemies; i++)
        {
            Instantiate(refEnemy, refEnemy.transform.position, transform.rotation, this.transform);
        }
        SetEnemyPos();
    }


    //Set the target position for each enemy
    void SetEnemyPos()
    {
        for (int i = 2; i < numberOfEnemies; i++)
        {
            GameObject currentEnemy = transform.GetChild(i).gameObject;
            //Must be modified as written ourself
            var x = DistanceFactor * Mathf.Sqrt(i) * Mathf.Cos(i * Radius);
            var z = DistanceFactor * Mathf.Sqrt(i) * Mathf.Sin(i * Radius);
            var TargetPos = new Vector3(x, currentEnemy.transform.localPosition.y, z);
            StartCoroutine(SendEnemyToPos(currentEnemy, TargetPos));
        }
    }

    //Decrease number of enemies
    public void SetCountText(GameObject triggeredEnemy)
    {
        numberOfEnemies = transform.childCount - 3;
        EnemyCountTx.text = numberOfEnemies.ToString();
        if(numberOfEnemies<=0)
        {
            gm.jsSensivity = tempJsSensivity;
            gm.runToEnemy = false;
            Destroy(gameObject);
        }
        else
        {
            Destroy(triggeredEnemy);
        }
    }

    //Send the enemy to the position
    IEnumerator SendEnemyToPos(GameObject ce, Vector3 tp)
    {
        for (int i = 0; i < 10; i++)
        {
            ce.transform.localPosition = Vector3.MoveTowards(ce.transform.localPosition, tp, groupSens * Time.deltaTime);
            yield return new WaitForSeconds(0.01f);
        }
        //ce.GetComponent<NavMeshAgent>().enabled = false;

    }

    public void Triggered()
    {
        tempJsSensivity = gm.jsSensivity;
        gm.jsSensivity = 0;
        gm.targetEnemy = this.transform.gameObject;
        gm.runToEnemy= true;
        for (int i = 2; i < numberOfEnemies+2; i++)
        {
            GameObject currentEnemy = transform.GetChild(i).gameObject;

            currentEnemy.GetComponent<NavMeshAgent>().destination = chars.transform.position;            
        }
    }
}
