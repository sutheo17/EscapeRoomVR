using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CardReader : XRSocketInteractor
{
    Transform m_KeycardTransform;
    Vector3 m_HoverEntry;
    bool m_SwipIsValid = false;
    public GameObject VisualLockToHide;
    public GameObject HandleToEnable;
    float AllowedUprightErrorRange = 0.1f;

    protected override void OnHoverEntered(HoverEnterEventArgs args)
    {
        base.OnHoverEntered(args);

        m_KeycardTransform = args.interactableObject.transform;
        m_HoverEntry = m_KeycardTransform.position;
        m_SwipIsValid = true;
    }

    protected override void OnHoverExited(HoverExitEventArgs args)
    {
        base.OnHoverExited(args);

        Vector3 entryToExit = m_KeycardTransform.position - m_HoverEntry;

        if (m_SwipIsValid && entryToExit.y < -0.15f)
        {
            VisualLockToHide.gameObject.SetActive(false);
            HandleToEnable.GetComponent<SlidingDoor>().SetReady();
            GetComponent<PlayQuickSound>().Play();
        }

        m_KeycardTransform = null;
    }

    public override bool CanSelect(IXRSelectInteractable interactable)
    {
        return false;
    }

    private void Start()
    {
        base.Start();
        
    }


    private void Update()
    {
        if (m_KeycardTransform != null)
        {
            Vector3 keycardUp = m_KeycardTransform.forward;
            float dot = Vector3.Dot(keycardUp, Vector3.up);

            if (dot < 1 - AllowedUprightErrorRange)
            {
                m_SwipIsValid = false;
            }
        }
    }
}
