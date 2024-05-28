using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ShowLevelClue : MonoBehaviour
{
    [Header("Clue")]
    public Canvas level2clue;
    private bool alreadyTriggered = false;
    private void OnTriggerEnter(Collider other)
    {
        if(!alreadyTriggered)
        {
            level2clue.GetComponent<FadeCanvas>().StartFadeIn();
            StartCoroutine(TurnOffClue(4f));
            alreadyTriggered = true;
        }
        
    }

    private IEnumerator TurnOffClue(float delay)
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(delay);
        level2clue.GetComponent<FadeCanvas>().StartFadeOut();

    }
}
