using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOnLights : MonoBehaviour
{
    public GameObject chandelier_1;
    public GameObject chandelier_2;
    public GameObject chandelier_3;
    public GameObject chandelier_4;
    public GameObject chandelier_5;
    public GameObject chandelier_6;

    public GameObject Door;
    private void OnTriggerEnter(Collider other)
    {
        this.transform.rotation = Quaternion.Euler(0f, 0f, 180f);
        chandelier_1.GetComponentInChildren<Light>().enabled = true;
        chandelier_2.GetComponentInChildren<Light>().enabled = true;
        chandelier_3.GetComponentInChildren<Light>().enabled = true;
        chandelier_4.GetComponentInChildren<Light>().enabled = true;
        chandelier_5.GetComponentInChildren<Light>().enabled = true;
        chandelier_6.GetComponentInChildren<Light>().enabled = true;

        Door.SetActive(false);
    }
}
