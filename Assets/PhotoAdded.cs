using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PhotoAdded : MonoBehaviour
{
    private int photoCount = 0;

    [Header("ProgressText")]
    public TextMeshProUGUI progressText;
    [Header("ProgressCanvas")]
    public Canvas progress;

    [Header("EndingCanvas")]
    public Canvas ending;
    public GameObject Boombox;

    private void OnTriggerEnter(Collider other)
    {
        photoCount++;
        other.gameObject.SetActive(false);
        progressText.text = "Progress: " + photoCount + "/3";
        if (photoCount == 3)
        {
            Boombox.GetComponent<PlayContinuousSound>().PlayChampion();
            ending.GetComponent<FadeCanvas>().StartFadeIn();
        }
        else
        {
            progress.GetComponent<FadeCanvas>().StartFadeIn();
            StartCoroutine(TurnOffClue(4f));
        }
        
        
      
    }

    private IEnumerator TurnOffClue(float delay)
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(delay);
        progress.GetComponent<FadeCanvas>().StartFadeOut();

    }
}
