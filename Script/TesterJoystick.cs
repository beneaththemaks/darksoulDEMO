using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TesterJoystick : MonoBehaviour
{


    // Update is called once per frame
    void Update()
    {
        print(Input.GetAxis("axisX"));
        //print(Input.GetAxis("Vertical"));
        //print("btn: " + Input.GetButtonDown("btn0"));
    }
}
