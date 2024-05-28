using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;

public class OutsideNoise : MonoBehaviour
{
    public GameObject levelSound;
    public bool inside = false;
    private void OnTriggerEnter(Collider other)
    {
        if(!inside)
        {
            StartCoroutine(TurnOnSound(1.5f));
            levelSound.GetComponent<PlayRandomSounds>().inside = false;
        }
        else
        {
            StartCoroutine(TurnOffSound(1.5f));
            levelSound.GetComponent<PlayRandomSounds>().inside = true;
        }
       

    }

    private IEnumerator TurnOffSound(float duration)
    {
        // Get the PlayContinuousSound component and its initial volume
        PlayContinuousSound soundComponent = levelSound.GetComponent<PlayContinuousSound>();
        if (soundComponent == null)
        {
            Debug.LogError("PlayContinuousSound component not found on levelSound");
            yield break;
        }

        float initialVolume = soundComponent.volume;
        float targetVolume = 0.25f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            soundComponent.UpdateVolume(Mathf.Lerp(initialVolume, targetVolume, elapsedTime / duration));
            soundComponent.volume = Mathf.Lerp(initialVolume, targetVolume, elapsedTime / duration);
            yield return null; // Wait until the next frame
        }

        // Ensure the volume is set to the target volume after the loop
        soundComponent.UpdateVolume(targetVolume);
        soundComponent.volume = targetVolume;

        levelSound.GetComponent<PlayRandomSounds>().bird_1.GetComponent<PlayQuickSound>().volume = 0.2f;
        levelSound.GetComponent<PlayRandomSounds>().bird_2.GetComponent<PlayQuickSound>().volume = 0.2f;
        levelSound.GetComponent<PlayRandomSounds>().bird_3.GetComponent<PlayQuickSound>().volume = 0.2f;
    }

    private IEnumerator TurnOnSound(float duration)
    {
        // Get the PlayContinuousSound component and its initial volume
        PlayContinuousSound soundComponent = levelSound.GetComponent<PlayContinuousSound>();
        if (soundComponent == null)
        {
            Debug.LogError("PlayContinuousSound component not found on levelSound");
            yield break;
        }

        float initialVolume = soundComponent.volume;
        float targetVolume = 0.5f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            soundComponent.UpdateVolume(Mathf.Lerp(initialVolume, targetVolume, elapsedTime / duration));
            soundComponent.volume = Mathf.Lerp(initialVolume, targetVolume, elapsedTime / duration);
            yield return null; // Wait until the next frame
        }

        // Ensure the volume is set to the target volume after the loop
        soundComponent.UpdateVolume(targetVolume);
        soundComponent.volume = targetVolume;

        levelSound.GetComponent<PlayRandomSounds>().bird_1.GetComponent<PlayQuickSound>().volume = 1.0f;
        levelSound.GetComponent<PlayRandomSounds>().bird_2.GetComponent<PlayQuickSound>().volume = 1.0f;
        levelSound.GetComponent<PlayRandomSounds>().bird_3.GetComponent<PlayQuickSound>().volume = 1.0f;
    }
}
