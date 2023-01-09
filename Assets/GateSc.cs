using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateSc : MonoBehaviour
{
    public string LeftGateOp = "Plus";
    public int LeftGateValue = 0;

    public string RightGateOp = "Minus";
    public int RightGateValue = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void FixedUpdate()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        //Passes from left
        if(other.transform.position.x<=0)
        {
            switch (LeftGateOp)
            {
                case "Plus":
                    LeftGateValue = 0;
                    break;

                case "Minus":
                    LeftGateValue = 0;
                    break;

            }
        }

        //Passes from left
        if (other.transform.position.x <= 0)
        {
            switch (LeftGateOp)
            {
                case "Plus":
                    LeftGateValue = 0;
                    break;

                case "Minus":
                    LeftGateValue = 0;
                    break;

            }
        }
    }
}
