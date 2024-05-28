using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMovement : MonoBehaviour
{
    public GameObject LevelProgess;
    public GameObject PlaySound;

    public Vector3 pointA = new Vector3(-2.73f, 30.34f, 4.73f);
    public Vector3 pointB = new Vector3(-2.73f, 28.177f, 6.759f);

    private Vector3 targetPosition;
    private float moveSpeed = 2.0f;

    void Start()
    {
        // Initialize the first target position
        SetNewTargetPosition();
    }

    void Update()
    {
        // Move towards the target position
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // Check if the object has reached the target position
        if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
        {
            // Set a new target position
            SetNewTargetPosition();
        }
    }

    void SetNewTargetPosition()
    {
        float x = pointA.x;
        float y = Random.Range(pointB.y, pointA.y);
        float z = Random.Range(pointA.z, pointB.z);

        targetPosition = new Vector3(x, y, z);
    }

    private void OnTriggerEnter(Collider other)
    {
        LevelProgess.GetComponent<Level3Progress>().TargetHit();
        PlaySound.GetComponent<PlayQuickSound>().Play();
        this.gameObject.SetActive(false);
    }
}
