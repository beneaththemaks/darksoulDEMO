using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorManager : MonoBehaviour
{
    public ActorController ac;

    [Header("=== Auto Generate if Null ===")]
    public BattleManager bm;
    public WeaponManager wm;
    public StateManager sm;
    // Start is called before the first frame update
    void Awake()
    {
        ac = GetComponent<ActorController> ();

        GameObject model = ac.model;
        GameObject sensor = transform.Find("sensor").gameObject;

        bm = Bind<BattleManager>(sensor);
        wm = Bind<WeaponManager>(model);      
        sm = Bind<StateManager>(gameObject);
        sm.Test();


    }

    private T Bind<T>(GameObject go) where T : IActorManagerInterface
    {
        T tempInst;
        tempInst = go.GetComponent<T> ();
        if (tempInst == null) { 
            tempInst = go.AddComponent<T> ();  
        }
        tempInst.am = this;

        return tempInst;
    } 


    // Update is called once per frame
    void Update()
    {
        
    }

    public void TryDoDamage()
    {
        //sm.HP -= 5;
        //if(sm.HP > 0) { 
        //   sm.AddHP(-5);
        //}
        if (sm.isImmortal)
        {
            //Do nothing!!
        }

        else if (sm.isDefense)
        {
            // Attack should be blocked
            Blocked();
        }
        else
        {
            if(sm.HP <= 0)
            {
                // Already dead

            }
            else
            {
                sm.AddHP(-5);
                if (sm.HP > 0)
                {
                    Hit();
                }
                else
                {
                    Die();
                }
            }
        }
    }

    public void Blocked()
    {
        ac.IssueTrigger("blocked");
    }

    public void Hit()
    {
        ac.IssueTrigger("hit");
    }

    public void Die()
    {
        ac.IssueTrigger("die");
        ac.pi.inputEnabled = false;
        if(ac.camcon.lockState == true)
        {
            ac.camcon.LockUnlock();
        }
            ac.camcon.enabled = false;
    }
}
