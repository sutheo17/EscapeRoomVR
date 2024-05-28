using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class TakePhoto : MonoBehaviour
{
    public GameObject photoPrefab = null;
    public Transform startPoint = null;
    public Camera camera = null;
    public Color clearColor = Color.white; // Set a default clear color

    public void Take_Photo()
    {
        GameObject newObject = Instantiate(photoPrefab, startPoint.position, startPoint.rotation);
        newObject.transform.SetParent(startPoint);

        Rigidbody rb = newObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true; // Prevent falling
        }
        else
        {
            Debug.LogWarning("Rigidbody component not found on the prefab.");
        }

        Transform film = newObject.transform.Find("Photograph_Film");

        if (film != null)
        {
            MeshRenderer filmRenderer = film.GetComponent<MeshRenderer>();

            if (filmRenderer != null)
            {
                StartCoroutine(CapturePhoto(filmRenderer));
            }
            else
            {
                Debug.LogWarning("MeshRenderer component not found on the 'film' child object.");
            }
        }
        else
        {
            Debug.LogWarning("Child object 'film' not found.");
        }

        StartCoroutine(WaitForSecondsCoroutine(newObject));
    }

    private IEnumerator CapturePhoto(MeshRenderer filmRenderer)
    {
        yield return new WaitForEndOfFrame();

        // Ensure the camera's clear flags and background color
        camera.clearFlags = CameraClearFlags.SolidColor;
        camera.backgroundColor = clearColor;

        RenderTexture currentRT = RenderTexture.active;
        RenderTexture.active = camera.targetTexture;

        camera.Render();

        // Capture the render image
        Texture2D image = new Texture2D(camera.targetTexture.width, camera.targetTexture.height, TextureFormat.RGB24, false);
        image.ReadPixels(new Rect(0, 0, camera.targetTexture.width, camera.targetTexture.height), 0, 0);
        image.Apply();
        RenderTexture.active = currentRT; // Reset active render texture

        filmRenderer.material.mainTexture = image;
    }

    private IEnumerator WaitForSecondsCoroutine(GameObject obj)
    {
        // Wait for 5 seconds
        yield return new WaitForSeconds(5);

        // Code here will execute after 5 seconds
        obj.transform.SetParent(null);
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        rb.isKinematic = false;
    }
}
