using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class BreakGlass : MonoBehaviour
{
    public GameObject glass_1;
    public GameObject glass_2;
    public GameObject glass_3;
    public GameObject glass_4;
    public GameObject glass_5;
    public GameObject glass_6;
    public GameObject glass_7;
    public GameObject glass_8;

   

    public GameObject glass;
    private void OnTriggerEnter(Collider other)
    {
        glass.GetComponent<PlayQuickSound>().Play();
        glass_1.SetActive(true);
        glass_2.SetActive(true);
        glass_3.SetActive(true);
        glass_4.SetActive(true);
        glass_5.SetActive(true);
        glass_6.SetActive(true);
        glass_7.SetActive(true);
        glass_8.SetActive(true);

       

        this.gameObject.SetActive(false);
        
        
        
    }
}
