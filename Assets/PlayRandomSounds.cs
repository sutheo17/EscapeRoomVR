using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayRandomSounds : MonoBehaviour
{
    public GameObject bird_1;
    public GameObject bird_2;
    public GameObject bird_3;
    public bool inside = false;

    void Start()
    {
        // Start the coroutine to play sounds randomly
        StartCoroutine(PlayRandomBirdSound());
    }


    private IEnumerator PlayRandomBirdSound()
    {
        while (true)
        {
            // Randomly select a time interval between 10 and 20 seconds
            float waitTime = Random.Range(10f, 20f);
            yield return new WaitForSeconds(waitTime);

            // Randomly select one of the birds
            int birdIndex = Random.Range(0, 3);

            // Play sound on the selected bird
            switch (birdIndex)
            {
                case 0:
                    bird_1.GetComponent<PlayQuickSound>().Play();
                    break;
                case 1:
                    bird_2.GetComponent<PlayQuickSound>().Play();
                    break;
                case 2:
                    bird_3.GetComponent<PlayQuickSound>().Play();
                    break;
            }

        }
    }
}
