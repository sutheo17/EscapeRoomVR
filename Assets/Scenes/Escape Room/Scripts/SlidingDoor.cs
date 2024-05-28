using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SlidingDoor : XRBaseInteractable
{
    public Transform DraggedTransform; // set to parent door object
    public Vector3 LocalDragDirection; // set to -1, 0, 0
    public float DragDistance; // set to 0.8
    public int DoorWeight = 35;
    public GameObject Door;

    private bool setReady = false;


    private Vector3 m_StartPosition;
    private Vector3 m_EndPosition;
    private Vector3 m_WorldDragDirection;
    private void Start()
    {
        DragDistance = 0.8f;
        DraggedTransform = Door.transform;
        LocalDragDirection = new Vector3(-1, 0, 0);
        m_WorldDragDirection = transform.TransformDirection(LocalDragDirection).normalized;
       

        m_StartPosition = DraggedTransform.position;
        m_EndPosition = m_StartPosition + m_WorldDragDirection * DragDistance;
    }

    public void SetReady()
    {
        setReady = true;
    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        if (isSelected)
        {
            if(setReady)
            {
                UnityEngine.Debug.Log("Door is ready to be dragged");
                var interactorTransform = firstInteractorSelecting.GetAttachTransform(this);
                Vector3 selfToInteractor = interactorTransform.position - transform.position;

                // calculate dot product of selfToInteractor onto drag direction
                float force = Vector3.Dot(selfToInteractor, m_WorldDragDirection);

                // calculate speed based the dot product
                float absoluteForce = Mathf.Abs(force);
                float speed = absoluteForce / Time.deltaTime / DoorWeight;
                //speed = 0.00005f;
                // move door based on speed using MoveTowards
                UnityEngine.Debug.Log("Dragged: " + DraggedTransform.position);
                DraggedTransform.position = Vector3.MoveTowards(DraggedTransform.position, m_EndPosition, speed * Time.deltaTime);
            }
           
        }
    }
}

   

