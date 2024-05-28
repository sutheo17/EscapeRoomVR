using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotating : MonoBehaviour
{

    [SerializeField]
    MeshRenderer mh;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        mh.transform.Rotate(90.0f, 90.0f, 90.0f);
    }
}
