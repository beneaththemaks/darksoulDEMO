using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickInput : IUserInput
{
    [Header("==== Joystick Settings =====")]
    public string axisX = "axisX";
    public string axisY = "axisY";
    public string axisJright = "axis4";
    public string axisJup = "axis5";
    public string btnA = "btn0";
    public string btnB = "btn1";
    public string btnX = "btn2";
    public string btnY = "btn3";
    public string btnLB = "btn4";
    public string btnRB = "btn5";
    public string btnLT = "LT";
    public string btnRT = "RT";
    public string btnJstick = "btn9";

    public MyButton buttonA = new MyButton ();
    public MyButton buttonB = new MyButton ();
    public MyButton buttonX = new MyButton ();
    public MyButton buttonY = new MyButton ();
    public MyButton buttonLB = new MyButton ();
    public MyButton buttonRB = new MyButton();
    public MyButton buttonLT = new MyButton ();
    public MyButton buttonRT = new MyButton();
    public MyButton buttonJstick = new MyButton();


    //[Header("==== Output singals =====")]
    //public float Dup;
    //public float Dright;
    //public float Dmag;
    //public Vector3 Dvec;
    //public float Jup;
    //public float Jright;

    ////1.pressing singal
    //public bool run;
    ////2.trigger once singal
    //public bool jump;
    //public bool lastJump;
    //public bool attack;
    //public bool lastAttack;
    ////3.double trigger


    //[Header("==== Others =====")]

    //public bool inputEnabled = true;

    //private float targetDup;
    //private float targetDright;
    //private float velocityDup;
    //private float velocityDright;


    // Update is called once per frame
    void Update()
    {
        buttonA.Tick(Input.GetButton(btnA));
        buttonB.Tick(Input.GetButton(btnB));
        buttonX.Tick(Input.GetButton(btnX));
        buttonY.Tick(Input.GetButton(btnY));
        buttonLB.Tick(Input.GetButton(btnLB));
        buttonRB.Tick(Input.GetButton(btnRB));
        buttonLT.Tick(Input.GetButton(btnLT));
        buttonRT.Tick(Input.GetButton(btnRT));
        buttonJstick.Tick(Input.GetButton(btnJstick));

        //print(buttonA.IsExtending && buttonA.OnPressed);
        //print(buttonJstick.OnPressed);


        Jup = -1 * Input.GetAxis(axisJup);
        Jright = Input.GetAxis(axisJright);

        targetDup = Input.GetAxis (axisY);
        targetDright = Input.GetAxis (axisX);


        if (!inputEnabled)
        {
            targetDup = 0;
            targetDright = 0;
        }

        Dup = Mathf.SmoothDamp(Dup, targetDup, ref velocityDup, 0.1f);
        Dright = Mathf.SmoothDamp(Dright, targetDright, ref velocityDright, 0.1f);

        Vector2 tempDAxis = SquareToCircle(new Vector2(Dright, Dup));
        float Dright2 = tempDAxis.x;
        float Dup2 = tempDAxis.y;

        Dmag = Mathf.Sqrt((Dup2 * Dup2) + (Dright2 * Dright2));
        Dvec = Dright2 * transform.right + Dup2 * transform.forward;

        run = (buttonA.IsPressing && !buttonA.IsDelaying) || buttonA.IsExtending;
        jump = buttonA.OnPressed && buttonA.IsExtending;
        roll = buttonA.OnReleased && buttonA.IsDelaying;

        defense = buttonLB.IsPressing;
        //attack = buttonX.OnPressed;
        rb = buttonRB.OnPressed;
        rt = buttonRT.OnPressed;
        lt = buttonLT.OnPressed;
        lb = buttonLB.OnPressed;
        lockon = buttonJstick.OnPressed;

        //bool newJump = Input.GetButton(btnB);
        //if (newJump != lastJump && newJump == true)
        //{
        //    jump = true;
        //}
        //else
        //{
        //    jump = false;
        //}
        //lastJump = newJump;


        //bool newAttack = Input.GetButton(btnX);


        //if (newAttack != lastJump && newAttack == true)
        //{
        //    attack = true;
        //}
        //else
        //{
        //    attack = false;
        //}
        //lastAttack = newAttack;
    }

}
