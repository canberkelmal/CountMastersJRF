using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyGroupSc : MonoBehaviour
{
    public int numberOfEnemies;
    public float groupSens;
    public Text EnemyCountTx;

    [Range(0f, 1f)][SerializeField] private float DistanceFactor, Radius;

    // Start is called before the first frame update
    void Start()
    {
        EnemyCountTx.text = numberOfEnemies.ToString();
        for (int i = 1; i < numberOfEnemies; i++)
        {
            Instantiate(transform.GetChild(1).gameObject, transform.GetChild(1).position, transform.rotation, this.transform);
        }
        SetEnemyPos();
    }

    //Set the target position for each enemy
    void SetEnemyPos()
    {
        for (int i = 1; i < numberOfEnemies; i++)
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
    public void SetCountText()
    {
        numberOfEnemies--;
        EnemyCountTx.text = numberOfEnemies.ToString();
        if(numberOfEnemies==0)
        {
            Destroy(gameObject);
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

    }
}
