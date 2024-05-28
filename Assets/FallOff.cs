using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallOff : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        GetComponent<LoadScene>().ReloadCurrentScene();
    }
}
