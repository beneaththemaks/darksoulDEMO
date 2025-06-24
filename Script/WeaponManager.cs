using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : IActorManagerInterface
{
    //public ActorManager am;

    private Collider weaponColL;
    private Collider weaponColR;

    public GameObject whL;
    public GameObject whR;

    public WeaponController wcL;
    public WeaponController wcR;


    void Start()
    {
        whL = transform.DeepFind("weaponHandleL").gameObject;
        whR = transform.DeepFind("weaponHandleR").gameObject;

        wcL = BindWeaponController (whL);
        wcR = BindWeaponController (whR);

        weaponColL = whL.GetComponentInChildren<Collider> ();
        weaponColR = whR.GetComponentInChildren<Collider> ();

        //weaponCol = whR.GetComponentInChildren<Collider> ();
        //print(transform.DeepFind("weaponHandleL"));
    }

    public WeaponController BindWeaponController (GameObject targetObj)
    {
        WeaponController tempWc;
        tempWc = targetObj.GetComponent<WeaponController> ();
        if (tempWc == null) {
            tempWc = targetObj.AddComponent<WeaponController>();
        }
        tempWc.wm = this;
        return tempWc;
    }

    public void WeaponEnable()
    {
        if (am.ac.CheckStateTag("attackL"))
        {
           weaponColL.enabled = true;
        }
        else
        {
            weaponColR.enabled = true;
        }
        //print("WeaponEnable");
    }

    public void WeaponDisable()
    {
        weaponColR.enabled = false;
        weaponColL.enabled = false;
        //print("WeaponDisable");
    }
}
