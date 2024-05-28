using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Hook : MonoBehaviour
{
    public GameObject parent;
    public int type;
    public string color;

    public void Awake()
    {
        GetComponent<XRSocketInteractor>().selectEntered.AddListener(OnSelectEnter);
    }
    
    public void OnSelectEnter(SelectEnterEventArgs args)
    {
        // Get the interactable object that was placed in the socket
        XRBaseInteractable interactable = (XRBaseInteractable)args.interactableObject;
        if (interactable != null)
        {
            if (type == 1)
            {
                if (color == "red")
                {
                    if (interactable.gameObject.name.Contains("Red"))
                    {
                        StartCoroutine(FreezeAfterDelay(interactable.gameObject));
                    }

                }
                else if (color == "blue")
                {
                    if (interactable.gameObject.name.Contains("Blue") && !interactable.gameObject.name.Contains("Brown"))
                    {
                        StartCoroutine(FreezeAfterDelay(interactable.gameObject));
                    }
                }
                else //brown
                {
                    if (interactable.gameObject.name.Contains("Brown"))
                    {
                        StartCoroutine(FreezeAfterDelay(interactable.gameObject));
                    }
                }
            }
            else
            {
                // Start the coroutine to freeze the item after the transformation
                StartCoroutine(FreezeAfterDelay(interactable.gameObject));
            }
        }
      
        
    }

    private IEnumerator FreezeAfterDelay(GameObject attachedItem)
    {
        // Wait for the end of the frame to ensure all transformations are applied
        yield return new WaitForSeconds(0.5f);

        // Disable the XRGrabInteractable component on the attached item
        XRGrabInteractable grabInteractable = attachedItem.GetComponent<XRGrabInteractable>();
        this.TryGetComponent(out SphereCollider sphereCollider);
        this.TryGetComponent(out BoxCollider boxCollider);
        if(type == 0)
        {
            sphereCollider.enabled = false;
        }
        else
        {
            boxCollider.enabled = false;
        }

        if (grabInteractable != null)
        {
            grabInteractable.enabled = false;
        }

        // Freeze the attached item in place by setting Rigidbody constraints
        Rigidbody itemRigidbody = attachedItem.GetComponent<Rigidbody>();
        if (itemRigidbody != null)
        {
            itemRigidbody.isKinematic = true;
            itemRigidbody.constraints = RigidbodyConstraints.FreezeAll;
        }

        var level = parent.GetComponentInParent<Level2Progress>();

        if(type == 0)
        {
            level.HatPlaced();
        }
        else
        {
            level.BookPlaced();
        }
        
    }
}
