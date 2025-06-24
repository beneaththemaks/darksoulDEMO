using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootMotionController : MonoBehaviour
{
    private Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator> ();
    }

    void OnAnimatorMove()
    {
        SendMessageUpwards("OnUpdateRM", (object)anim.deltaPosition);

    }
    //void OnAnimatorMove()
    //{
    //    if (anim != null && anim.isActiveAndEnabled)
    //    {
    //        SendMessageUpwards("OnUpdateRM", anim.deltaPosition, SendMessageOptions.DontRequireReceiver);
    //    }
    //}

}
