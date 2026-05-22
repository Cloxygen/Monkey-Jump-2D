using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowerBounds : MonoBehaviour
{
    [SerializeField] Transform camera;
    [SerializeField] float towDistance = 7f;
    Vector3 startPosition;
    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        moveUpWithCamera();
    }

    void moveUpWithCamera()
    {
        if(camera.position.y > transform.position.y + towDistance)
        {
            Vector3 newPosition = transform.position;
            newPosition.y = camera.position.y - towDistance;
            transform.position = newPosition;
        }
            
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.Instance.CurrentState = GameManager.State.GameOver;
            transform.position = startPosition;
        }
    }
}
