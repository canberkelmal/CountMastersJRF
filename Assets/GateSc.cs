using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class GateSc : MonoBehaviour
{
    #region variables
    enum LeftGateOp {Plus, Minus, Multiply, Devide}
    [SerializeField]
    [Header("Left Door Settings")]
    LeftGateOp leftGateOp;
    public int LeftGateValue = 0;

    enum RightGateOp {Plus, Minus, Multiply, Devide}
    [SerializeField]
    [Header("Right Door Settings")]
    RightGateOp rightGateOp;
    public int RightGateValue = 0;

    GameManager gm;
    #endregion

    #region Unity
    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void FixedUpdate()
    {

    }
    void OnTriggerEnter(Collider other)
    {
        //Passes from left side of the gate
        if (other.transform.position.x <= 0)
        {
            switch (leftGateOp)
            {
                case LeftGateOp.Plus:
                    LeftGateValue = 0;
                    break;

                case LeftGateOp.Minus:
                    LeftGateValue = 0;
                    break;

            }
        }

        //Passes from right side of the gate
        if (other.transform.position.x > 0)
        {
            switch (rightGateOp)
            {
                case RightGateOp.Plus:
                    RightGateValue = 0;
                    break;

                case RightGateOp.Minus:
                    RightGateValue = 0;
                    break;

            }
        }
    }
    #endregion

    #region Functions



    #endregion

}
