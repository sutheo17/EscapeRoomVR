using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Painting : MonoBehaviour
{
    [Header("Painting")]
    public GameObject Painting_object;
    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        var obj = Painting_object.GetComponent<XRGrabInteractable>();

        rb = obj.GetComponent<Rigidbody>();
        
        rb.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;

        obj.selectEntered.AddListener(OnGrab);
    }

    private void OnGrab(SelectEnterEventArgs interactor)
    {
        rb.constraints = RigidbodyConstraints.None;
    }


}
