using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock : MonoBehaviour
{
    public float timeSinceLastSpawn = 0;
    public int lastMin;
    public int lastHour;


    [SerializeField]
    public GameObject hour;
    public GameObject minute;
    public GameObject second;


    // Start is called before the first frame update
    void Start()
    {
        lastMin = DateTime.Now.Minute;
        lastHour = DateTime.Now.Hour;
        float rotation = 1 / DateTime.Now.Minute;
        hour.transform.Rotate(0.0f, (360/12)*DateTime.Now.Hour, 0.0f);
        hour.transform.Rotate(0.0f, (DateTime.Now.Minute), 0.0f);
        minute.transform.Rotate(0.0f, (360/60)*DateTime.Now.Minute, 0.0f);
        second.transform.Rotate(0.0f, (360 / 60) * DateTime.Now.Second, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceLastSpawn += Time.deltaTime;
        
        if(timeSinceLastSpawn >= 1f)
        {
            second.transform.Rotate(0.0f, (360 / 60), 0.0f);
            minute.transform.Rotate(0.0f, 0.1f, 0.0f);
            hour.transform.Rotate(0.0f, (360 / 12 / 60 / 60), 0.0f);
            timeSinceLastSpawn = 0;
        }
        
    }
}
