using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnablePhysics : MonoBehaviour
{
    public GameObject gameObject = null;
    // Start is called before the first frame update
    public void Enable_Physics()
    {
        Debug.Log("left");
        this.transform.SetParent(null);
    }

}
