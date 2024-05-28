using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadFirstClue : MonoBehaviour
{
    public Canvas firstClue;
    public void LoadClue()
    {
        firstClue.GetComponent<FadeCanvas>().StartFadeIn();
        StartCoroutine(TurnOffClue(6f));
    }

    private IEnumerator TurnOffClue(float delay)
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(delay);
        firstClue.GetComponent<FadeCanvas>().StartFadeOut();

    }
}
