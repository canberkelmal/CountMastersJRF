using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
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
    Text leftGateText;
    Text rightGateText;
    #endregion

    #region Unity
    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.Find("Game Manager").GetComponent<GameManager>();
        leftGateText = transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>();
        rightGateText = transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<Text>();
        
        switch (leftGateOp)
        {
            case LeftGateOp.Plus:
                leftGateText.text = "+" + LeftGateValue.ToString();
                break;
            case LeftGateOp.Minus:
                leftGateText.text = "-" + LeftGateValue.ToString();
                break;
            case LeftGateOp.Devide:
                leftGateText.text = "/" + LeftGateValue.ToString();
                break;
            case LeftGateOp.Multiply:
                leftGateText.text = "x" + LeftGateValue.ToString();
                break;
        }

        switch (rightGateOp)
        {
            case RightGateOp.Plus:
                rightGateText.text = "+" + RightGateValue.ToString();
                break;
            case RightGateOp.Minus:
                rightGateText.text = "-" + RightGateValue.ToString();
                break;
            case RightGateOp.Devide:
                rightGateText.text = "/" + RightGateValue.ToString();
                break;
            case RightGateOp.Multiply:
                rightGateText.text = "x" + RightGateValue.ToString();
                break;
        }
    
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
        if (other.gameObject.tag == "Player")
        {
            this.gameObject.GetComponent<BoxCollider>().enabled = false;
            GateDetector(other.transform.parent.gameObject);
        }        
    }


    #endregion

    #region Functions
    void GateDetector(GameObject player)
    {
        //Passes from left side of the gate
        if (player.transform.position.x <= 0)
        {
            switch (leftGateOp)
            {
                case LeftGateOp.Plus:
                    print("Plus triggered.");
                    break;

                case LeftGateOp.Minus:
                    print("Minus triggered.");
                    break;

            }
        }

        //Passes from right side of the gate
        if (player.transform.position.x > 0)
        {
            switch (rightGateOp)
            {
                case RightGateOp.Plus:
                    print("Plus triggered.");
                    break;

                case RightGateOp.Minus:
                    print("Minus triggered.");
                    break;

            }
        }
    }


    #endregion

}
