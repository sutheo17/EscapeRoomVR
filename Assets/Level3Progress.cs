using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level3Progress : MonoBehaviour
{
    private int targetsHit = 0;

    public GameObject Door;
    public GameObject BoomBox;
    public void TargetHit()
    {
        targetsHit++;
        if (targetsHit == 3)
        {
            Door.SetActive(false);
            GetComponent<PlayQuickSound>().Play();
            BoomBox.GetComponent<PlayContinuousSound>().Play();
        }
    }

}
