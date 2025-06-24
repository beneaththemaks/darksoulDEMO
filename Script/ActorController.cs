using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class ActorController : MonoBehaviour
{

    public GameObject model;
    public CameraController camcon;
    public IUserInput pi;
    public float walkSpeed = 2.4f;
    public float runMultiplier = 2.0f;
    public float jumpVelocity = 5.0f;
    public float rollVelocity = 1.0f;


    [Space(10)]
    [Header("===== Friction Settings =====")]
    public PhysicMaterial frictionOne;
    public PhysicMaterial frictionZero;

    private Animator anim;
    private Rigidbody rigid;
    private Vector3 planarVec;
    private Vector3 thrustVec;
    private bool canAttack;
    private bool lockPlanar = false;
    private bool trackDirection = false;
    private CapsuleCollider col;
    //private float lerpTarget;
    private Vector3 deltaPos;

    
    public bool leftIsShield = true;


    // Start is called before the first frame update
    void Awake ()
    {
        IUserInput[] inputs = GetComponents<IUserInput> ();
        foreach (var input in inputs) {
            if (input.enabled == true) {
                pi = input;
                    break;
            }
        }

        anim = model.GetComponent<Animator> ();
        rigid = GetComponent<Rigidbody> ();
        col = GetComponent<CapsuleCollider> ();
    }

    // Update is called once per frame
    void Update() //Time.DeltaTime = 1/60
    {
        if (pi.lockon)
        {
            camcon.LockUnlock();
        }

        if(camcon.lockState == false)
        {
            anim.SetFloat("forward", pi.Dmag * Mathf.Lerp(anim.GetFloat("forward") , ((pi.run) ? 2.0f : 1.0f), 0.5f));
            anim.SetFloat("right", 0);
        }
        else
        {
            Vector3 localDvec = transform.InverseTransformVector(pi.Dvec);
            anim.SetFloat("forward", localDvec.z * ((pi.run) ? 2.0f : 1.0f));
            anim.SetFloat("right", localDvec.x * ((pi.run) ? 2.0f : 1.0f));
        }

            //print(pi.Dup);
            //float targetRunMulti = ((pi.run) ? 2.0f : 1.0f);
            //anim.SetBool("defense", pi.defense);


        if(pi.roll || rigid.velocity.magnitude > 7f)
        {
            anim.SetTrigger("roll");
            canAttack = false;
        }

        if (pi.jump)
        {
            anim.SetTrigger("jump");
            canAttack = false;   
        } 

        if ((pi.rb || pi.lb) && canAttack && (CheckState("ground") || CheckStateTag("attackR") || CheckStateTag("attackL")) )
        {
            if (pi.rb)
            {
                anim.SetBool("R0L1" , false);
                anim.SetTrigger("attack");
            }
            else if(pi.lb && !leftIsShield)
            {
                anim.SetBool("R0L1" , true);
                anim.SetTrigger("attack");
            }
        }

        if ((pi.lb || pi.lt) && (CheckState("ground") || CheckStateTag("attackR") || CheckStateTag("attackL")))
        {
            if (pi.rt)
            {
                //do right heavy attack
            }
            else
            {
                if (!leftIsShield) { 
                    //do left heavy attack
                }
                else { 
                    anim.SetTrigger("counterBack"); 
                }
            }
        }

        if (leftIsShield)
        {
            if (CheckState("ground") || CheckState("blcoked"))
            {
                anim.SetBool("defense", pi.defense);
                anim.SetLayerWeight(anim.GetLayerIndex("defense"), 1);
            }
            else
            {
                anim.SetBool("defense", false);
                anim.SetLayerWeight(anim.GetLayerIndex("defense"), 0);
            }
        }
        else
        {
            anim.SetLayerWeight(anim.GetLayerIndex("defense"), 0);
        }

        //anim.SetLayerWeight(anim.GetLayerIndex("defense"), 0);

        if(camcon.lockState == false)
        {
            if (pi.Dmag > 0.1f)
           {

              model.transform.forward = Vector3.Slerp(model.transform.forward, pi.Dvec, 0.3f);
           }

            if (lockPlanar == false)
           {
              planarVec = pi.Dmag * model.transform.forward * walkSpeed * ((pi.run) ? runMultiplier : 1.0f);
           }

        }
        else
        {
            if(trackDirection == false)
            {
                model.transform.forward = transform.forward;
            }
            else
            {
                model.transform.forward = planarVec.normalized;
            }

            if (lockPlanar == false)
            {
                planarVec = pi.Dvec * walkSpeed * ((pi.run) ? runMultiplier : 1.0f);
            }
        }

    }

    void FixedUpdate()  // Time.fixedDeltaTime = 1/50
    {
        rigid.position += deltaPos; 
        // rigid.position += planarVec * Time.fixedDeltaTime;
        rigid.velocity = new Vector3(planarVec.x , rigid.velocity.y , planarVec.z) + thrustVec;
        thrustVec = Vector3.zero;
        deltaPos = Vector3.zero;
    }


    public bool CheckState(string stateName, string layerName = "Base Layer")
    {
       return anim.GetCurrentAnimatorStateInfo(anim.GetLayerIndex (layerName)).IsName(stateName);
    }

    public bool CheckStateTag(string tagName, string layerName = "Base Layer")
    {
       return anim.GetCurrentAnimatorStateInfo(anim.GetLayerIndex(layerName)).IsTag(tagName);
    }

    /// 
    /// Message processing block
    ///


    public void OnJumpEnter()
    {
        //print("on jump enter");
        thrustVec = new Vector3(0, jumpVelocity, 0);
        pi.inputEnabled = false;
        lockPlanar = true;
        trackDirection = true;
    }

//   public void OnJumpExit()
//    {
//        //print("on jump exit");
//        pi.inputEnabled = true;
//        lockPlanar = false;
//    }

    public void IsGround()
    {
        //print("is on ground");
        anim.SetBool("isGround", true);
    }

    public void IsNotGround()
    {
        //print("is not on ground!!!!");
        anim.SetBool("isGround", false);
    }

    public void OnGroundEnter()
    {
        pi.inputEnabled = true;
        lockPlanar = false;
        canAttack = true;
        col.material = frictionOne;
        trackDirection = false;
    }

    public void OnGroundExit()
    {
        col.material = frictionZero;
    }

    public void OnFallEnter()
    {
        pi.inputEnabled = false;
        lockPlanar = true;
    }

    public void OnRollEnter()
    {
        thrustVec = new Vector3(0, rollVelocity, 0);
        pi.inputEnabled = false;
        lockPlanar = true;
        trackDirection = true;
    }

    public void OnJabEnter()
    {
        
        pi.inputEnabled = false;
        lockPlanar = true;
    }

    public void OnJabUpdate()
    {
       thrustVec = model.transform.forward * anim.GetFloat("jabVelocity");
    }



    public void OnAttack1hAEnter()
    {
        pi.inputEnabled = false;
        //lockPlanar = true;
        //lerpTarget  = 1.0f;
    }


    public void OnAttack1hAUpdate()
    {
        thrustVec = model.transform.forward * anim.GetFloat("attack1hAVelocity"); 
        //anim.SetLayerWeight (anim.GetLayerIndex("attack"), Mathf.Lerp(anim.GetLayerWeight(anim.GetLayerIndex("attack")), lerpTarget, 0.01f));
    }

    public void OnAttackExit()
    {
        model.SendMessage("WeaponDisable");
    }

    public void OnHitEnter()
    {
        pi.inputEnabled = false;
        planarVec = Vector3.zero;      //受击瞬间失去速度
    }

    public void OnDieEnter()
    {
        pi.inputEnabled = false;
        planarVec = Vector3.zero;
    }

    public void OnBlockedEnter()
    {
        pi.inputEnabled = false;
    }

    public void OnStunnedEnter()
    {
        pi.inputEnabled = false;
        planarVec = Vector3.zero;
    }

    public void OnCounterBackEnter()
    {
        pi.inputEnabled = false;
        planarVec = Vector3.zero;
    }

    public void OnUpdateRM(object _deltaPos)
    {
        if (CheckState("attack1hC")) {

            deltaPos += (0.8f * deltaPos + 0.2f* (Vector3)_deltaPos) / 1.0f; 
            //deltaPos += (Vector3)_deltaPos;
        }
    }

    public void IssueTrigger (string triggerName)
    {
        anim.SetTrigger(triggerName);
    }


}
