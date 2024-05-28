using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class MovementScript : MonoBehaviour
{
    public XROrigin rig;
    public Toggle snap;
    public Toggle continuous;
    
    public void SnapChanged()
    {
        if(snap.isOn)
        {
            continuous.interactable = false;
            rig.GetComponent<ActionBasedSnapTurnProvider>().enabled = true;
        }
        else
        {
            rig.GetComponent<ActionBasedSnapTurnProvider>().enabled = false;
            continuous.interactable = true;
        }
    }

    public void ContinuousChanged()
    {
        if (continuous.isOn)
        {
            snap.interactable = false;
            rig.GetComponent<ActionBasedContinuousTurnProvider>().enabled = true;
        }
        else
        {
            rig.GetComponent<ActionBasedContinuousTurnProvider>().enabled = false;
            snap.interactable = true;
        }
    }
}
