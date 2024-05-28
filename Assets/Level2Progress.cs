using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Level2Progress : MonoBehaviour
{
    [Header("DoorToDestroy")]
    public GameObject Door;
    [Header("ProgressText")]
    public TextMeshProUGUI progressText;
    [Header("ProgressCanvas")]
    public Canvas progress;

    private int hatPlaced = 0;
    private int bookPlaced = 0;
    private int sportItemPlaced = 0;

    private int subTasksCompleted = 0;

    public void HatPlaced()
    {
        hatPlaced++;
        if(hatPlaced == 3)
        {
            GetComponent<PlayQuickSound>().Play();
            subTasksCompleted++;
            if (subTasksCompleted == 3)
            {
                CheckIfLevelIsCompleted();
            }
            else
            {
                progressText.text = "Progress: " + subTasksCompleted + "/3";
                progress.GetComponent<FadeCanvas>().StartFadeIn();
                StartCoroutine(TurnOffClue(4f));
            }
           
        }
    }
    
    public void BookPlaced()
    {
        bookPlaced++;
        if(bookPlaced == 3)
        {
            GetComponent<PlayQuickSound>().Play();
            subTasksCompleted++;
            if (subTasksCompleted == 3)
            {
                CheckIfLevelIsCompleted();
            }
            else
            {
                progressText.text = "Progress: " + subTasksCompleted + "/3";
                progress.GetComponent<FadeCanvas>().StartFadeIn();
                StartCoroutine(TurnOffClue(4f));
            }
           
        }
    }

    public void SportItemPlaced()
    {
        sportItemPlaced++;
        if (sportItemPlaced == 6)
        {
            GetComponent<PlayQuickSound>().Play();
            subTasksCompleted++;
            if(subTasksCompleted == 3)
            {
                CheckIfLevelIsCompleted();
            }
            else
            {
                progressText.text = "Progress: " + subTasksCompleted + "/3";
                progress.GetComponent<FadeCanvas>().StartFadeIn();
                StartCoroutine(TurnOffClue(4f));
            }
           
        }
    }

    public void CheckIfLevelIsCompleted()
    {
        if(subTasksCompleted == 3)
        {
            Door.SetActive(false);
        }
    }

    private IEnumerator TurnOffClue(float delay)
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(delay);
        progress.GetComponent<FadeCanvas>().StartFadeOut();

    }
}
