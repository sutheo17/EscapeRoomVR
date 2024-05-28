using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Storage : MonoBehaviour
{
    private List<string> alreadyIn = new List<string>();
    private void OnTriggerEnter(Collider other)
    {
        XRGrabInteractable gameObject = other.GetComponent<XRGrabInteractable>();
        if (gameObject != null && !alreadyIn.Contains(other.name))
        {
            Debug.Log(gameObject.interactionLayers.value);
            if (gameObject.interactionLayers.value != 2 && gameObject.interactionLayers.value != 32)
            {
                alreadyIn.Add(gameObject.name);

                other.gameObject.SetActive(false);
                var level = GetComponentInParent<Level2Progress>();
                level.SportItemPlaced();
                gameObject.enabled = false;
            }
            else
            {
                // Convert comma-separated ranges to period-separated ranges
                float minX = 7.04f;
                float maxX = 1.2f;
                float y = 29.06f;
                float minZ = -2.6f;
                float maxZ = -0.36f;


                // Generate random x, y, and z positions within the specified ranges
                float randomX = Random.Range(minX, maxX);
                float randomZ = Random.Range(minZ, maxZ);

                other.gameObject.transform.position = new Vector3(randomX, y, randomZ);
            }
        }

    }
}
