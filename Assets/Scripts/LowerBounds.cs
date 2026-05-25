using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowerBounds : MonoBehaviour
{
    [SerializeField] Transform cameraTransform;
    [SerializeField] float towDistance = 7f;
    Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        MoveUpWithCamera();
    }

    void MoveUpWithCamera()
    {
        if (cameraTransform.position.y > transform.position.y + towDistance)
        {
            Vector3 newPosition = transform.position;
            newPosition.y = cameraTransform.position.y - towDistance;
            transform.position = newPosition;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            TriggerGameOver();
        }
    }

    private void TriggerGameOver()
    {
        GameManager.Instance.CurrentState = GameManager.State.GameOver;
        transform.position = startPosition;
    }
}
