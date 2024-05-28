using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwitchingLight : MonoBehaviour
{
    // Blink parameters
    public float blinkDuration = 0.2f;  // Duration of each blink
    public float waitDuration = 3f;     // Duration to wait after blinking
    public int blinkCount = 3;          // Number of blinks

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(BlinkLight());
    }

    private IEnumerator BlinkLight()
    {
        while (true)
        {
            for (int i = 0; i < blinkCount; i++)
            {
                // Turn the light off
                GetComponent<Light>().enabled = false;
                yield return new WaitForSeconds(blinkDuration);

                // Turn the light on
                GetComponent<Light>().enabled = true;
                yield return new WaitForSeconds(blinkDuration);
            }

            // Wait for the specified duration before repeating the blinks
            yield return new WaitForSeconds(waitDuration);
        }
    }
}
