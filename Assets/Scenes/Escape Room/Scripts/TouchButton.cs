using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TouchButton : XRGrabInteractable
{
    [Header("Button Data")]
    public GameObject button;
    public string digit;
    private Color oldColor;
    private NumberPad numberPad;

    protected override void OnHoverEntered(HoverEnterEventArgs interactor)
    {
        base.OnHoverEntered(interactor);
        var renderer = button.GetComponent<Renderer>();
        oldColor = renderer.material.color;
        numberPad = GetComponentInParent<NumberPad>();

        var sound = button.GetComponent<PlayQuickSound>();
        sound.Play();

        numberPad.EnterDigit(digit);

        renderer.material.color = Color.green;
    }

    protected override void OnHoverExited(HoverExitEventArgs interactor)
    {
        base.OnHoverExited(interactor);
        var renderer = button.GetComponent<Renderer>();
        renderer.material.color = oldColor;
    }

    public void Start()
    {
        
    }
}
