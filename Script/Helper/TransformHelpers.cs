using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransformHelpers 
{
    //public static void hihi(this Transform trans, string say){ 
    
    //   Debug.Log(trans.name + " says " + say);
    //}

    public static Transform DeepFind(this Transform parent, string targetName)
    {
        Transform temptrans = null;

        foreach(Transform child in parent)
        {
            //Debug.Log(child.name);
            if (child.name == targetName) 
            { 
                return child; 
            }
            else {
                temptrans = DeepFind(child, targetName);
                if (temptrans != null) { return temptrans; }
            }
        }
        return null;
    }
}
